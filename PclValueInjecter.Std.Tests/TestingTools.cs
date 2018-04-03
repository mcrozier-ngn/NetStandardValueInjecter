using Xunit;

namespace Xciles.PclValueInjecter.Tests
{
    public static class TestingTools
    {
        public static void IsEqualTo(this object o, object to)
        {
            Assert.Equal(to, o);
        }

        public static void IsNotNull(this object o)
        {
            Assert.NotNull(o);
        }

    }
}