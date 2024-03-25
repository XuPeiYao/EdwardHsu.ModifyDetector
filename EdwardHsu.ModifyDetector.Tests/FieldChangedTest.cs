using EdwardHsu.ModifyDetector.Tests.Models;

namespace EdwardHsu.ModifyDetector.Tests
{
    public class FieldChangedTest
    {
        [Fact(DisplayName = "Field Changed - 1")]
        public void FieldChanged1()
        {
            var node = new TestNode2();
            
            node.Name = "New Name";
            Assert.True(node.HasModified(out var modifiedMembers));

            Assert.Single(modifiedMembers);
            Assert.Equal(nameof(TestNode2.Name), modifiedMembers.First().Member.Name);
        }

        [Fact(DisplayName = "Field Changed - 2")]
        public void FieldChanged2()
        {
            var node = new TestNode2();

            node.Id = Guid.NewGuid();
            node.Name = "New Name";

            var changedProperties = new List<string> { nameof(TestNode2.Id), nameof(TestNode2.Name)};

            Assert.True(node.HasModified(out var modifiedMembers));
            
            Assert.Equal(2, modifiedMembers.Count());


            for (int i = 0; i < 2; i++)
            {
                Assert.Equal(changedProperties[i], modifiedMembers.ElementAt(i).Member.Name);
            }
        }

        [Fact(DisplayName = "Field Changed - 3")]
        public void FieldChanged3()
        {
            var node = new TestNode2();

            node.Name = "New Name";
            node.Description = "New Name";
            Assert.True(node.HasModified(out var modifiedMembers));

            Assert.Equal(2, modifiedMembers.Count());
            Assert.Equal(nameof(TestNode2.Name), modifiedMembers.First().Member.Name);
        }


        [Fact(DisplayName = "Field Changed with cycle reference - 1")]
        public void FieldChangedWithCycleReference1()
        {
            var node = new TestNode2();

            node.Id = Guid.NewGuid();
            node.Name = "New Name";
            node.Parent = node;

            var changedProperties = new List<string> { nameof(TestNode2.Id), nameof(TestNode2.Name), nameof(TestNode2.Parent) };

            Assert.True(node.HasModified(out var modifiedMembers));

            Assert.Equal(3, modifiedMembers.Count());


            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(changedProperties[i], modifiedMembers.ElementAt(i).Member.Name);
            }
        }

        [Fact(DisplayName = "Field Changed with cycle reference - 2")]
        public void FieldChangedWithCycleReference2()
        {
            var node = new TestNode2();

            node.Id = Guid.NewGuid();
            node.Name = "New Name";
            node.Children = new List<TestNode2>()
            {
                new TestNode2()
                {
                    Children = new List<TestNode2>()
                    {
                        node
                    }
                },
                node
            };

            var changedProperties = new List<string> { nameof(TestNode2.Id), nameof(TestNode2.Name), nameof(TestNode2.Children) };

            Assert.True(node.HasModified(out var modifiedMembers));

            Assert.Equal(3, modifiedMembers.Count());


            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(changedProperties[i], modifiedMembers.ElementAt(i).Member.Name);
            }
        }

        [Fact(DisplayName = "Field Changed with cycle reference - 3")]
        public void FieldChangedWithCycleReference3()
        {
            var node = new TestNode2();

            node.Id = Guid.NewGuid();
            node.Name = "New Name";
            node.Children = new List<TestNode2>()
            {
                new TestNode2()
                {
                    Children = new List<TestNode2>()
                    {
                        node
                    }
                }
            };

            var changedProperties = new List<string> { nameof(TestNode2.Id), nameof(TestNode2.Name), nameof(TestNode2.Children) };

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

            Assert.Equal(nameof(TestNode2.Children), modifiedMembers.First().Member.Name);

            Assert.Equal(nameof(node.Name),modifiedMembers.First().Children.First().Children.First().Member.Name);

        }

        [Fact(DisplayName = "Field Changed then rollback")]
        public void FieldChangedThenRollback()
        {
            var node = new TestNode2();

            var origionName = node.Name;
            node.Name = "New Name";
            Assert.True(node.HasModified(out var modifiedMembers));
            Assert.Single(modifiedMembers);
            Assert.Equal(nameof(TestNode2.Name), modifiedMembers.First().Member.Name);

            node.Name = origionName;

            Assert.False(node.HasModified(out _));
        }

        [Fact(DisplayName = "Field Changed then UpdateDetectorState")]
        public void FieldChangedThenRollbackWithChildren()
        {
            var node = new TestNode2();
            
            node.Name = "New Name";
            Assert.True(node.HasModified(out var modifiedMembers));
            Assert.Single(modifiedMembers);
            Assert.Equal(nameof(TestNode2.Name), modifiedMembers.First().Member.Name);

            node.UpdateDetectorState();

            Assert.False(node.HasModified(out _));
        }
    }
}