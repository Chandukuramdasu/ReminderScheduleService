using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ReminderScheduleService
{

    public class SchedulerService
    {
        private static SchedulerService _instance;
        private List<Timer> timers = new List<Timer>();

        private SchedulerService() { }

        public static SchedulerService Instance => _instance ?? (_instance = new SchedulerService());

        public void ScheduleTask(int hour, int min, double intervalInHour, Action task)
        {
            Console.WriteLine("schedule task-1:" + hour + ", min:" + min);
            DateTime now = DateTime.Now;
            DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);
            if (now > firstRun)
            {
                firstRun = firstRun.AddDays(1);
            }

            TimeSpan timeToGo = firstRun - now;
            if (timeToGo <= TimeSpan.Zero)
            {
                timeToGo = TimeSpan.Zero;
            }

            var timer = new Timer(x =>
            {
                Console.WriteLine("schedule task timer:" + hour + ", min:" + min);
                task.Invoke();
            }, null, timeToGo, TimeSpan.FromHours(intervalInHour));

            timers.Add(timer);
        }
    }
}
