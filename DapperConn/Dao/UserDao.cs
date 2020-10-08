using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DapperConn.Dao
{
    public class UserDao
    {
        #region 多表查询
        /// <summary>
        /// 多表关联查询返回单类
        /// </summary>
        public List<User> getSome()
        {
            using (IDbConnection db = new SqlConnection(DBHelper.ConnStrings))
            {
                var sql = @"select * from t_user u
                            left join t_user_info ui on ui.UserNo = u.id";

                // 执行查询：多表Query( 类型一，类型二，返回值)
                var list = db.Query<User, UserInfo, User>(
                    sql,
                    (users, infos) => { return users; },     // 变量 students 对应的 StuInfo 类型 scores 对应的 StuMark 类型
                    null,   // 存储过程的参数无
                    null,   // 事务无
                    true,
                    splitOn: "id"    // 该参数是用来划分查询中的字段是属于哪个类的 splitOn 可省略, 从最后字段开始匹配
                ).ToList();

                return list;
                /* 
                SplitOn: stuNo 划分查询中的字段是属于哪个表的 也就是查询结构映射到哪个实体，上边的sql 运行时：会从查询结果所有
                字段列表的最后一个字段进行匹配，一直找到stuNo 这个字段（大小写不计），找到的第一个 stuNo 字段匹配 StuInfo 类的 stuNo 属性
                那么从 stuNo 到最后一个字段都属于 StuInfo, StuNo 以前的字段都映射到StuMark 表
                通过(T, P)=> (return T) 把两个类的实例解析出来 */

            }
        }

        /// 多表查询操作一 OneToOne
        public List<User> OneToMany1()
        {
            List<User> userlist = new List<User>();
            using (IDbConnection db = new SqlConnection(DBHelper.ConnStrings))
            {
                // 注意返回值顺序 -- splitNo -- 分割点
                string sql = @"select u.*, ui.UserNo, ui.Email, ui.id, ui.Name, ui.Phone, ui.Sex from t_user u
                                left join t_user_info ui on ui.UserNo = u.id";
                userlist = db.Query<User, UserInfo, User>(  // 第三个参数是返回值类型
                    sql,
                    (user, info) => { user.userInfo = info; return user; },
                    null,
                    null,
                    true,
                    "UserNo"
                ).ToList();
            }

            return userlist;
        }

        /// 多表查询操作二 OneToMany
        public IEnumerable<User> OneToMany2()
        {
            List<User> userList = new List<User>();
            using (IDbConnection db = new SqlConnection(DBHelper.ConnStrings))
            {
                string sql = @"select u.*, ur.UserId, ur.RoleId, r.* from  t_user u
                                left join user_role ur on u.id = ur.UserId
                                left join t_role r on r.id = ur.RoleId
                                where u.id =1";
                var lookup = new Dictionary<int, User>();
                userList = db.Query<User, Role, User>(
                    sql,
                    (user, role) =>
                    {
                        User u;
                        // 如果根据id 获取不到
                        if (!lookup.TryGetValue(user.id, out u))    // TryGetValue 获取与指定的键相关联的值User
                        {
                            lookup.Add(user.id, u = user);
                        }
                        if (u.roles == null)
                        {
                            u.roles = new List<Role>();
                        }
                        u.roles.Add(role);
                        //user.roles.Add(role);
                        return user;
                    }, null, null, true, "RoleId", null, null   // 后面的两个 null 可选
                ).ToList();
                // 获取集合中的值
                IEnumerable<User> result = lookup.Values; // 值的集合
                //List<User> users = result.ToList(); //获取值成功
                return result;
            }
        }

        /// 多表查询三 oneToManyAndMany
        public User oneToOneAndMany()
        {
            using (IDbConnection conn = new SqlConnection(DBHelper.ConnStrings))
            {
                string sql = @"select u.*, r.*, u.*, ui.* from  t_user u
                            left join user_role ur on u.id = ur.UserId
                            left join t_role r on r.id = ur.RoleId
                            left join t_user_info ui on ui.UserNo = u.id
                            where u.id = 1";
                User lookup = null;
                conn.Query<User, Role, UserInfo, User>(sql, (u, r, ui) =>
                {
                    if (lookup == null)
                    {
                        lookup = u;
                    }
                    if(lookup.roles == null)
                    {
                        lookup.roles = new List<Role>();
                    }
                    Role tmp = lookup.roles.Find(f => f.id == r.id);
                    if (tmp == null)
                    {
                        tmp = r;
                        lookup.roles.Add(tmp);
                    }
                    lookup.userInfo = ui;
                    return u;
                },
                null,
                splitOn: "id");
                return lookup;
            }

        }

        #endregion

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteData(int id)
        {
            // 下面是根据id 执行删除
            using (IDbConnection db = new SqlConnection(DBHelper.ConnStrings))
            {
                // 准备 sql 语句
                string sql = "delete from t_user where id = @ID";

                // 执行删除
                int result = db.Execute(sql, new { ID = id });  // 匿名对象

                return result > 0;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateData(User user)
        {
            using (IDbConnection db = new SqlConnection(DBHelper.ConnStrings))
            {
                // 准备更新语句
                string sql = "update t_user set UserName = @userName, Pwd = @pwd where id = @id";

                // 执行更新语句
                int result = db.Execute(sql, user);

                return result > 0;
            }
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public bool InsertData(User user)
        {
            using (IDbConnection db = new SqlConnection(DBHelper.ConnStrings))
            {
                // 准备插入的SQL 语句
                string sql = "insert into t_user (UserName, Pwd) values" +
                    "(@userName, @pwd)";

                // 调用Dapper 中的 IDbConnection 扩展方法Excute() 来执行插入操作
                int result = db.Execute(sql, user);   // 第一个参数 SQL 语句 第二个参数Person 对象 

                // 简化条件判断返回
                return result > 0;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<User> FindListByLastName(string username)
        {
            using (IDbConnection db = new SqlConnection(DBHelper.ConnStrings))
            {

                // 1. string sql = "select * from Person where LastName = 'xxx'";

                // 2. 字符串拼接
                // string sql = "select * from Person where Lastname = '" + lastname + "'";

                // 3. Format 格式化
                // string sql = string.Format("select * from Person where LastName='{0}'", lastname);

                // 4. C#6 语法$
                // string sql = $"select * from Person where LastName = '{lastname}'";

                // IEnumerable<Person> lst  = db.Query<Person>(sql);	// Query 为db 对象扩展方法，返回值类型

                // 上面方法存在 SQL 注入问题

                // 5. 解决 SQL　注入问题
                string sql = $"select * from t_user where UserName = @username";    // 用@作为参数

                IEnumerable<User> lst = db.Query<User>(sql, new { username = username });    // 
                return lst.ToList();    // 转换为List 的类型返回
            }
        }
    }
}
