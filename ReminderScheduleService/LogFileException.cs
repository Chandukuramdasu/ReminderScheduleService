using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReminderScheduleService
{
	public static class LogFileException
	{
		
		#region"Logs code"

		public static object Write_Log_Exception(string mappath, dynamic strMsg)
		{
			string strPath = mappath + "\\" + DateTime.Now.ToString("MMddyyyy");
			if (!Directory.Exists(strPath))
				Directory.CreateDirectory(strPath);
			string path2 = strPath + "\\" + "submittedData" + DateTime.Now.ToString("yyyyMMddhhmmssmmm");
			StreamWriter swLog = new StreamWriter(path2 + ".txt", true);
			swLog.WriteLine(DateTime.Now.ToString("ddMMyyHHmmssttt") + ":" + strMsg);
			swLog.Close();
			swLog.Dispose();
			return "";
		}
		#endregion
	}
}
