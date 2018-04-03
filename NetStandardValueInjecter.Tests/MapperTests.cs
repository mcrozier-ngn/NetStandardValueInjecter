using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Xciles.NetStandardValueInjecter.Tests
{
    public class MapperTests : IDisposable
    {
        public enum EFoo
        {
            Foo,
            Bar
        }
        public enum EBar
        {
            Foo,
            Bar
        }

        public class Foo
        {
            public string Name { get; set; }
            public int? Tint { get; set; }
            public int Xyz { get; set; }
            public EFoo SomeType { get; set; }
            public string Props { get; set; }
            public Foo Child { get; set; }
            public IEnumerable<Foo> Foos { get; set; }
            public IList<int> FooIds { get; set; }
        }

        public class Bar
        {
            public string Name { get; set; }
            public int Tint { get; set; }
            public EBar SomeType { get; set; }
            public string NoConvention { get; set; }
            public Bar Child { get; set; }
            public ICollection<Bar> Foos { get; set; }
            public IList<int> BarIds { get; set; }
        }

        private void End()
        {
            NetStandardValueInjecter.MapperFactory.ClearMappers();
        }

        [Fact]
        public void MapShouldMapToExistingObject()
        {
            var f2 = new Foo { Xyz = 43 };
            NetStandardValueInjecter.Mapper.Map(new { Name = "hi" }, f2);

            Assert.Equal("hi", f2.Name);
            Assert.Equal(43, f2.Xyz);
        }

        [Fact]
        public void MapShouldCreateNewMappedObject()
        {
            var foo = new Foo { Name = "f1", Props = "p", Xyz = 123 };
            var foo2 = NetStandardValueInjecter.Mapper.Map<Foo, Foo>(foo);

            Assert.Equal(foo.Name, foo2.Name);
            Assert.Equal(foo.Props, foo2.Props);
            Assert.Equal(foo.Xyz, foo2.Xyz);
        }

        [Fact]
        public void ShouldMapChildPropertiesFooToBar()
        {
            var foo = new Foo { Child = new Foo { Name = "aaa" } };
            var bar = new Bar();

            NetStandardValueInjecter.Mapper.Map(foo, bar);

            Assert.Equal("aaa", bar.Child.Name);
        }

        [Fact]
        public void MapShouldMapCollections()
        {
            var foos = new List<Foo>
                           {
                               new Foo { Name = "f1", SomeType = EFoo.Bar, FooIds = new List<int>() { 1,2,3,4 } },
                               new Foo { Name = "f2", SomeType = EFoo.Foo, FooIds = new List<int>() { 1,2,3,4 } },
                               new Foo { Name = "f3", SomeType = EFoo.Bar, FooIds = new List<int>() { 1,2,3,4 } },
                           };

            var foos2 = NetStandardValueInjecter.Mapper.Map<IEnumerable<Foo>, IList<Foo>>(foos);
            Assert.Equal(3, foos2.Count());
            Assert.Equal("f1", foos2.First().Name);
            Assert.Equal("f2", foos2.Skip(1).First().Name);
            Assert.Equal("f3", foos2.Last().Name);

            Assert.Equal(EFoo.Bar, foos2.First().SomeType);
            Assert.Equal(EFoo.Foo, foos2.Skip(1).First().SomeType);
            Assert.Equal(EFoo.Bar, foos2.Last().SomeType);

            for (var i = 0; i < foos2.Count(); i++)
            {
                for (int j = 0; j < foos2[i].FooIds.Count(); j++)
                {
                    Assert.Equal(foos2[i].FooIds[j], foos[i].FooIds[j]);
                }
            }
        }

        [Fact]
        public void MapShouldMapCollectionTypeProperties()
        {
            var foo = new Foo
            {
                Foos = new List<Foo>
                           {
                               new Foo{Name = "f1", SomeType = EFoo.Bar},
                               new Foo{Name = "f2", SomeType = EFoo.Foo},
                               new Foo{Name = "f3", SomeType = EFoo.Bar},
                           }
            };

            var bar = NetStandardValueInjecter.Mapper.Map<Foo, Bar>(foo);

            Assert.Equal(foo.Foos.Count(), bar.Foos.Count());
            Assert.Equal("f1", bar.Foos.First().Name);
            Assert.Equal("f2", bar.Foos.Skip(1).First().Name);
            Assert.Equal("f3", bar.Foos.Last().Name);

            Assert.Equal(EBar.Bar, bar.Foos.First().SomeType);
            Assert.Equal(EBar.Foo, bar.Foos.Skip(1).First().SomeType);
            Assert.Equal(EBar.Bar, bar.Foos.Last().SomeType);
        }

        [Fact]
        public void MapPlainLists()
        {
            var a = new List<int>() { 1, 2, 3, 4 };

            var b = NetStandardValueInjecter.Mapper.Map<IEnumerable<int>, IList<int>>(a);

            for (int j = 0; j < b.Count(); j++)
            {
                Assert.Equal(b[j], a[j]);
            }

            var a1 = new List<double>() { 1.0, 2.0, 3.0, 4.0 };

            var b1 = NetStandardValueInjecter.Mapper.Map<IEnumerable<double>, IList<double>>(a1);

            for (int j = 0; j < b1.Count(); j++)
            {
                Assert.Equal(b1[j], a1[j]);
            }

            var c = new List<string>() { "a", "b", "c", "d" };

            var d = NetStandardValueInjecter.Mapper.Map<IEnumerable<string>, IList<string>>(c);

            for (int j = 0; j < d.Count(); j++)
            {
                Assert.Equal(d[j], c[j]);
            }
        }



        public class FooBar : NetStandardValueInjecter.TypeMapper<Foo, Bar>
        {
            public override Bar Map(Foo source, Bar target)
            {
                base.Map(source, target);
                target.NoConvention = source.Name + source.Xyz + source.Props;
                return target;
            }
        }

        [Fact]
        public void MapShouldUseFooBarTypeMapperForMapping()
        {
            NetStandardValueInjecter.MapperFactory.AddMapper(new FooBar());
            var foo = new Foo { Name = "a", Props = "b", Xyz = 123 };
            var bar = new Bar();

            NetStandardValueInjecter.Mapper.Map(foo, bar);

            Assert.Equal("a123b", bar.NoConvention);
            Assert.Equal(foo.Name, bar.Name);
        }

        [Fact]
        public void MapShouldMapCollectionPropertiesAndUseFooBarTypeMapper()
        {
            NetStandardValueInjecter.MapperFactory.AddMapper(new FooBar());
            var foo = new Foo
            {
                Foos = new List<Foo>
                           {
                               new Foo{Name = "f1",Props = "v",Xyz = 19},
                               new Foo{Name = "f2",Props = "i",Xyz = 7},
                               new Foo{Name = "f3",Props = "v",Xyz = 3},
                           }
            };

            var bar = NetStandardValueInjecter.Mapper.Map<Foo, Bar>(foo);

            Assert.Equal(foo.Foos.Count(), bar.Foos.Count());

            var ffoos = foo.Foos.ToArray();
            var bfoos = bar.Foos.ToArray();

            for (var i = 0; i < ffoos.Count(); i++)
            {
                Assert.Equal(ffoos[i].Name, bfoos[i].Name);
                Assert.Equal(ffoos[i].Name + ffoos[i].Xyz + ffoos[i].Props, bfoos[i].NoConvention);
            }
        }

        public void Dispose()
        {
            End();
        }
    }
}
