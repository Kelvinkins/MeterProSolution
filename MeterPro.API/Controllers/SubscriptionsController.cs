using MeterPro.DATA.DAL;
using MeterPro.DATA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace MeterPro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        public SubscriptionsController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
           
        }

        [HttpPost]
        [Route("Activate")]
        public async Task<IActionResult> Activate(Subscription model)
        {
            var filter = Builders<Meter>.Filter;
            var query = filter.Eq(x => x.MeterSn, model.MeterSn);

            var device = unitOfWork.MeterDataRepository.GetAll(query).Result.FirstOrDefault();
            if(device == null)
            {
                return NotFound();
            }
            model.Balance =model.SubscriptionValue;
            model.InitialValue = device.TotalUsageAccum;
            model.DateActivated = DateTime.Now;
            model.Active = true;
            await unitOfWork.SubscriptionRepository.Add(model);
            await unitOfWork.CommitAsync();
            return Ok(model);
        }
    }
}
