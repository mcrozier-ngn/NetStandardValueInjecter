using System.ComponentModel;
using System.Diagnostics;
using Xunit;

namespace Xciles.PclValueInjecter.Tests
{
    
    public class PropertyInfosStorageTests
    {
        public class Foo
        {
            public string Name { get; set; }
        }

        [Fact]
        public void GetsPropertiesFromType()
        {
            PropertyInfosStorage.GetProps(typeof(Foo)).IsNotNull();
        }

        [Fact]
        public void GetsPropertiesFromInstance()
        {
            PropertyInfosStorage.GetProps(new Foo()).IsNotNull();
        }

        [Fact]
        public void SpeedTestTypeDescriptor()
        {
            TypeDescriptor.GetProperties(typeof(Bar));
            TypeDescriptor.GetProperties(typeof(FooBar));
            TypeDescriptor.GetProperties(typeof(Address));
            TypeDescriptor.GetProperties(typeof(Person));
            TypeDescriptor.GetProperties(typeof(PersonViewModel));
            var w = new Stopwatch();

            w.Start();
            for (int i = 0; i < 10000; i++)
            {
                TypeDescriptor.GetProperties(typeof(Foo));
            }
            w.Stop();
            System.Console.Out.WriteLine(w.Elapsed);
        }

        [Fact]
        public void SpeedTestPropertyInfosStorage()
        {
            PropertyInfosStorage.GetProps(typeof (Bar));
            PropertyInfosStorage.GetProps(typeof (FooBar));
            PropertyInfosStorage.GetProps(typeof (Address));
            PropertyInfosStorage.GetProps(typeof (Person));
            PropertyInfosStorage.GetProps(typeof (PersonViewModel));
            var w = new Stopwatch();
            w.Reset();
            w.Start();
            for (int i = 0; i < 10000; i++)
            {
                PropertyInfosStorage.GetProps(typeof(Foo));
            }
            w.Stop();
            System.Console.Out.WriteLine(w.Elapsed);
        }
    }
}