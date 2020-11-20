using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace VacationTrackingSystem
{
    public class DBConnection
    {
        //Подключение к базе данных 
        public static SqlConnection connection = new SqlConnection(
            "Data Source=DESKTOP-2OC8HFJ\\MYGRIT; Initial Catalog=EmployeeVacationTracking;" +
            "Integrated Security=True; Connect Timeout=30; Encrypt=False;" +
            "TrustServerCertificate=False; ApplicationIntent=ReadWrite; MultiSubnetFailover=False");

        //Получение данных из таблиц БД
        
    }
}