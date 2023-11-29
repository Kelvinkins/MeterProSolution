using MeterPro.DATA.DAL;
using MeterPro.DATA.Models;
using MeterPro.MQTT.Logics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using uPLibrary.Networking.M2Mqtt;
using MeterPro.DATA.CommandModels;
using MeterPro.DATA.AuthModels;
using MeterPro.DATA.Enums;

namespace MeterPro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetersController : ControllerBase
    {

        private readonly UnitOfWork unitOfWork;
        private readonly CommandsController commandController;

        public MetersController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            commandController = new CommandsController(this.unitOfWork);

        }

        [HttpPost]
        [Route("AddDeviceData")]

        public async Task<IActionResult> AddDeviceData(DeviceData deviceData)
        {

            var filter = Builders<Meter>.Filter;
            var query = filter.Eq(x => x.MeterSn, deviceData.MeterSn);

            var device = unitOfWork.MeterDataRepository.GetAll(query).Result.FirstOrDefault();
            if (device == null)
            {
                //Enroll the device
                var newDevice = new Meter()
                {
                    DateEnrolled = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    Latitude = 3.0000,
                    Longitude = 6.0000,
                    MeterSn = deviceData.MeterSn,
                    Status = deviceData.State,
                    PowerStatus = deviceData.SwitchSta == "1" ? "ON" : "OFF",
                    TotalUsageAccum = Convert.ToDecimal(deviceData.EPI)
                };

                await unitOfWork.MeterDataRepository.Add(newDevice);
                await unitOfWork.CommitAsync();
            }
            else
            {
                device.LastUpdated = DateTime.Now;
                var update = Builders<Meter>.Update
                                .Set("LastUpdated", device.LastUpdated)
                                .Set("PowerStatus", deviceData.SwitchSta == "1" ? "ON" : "OFF")
                                .Set("TotalUsageAccum", Convert.ToDouble(deviceData.EPI));
                await unitOfWork.MeterDataRepository.Update(update, "MeterSn", device.MeterSn!);
                await unitOfWork.CommitAsync();

            }
            await unitOfWork.DeviceDataRepository.Add(deviceData);
            await unitOfWork.CommitAsync();

            var filterSub = Builders<Subscription>.Filter;
            var querySub = filterSub.Eq(x => x.MeterSn, deviceData.MeterSn);
            var subscription = await unitOfWork.SubscriptionRepository.GetAll(querySub);
            if (subscription.FirstOrDefault() == null && device!.PowerStatus == "ON")
            {

                //ShutOff meter
                var command = new Command()
                {
                    MeterSn = deviceData.MeterSn,
                    Method = "FORCESWITCH",
                    Value = new Value() { ForceSwitch = 0 }
                };
                await commandController.FireV2(command);
                device.LastUpdated = DateTime.Now;
                var update = Builders<Meter>.Update
                                .Set("LastUpdated", device.LastUpdated)
                                .Set("ShutOffBy", ShutOffBy.System);
                await unitOfWork.MeterDataRepository.Update(update, "MeterSn", device.MeterSn!);
                await unitOfWork.CommitAsync();
            }
            else
            {


                var sub = subscription.FirstOrDefault();
                if (device!.TotalUsageAccum > sub!.ValueTrend)
                {
                    var reconciledValue = device!.TotalUsageAccum - sub!.InitialValue;
                    sub.Balance = Math.Round(sub.SubscriptionValue - reconciledValue, 2);

                    var subUpdate = Builders<Subscription>.Update
                                    .Set("Balance", sub.Balance)
                                    .Set("ValueTrend", device.TotalUsageAccum);
                    await unitOfWork.SubscriptionRepository.Update(subUpdate, "MeterSn", device.MeterSn!);
                    await unitOfWork.CommitAsync();
                }

                if (sub.Balance <= 0 && device.PowerStatus == "ON")
                {

                    var command = new Command()
                    {
                        MeterSn = deviceData.MeterSn,
                         Method = "FORCESWITCH",
                        Value = new Value() { ForceSwitch = 0 }
                    };
                    await commandController.FireV2(command);
                    device.LastUpdated = DateTime.Now;
                    var update = Builders<Meter>.Update
                                    .Set("LastUpdated", device.LastUpdated)
                                    .Set("ShutOffBy", ShutOffBy.System);
                    await unitOfWork.MeterDataRepository.Update(update, "MeterSn", device.MeterSn!);
                    await unitOfWork.CommitAsync();
                }
                else
                {
                    if (sub.Balance > 0 && device.PowerStatus == "OFF")
                    {
                        var command = new Command()
                        {
                            MeterSn = deviceData.MeterSn,
                            Method = "FORCESWITCH",
                            Value = new Value() { ForceSwitch = 1 }
                        };
                        await commandController.FireV2(command);
                    }
                }
            }

            return Ok();
        }

        //[HttpGet]
        //[Route("Test")]
        //public async Task<IActionResult> Test()
        //{
        //    string broker = "kd7e91be.ala.us-east-1.emqxsl.com";
        //    int port = 8883;
        //    //string topic = "sys/dev/NzIxOTYzNTc4MDEwNDMxNDg4/12204154140174";
        //    //string topic = "Csharp/mqtt";

        //    string clientId = Guid.NewGuid().ToString();
        //    // If the broker requires authentication, set the username and password
        //    string username = "testmeter1";
        //    string password = "test123";
        //    //string username = "emqx";
        //    //string password = "public";
        //    MqttClient client = BrokerService.ConnectMQTT(broker, port, clientId, username, password);

        //    var serializerSettings = new JsonSerializerSettings
        //    {
        //        ContractResolver = new CamelCasePropertyNamesContractResolver()
        //    };

        //    var login = new LoginModelV2()
        //    {

        //        msgid = 567,
        //        method = "login",
        //        res = 1,
        //        sn = "12204154140174"
        //    };
        //    var loginData = JsonConvert.SerializeObject(login, settings: serializerSettings);   
        //    BrokerService.Subscribe(client, "sys/dev/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174");//Subscribe to login ack
        //    BrokerService.SendCommand(client, "sys/dev/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174",loginData);//login


        //    var loginAck = new LoginModelV2()
        //    {

        //        msgid = 567,
        //        method = "login",
        //        res = 1,
        //        sn = "12204154140174"
        //    };
        //    BrokerService.Subscribe(client, "sys/server/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174");//Subscribe to login ack
        //    var loginAckData = JsonConvert.SerializeObject(loginAck, settings: serializerSettings);
        //    BrokerService.SendCommand(client, "sys/server/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174", loginAckData);//acknowledge login

        //    BrokerService.Subscribe(client, "data/up/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174");//Subscribe to data topic
        //    BrokerService.SendCommand(client, "sys/server/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174", loginAckData);//acknowledge login

        //    return Ok();
        //}

            [HttpGet]
        [Route("GetMeters")]
        public async Task<IActionResult> GetMeters()
        {
            var filter = Builders<Meter>.Filter;
            var query = filter.Empty;
            var meters = await unitOfWork.MeterDataRepository.GetAll(query);
            return Ok(meters);
        }

        [HttpGet]
        [Route("GetMeterLogs")]
        public async Task<IActionResult> GetMeterLogs(string meterSn)
        {
            var filter = Builders<TimeData>.Filter;
            var query = filter.Eq(x => x.meterSn,meterSn);
            var timeDatas = await unitOfWork.TimeDataRepository.GetAll(query);
            return Ok(timeDatas);
        }


    }
}
