using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 充血模型
{
    public class User
    {
        public string UserName { get; init; }
        public int Credits { get; set; }
        public string PasswordHash;
        public User(string username,int credits = 10)
        {
            this.UserName = username;
            this.Credits = credits;
        }

        public void ChangePassword(string value)
        {
            string hashValue = HashHelper.Hash(value);
            this.PasswordHash = hashValue;
        }

        public bool CheckPassword(string value)
        {
            string hashValue = HashHelper.Hash(value);
            return this.PasswordHash == hashValue;
        }

        public bool DeductCredits(int credits)
        {
            if(this.Credits < 3)
            {
                Console.WriteLine("当前用户积分已不足以扣除！！");
                return false;
            }
            this.Credits -= credits;
            return true;
        }

        public void AddCredits(int credits)
        {
            this.Credits += credits;
        }
    }
}
