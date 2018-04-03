using System;
using Xunit;

namespace Xciles.NetStandardValueInjecter.Tests
{
    public class Attr
    {
        [Fact]
        public void Test()
        {
            var attr = typeof(Aaaa).GetCustomAttributes(true);
            attr.Length.IsEqualTo(1);
        }

        public class Foo
        {
            public Aaaa Aa { get; set; }
        }

        public class Oo : Attribute
        {}

        [Oo]
        public class Aaaa {}

    }
    public class TrimTests
    {
        [Fact]
        public void TrimFromAnonymousToFoo()
        {
            var a = new {Aa = " hello ", Bb = "  yo "};
            var foo = new Foo();
            foo.InjectFrom<TrimStrings>(a);
            Assert.Equal("hello", foo.Aa);
            Assert.Equal("yo", foo.Bb);
        }

        [Fact]
        public void TrimFooToFoo()
        {
            var foo = new Foo {Aa = " a ", Bb = " x "};
            foo.InjectFrom<TrimStrings>(foo);
            Assert.Equal("a", foo.Aa);
            Assert.Equal("x", foo.Bb);
        }

        [Fact]
        public void SelfTrim()
        {
            var foo = new Foo {Aa = " a ", Bb = " b "};
            foo.InjectFrom<TrimSelf>();
            Assert.Equal("a", foo.Aa);
            Assert.Equal("b", foo.Bb);
        }

        public class TrimStrings : LoopValueInjection<string,string>
        {
            protected override string SetValue(string sourcePropertyValue)
            {
                return sourcePropertyValue.Trim();
            }
        }

        public class TrimSelf : NoSourceValueInjection
        {
            protected override void Inject(object target)
            {
                var props = target.GetProps();
                for (var i = 0; i < props.Count; i++)
                {
                    if (props[i].PropertyType != typeof(string)) continue;
                    var value = props[i].GetValue(target);

                    if(value != null) props[i].SetValue(target, value.ToString().Trim());
                }
            }
        }

        public class Foo
        {
            public string Aa { get; set; }
            public string Bb { get; set; }
        }
    }
}