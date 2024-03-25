namespace 自定义特性的运用
{
    public abstract class AbstractValidateAttribute : Attribute
    {
        public abstract bool IsValid(object? obj, string pName, out string msg);
    }
}
