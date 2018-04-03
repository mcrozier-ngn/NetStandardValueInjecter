using System.Collections.Generic;
using Xunit;

namespace Xciles.PclValueInjecter.Tests
{
    
    public class TunnelierTests
    {
        public class Foo
        {
            public string Name { get; set; }
            public Foo Parent { get; set; }
        }

        [Fact]
        public void GetValueFromAUnfullBranchReturnNull()
        {
            var o = new Foo() { Parent = new Foo() { } };
            Tunnelier.GetValue(new List<string>() { "Parent", "Parent", "Name" }, o).IsEqualTo(null);
        }

        [Fact]
        public void GetValueReturns()
        {
            var o = new Foo { Parent = new Foo { Parent = new Foo { Name = "hey" } } };
            var endpoint = Tunnelier.GetValue(new List<string>{"Parent", "Parent","Name"},o );
            endpoint.Property.GetValue(endpoint.Component).IsEqualTo("hey");
        }

        [Fact]
        public void DiggDuggs()
        {
            var o = new Foo();
            Tunnelier.Digg(new List<string> {"Parent", "Parent", "Name"}, o).IsNotNull();
        }

    }
}