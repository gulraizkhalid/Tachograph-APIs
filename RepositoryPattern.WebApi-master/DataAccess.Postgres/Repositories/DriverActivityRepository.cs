using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Enums;
using System.IO;
using Newtonsoft.Json;

namespace DataAccess.Postgres.Repositories
{
    public class DriverActivityRepository : GenericRepository<DriverActivity>, IDriverActivityRepository
    {
        List<Driver> drivers = null;
        public DriverActivityRepository(AppDbContext context) : base(context)
        {
            drivers = new List<Driver>
        {
            new Driver { DriverId = 1, FirstName = "John", LastName = "Doe", Address = "Diktargatan 25", MobileNumber = 0722897445, Nationality = "SWE", LicenseNumber = 672939941},
            new Driver { DriverId = 2, FirstName = "Peter", LastName = "Parker", Address = "Diktargatan 26", MobileNumber = 0722836445, Nationality = "SWE", LicenseNumber = 149939941},
            new Driver { DriverId = 3, FirstName = "Tony", LastName = "Stark", Address = "Diktargatan 27", MobileNumber = 0722787445, Nationality = "SWE", LicenseNumber = 455939941},
            new Driver { DriverId = 4, FirstName = "Stuart", LastName = "Broad", Address = "Diktargatan 28", MobileNumber = 0718897445, Nationality = "SWE", LicenseNumber = 402939941},
            new Driver { DriverId = 5, FirstName = "James", LastName = "Cook", Address = "Diktargatan 29", MobileNumber = 0722937445, Nationality = "SWE", LicenseNumber = 450239941},
            // Add more sample data as needed
        };
        }

        public IEnumerable<Driver> GetAllDrivers()
        {
            //var driverActivity = _context.DriverActivity.Distinct().ToList();
            //return from da in driverActivity
            //       join d in drivers on da.DriverId equals d.DriverId
            //       select d;
            return drivers;
        }

        //List of drivers who have exceeded single drive time violations
        public IEnumerable<Driver> DriversExceedingSingleTimeViolations(List<int> driverIds, DateTime date)
        {
            var query = _context.DriverActivity.Where(x => x.Date.Date == date.Date && x.Activity == Activity.Driving);

            if (driverIds != null && driverIds.Any())
            {
                query = query.Where(da => driverIds.Contains(da.DriverId));
            }
            var driverActivity = query.ToList();

            driverActivity = driverActivity.Where(x => (x.EndTime - x.StartTime).TotalHours > 4).ToList();

            return from da in driverActivity
                   join d in drivers on da.DriverId equals d.DriverId
                   select d;
        }

        //List of drivers who have rest time violations
        public IEnumerable<Driver> DriversWithRestTimeViolations(List<int> driverIds, DateTime date)
        {
            var query = _context.DriverActivity.Where(x => x.Date.Date == date.Date);

            if (driverIds != null && driverIds.Any())
            {
                query = query.Where(da => driverIds.Contains(da.DriverId));
            }
            var driverActivity = query.ToList();

            var filteredDrivers = driverActivity.Where(x => (x.Activity == Activity.Driving && (x.EndTime - x.StartTime).TotalHours >= 4) ||
                                                (x.Activity == Activity.Rest && (x.EndTime - x.StartTime).TotalMinutes < 45))
                .GroupBy(x => x.DriverId)
                .Select(group => new
                {
                    DriverId = group.Key,
                    Activities = group.ToList()
                })
                .ToList();

            return from da in filteredDrivers
                   join d in drivers on da.DriverId equals d.DriverId
                   where da.Activities.Any(x => x.Activity == Activity.Rest)
                   select d;
        }

        //List of drivers who have Day drive time violations
        public IEnumerable<Driver> DriversDayTimeViolations(List<int> driverIds, DateTime date)
        {
            var query = _context.DriverActivity.Where(x => x.Date.Date == date.Date && x.Activity == Activity.Driving);

            if (driverIds != null && driverIds.Any())
            {
                query = query.Where(da => driverIds.Contains(da.DriverId));
            }
            var driverActivity = query.ToList();

            var groupedByDriver = driverActivity
             .GroupBy(activity => activity.DriverId)
             .Select(group => new
             {
                 DriverId = group.Key,
                 Records = group.ToList(),
                 time = group.Sum(x => (x.EndTime - x.StartTime).TotalHours)
             })
             .ToList();

            groupedByDriver = groupedByDriver.Where(x => x.time > 12).ToList();

            return from da in groupedByDriver
                   join d in drivers on da.DriverId equals d.DriverId
                   select d;
        }

        //List of drivers who have Week drive time violations
        public IEnumerable<Driver> DriversWeekTimeViolations(List<int> driverIds, DateTime startDate, DateTime endDate)
        {
            var query = _context.DriverActivity.Where(x => (startDate.Date <= x.Date.Date && x.Date.Date <= endDate.Date) && x.Activity == Activity.Driving);

            if (driverIds != null && driverIds.Any())
            {
                query = query.Where(da => driverIds.Contains(da.DriverId));
            }
            var driverActivity = query.ToList();

            var groupedByDriver = driverActivity
             .GroupBy(activity => activity.DriverId)
             .Select(group => new
             {
                 DriverId = group.Key,
                 Records = group.ToList(),
                 time = group.Sum(x => (x.EndTime - x.StartTime).TotalHours)
             })
             .ToList();

            groupedByDriver = groupedByDriver.Where(x => x.time > 60).ToList();

            return from da in groupedByDriver
                   join d in drivers on da.DriverId equals d.DriverId
                   select d;
        }

        public List<DriverActivity> GetRecordsFromFileAndAddIntoDb() 
        {
            List<DriverActivity> lstDriverActivity = new List<DriverActivity>();
            string folderPath = @"D:\Driver Files\" + DateTime.Now.ToString("yyyy-MM-dd"); ;

            if (Directory.Exists(folderPath))
            {
                string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

                foreach (var jsonFile in jsonFiles)
                {
                    string jsonContent = File.ReadAllText(jsonFile);
                    var driverActivity = JsonConvert.DeserializeObject<DriverActivity>(jsonContent);
                    lstDriverActivity.Add(driverActivity);
                }
            }
            return lstDriverActivity;
        }
    }
}
