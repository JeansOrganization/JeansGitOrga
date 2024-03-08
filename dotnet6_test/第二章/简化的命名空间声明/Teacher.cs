
using System.Reflection;
using System.Text;

namespace TMS.Admin;

class Teacher
{
    public string name { get; set; }
    public int age { get; set; }

    public Teacher(string name,int age)
    {
        this.name = name;
        this.age = age;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Teacher");
        sb.Append("{");
        Type type = typeof(Teacher);
        foreach(PropertyInfo p in type.GetProperties())
        {
            sb.Append(p.Name + ":" + p.GetValue(this) + ",");
        }
        sb.Append("}");
        return sb.ToString();
    }
}
