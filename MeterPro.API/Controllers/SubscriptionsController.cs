using MeterPro.DATA.AuthModels;
using MeterPro.DATA.DAL;
using MeterPro.DATA.Models;
using MeterPro.DATA.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        public async Task<IActionResult> Activate(SubscriptionVm model)
        {
            var filter = Builders<Meter>.Filter;
            var query = filter.Eq(x => x.MeterSn, model.MeterSn);

            var device = unitOfWork.MeterDataRepository.GetAll(query).Result.FirstOrDefault();
            if (device == null)
            {
                return NotFound();
            }

            var subFilter = Builders<Subscription>.Filter;
            var subQuery = subFilter.Eq(x => x.MeterSn, model.MeterSn);
            var subscription = unitOfWork.SubscriptionRepository.GetAll(subQuery).Result.FirstOrDefault();
            if (subscription == null)
            {
                var data = new Subscription()
                {
                    Balance = model.SubscriptionValue,
                    InitialValue = device.TotalUsageAccum,
                    DateActivated = DateTime.Now,
                    MeterSn=model.MeterSn,
                    SubscriptionValue=model.SubscriptionValue
                };

                await unitOfWork.SubscriptionRepository.Add(data);
                await unitOfWork.CommitAsync();
                return Ok(model);
            }
            else
            {
                var data = new SubscriptionHistory()
                {
                    Balance = subscription.Balance,
                    InitialValue = subscription.InitialValue,
                    DateActivated = subscription.DateActivated,
                    DateDeactivated = DateTime.Now,
                    MeterSn = subscription.MeterSn,
                    SubscriptionValue=subscription.SubscriptionValue
                   
                };

                
                await unitOfWork.SubscriptionHistoryRepository.Add(data);
                await unitOfWork.CommitAsync();

                var subUpdate = Builders<Subscription>.Update
                         .Set("InitialValue", device.TotalUsageAccum)
                         .Set("DateActivated", DateTime.Now)
                         .Set("Balance", subscription.Balance + model.SubscriptionValue);
                await unitOfWork.SubscriptionRepository.Update(subUpdate, "MeterSn", device.MeterSn!);
                await unitOfWork.CommitAsync();
            
                return Ok(model);
            }
           
        }
    }
}
