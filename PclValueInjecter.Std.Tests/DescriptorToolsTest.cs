using System;
using System.Diagnostics;
using Xunit;

namespace Xciles.PclValueInjecter.Tests
{
    
    public class DescriptorToolsTest
    {
        public class Foo
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Name { get; set; }
        }

        [Fact]
        public void Aa()
        {
            var props = typeof(Foo).GetProps();
            var w = new Stopwatch();
            w.Start();
            for (int i = 0; i < 100; i++)
            {
                props.GetByName("Name", true);
            }

            w.Stop();
            Console.WriteLine(w.Elapsed);

            w.Reset();
            w.Start();
            for (int i = 0; i < 100; i++)
            {
                props.GetByName("Name");
            }

            w.Stop();
            Console.WriteLine(w.Elapsed);
        }

        [Fact]
        public void GetByNameIgnoresCase()
        {
            new Foo().GetProps().GetByName("name", true).IsNotNull();
            new Foo().GetProps().GetByName("name", false).IsEqualTo(null);
        }

        [Fact]
        public void GetByName()
        {
            new Foo().GetProps().GetByName("Name").IsNotNull();
            new Foo().GetProps().GetByName("unexistent").IsEqualTo(null);
        }

        [Fact]
        public void GetByNameType()
        {
            new Foo().GetProps().GetByNameType<string>("Name").IsNotNull();
            new Foo().GetProps().GetByNameType<int>("Name").IsEqualTo(null);
        }

    }
}