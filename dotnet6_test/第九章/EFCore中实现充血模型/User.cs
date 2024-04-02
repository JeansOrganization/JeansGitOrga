﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore中实现充血模型
{
    public class User
    {
        public int Id { get; set; }//特征一
        public DateTime CreatedDateTime { get; init; }//特征一
        public string UserName { get; private set; }//特征一
        public int Credit { get; private set; }
        private string? passwordHash;//特征三
        private string? remark;
        public string? Remark //特征四
        {
            get { return remark; }
        }
        public string? Tag { get; set; }//特征五
        private User()//特征二
        {
        }
        public User(string userName)//特征二
        {
            this.UserName = userName;
            this.CreatedDateTime = DateTime.Now;
            this.Credit = 10;
        }
        public void ChangeUserName(string newValue)
        {
            this.UserName = newValue;
        }
        public void ChangePassword(string newValue)
        {
            if (newValue.Length < 6)
            {
                throw new ArgumentException("密码太短");
            }
            this.passwordHash = HashHelper.Hash(newValue);
        }
    }

}
