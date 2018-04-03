namespace Xciles.NetStandardValueInjecter
{
    public abstract class LoopValueInjectionBase : ValueInjection
    {
        protected virtual bool AllowSetValue(object value)
        {
            return true;
        }
    }
}