using MeterPro.DATA.DAL;
using MeterPro.DATA.Models;
using MeterPro.DATA.StatisticModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace MeterPro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardsController : ControllerBase
    {

        private readonly UnitOfWork unitOfWork;
        public DashboardsController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var filter = Builders<Meter>.Filter;
            var query = filter.Empty;
            var meters = await unitOfWork.MeterDataRepository.GetAll(query);
            var dash = new DashData()
            {
                AllMetersCount = meters.Count(),
                ConnectedMetersCount = meters.Where(a => a.Status == "ONLINE").Count(),
                DisconnectedMetersCount = meters.Where(a => a.Status == "OFFLINE").Count(),
                PowerOutMeters = meters.Where(a => a.PowerStatus == "OFF").Count()

            };
            return Ok(dash);
        }

    }
}
