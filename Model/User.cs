using System;
using System.Collections.Generic;

namespace Model
{
    public class User
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string pwd { get; set; }

        // public List<Role> roles = new List<Role>();  // 不能直接赋值，api 传递获取不到值，不会将数据传给前台
        public List<Role> roles { get; set; }

        public UserInfo userInfo { get; set; }
    }
}
