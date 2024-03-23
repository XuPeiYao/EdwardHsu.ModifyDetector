using EdwardHsu.ModifyDetector.Tests.Models;

namespace EdwardHsu.ModifyDetector.Tests
{
    public class PropertyChangedTest
    {
        [Fact(DisplayName = "Property Changed - 1")]
        public void PropertyChanged1()
        {
            var node = new Node();
            
            node.Name = "New Name";
            Assert.True(node.HasModified(out var modifiedMembers));

            Assert.Single(modifiedMembers);
            Assert.Equal(nameof(Node.Name), modifiedMembers.First().Member.Name);
        }

        [Fact(DisplayName = "Property Changed - 2")]
        public void PropertyChanged2()
        {
            var node = new Node();

            node.Id = Guid.NewGuid();
            node.Name = "New Name";

            var changedProperties = new List<string> { nameof(Node.Id), nameof(Node.Name)};

            Assert.True(node.HasModified(out var modifiedMembers));
            
            Assert.Equal(2, modifiedMembers.Count());


            for (int i = 0; i < 2; i++)
            {
                Assert.Equal(changedProperties[i], modifiedMembers.ElementAt(i).Member.Name);
            }
        }

        [Fact(DisplayName = "Property Changed with cycle reference - 1")]
        public void PropertyChangedWithCycleReference1()
        {
            var node = new Node();

            node.Id = Guid.NewGuid();
            node.Name = "New Name";
            node.Parent = node;

            var changedProperties = new List<string> { nameof(Node.Id), nameof(Node.Name), nameof(Node.Parent) };

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
            var node = new Node();

            node.Id = Guid.NewGuid();
            node.Name = "New Name";
            node.Children = new List<Node>()
            {
                new Node()
                {
                    Children = new List<Node>()
                    {
                        node
                    }
                },
                node
            };

            var changedProperties = new List<string> { nameof(Node.Id), nameof(Node.Name), nameof(Node.Children) };

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
            var node = new Node();

            node.Id = Guid.NewGuid();
            node.Name = "New Name";
            node.Children = new List<Node>()
            {
                new Node()
                {
                    Children = new List<Node>()
                    {
                        node
                    }
                }
            };

            var changedProperties = new List<string> { nameof(Node.Id), nameof(Node.Name), nameof(Node.Children) };

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

            Assert.Equal(nameof(Node.Children), modifiedMembers.First().Member.Name);

            Assert.Equal(nameof(node.Name),modifiedMembers.First().Children.First().Children.First().Member.Name);

        }

        [Fact(DisplayName = "Property Changed then rollback")]
        public void PropertyChangedThenRollback()
        {
            var node = new Node();

            var origionName = node.Name;
            node.Name = "New Name";
            Assert.True(node.HasModified(out var modifiedMembers));
            Assert.Single(modifiedMembers);
            Assert.Equal(nameof(Node.Name), modifiedMembers.First().Member.Name);

            node.Name = origionName;

            Assert.False(node.HasModified(out _));
        }

        [Fact(DisplayName = "Property Changed then UpdateDetectorState")]
        public void PropertyChangedThenRollbackWithChildren()
        {
            var node = new Node();
            
            node.Name = "New Name";
            Assert.True(node.HasModified(out var modifiedMembers));
            Assert.Single(modifiedMembers);
            Assert.Equal(nameof(Node.Name), modifiedMembers.First().Member.Name);

            node.UpdateDetectorState();

            Assert.False(node.HasModified(out _));
        }
    }
}