//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Web.Mvc;
//using Xunit;

//namespace Xciles.PclValueInjecter.Tests
//{
//    
//    public class UberTest
//    {
//        [TestFixtureSetUp]
//        public void Start()
//        {
//            //speed tweak
//            //it is going to make the TypeDescriptor (which is used by the ValueInjecter) work a bit faster
//            //but prevents some implicit conversions while unboxing (if you don't have this kind of valueinjections than you can use it)
//            PropertyInfosStorage.RegisterActionForEachType(HyperTypeDescriptionProvider.Add);
//        }

//        [Fact]
//        public void SameNameTypeTest()
//        {
//            var foo = new Foo { FirstName = "F1", LastName = "L1", NotImportant = 123 };
//            var bar = new Bar { Date = DateTime.Now, StreetNo = 32, Whatever2 = 654 };
//            var fooBar = new FooBar { Something = "something" };

//            //interface for Mocking 
//            IValueInjecter injecter = new ValueInjecter();

//            //Act
//            injecter.Inject(fooBar,foo,bar);

//            Assert.Equal(fooBar.FirstName, foo.FirstName);
//            Assert.Equal(fooBar.LastName, foo.LastName);
//            Assert.Equal(fooBar.Date, bar.Date);
//            Assert.Equal(fooBar.StreetNo, bar.StreetNo);
//            Assert.Equal(fooBar.Something, "something");
//        }

//        [Fact]
//        public void CountryToIntTest()
//        {
//            var address = new Address
//                              {
//                                  Country1 = new Country { Id = 7, Name = "Country7" },
//                                  Country2 = new Country { Id = 9, Name = "Country9" },
//                                  I1 = 1,
//                                  S1 = "s"
//                              };
//            var avm = new AddressViewModel();
//            IValueInjecter injecter = new ValueInjecter();

//            //Act
//            injecter.Inject(avm, address);
//            injecter.Inject<CountryToInt>(avm, address);

//            Assert.Equal(avm.Country1, 7);
//            Assert.Equal(avm.Country2, 9);
//            Assert.Equal(avm.S1, "s");
//        }

//        [Fact]
//        public void IntToCountryTest()
//        {
//            var avm = new AddressViewModel { Country1 = 7, Country2 = 9, S1 = "s" };
//            var address = new Address();

//            //using static value injecter here
//            //Act
//            address.InjectFrom(avm)
//                   .InjectFrom<IntToCountry>(avm);

//            Assert.Equal(address.Country1.Id, 7);
//            Assert.Equal(address.Country2.Id, 9);
//            Assert.Equal(address.Country1.Name, "Country7");
//            Assert.Equal(address.Country2.Name, "Country9");
//            Assert.Equal(address.S1, "s");
//        }

//        [Fact]
//        public void LookupToCountryTest()
//        {
//            var personViewModel = new PersonViewModel { Id = 1, Name = "Jimmy", Country = new[] { "3" }, Country2 = new[] { "9" } };
//            var person = new Person();

//            //Act
//            person.InjectFrom(personViewModel)
//                  .InjectFrom<LookupToCountry>(personViewModel);

//            Assert.Equal(person.Id, personViewModel.Id);
//            Assert.Equal(person.Name, personViewModel.Name);
//            Assert.Equal(person.Country.Id, 3);
//            Assert.Equal(person.Country2.Id, 9);
//        }

//        [Fact]
//        public void CountryToLookupTest()
//        {
//            var person = new Person
//            {
//                Id = 1,
//                Name = "Jimmy",
//                Country = new Country { Id = 3, Name = "Country3" },
//                Country2 = new Country { Id = 9, Name = "Country9" },

//            };
//            var personViewModel = new PersonViewModel();

//            //Act
//            personViewModel.InjectFrom(person)
//                           .InjectFrom<CountryToLookup>(person);

//            Assert.Equal(personViewModel.Id, personViewModel.Id);
//            Assert.Equal(personViewModel.Name, personViewModel.Name);

//            var selectCountry = (personViewModel.Country as IEnumerable<SelectListItem>).Where(o => o.Selected).Single();
//            var selectCountry2 = (personViewModel.Country2 as IEnumerable<SelectListItem>).Where(o => o.Selected).Single();

//            Assert.Equal(person.Country.Id, Convert.ToInt32(selectCountry.Value));
//            Assert.Equal(person.Country.Name, selectCountry.Text);
//            Assert.Equal(person.Country2.Id, Convert.ToInt32(selectCountry2.Value));
//            Assert.Equal(person.Country2.Name, selectCountry2.Text);
//        }

//        [Fact]
//        public void SpeedTestAgainstAutomapper()
//        {
//            var watch = new Stopwatch();
//            watch.Start();
//            for (var i = 0; i < 10000; i++)
//            {
//                new Foo().InjectFrom(new Foo { FirstName = "fname", LastName = "lname" });
//            }
//            watch.Stop();
//            Console.Out.WriteLine("ValueInjecter: {0} ", watch.Elapsed);

//            AutoMapper.Mapper.CreateMap<Foo, Foo>();
//            var awatch = new Stopwatch();
//            awatch.Start();
//            for (var i = 0; i < 10000; i++)
//            {
//                Mapper.Map(new Foo { FirstName = "fname", LastName = "lname" }, new Foo());
//            }
//            awatch.Stop();
//            Console.Out.WriteLine("Automapper: {0} ", awatch.Elapsed);

//        }

//        public class CountryToInt : LoopValueInjection<Country, int>
//        {
//            protected override int SetValue(Country sourcePropertyValue)
//            {
//                return sourcePropertyValue != null ? sourcePropertyValue.Id : 0;
//            }
//        }

//        public class IntToCountry : LoopValueInjection<int, Country>
//        {
//            protected override Country SetValue(int sourcePropertyValue)
//            {
//                return IoC.Resolve<ICountryRepository>().Get(sourcePropertyValue);
//            }
//        }

//        public class CountryToLookup : LoopValueInjection<Country, object>
//        {
//            protected override object SetValue(Country sourcePropertyValue)
//            {
//                var key = sourcePropertyValue != null ? sourcePropertyValue.Id : 0;

//                return IoC.Resolve<ICountryRepository>().GetAll()
//                       .Select(o => new SelectListItem
//                       {
//                           Text = o.Name,
//                           Value = o.Id.ToString(),
//                           Selected = o.Id == key
//                       });
//            }
//        }

//        public class LookupToCountry : LoopValueInjection<object, Country>
//        {
//            protected override Country SetValue(object sourcePropertyValue)
//            {
//                var key = Convert.ToInt32((((string[])sourcePropertyValue)[0]));
//                return IoC.Resolve<ICountryRepository>().Get(key);
//            }
//        }
//    }
//}
