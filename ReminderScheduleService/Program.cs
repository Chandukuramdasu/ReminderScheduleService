using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using CorePush.Google;
using static ReminderScheduleService.GoogleNotification;
using Newtonsoft.Json;
namespace ReminderScheduleService
{
    class Program
    {

        static string SenderId = "296777585728";//"296777585728";
        static string ServerKey = "AAAARRlSlEA:APA91bHfKY0jpvqkAiRsltIoff4ouqSa95MquRDwikaLHy1dR_QYQ_XUvkhnsOysxrz8wyRYyQnKhF3d4QlJHed3lNNSW-mWVwr7fq-ddBUk2Q6ztHe6qNbF2yJZKEMXWG50S_hihy8t";
            //"AAAAGoJ1rjY:APA91bGSDKOf1UXwcRTzKNAf0NxNYg68CDFTyahXwjtBwAcNggWWvPugzAi7w6QMYVl_TxmKzlnIuE67iVcOKdbCKMF4g8alCRtTopp4MCdkN2qlupzoei-ZLyAp5ulyFo3M7XhOmej9";
            //"AAAARRlSlEA:APA91bF6U1fYxGOw76ZhCU-x1nJ6_rKbWqDXHUVVmKPeHeQm_-FcAsClff2BlGEpM61ioFkUj7wQdSmd9uS53P8uBLrHgDp8JVO7eUVF1vLViG-VQ47bWRfPYMxQvWcI9JlKrFUAqcuW";
        private static string exFolder = Path.Combine("ReminderFailueLogs");
        private static string sucFolder = Path.Combine("ReminderSuccessLogs");
        private static string exPathToSave = string.Empty;
         static void Main(string[] args)
        {

            Console.WriteLine("Start Scheduler");
            // For Interval in Seconds 
            // This Scheduler will start at 11:10 and call after every 15 Seconds
            // IntervalInSeconds(start_hour, start_minute, seconds)
            //MyScheduler.IntervalInSeconds(11, 10, 15,
            //() => {

            //    Console.WriteLine("//here write the code that you want to schedule");
            //});

            // For Interval in Minutes 
            // This Scheduler will start at 22:00 and call after every 30 Minutes
            // IntervalInSeconds(start_hour, start_minute, minutes)
            //Console.WriteLine("Current Date:" + DateTime.Now);
            //int hours = Convert.ToInt32(DateTime.Now.ToString("HH"));
            //int min = Convert.ToInt32(DateTime.Now.ToString("mm"))+1;
            //Console.WriteLine("Current Time:" + hours+"min:"+min);
            //MyScheduler.IntervalInMinutes(hours, min, 15,
            //async () => {
                try
                {
                    SqlConHelper sqlConHelper = new SqlConHelper();
                    Console.WriteLine("getting PillReminder List");
                    DataTable dtresult = sqlConHelper.GetPillReminder();
                    if (dtresult != null && dtresult.Rows.Count > 0)
                    {
                        
                     List<ReminderScheduleModel> ReminderDetails = new List<ReminderScheduleModel>();
              
                   ReminderDetails = ConvertDataTable<ReminderScheduleModel>(dtresult);
                   Console.WriteLine("reminderList" + ReminderDetails.Count);
                    //    var UserList = ReminderDetails.GroupBy(user => user.UserId)
                    //.Select(grp => grp.First())
                    //.ToList();
                    // Console.WriteLine("pushing notification" + UserList.Count);
                    foreach (ReminderScheduleModel item in ReminderDetails)
                    {
                        DataTable dttokens = sqlConHelper.GetDeviceToken(item.UserId);
                        item.dttoken = dttokens;
                        DateTime datetime1 = item.ReminderTime;
                        //int hhs = Convert.ToInt32('0'+datetime1.Split(" ")[1].ToString().Split(":")[0]);
                        //int mins = Convert.ToInt32(datetime1.Split(" ")[1].ToString().Split(":")[1]);
                       // Console.WriteLine("pushing notification: UserId" + item.UserId + "Time hours:" + hhs + "minutes:" + mins);
                        DateTime now = DateTime.Now;
                       // DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hhs, mins, 0, 0);
                                                TimeSpan timeToGo = datetime1 - now;
                       if(timeToGo.TotalMilliseconds>0)
                        MultipleTask.loadlist(timeToGo.TotalMilliseconds,item);

                        //MyScheduler.IntervalInMinutes(hhs, mins, 0,
                        //      async () =>
                        //      {
                        //try
                        //{

                        //    Console.WriteLine("pushing notification" + item.UserId);
                        //    DataTable dttokens = sqlConHelper.GetDeviceToken(item.UserId);
                        //    if (dttokens != null && dttokens.Rows.Count > 0)
                        //    {
                        //        foreach (DataRow dr in dttokens.Rows)
                        //        {
                        //            Console.WriteLine("pushing notification :userId" + item.UserId + ", DeviceId:" + dr["DeviceId"].ToString() + "Push Details:" + JsonConvert.SerializeObject(item));
                        //            string status = await PushNotificationReminder(item.UserId, item.MedicineName, item.MedicineTime, item.RemindersDatesId.ToString(), item.MedicineInstruction, dr["DeviceId"].ToString());
                        //            exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), sucFolder);
                        //            LogFileException.Write_Log_Exception(exPathToSave, "Success Notofication:" + item + ":" + status);

                        //            Console.WriteLine("Notification Status:" + status + "DeviceId:" + dr["DeviceId"].ToString());
                        //        }
                        //    }
                        //    else
                        //    {
                        //        Console.WriteLine("No Token found for this user");
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    Console.WriteLine("Exception inner:" + ex.Message.ToString());
                        //    exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), exFolder);
                        //    LogFileException.Write_Log_Exception(exPathToSave, ex.Message);
                        //}
                    //});

                }

                    Console.WriteLine("pushing notification end");

                    // Console.WriteLine("//here write the code that you want to schedule");
                     }
               else
                {
                    System.Environment.Exit(0);
                }
                   }
                catch (Exception ex)
                {
                Console.WriteLine("Exception main:" + ex.Message.ToString());
                exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), exFolder);
                    LogFileException.Write_Log_Exception(exPathToSave, ex.Message);
                }
            

