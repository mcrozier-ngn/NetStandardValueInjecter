using System;

namespace Xciles.NetStandardValueInjecter.CustomInjections
{
    public class NormalToNullables : ConventionInjection
    {
        protected override bool Match(ConventionInfo c)
        {
            return c.SourceProp.Name == c.TargetProp.Name &&
                   c.SourceProp.Type == Nullable.GetUnderlyingType(c.TargetProp.Type);
        }
    }
}