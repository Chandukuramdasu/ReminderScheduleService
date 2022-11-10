using CorePush.Google;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ReminderScheduleService
{
    public static class MultipleTask
    {
        static string SenderId = "296777585728";//"296777585728";
        static string ServerKey = "AAAARRlSlEA:APA91bHfKY0jpvqkAiRsltIoff4ouqSa95MquRDwikaLHy1dR_QYQ_XUvkhnsOysxrz8wyRYyQnKhF3d4QlJHed3lNNSW-mWVwr7fq-ddBUk2Q6ztHe6qNbF2yJZKEMXWG50S_hihy8t";
        private static string exFolder = Path.Combine("ReminderFailueLogs");
        private static string sucFolder = Path.Combine("ReminderSuccessLogs");
        private static string exPathToSave = string.Empty;
        public static void loadlist(double milliseconds, ReminderScheduleModel scheduleModel)
            {
            Console.WriteLine("timer:" + milliseconds);   
             System.Timers.Timer runonce = new System.Timers.Timer(milliseconds);
               runonce.Elapsed += (s, e) => { action(scheduleModel);   };
                  runonce.AutoReset = false;
                        runonce.Start();
            }
        public static void action(ReminderScheduleModel item)
        {
            if (item.dttoken != null && item.dttoken.Rows.Count > 0)
            {
                foreach (DataRow dr in item.dttoken.Rows)
                {
                    Console.WriteLine("pushing notification :userId" + item.UserId + ", DeviceId:" + dr["DeviceId"].ToString() + "Push Details:" + JsonConvert.SerializeObject(item));
                    string status =  PushNotificationReminder(item, dr["DeviceId"].ToString()).Result;
                    exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), sucFolder);
                    LogFileException.Write_Log_Exception(exPathToSave, "Success Notofication:" + item + ":" + status);

                    Console.WriteLine("Notification Status:" + status + "DeviceId:" + dr["DeviceId"].ToString());
                }
            }
            else
            {
                Console.WriteLine("No Token found for this user");
            }
        }
        //ReminderScheduleModel
        private async static Task<string> PushNotificationReminder(ReminderScheduleModel item, string deviceId)
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
                NotificationPharmacypushreq _params = new NotificationPharmacypushreq();
                _params.notificationCategoryId = "7034";
                _params.notificationDate = DateTime.Now.ToString();
                _params.notificationDetailMessage = item.MedicineInstruction;
                _params.notificationHeadingMessage = "Pill Reminder";
                _params.notificationRedirectId = item.RemindersDatesId.ToString();
                _params.userId = item.UserId;
                _params.notificationUnReadFlag = "N";
                _params.medicineReadLoud = item.MedicineReadLoud;
                _params.medicineAlertSound = item.MedicineAlertSound;
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
                    updateNotificationStatus(item.UserId, item.RemindersDatesId.ToString(),item.MedicineInstruction);
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

        private static string updateNotificationStatus(string user,string reminderdateid,string NotificationMsg)
        {
            try
            {
                SqlConHelper sqlConHelper = new SqlConHelper();
               int j= sqlConHelper.UpdatePillReminderNotificationStaus(reminderdateid,user, NotificationMsg);
                return j > 0 ? "Success" : "Failure";
            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}
