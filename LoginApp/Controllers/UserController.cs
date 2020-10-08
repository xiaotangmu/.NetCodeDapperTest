using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoginApp.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {

        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;

        public UserController(ILogger<UserController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public User Post([FromBody]User user)   // 没有FromBody 可以顺利传递
        {
            #region Dapper单表增删改查

            //// 1. 插入
            //bool res1 = _userService.addUser(user);
            //_logger.LogInformation(" **********************addUser: " + res1);

            //// 2. 查询
            //List<User> users = _userService.getUsersByUsername(user.userName);
            //users.ForEach(u =>
            //{
            //    _logger.LogInformation(" *******************getUsersByUserName: " + u.id + "; " + u.userName + "; " + u.pwd);
            //});

            //// 3. 更新
            //User user1 = new User
            //{
            //    userName = "tom",
            //    pwd = "54321",
            //    id = 1
            //};
            //bool res2 = _userService.updateUser(user1);
            //_logger.LogInformation(" ********************updateUser: " + res2);

            //// 4. 删除
            //bool res3 = _userService.deleteUserById(2);
            //_logger.LogInformation(" ********************deleteUserById: " + res3);
            #endregion

            #region Dapper 多表操作
            //List<User> users = _userService.getUsersFromTables();

            // oneToOne
            //List<User> users = _userService.oneToOne();

            //IEnumerable<User> users = _userService.ontToMany();
            //string str = JsonConvert.SerializeObject(users);  // 传Json 字符串

            user = _userService.oneToOneAndMany();
            #endregion
            return user;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IEnumerable<string> Put(int id, [FromBody]string value)
        {
            return new string[] { id + "", value };
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            return id;
        }
    }
}
