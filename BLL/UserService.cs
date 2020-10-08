using Model;
using System;
using System.Collections.Generic;

namespace BLL
{
    public interface UserService
    {
        #region 测试Dapper 单表
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        List<User> getUsersByUsername(string username);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool updateUser(User user);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool addUser(User user);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool deleteUserById(int id);
        #endregion

        #region Dapper 多表
        List<User> getUsersFromTables();

        List<User> oneToOne();

        IEnumerable<User> oneToMany();

        User oneToOneAndMany();
        #endregion

    }
}
