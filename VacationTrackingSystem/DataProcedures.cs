using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Crypt_Library;

namespace VacationTrackingSystem
{
    public class DataProcedures
    {
        private SqlCommand command = new SqlCommand("", DBConnection.connection);
        //Для работы с базой данных из БД
        private void commandConfig(string config)
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "[dbo].[" + config + "]";
            command.Parameters.Clear();
        }

        //Добавление отпуска
        public void VacationList_Insert(string VacationStartDate, string VacationEndDate, string ApplicationDate, bool Status, int DaysAmount,
            int VacationTypeID, int PersonalCardID)
        {
            
            commandConfig("VacationList_Insert");
            command.Parameters.AddWithValue("@VacationStartDate", VacationStartDate);
            command.Parameters.AddWithValue("@VacationEndDate", VacationEndDate);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@Status", Status);
            command.Parameters.AddWithValue("@DaysAmount", DaysAmount);
            command.Parameters.AddWithValue("@VacationTypeID", VacationTypeID);
            command.Parameters.AddWithValue("@PersonalCardID", PersonalCardID);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        //Обновление отпуска
        public void VacationList_Update(int IDVacationList, string VacationStartDate, string VacationEndDate, string ApplicationDate, bool Status, int DaysAmount,
            int VacationTypeID, int PersonalCardID)
        {

            commandConfig("VacationList_Update");
            command.Parameters.AddWithValue("@IDVacationList", IDVacationList);
            command.Parameters.AddWithValue("@VacationStartDate", VacationStartDate);
            command.Parameters.AddWithValue("@VacationEndDate", VacationEndDate);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@Status", Status);
            command.Parameters.AddWithValue("@DaysAmount", DaysAmount);
            command.Parameters.AddWithValue("@VacationTypeID", VacationTypeID);
            command.Parameters.AddWithValue("@PersonalCardID", PersonalCardID);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        //Удаление отпуска
        public void VacationList_Delete(int IDVacationList)
        {
            commandConfig("VacationList_Delete");
            command.Parameters.AddWithValue("@IDVacationList", IDVacationList);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        //Добавление личной карточки
        public void PersonalCard_Insert(string Surname, string Name, string MiddleName, int DaysAmount, string Login, string Password, int PositionID)
        {
            Password = Crypt.Encrypt(Password);
            commandConfig("PersonalCard_Insert");
            command.Parameters.AddWithValue("@Surname", Surname);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@MiddleName", MiddleName);
            command.Parameters.AddWithValue("@DaysAmount", DaysAmount);
            command.Parameters.AddWithValue("@Login", Login);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@PositionID", PositionID);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        //Обновление личной карточки
        public void PersonalCard_Update(int IDPersonalCard, string Surname, string Name, string MiddleName, int DaysAmount, string Login, string Password, int PositionID)
        {
            Password = Crypt.Encrypt(Password);
            commandConfig("PersonalCard_Update");
            command.Parameters.AddWithValue("@IDPersonalCard", IDPersonalCard);
            command.Parameters.AddWithValue("@Surname", Surname);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@MiddleName", MiddleName);
            command.Parameters.AddWithValue("@DaysAmount", DaysAmount);
            command.Parameters.AddWithValue("@Login", Login);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@PositionID", PositionID);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        //Удаление личной карточки
        public void PersonalCard_Delete(int IDPersonalCard)
        {
            commandConfig("PersonalCard_Delete");
            command.Parameters.AddWithValue("@IDPersonalCard", IDPersonalCard);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        //Добавление типа
        public void VacationType_Insert(string TypeName)
        {
            commandConfig("VacationType_Insert");
            command.Parameters.AddWithValue("@TypeName", TypeName);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        //Обновление типа
        public void VacationType_Update(int IDVacationType, string TypeName)
        {
            commandConfig("VacationType_Update");
            command.Parameters.AddWithValue("@IDVacationType", IDVacationType);
            command.Parameters.AddWithValue("@TypeName", TypeName);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
        //Удаление типа
        public void VacationType_Delete(int IDVacationType)
        {
            commandConfig("VacationType_Delete");
            command.Parameters.AddWithValue("@IDVacationType", IDVacationType);
            DBConnection.connection.Open();
            command.ExecuteNonQuery();
            DBConnection.connection.Close();
        }
    }
}