using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Crypt_Library;

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
        public static string qrVacation_List = "select [IDVacationList], [VacationTypeID], [TypeName] as 'Тип отпуска', " +
            "[VacationStartDate] as 'Дата начала', [VacationEndDate] as 'Дата окончания', " +
            "[Status] as 'Отпуск согласован' from [VacationList] " +
            "inner join [VacationType] on [VacationTypeID] = [IDVacationType]",
            qrType = "select [IDVacationType], [TypeName] as 'Тип отпуска' from [VacationType]";


        private SqlCommand command = new SqlCommand("", connection);


        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns>id пользователя</returns>
        public int Authorization(string login, string password)
        {
            string passwordEnc = Crypt.Encrypt(password); //Зашифрованный пароль
            try
            {
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "select [IDPersonalCard] from [dbo].[PersonalCard] " +
               "where [Login] = '" + login + "' and [Password] = '" + passwordEnc + "'";
                DBConnection.connection.Open();
                return Convert.ToInt32(command.ExecuteScalar().ToString()); ;
            }
            catch
            {
                return 0;
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// Должность/роль пользователя
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public string userRole(Int32 idUser)
        {
            string RoleID;
            try
            {
                command.CommandText = "select [PositionName] from [PersonalCard] inner join [Position] on [PositionID] = [IDPosition] " +
                    "where [IDPersonalCard] like '%" + idUser + "%'";
                connection.Open();
                RoleID = command.ExecuteScalar().ToString();
                return RoleID;
            }
            catch
            {
                RoleID = "Не указана";
                return RoleID;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}