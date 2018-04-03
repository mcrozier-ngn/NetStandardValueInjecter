using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Xciles.PclValueInjecter.Tests
{
    
    public class CollectionsTest
    {
        [Fact]
        public void Test()
        {
            var p = new Person
                        {
                            Name = "a",
                            Age = 3,
                            Children = new[] 
                                           { 
                                               new Person { Age = 1 }, 
                                               new Person { Age = 2 }, 
                                               new Person { Age = 3 } 
                                           }
                        };

            var pwm = new PersonViewModel();

            pwm.InjectFrom(p);
            pwm.Children = p.Children.Select(c => new PersonViewModel().InjectFrom(c)).Cast<PersonViewModel>();

            pwm.Name.IsEqualTo(p.Name);
            pwm.Age.IsEqualTo(p.Age);
            pwm.Children.Count().IsEqualTo(3);
            pwm.Children.ToArray()[0].Age.IsEqualTo(1);
        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public IEnumerable<Person> Children { get; set; }
        }

        public class PersonViewModel
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public IEnumerable<PersonViewModel> Children { get; set; }
        }
    }
}