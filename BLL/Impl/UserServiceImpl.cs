using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace BLL.Impl
{
    public class UserServiceImpl : UserService
    {
        private readonly DapperConn.Dao.UserDao _userDao1;
        
        public UserServiceImpl(DapperConn.Dao.UserDao userDao1)
        {
            _userDao1 = userDao1;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool addUser(User user)
        {
            return _userDao1.InsertData(user);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool deleteUserById(int id)
        {
            return _userDao1.DeleteData(id);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<User> getUsersByUsername(string username)
        {
            return _userDao1.FindListByLastName(username);
        }

        public List<User> getUsersFromTables()
        {
            return _userDao1.getSome();
        }

        public List<User> oneToOne()
        {
            return _userDao1.OneToMany1();
        }

        public IEnumerable<User> oneToMany()
        {
            return _userDao1.OneToMany2();
        }

        public User oneToOneAndMany()
        {
            return _userDao1.oneToOneAndMany();
        }

        public bool updateUser(User user)
        {
            return _userDao1.UpdateData(user);
        }

    }
}
