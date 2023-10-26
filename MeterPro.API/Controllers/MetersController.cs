using MeterPro.DATA.DAL;
using MeterPro.DATA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace MeterPro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetersController : ControllerBase
    {

        private readonly UnitOfWork _unitOfWork;

        public MetersController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetMeters")]
        public async Task<IActionResult> GetMeters()
        {
            var filter = Builders<Meter>.Filter;
            var query = filter.Empty;
            var meters = await _unitOfWork.MeterDataRepository.GetAll(query);
            return Ok(meters);
        }

        [HttpGet]
        [Route("GetMeterLogs")]
        public async Task<IActionResult> GetMeterLogs()
        {
            var filter = Builders<TimeData>.Filter;
            var query = filter.Empty;
            var timeDatas = await _unitOfWork.TimeDataRepository.GetAll(query);
            return Ok(timeDatas);
        }


    }
}
