using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ReminderScheduleService
{
    public class ReminderScheduleModel
    {
        public int RemindersDatesId { get; set; }
        public int ReminderId { get; set; }
        public string MedicineName { get; set; }
        public string MedicineTime { get; set; }
        public string MedicineInstruction { get; set; }
        public string UserId { get; set; }
        public DateTime ReminderTime { get; set; }
        public string NotificationMsg { get; set; }
        public string MedicineReadLoud { get; set; }
        public string MedicineAlertSound { get; set; }
        public DataTable dttoken { get; set; }
    }
    public class GoogleNotification
    {
        public Rootschedule rootschedule {get;set;}

        [JsonProperty("data")]
        public Rootschedule Data { get; set; }
        [JsonProperty("notification")]
        public Rootschedule Notification { get; set; }
    }
}
