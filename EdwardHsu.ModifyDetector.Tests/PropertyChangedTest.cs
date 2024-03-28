using EdwardHsu.ModifyDetector.Tests.Models;

namespace EdwardHsu.ModifyDetector.Tests
{
    public class PropertyChangedTest
    {
        [Fact(DisplayName = "UninitializedState")]
        public void UninitializedState()
        {
            var node = new TestNode3();
            node.Name = "New Name";

            Assert.Throws<InvalidOperationException>(() =>
            {
                node.HasModified(out _);
            });
        }

        [Fact(DisplayName = "Property Changed - 1")]
        public void PropertyChanged1()
        {
            var node = new TestNode1();
            
            node.Name = "New Name";
            Assert.True(node.HasModified(out var modifiedMembers));

            Assert.Single(modifiedMembers);
            Assert.Equal(nameof(TestNode1.Name), modifiedMembers.First().Member.Name);
        }

        [Fact(DisplayName = "Property Changed - 2")]
        public void PropertyChanged2()
        {
            var node = new TestNode1();

            node.Id = Guid.NewGuid();
            node.Name = "New Name";

            var changedProperties = new List<string> { nameof(TestNode1.Id), nameof(TestNode1.Name)};

            Assert.True(node.HasModified(out var modifiedMembers));
            
            Assert.Equal(2, modifiedMembers.Count());


            for (int i = 0; i < 2; i++)
            {
                Assert.Equal(changedProperties[i], modifiedMembers.ElementAt(i).Member.Name);
            }
        }

        [Fact(DisplayName = "Property Changed - 3")]
        public void PropertyChanged3()
        {
            var node = new TestNode1();

            node.Name = "New Name";
            node.Description = "New Name";
            Assert.True(node.HasModified(out var modifiedMembers));

            Assert.Equal(2, modifiedMembers.Count());
            Assert.Equal(nameof(TestNode1.Name), modifiedMembers.First().Member.Name);
        }

        [Fact(DisplayName = "Property Changed - 4")]
        public void PropertyChanged4()
        {
            var node = new TestNode1();

            node.Name = "New Name";
            node.Description = "New Name";
            node.Parent = new TestNode1()
            {
                Name = "New Parent Name"
            };
            
            Assert.True(node.HasModified(out _));
        }


        [Fact(DisplayName = "Property Changed with cycle reference - 1")]
        public void PropertyChangedWithCycleReference1()
        {
            var node = new TestNode1();

            node.Id = Guid.NewGuid();
            node.Name = "New Name";
            node.Parent = node;

            var changedProperties = new List<string> { nameof(TestNode1.Id), nameof(TestNode1.Name), nameof(TestNode1.Parent) };

            Assert.True(node.HasModified(out var modifiedMembers));

            Assert.Equal(3, modifiedMembers.Count());


            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(changedProperties[i], modifiedMembers.ElementAt(i).Member.Name);
            }
        }

