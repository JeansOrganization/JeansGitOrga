using System.Reflection;

namespace 自定义特性的运用
{
    public static class ValidataExtension
    {
        public static bool IsValid(this object obj,out string msg)
        {
            msg = "";
            Type type = obj.GetType();
            foreach (PropertyInfo p in type.GetProperties())
            {
                LengthAttribute? lengthAttribute = p.GetCustomAttribute<LengthAttribute>();
                if(lengthAttribute != null && !lengthAttribute.IsValid(p.GetValue(obj),p.Name,out msg))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
