using System.Collections;
using System.Collections.Immutable;
using System.Reflection;

namespace EdwardHsu.ModifyDetector
{
    public class ModifyDetector: IModifyDetector
    {
        private const int DEFAULT_HASH_LOOP = 0x1111111;
        private const int DEFAULT_HASH_NULL = 0xaaaaaaa;
        private const int DEFAULT_HASH_INIT = 0x5555555;


        private int? _detectorState;
        private Dictionary<MemberInfo, int> _memberStateMap = new Dictionary<MemberInfo, int>();

        public void UpdateDetectorState()
        {
            _detectorState = ComputeDetectorState(this, null);
            _memberStateMap = ComputeMemberState(this);
        }

        public bool HasModified(out IEnumerable<ModifiedMember> modifiedMembers)
        {
            var modifiedMembersMap = new Dictionary<ModifyDetector, IList<ModifiedMember>>();
            var hasModified = InternalHasModified(this, modifiedMembersMap);

            if (hasModified == false)
            {
                modifiedMembers = Enumerable.Empty<ModifiedMember>();

                return false;
            }

            modifiedMembers = modifiedMembersMap[this].AsReadOnly();
            return true;
        }


        private static bool InternalHasModified(ModifyDetector detector,
            Dictionary<ModifyDetector, IList<ModifiedMember>> modifiedMembersMap)
        {
            if (detector._detectorState == null)
            {
                throw new InvalidOperationException("Detector state is not initialized. Please call UpdateDetectorState() first.");
            }

            modifiedMembersMap[detector] = new List<ModifiedMember>();

            if (detector._detectorState == ComputeDetectorState(detector, null))
            {
                // No modified
                return false;
            }

            var memberStateMap = ComputeMemberState(detector);

            var detectTargets = GetDetectTargets(detector).ToArray();

            foreach (var memberState in detector._memberStateMap.OrderBy(x => Array.IndexOf(detectTargets, x.Key)))
            {
                var memberInfo = memberState.Key;
                var origionState = memberState.Value;
                var currentState = memberStateMap[memberInfo];

                if (origionState != currentState)
                {
                    var modifiedMember = MemberInfoToModifiedMember(memberInfo);

                    modifiedMembersMap[detector].Add(modifiedMember);

                    var memberValue = GetMemberValueByMemberInfo(detector, memberInfo);

                    if (memberValue == null) continue;
                    

                    if (memberValue is ModifyDetector memberMD)
                    {
                        if (modifiedMembersMap.ContainsKey(memberMD))
                        {
                            modifiedMember.Children = modifiedMembersMap[memberMD];
                        }
                        else if (memberValue is IModifyDetector md)
                        {
                            InternalHasModified(memberMD, modifiedMembersMap);
                            modifiedMember.Children = modifiedMembersMap[memberMD];
                        }
                    }
                    else if (memberValue is IEnumerable enumData)
                    {
                        var index = 0;
                        foreach (var item in enumData)
                        {
                            if (item is ModifyDetector itemMD)
                            {
                                if (modifiedMembersMap.ContainsKey(itemMD))
                                {
                                    modifiedMember.Children = modifiedMember.Children ?? new List<ModifiedMember>();
                                    modifiedMember.Children.Add(new ModifiedMember(index)
                                    {
                                        Children = modifiedMembersMap[itemMD]
                                    });
                                }
                                else if (item is IModifyDetector md)
                                {
                                    InternalHasModified(itemMD, modifiedMembersMap);
                                    modifiedMember.Children = modifiedMember.Children ?? new List<ModifiedMember>();
                                    modifiedMember.Children.Add(new ModifiedMember(index)
                                    {
                                        Children = modifiedMembersMap[itemMD]
                                    });
                                }
                            }
                            index++;
                        }
                    }
                }
            }

            return true;
        }


        private static IEnumerable<MemberInfo> GetDetectTargets(IModifyDetector detector, bool order = true)
        {
            var detectTargets = detector.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public |
                                                              BindingFlags.NonPublic | BindingFlags.GetProperty |
                                                              BindingFlags.GetField)
                .Select(x => (attr: x.GetCustomAttribute<ModifyDetectTargetAttribute>(), memberInfo: x))
                .Where(x => x.attr != null);

            if (order)
            {

                var nonCustomOrderedDetectTargets = detectTargets.Where(x => x.attr.Order == int.MinValue)
                    .OrderBy(x => x.memberInfo.Name);

                var customOrderedDetectTargets = detectTargets.Where(x => x.attr.Order > int.MinValue)
                    .OrderBy(x => x.attr.Order);

                var orderedDetectTargets = customOrderedDetectTargets.Concat(nonCustomOrderedDetectTargets);

                return orderedDetectTargets.Select(x => x.memberInfo).ToImmutableArray();
            }
            else
            {
                return detectTargets.Select(x=>x.memberInfo).ToImmutableArray();
            }
        }

        /// <summary>
        /// Compute the hash code of the object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static int ComputeDetectorState(object obj, List<object> path)
        {
            if(obj == null) return DEFAULT_HASH_NULL;
            if(path == null) path = new List<object>();

            if (path.Contains(obj))
            {
                // Detect circular reference
                return DEFAULT_HASH_LOOP >> (path.Count - path.IndexOf(obj));
            }

            path.Add(obj);

            if (obj is IModifyDetector md)
            {
                var orderedDetectTargets = GetDetectTargets(md);

                var state = DEFAULT_HASH_INIT;

                foreach (var detectTarget in orderedDetectTargets)
                {
                    var memberValue = detectTarget switch
                    {
                        PropertyInfo pi => pi.GetValue(obj),
                        FieldInfo fi => fi.GetValue(obj),
                        _ => null
                    };

                    state ^= ComputeDetectorState(memberValue, path.ToList());
                }

                return state;
            }
            else if(obj is IEnumerable enumData)
            {
                var state = DEFAULT_HASH_INIT;

                foreach (var item in enumData)
                {
                    state ^= ComputeDetectorState(item, path.ToList());
                }

                return state;
            }

            return obj.GetHashCode();
        }

        private static Dictionary<MemberInfo, int> ComputeMemberState(IModifyDetector detector)
        {
            if (detector == null) throw new ArgumentNullException(nameof(detector));

            var detectTargets = GetDetectTargets(detector, false);

            var stateMap = new Dictionary<MemberInfo, int>();

            foreach (var detectTarget in detectTargets)
            {
                var memberValue = detectTarget switch
                {
                    PropertyInfo pi => pi.GetValue(detector),
                    FieldInfo fi => fi.GetValue(detector),
                    _ => null
                };
                
                var tempPath = new List<object>();
                tempPath.Add(detector);
                stateMap.Add(detectTarget, ComputeDetectorState(memberValue, tempPath));
            }

            return stateMap;
        }

        private static object GetMemberValueByMemberInfo(object obj, MemberInfo memberInfo)
        {
            if(obj == null) throw new ArgumentNullException(nameof(obj));

            return memberInfo switch
            {
                PropertyInfo pi => pi.GetValue(obj),
                FieldInfo fi => fi.GetValue(obj),
                _ => throw new NotSupportedException("Unsupported member type. Currently only support PropertyInfo and FieldInfo")
            };
        }

        private static ModifiedMember MemberInfoToModifiedMember(MemberInfo memberInfo)
        {
            if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

            return memberInfo switch
            {
                PropertyInfo pi => new ModifiedProperty(pi),
                FieldInfo fi => new ModifiedField(fi),
                _ => throw new NotSupportedException("Unsupported member type. Currently only support PropertyInfo and FieldInfo")
            };
        }
    }
}