        [Fact(DisplayName = "Property Changed with cycle reference - 2")]
        public void PropertyChangedWithCycleReference2()
        {
            var node = new TestNode1();

            node.Id = Guid.NewGuid();
            node.Name = "New Name";
            node.Children = new List<TestNode1>()
            {
                new TestNode1()
                {
                    Children = new List<TestNode1>()
                    {
                        node
                    }
                },
                node
            };

            var changedProperties = new List<string> { nameof(TestNode1.Id), nameof(TestNode1.Name), nameof(TestNode1.Children) };

            Assert.True(node.HasModified(out var modifiedMembers));

            Assert.Equal(3, modifiedMembers.Count());


            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(changedProperties[i], modifiedMembers.ElementAt(i).Member.Name);
            }
        }

        [Fact(DisplayName = "Property Changed with cycle reference - 3")]
        public void PropertyChangedWithCycleReference3()
        {
            var node = new TestNode1();

            node.Id = Guid.NewGuid();
            node.Name = "New Name";
            node.Children = new List<TestNode1>()
            {
                new TestNode1()
                {
                    Children = new List<TestNode1>()
                    {
                        node
                    }
                }
            };

            var changedProperties = new List<string> { nameof(TestNode1.Id), nameof(TestNode1.Name), nameof(TestNode1.Children) };

            Assert.True(node.HasModified(out var modifiedMembers));

            Assert.Equal(3, modifiedMembers.Count());


            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(changedProperties[i], modifiedMembers.ElementAt(i).Member.Name);
            }

            node.UpdateDetectorState();

            node.Children[0].Name = "New Name 2";

            Assert.True(node.HasModified(out modifiedMembers));

            Assert.Single(modifiedMembers);

            Assert.Equal(nameof(TestNode1.Children), modifiedMembers.First().Member.Name);

            Assert.Equal(nameof(node.Name),modifiedMembers.First().Children.First().Children.First().Member.Name);

        }

        [Fact(DisplayName = "Property Changed then rollback")]
        public void PropertyChangedThenRollback()
        {
            var node = new TestNode1();

            var origionName = node.Name;
            node.Name = "New Name";
            Assert.True(node.HasModified(out var modifiedMembers));
            Assert.Single(modifiedMembers);
            Assert.Equal(nameof(TestNode1.Name), modifiedMembers.First().Member.Name);

            node.Name = origionName;

            Assert.False(node.HasModified(out _));
        }

        [Fact(DisplayName = "Property Changed then UpdateDetectorState")]
        public void PropertyChangedThenRollbackWithChildren()
        {
            var node = new TestNode1();
            
            node.Name = "New Name";
            Assert.True(node.HasModified(out var modifiedMembers));
            Assert.Single(modifiedMembers);
            Assert.Equal(nameof(TestNode1.Name), modifiedMembers.First().Member.Name);

            node.UpdateDetectorState();

            Assert.False(node.HasModified(out _));
        }


        [Fact(DisplayName = "Compare the differences between two objects - 1")]
        public void CompareTwoObjects1()
        {
            var node1 = new TestNode1();
            var node2 = new TestNode1(){Id = node1.Id, Children = node1.Children};

            node1.Name = "New Name";
            node2.Name = "New Name";

            Assert.False(ModifyDetector.HasDifference(node1,node2, out _));

            node2.Name = "New Name 2";

            Assert.True(ModifyDetector.HasDifference(node1, node2, out var differenceMembers));

            Assert.Single(differenceMembers);

            Assert.Equal(nameof(TestNode1.Name), differenceMembers.First().Member.Name);

            node2.Parent = new TestNode1();

            Assert.True(ModifyDetector.HasDifference(node1, node2, out differenceMembers));

            Assert.Equal(2, differenceMembers.Count());
            
            Assert.Equal(nameof(TestNode1.Name), differenceMembers.First().Member.Name);
            Assert.Equal(nameof(TestNode1.Parent), differenceMembers.Last().Member.Name);
            
            Assert.True(ModifyDetector.HasDifference(node1, new TestNode4(), out differenceMembers));

            Assert.Equal(5, differenceMembers.Count());
        }

        [Fact(DisplayName = "Compare the differences between two objects - 2")]
        public void CompareTwoObjects2()
        {
            var student1 = new Student()
            {
                Id = 1,
                Name = new FullName()
                {
                    FirstName = "Edward",
                    LastName = "Hsu"
                }
            };
            var student2 = new Student()
            {
                Id = 1,
                Name = new FullName()
                {
                    FirstName = "Bill",
                    LastName = "Chen"
                }
            };

            Assert.True(ModifyDetector.HasDifference(student1, student2, out var differenceMembers));

            Assert.Equal(1, differenceMembers.Count());
            Assert.Equal(nameof(Student.Name), differenceMembers.First().Member.Name);

            Assert.Equal(2, differenceMembers.First().Children.Count);
            Assert.Equal(nameof(FullName.FirstName), differenceMembers.First().Children.First().Member.Name);
            Assert.Equal(nameof(FullName.LastName), differenceMembers.First().Children.Last().Member.Name);
        }
    }
}