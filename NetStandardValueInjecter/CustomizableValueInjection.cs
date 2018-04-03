using System;

namespace Xciles.NetStandardValueInjecter
{
    public abstract class CustomizableValueInjection : PrefixedValueInjection
    {
        protected virtual bool TypesMatch(Type sourceType, Type targetType)
        {
            return targetType == sourceType;
        }

        protected virtual object SetValue(object v)
        {
            return v;
        }
    }
}