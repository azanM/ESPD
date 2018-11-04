using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eSPD.Core
{
    public class JobScheduler
    {
        public static void Start()
        {
            using (var ctx =   new dsSPDDataContext())
            {
                JobSch dataJob = ctx.JobSches.FirstOrDefault(o => o.Id == 1);

                int hour = 24; int hourday = 7; int minuteday = 0;
                if (dataJob != null)
                {
                    hour = dataJob.IntervalHour;
                    hourday = dataJob.Hour;
                    minuteday = dataJob.Minute;

                    IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                    scheduler.Start();

                    IJobDetail job = JobBuilder.Create<EmailJob>().Build();
                    IJobDetail jobClaim = JobBuilder.Create<EmailClaimJob>().Build();

                    ITrigger trigger = TriggerBuilder.Create()
                        .WithDailyTimeIntervalSchedule
                          (s =>
                             s.WithIntervalInHours(hour)
                            .OnEveryDay()
                            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(hourday, minuteday))
                          )
                        .Build();

                    ITrigger triggerClaim = TriggerBuilder.Create()
                        .WithDailyTimeIntervalSchedule
                          (s =>
                             s.WithIntervalInHours(hour)
                            .OnEveryDay()
                            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(hourday, minuteday))
                          )
                        .Build();

                    scheduler.ScheduleJob(job, trigger);
                    scheduler.ScheduleJob(jobClaim, triggerClaim);
                    scheduler.Start();
                }
            }
           
            //ITrigger trigger = TriggerBuilder.Create()
            //   .WithIdentity("trigger1", "group1")
            //   .StartNow()
            //   .WithSimpleSchedule(x => x
            //       .WithIntervalInMinutes(1)
            //       .WithRepeatCount(1))
            //   .Build();

            //scheduler.ScheduleJob(job, trigger);
            //scheduler.Start();
        }

        public static void ShutDown()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Shutdown();
        }

        public static bool isStart()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            return scheduler.IsStarted;
        }
    }
}
