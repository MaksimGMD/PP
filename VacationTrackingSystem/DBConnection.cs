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

        //Переменные
        public static int idRecord, idUser;

        //Получение данных из таблиц БД
        public static string qrVacation_List = "select [IDVacationList], [VacationTypeID], [TypeName] as 'Тип отпуска', " +
            "[VacationStartDate] as 'Дата начала', [VacationEndDate] as 'Дата окончания', [DaysAmount] as 'Количество дней', " +
            "[Status] as 'Статус отпуска', [ApplicationDate] as 'Дата заявления' from [VacationList] " +
            "inner join [VacationType] on [VacationTypeID] = [IDVacationType]",
            qrType = "select [IDVacationType] as 'ID', [TypeName] as 'Тип отпуска' from [VacationType]",
            qrPersonalCard = "select [IDPersonalCard] as 'ID', [Surname] as 'Фамилия', [Name] as 'Имя', [MiddleName] as 'Отчество', " +
            "[DaysAmount] as 'Количество дней', [IDPosition], [PositionName] as 'Должность', [Login] as 'Логин', [Password] from [PersonalCard] " +
            "inner join [Position] on [PositionID] = [IDPosition]",
            qrPositions = "select [IDPosition], [PositionName] as 'Должность' from [Position]",
            qrVacation_Order = "select [IDVacationList] as 'ID', [VacationTypeID], [PersonalCardID], [Surname] + ' ' + [Name] + ' ' + [MiddleName] as 'Сотрудник', [PositionName] as 'Должность'," +
            "[TypeName] as 'Тип отпуска', [VacationStartDate] as 'Дата начала', [VacationEndDate] as 'Дата окончания', " +
            "[VacationList].[DaysAmount] as 'Количество дней',  [Status] as 'Статус отпуска', [ApplicationDate] as 'Дата заявления', [IDVacationOrder], " +
            "[OrderNumber], [OrderDate] as 'Дата приказа', [PersonalCard].[DaysAmount] from [VacationOrder]  " +
            "right join [VacationList] on [IDVacationList] = [VacationListID] " +
            "inner join [VacationType] on [VacationTypeID] = [IDVacationType] " +
            "inner join [PersonalCard] on [PersonalCardID] = [IDPersonalCard] " +
            "inner join[Position] on[PositionID] = [IDPosition]",
            qrExtension = "select [IDExtensionOrder] as 'ID', [VacationOrderID], [OrderNumber] as 'Номер отпуска', [ExtensionOrderNumber] as 'Номер приказа', " +
            "[ExtensionDate] as 'Дата приказа', [ExtensionReason] as 'Причина продления', [ExtensionStartDate] as 'Дата начала', " +
            "[ExtensionEndDate] as 'Дата окончания' from [ExtensionOrder] " +
            "inner join [VacationOrder] on [VacationOrderID] = [IDVacationOrder]",
            qrOrdersList = "select [IDVacationOrder], '№'+[OrderNumber]+ ' ' + [VacationStartDate]+' - '+[VacationEndDate] as 'Приказ' from [VacationOrder] " +
            "inner join VacationList on [IDVacationList] = [VacationListID]",
            qrRecallOrder = "select [IDRecallOrder] as 'ID', [VacationOrderID], [OrderNumber] as 'Номер отпуска', [RecallOrderNumber] as 'Номер приказа', " +
            "[RecallOrderDate] as 'Дата приказа', [RecallReason] as 'Причина отзыва', [RecallDate] as 'Дата отзыва' from [RecallOrder] " +
            "inner join [VacationOrder] on [VacationOrderID] = [IDVacationOrder]";


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
                idUser = Convert.ToInt32(command.ExecuteScalar().ToString());
                return (idUser);
            }
            catch
            {
                idUser = 0;
                return (idUser);
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
        /// <summary>
        /// Проверка уникальностии логина
        /// </summary>
        /// <param name="login">Логин</param>
        /// <returns>Количество найденных пользователей</returns>
        public Int32 LoginCheck(string login)
        {
            int loginCheck;
            try
            {
                command.CommandText = "select count (*) from [PersonalCard] where Login like '%" + login + "%'";
                connection.Open();
                loginCheck = Convert.ToInt32(command.ExecuteScalar().ToString());
                return loginCheck;
            }
            catch
            {
                loginCheck = 0;
                return loginCheck;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}