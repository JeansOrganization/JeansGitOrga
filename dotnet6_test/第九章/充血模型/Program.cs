using 充血模型;

/* 贫血模型 */
//User user = new User("Jean");
//user.PasswordHash = HashHelper.Hash("123456");
//string? password = null;
//while (string.IsNullOrEmpty(password))
//{
//    password = Console.ReadLine();
//}
//bool isok = false;
//if (HashHelper.Hash(password) == user.PasswordHash)
//{
//    user.Credits += 5;
//    isok = true;
//}
//else
//{
//    if(user.Credits < 3)
//    {
//        Console.WriteLine("当前用户积分已不足以扣除！！");
//        return;
//    }
//    user.Credits -= 3;
//    isok = false;
//}
//Console.WriteLine($"账号登录{(isok ? "成功" : "失败")},当前账号信用分为{user.Credits}");
//Console.ReadKey();
/* 贫血模型 */

/* 充血模型 */
User user = new User("Jean");
user.ChangePassword("123456");
string? password = null;
while (string.IsNullOrEmpty(password))
{
    password = Console.ReadLine();
}
bool isok = false;
if (user.CheckPassword(password))
{
    user.AddCredits(5);
    isok = true;
}
else
{
    if (!user.DeductCredits(3)) return;
    isok = false;
}
Console.WriteLine($"账号登录{(isok ? "成功" : "失败")},当前账号信用分为{user.Credits}");
Console.ReadKey();
/* 充血模型 */






