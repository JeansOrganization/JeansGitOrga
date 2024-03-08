
public interface IUserBiz
{
    public bool CheckLogin(string username, string password, out string msg);
}