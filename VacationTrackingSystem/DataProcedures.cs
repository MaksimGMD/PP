using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

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
            
            commandConfig("Users_Insert");
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
    }
}