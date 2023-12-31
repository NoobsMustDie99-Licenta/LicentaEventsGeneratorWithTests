﻿using EventsGenerator.Entities;
using EventsGenerator.EventProcessorsInterfaces;
using EventsGenerator.Utils;
using EventsGenerator.UtilsInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsGenerator.EventProcessors
{
    public class CommonProcessor : ICommonProcessor
    {
        public readonly IFetch _fetch;
        public readonly IProcessingUtils _processingUtils;
        public CommonProcessor(IFetch fetch, IProcessingUtils processingUtils)
        {
            _fetch = fetch;
            _processingUtils = processingUtils;
        }
        public async void DeleteExpiredSchedules()
        {
            try
            {
                List<Schedule> allSchedules = await _fetch.getAllSchedules();
                int count = 0;
                foreach (Schedule schedule in allSchedules)
                {
                    //delete schedule if all days and timeframe is in the past
                    if (_processingUtils.scheduleIsExpired(schedule))
                    {
                        await _fetch.deleteSchedule(schedule.Id);
                        count++;
                    }
                }
                
                if(count != 0)
                {
                    Console.WriteLine($"Deleted {count} schedules");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Coudn't get schedules from DB in order to delete expired ones");
            }
        }

        public async void DeletePassedEvents()
        {
            try
            {
                List<Event> allEvents = await _fetch.getAllEvents();
                int count = 0;
                foreach (Event evnt in allEvents)
                {
                    //delete events if all days and timeframe is in the past
                    if (_processingUtils.eventIsExpired(evnt))
                    {
                        await _fetch.deleteEvent(evnt.Id);
                        count++;
                    }
                }

                if (count != 0)
                {
                    Console.WriteLine($"Deleted {count} passed events");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Coudn't get events from DB in order to delete expired ones");
            }

        }
    }
}
