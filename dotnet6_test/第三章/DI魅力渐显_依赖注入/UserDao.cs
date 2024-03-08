
using MySql.Data.MySqlClient;
using System.Data;

public class UserDao : IUserDao
{

    public readonly IDbConnection conn;

    public UserDao(IDbConnection conn)
    {
        this.conn = conn;
    }

    public User GetUser(string username)
    {
        List<User> list = SqlHelper.QueryData<User>(conn, $"select * from fa_user a where a.username = {username}");
        return list.FirstOrDefault();
    }
}