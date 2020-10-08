using Common;
using Microsoft.Extensions.Configuration;
using System;

namespace DapperConn
{
    public class DBHelper
    {
        public static string ConnStrings
        {
            get
            {
                //  获取二级子节点
                //return AppConfigurtaionServices.Configuration["Appsettings:SystemName"];
                return AppConfigurtaionServices.Configuration.GetConnectionString("SqlServerConnection");
            }
        }
    }
}
