using System;
using System.Collections.Generic;
using System.Text;

namespace ReminderScheduleService
{
    public class Msg
    {
           
        public NotificationPharmacypushreq param { get; set; }
    }

    public class Params
    {
       
            public string reminderid { get; set; }
            public string medicineAlertSound { get; set; }
            public string medicineCritical { get; set; }
            public string readaloud { get; set; }
        
    }
    public class NotificationPharmacypushreq
    {

        public int tranID { get; set; }
        public string notificationCategoryId { get; set; }
        public string notificationDate { get; set; }
        public string notificationHeadingMessage { get; set; }
        public string notificationDetailMessage { get; set; }
        public string notificationRedirectScreen { get; set; }
        public string notificationRedirectId { get; set; }
        public string notificationUnReadFlag { get; set; }
        public int totalRecords { get; set; }
        public string userId { get; set; }
        public string medicineReadLoud { get; set; }
        public string medicineAlertSound { get; set; }

    }


    public class Rootschedule
    {
       
        public string[] keys { get; set; }
        public Msg msg { get; set; }
    }
}
