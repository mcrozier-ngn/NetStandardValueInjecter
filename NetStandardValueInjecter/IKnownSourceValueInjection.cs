namespace Xciles.NetStandardValueInjecter
{
    public interface IValueInjection
    {
        object Map(object source, object target);
    }
}