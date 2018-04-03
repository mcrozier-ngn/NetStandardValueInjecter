using System;
using Xunit;

namespace Xciles.NetStandardValueInjecter.Tests
{
    
    public class EnumInjectionsTests
    {
        [Fact]
        public void Test()
        {
            var e = new Entity
            {
                Color = Colors.Blue,
                Color2 = Colors.Green,
                Mood = Moods.VeryHappy,
                Mood2 = Moods.Great,
            };
            var dto = new Dto();
            dto.InjectFrom<EnumToInt>(e);

            Assert.Equal(2, dto.Color);
            Assert.Equal(1, dto.Color2);
            Assert.Equal(2, dto.Mood);
            Assert.Equal(3, dto.Mood2);


            var e2 = new Entity();
            e2.InjectFrom<IntToEnum>(dto);
            Assert.Equal(dto.Color, 2);
            Assert.Equal(dto.Color2, 1);
            Assert.Equal(dto.Mood, 2);
            Assert.Equal(dto.Mood2, 3);
        }

        public enum Colors
        {
            Red, Green, Blue
        }

        public enum Moods
        {
            Happy, Awesome, VeryHappy, Great
        }

        public class Entity
        {
            public Colors Color { get; set; }
            public Colors Color2 { get; set; }
            public Moods Mood { get; set; }
            public Moods Mood2 { get; set; }
        }

        public class Dto
        {
            public int Color { get; set; }
            public int Color2 { get; set; }
            public int Mood { get; set; }
            public int Mood2 { get; set; }
        }

        public class EnumToInt : ConventionInjection
        {
            protected override bool Match(ConventionInfo c)
            {
                return c.SourceProp.Name == c.TargetProp.Name &&
                    c.SourceProp.Type.IsSubclassOf(typeof (Enum)) && c.TargetProp.Type == typeof (int);
            }
        }      
     
        public class IntToEnum : ConventionInjection
        {
            protected override bool Match(ConventionInfo c)
            {
                return c.SourceProp.Name == c.TargetProp.Name &&
                    c.SourceProp.Type == typeof (int) && c.TargetProp.Type.IsSubclassOf(typeof (Enum));
            }
        }
    }
}
