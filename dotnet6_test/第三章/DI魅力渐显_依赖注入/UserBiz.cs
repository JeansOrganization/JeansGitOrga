
using System.Reflection;
using System.Text;

public class UserBiz : IUserBiz
{
    public readonly IUserDao userDao;
    public UserBiz()
    {
        
    }
    public UserBiz(IUserDao userDao)
    {
        this.userDao = userDao;
    }
    public bool CheckLogin(string username,string password,out string msg)
    {
        msg = "密码错误！！";
        bool isOk = false;
        User user = userDao.GetUser(username);
        if (user == null) return isOk;
        StringBuilder stringBuilder = new StringBuilder();
        foreach (PropertyInfo p in user.GetType().GetProperties())
        {
            stringBuilder.Append(p.Name + ":" + p.GetValue(user) + ",");
        }
        Console.WriteLine(stringBuilder.ToString().TrimEnd(','));
        if (password == user?.passWord)
        {
            msg = "密码正确！！";
            isOk = true;
        }
        return isOk;
    }
}