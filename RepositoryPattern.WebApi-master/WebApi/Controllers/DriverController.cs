using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebApi.HandleException;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DriverController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("GetAllDrivers")]
        [HttpGet]
        public IActionResult Get()
        {
            var allDrivers = _unitOfWork.DriverActivity.GetAllDrivers();
            return Ok(allDrivers);
        }

        [Route("DriversExceedingSingleTimeViolations")]
        [HttpGet]
        public IActionResult DriversExceedingSingleTimeViolations([FromQuery] List<int> driverIds, DateTime date)
        {
            var allDrivers = _unitOfWork.DriverActivity.DriversExceedingSingleTimeViolations(driverIds, date);
            return Ok(allDrivers);
        }

        [Route("DriversWithRestTimeViolations")]
        [HttpGet]
        public IActionResult DriversWithRestTimeViolations([FromQuery] List<int> driverIds, DateTime date)
        {
            var allDrivers = _unitOfWork.DriverActivity.DriversWithRestTimeViolations(driverIds, date);
            return Ok(allDrivers);
        }

        [Route("DriversDayTimeViolations")]
        [HttpGet]
        public IActionResult DriversDayTimeViolations([FromQuery] List<int> driverIds, DateTime date)
        {
            var allDrivers = _unitOfWork.DriverActivity.DriversDayTimeViolations(driverIds, date);
            return Ok(allDrivers);
        }

        [Route("DriversWeekTimeViolations")]
        [HttpGet]
        public IActionResult DriversWeekTimeViolations([FromQuery] List<int> driverIds, DateTime startDate, DateTime endDate)
        {
            var allDrivers = _unitOfWork.DriverActivity.DriversWeekTimeViolations(driverIds, startDate, endDate);
            return Ok(allDrivers);
        }

        [Route("GetRecordsFromFileAndAddIntoDb")]
        [HttpPost]
        public IActionResult GetRecordsFromFileAndAddIntoDb()
        {
            var results = _unitOfWork.DriverActivity.GetRecordsFromFileAndAddIntoDb();
            _unitOfWork.DriverActivity.AddRange(results);
            _unitOfWork.Complete();
            return Ok();
        }
    }
}
