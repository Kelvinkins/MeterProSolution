using MeterPro.DATA.CommandModels;
using MeterPro.DATA.DAL;
using MeterPro.DATA.Models;
using MeterPro.DATA.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace MeterPro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHooksController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
      private readonly  CommandsController commandController;

        public WebHooksController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            commandController = new CommandsController(this.unitOfWork);
        }

      


        [HttpPost]
        [Route("MeterDump")]
        public async Task<IActionResult> MeterDump(TimeData[] model)
        {
            try
            {



                foreach (var item in model)
                {
                    var filter = Builders<Meter>.Filter;
                    var query = filter.Eq(x => x.MeterSn, item.meterSn);

                    var device = unitOfWork.MeterDataRepository.GetAll(query).Result.FirstOrDefault();
                    if (device == null)
                    {
                        //Enroll the device
                        var newDevice = new Meter()
                        {
                            DateEnrolled = DateTime.Now,
                            gatewaySn = item.gatewaySn,
                            LastUpdated = DateTime.Now,
                            Latitude = 3.0000,
                            Longitude = 6.0000,
                            MeterSn = item.meterSn,
                            Status = item.state,
                            PowerStatus = item.SwitchSta == "1" ? "ON" : "OFF",
                            TotalUsageAccum = Convert.ToDouble(item.EPI)
                        };

                        await unitOfWork.MeterDataRepository.Add(newDevice);
                        await unitOfWork.CommitAsync();
                    }
                    else
                    {
                        device.LastUpdated = DateTime.Now;
                        var update = Builders<Meter>.Update
                                        .Set("LastUpdated", device.LastUpdated)
                                        .Set("PowerStatus", item.SwitchSta == "1" ? "ON" : "OFF")
                                        .Set("TotalUsageAccum", Convert.ToDouble(item.EPI));
                        await unitOfWork.MeterDataRepository.Update(update, "MeterSn", device.MeterSn!);
                        await unitOfWork.CommitAsync();

                    }

                    await unitOfWork.TimeDataRepository.AddBulk(model!);
                    await unitOfWork.CommitAsync();
                    var filterSub = Builders<Subscription>.Filter;
                    var querySub = filterSub.Eq(x => x.MeterSn, item.meterSn);
                    var subscription = await unitOfWork.SubscriptionRepository.GetAll(querySub);
                    if (subscription.FirstOrDefault() == null)
                    {
                        //ShutOff meter
                        var command = new Command()
                        {
                            MeterSn = item.meterSn,
                            GatewaySn = item.gatewaySn,
                            Method = "FORCESWITCH",
                            Value = new Value() { ForceSwitch = 0 }
                        };
                        await commandController.Fire(command);
                    }
                    else
                    {
                        if (!(subscription!.FirstOrDefault()!.Active))
                        {
                            //ShutOff meter
                            var command = new Command()
                            {
                                MeterSn = item.meterSn,
                                GatewaySn = item.gatewaySn,
                                Method = "FORCESWITCH",
                                Value = new Value() { ForceSwitch = 0 }
                            };
                            await commandController.Fire(command);
                        }
                        else
                        {
                            var sub=subscription.FirstOrDefault();
                            var reconciledValue= sub!.InitialValue - device!.TotalUsageAccum;
                            sub.Balance = sub.Balance-reconciledValue;

                            var subUpdate = Builders<Subscription>.Update
                                            .Set("Balance", sub.Balance);
                            await unitOfWork.SubscriptionRepository.Update(subUpdate, "MeterSn", device.MeterSn!);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            return Ok(new
            {
                status = true,
                message = $"Successful",
                data = model
            });
        }


        [HttpPost]
        [Route("StatusUpdate")]
        public async Task<IActionResult> StatusUpdate()
        {
            try
            {

                var filter = Builders<Meter>.Filter;
                var query = filter.Empty;

                var devices = await unitOfWork.MeterDataRepository.GetAll(query);

                foreach (var device in devices)
                {
                    if (DateTimeHelper.HasNotReportedInLastTwoMinutes(Convert.ToDateTime(device.LastUpdated)))
                    {
                     
                        var update = Builders<Meter>.Update
                                        .Set("LastUpdated", DateTime.Now)
                                        .Set("Status", "OFFLINE");
                        await unitOfWork.MeterDataRepository.Update(update, "MeterSn", device!.MeterSn!);
                        await unitOfWork.CommitAsync();

                    }

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            return Ok(new
            {
                status = true,
                message = $"Successful",
            });
        }

    }
}
