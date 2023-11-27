using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IDriverActivityRepository : IGenericRepository<DriverActivity>
    {
        IEnumerable<Driver> GetAllDrivers();
        IEnumerable<Driver> DriversExceedingSingleTimeViolations(List<int> driverIds, DateTime date);
        IEnumerable<Driver> DriversWithRestTimeViolations(List<int> driverIds, DateTime date);
        IEnumerable<Driver> DriversDayTimeViolations(List<int> driverIds, DateTime date);
        IEnumerable<Driver> DriversWeekTimeViolations(List<int> driverIds, DateTime startDate, DateTime endDate);
        List<DriverActivity> GetRecordsFromFileAndAddIntoDb();
    }
}
