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
                    SubscriptionValue=model.SubscriptionValue,
                    ValueTrend=device.TotalUsageAccum
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
                         .Set("ValueTrend", device.TotalUsageAccum)
                         .Set("DateActivated", DateTime.Now)
                         .Set("SubscriptionValue", subscription.Balance + model.SubscriptionValue)
                         .Set("Balance", Math.Round(subscription.Balance + model.SubscriptionValue,2));
                await unitOfWork.SubscriptionRepository.Update(subUpdate, "MeterSn", device.MeterSn!);
                await unitOfWork.CommitAsync();
            
                return Ok(model);
            }
           
        }


        [HttpGet]
        [Route("GetBalanceData")]
        public async Task<IActionResult> GetBalanceData(string meterSn)
        {
            var filter = Builders<Subscription>.Filter;
            var query = filter.Eq(x => x.MeterSn, meterSn);
            var data = await unitOfWork.SubscriptionRepository.GetAll(query);
            return Ok(data);
        }


        [HttpGet]
        [Route("GetAggregateBalance")]
        public async Task<IActionResult> GetAggregateBalance(string owner)
        {
            var filter = Builders<Subscription>.Filter;
            var query = filter.Eq(x => x.Owner, owner);
            var data = await unitOfWork.SubscriptionRepository.GetAll(query);
            var aggregateBalance =new Subscription()
            {
                Balance=data.Sum(x => x.Balance),
                InitialValue=data.Sum(x => x.InitialValue),
                SubscriptionValue=data.Sum(x => x.SubscriptionValue)
            };
            return Ok(aggregateBalance);
        }

    }
}
