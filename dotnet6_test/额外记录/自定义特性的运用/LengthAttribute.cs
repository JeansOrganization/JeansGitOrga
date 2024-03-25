namespace 自定义特性的运用
{
    public class LengthAttribute : AbstractValidateAttribute
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public LengthAttribute(int max)
        {
            Min = int.MinValue;
            Max = max;
        }
        
        public LengthAttribute(int min,int max)
        {
            Min = min;
            Max = max;
        }
        public override bool IsValid(object? obj,string pName,out string msg)
        {
            msg = "";
            if(obj == null)
            {
                msg = pName + " 数据为空";
                return false;
            }
            if(!int.TryParse(obj.ToString(), out int num))
            {
                msg = pName + " 非整型数据";
                return false;
            }
            if (num.ToString().Length < Min || num.ToString().Length > Max)
            {
                msg = pName + " 不在长度限制范围内";
                return false;
            }
            return true;
        }
    }
}
