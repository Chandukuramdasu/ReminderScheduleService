using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace ReminderScheduleService
{
    public class SqlConHelper
    {
        private  string exFolder = Path.Combine("ReminderExceptionLogs");
        private  string exPathToSave = string.Empty;
   string connectionString = "Data Source=EC2AMAZ-G2DPGDU;Initial Catalog=OHC-Dev;Integrated Security=True; User ID=ohcdbuser; Password=3$healthCare@OHC; Connection Timeout=30;";
  //      string connectionString = "Data Source=DESKTOP-HA7VC32\\MSSQLSERVER01;Initial Catalog=OHC-Dev;Integrated Security=True; Connection Timeout=30";

        public SqlConHelper()
        {
            exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), exFolder);
        }
        public DataTable GetPillReminder()
        {
            
            try
            {
                using (SqlConnection _con = new SqlConnection(connectionString))
                {
                    DataTable dt = new DataTable();

                    using (SqlCommand _cmd = new SqlCommand("[SP_GetReminderScheduleDetails]", _con))
                    {

                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                        _con.Open();
                        _dap.Fill(dt);
                        _con.Close();
                        return dt;
                    }
                }
            }
            catch(Exception ex)
            {
                LogFileException.Write_Log_Exception(exPathToSave, "GetPillReminderD_SP :  errormessage:" + ex.Message.ToString());
                throw ex;
            }
        }

        public DataTable GetMissedPillReminderList()
        {

            try
            {
                using (SqlConnection _con = new SqlConnection(connectionString))
                {
                    DataTable dt = new DataTable();

                    using (SqlCommand _cmd = new SqlCommand("[SP_MissedReminderList]", _con))
                    {

                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                        _con.Open();
                        _dap.Fill(dt);
                        _con.Close();
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                LogFileException.Write_Log_Exception(exPathToSave, "GetPillReminderD_SP :  errormessage:" + ex.Message.ToString());
                throw ex;
            }
        }

        public int UpdatePillReminderStatus( string reminderdateId)
        {

            try
            {
                using (SqlConnection _con = new SqlConnection(connectionString))
                {
                    DataTable dt = new DataTable();

                    using (SqlCommand _cmd = new SqlCommand("[SP_UpdateReminderStatus]", _con))
                    {

                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        _cmd.Parameters.AddWithValue("@PRemindersDatesId", 50).Value = reminderdateId;
                          _con.Open();
                        int status = _cmd.ExecuteNonQuery();
                        _con.Close();
                        return status;                       
                        
                    }
                }
            }
            catch (Exception ex)
            {
                LogFileException.Write_Log_Exception(exPathToSave, "GetPillReminderD_SP :  errormessage:" + ex.Message.ToString());
                throw ex;
            }
          
        }

        public int UpdatePillReminderNotificationStaus(string reminderdateId,string userId,string NotificationMsg)
        {

            try
            {
                using (SqlConnection _con = new SqlConnection(connectionString))
                {
                    DataTable dt = new DataTable();

                    using (SqlCommand _cmd = new SqlCommand("[SP_UpdatePillReminderNotificationStaus]", _con))
                    {

                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        _cmd.Parameters.AddWithValue("@RemindersDatesId", 50).Value = reminderdateId;
                        _cmd.Parameters.AddWithValue("@UserId", 50).Value = userId;
                        _cmd.Parameters.AddWithValue("@NotificationMsg", 500).Value = NotificationMsg;
                        _con.Open();
                        int status = _cmd.ExecuteNonQuery();
                        _con.Close();
                        return status;

                    }
                }
            }
            catch (Exception ex)
            {
                LogFileException.Write_Log_Exception(exPathToSave, "UpdatePillReminderNotificationStaus :  errormessage:" + ex.Message.ToString());
                throw ex;
            }

        }

        public DataTable GetDeviceToken(string  userId)
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(connectionString))
                {
                    DataTable dt = new DataTable();

                    using (SqlCommand _cmd = new SqlCommand("[SP_GetNotificationTokenByID]", _con))
                    {

                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        _cmd.Parameters.AddWithValue("@PUserId", userId);
                        SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                        _con.Open();
                        _dap.Fill(dt);
                        _con.Close();
                        return dt;
                    }
                }
            }
            catch(Exception ex)
            {
                LogFileException.Write_Log_Exception(exPathToSave, "GetDeviceToken :  errormessage:" + ex.Message.ToString());

                throw ex;
            }
        }

    }
}