            // For Interval in Hours 
            // This Scheduler will start at 9:44 and call after every 1 Hour
            // IntervalInSeconds(start_hour, start_minute, hours)
            //MyScheduler.IntervalInHours(9, 44, 1,
            //() => {

            //    Console.WriteLine("//here write the code that you want to schedule");
            //});

            //// For Interval in Seconds 
            //// This Scheduler will start at 17:22 and call after every 3 Days
            //// IntervalInSeconds(start_hour, start_minute, days)
            //MyScheduler.IntervalInDays(23, 30, 1,
            //() =>
            //{
            //    ScheduleHelper scheduleHelper = new ScheduleHelper();
            //   string result= scheduleHelper.GetPillRemiderList();
            //    exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), sucFolder);
            //    LogFileException.Write_Log_Exception(exPathToSave, "Updated Scheduele Status:" + result);

            //    Console.WriteLine("Update Schedule Status:" + result);

                
            //});

            Console.ReadLine();
        }
        private async static Task<string> PushNotificationReminder(string user,string medicinename,string medicinetime,string reminderId,string shifttype,string deviceId)
        {
            SqlConHelper sqlConHelper = new SqlConHelper();
            try
            {
               
                    /* FCM Sender (Android Device) */
                    FcmSettings settings = new FcmSettings()
                    {
                        SenderId = SenderId,
                        ServerKey = ServerKey
                    };
                   
                        HttpClient httpClient = new HttpClient();

                        string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                        string deviceToken = deviceId;
                    Console.WriteLine(deviceId);
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                        httpClient.DefaultRequestHeaders.Accept
                                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        Rootschedule rootschedule = new Rootschedule();
                string[] keyval = new string[] { "PushConnectorClass_2" };
                Msg msg = new Msg();
                          //Params _params = new Params();
                //_params.medicineAlertSound = "";
                //_params.medicineCritical = "";
                //_params.reminderid = reminderId;
                //_params.readaloud = "Y";
                NotificationPharmacypushreq _params = new NotificationPharmacypushreq();
                _params.notificationCategoryId = "7034";
                _params.notificationDate = DateTime.Now.ToString();
                _params.notificationDetailMessage = "[" + shifttype + "], make sure to take your " + medicinetime + "[" + medicinename + "] and mark as taken"; ;
                _params.notificationHeadingMessage = "Pill Reminder";
                _params.notificationRedirectId = reminderId;
                _params.userId = user;
                _params.notificationUnReadFlag = "N"; 
                        msg.param = _params;
               
                rootschedule.msg = msg;
                rootschedule.keys = keyval;
               // string message = JsonConvert.SerializeObject(rootschedule);
                      
                       
                        GoogleNotification notification = new GoogleNotification();
                        notification.Notification = rootschedule;
                        notification.Data = rootschedule;
                        var fcm = new FcmSender(settings, httpClient);
                        var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);


                        if (fcmSendResponse.IsSuccess())
                        {
                            return "sucess";
                        }
                        else
                        {
                            return fcmSendResponse.Results[0].Error;
                        }
                    
                
               
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

      
        
    }

}
