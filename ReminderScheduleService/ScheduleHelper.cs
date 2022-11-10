using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace ReminderScheduleService
{
    public class ScheduleHelper
    {
        private static string exFolder = Path.Combine("PillReminderFailueLogs");
        private static string sucFolder = Path.Combine("PillReminderSuccessLogs");
        private static string exPathToSave = string.Empty;
       
        public  string   GetPillRemiderList()
        {
            SqlConHelper sqlConHelper = new SqlConHelper();
            try
            {
                exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), sucFolder);
                DataTable dtresult = sqlConHelper.GetMissedPillReminderList();
                if(dtresult!=null && dtresult.Rows.Count>0)
                {
                    foreach(DataRow dr in dtresult.Rows)
                    {
                        int status = sqlConHelper.UpdatePillReminderStatus(dr["RemindersDatesId"].ToString());
                        if (status > 0)
                        {
                            LogFileException.Write_Log_Exception(exPathToSave, "Success Pill Remider Status Update:" + dr["RemindersDatesId"].ToString() + ":" + status);

                        }
                        else
                        {
                            LogFileException.Write_Log_Exception(exPathToSave, "Success Pill Remider Status Update:" + dr["RemindersDatesId"].ToString() + ":" + status);


                        }

                    }
                    LogFileException.Write_Log_Exception(exPathToSave, "Complted Pill Remider Status Update:" + dtresult.Rows.Count );
                    return "Success";
                }
                else
                {
                    return "Not Data Found";
                }



            }
            catch(Exception ex)
            {
                exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), exFolder);
                LogFileException.Write_Log_Exception(exPathToSave, ex.Message);
                throw ex;
            }
        }
    }
}
