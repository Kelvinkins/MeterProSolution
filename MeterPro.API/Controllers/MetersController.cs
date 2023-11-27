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

        [HttpPost]
        [Route("AddDeviceData")]

        public async Task<IActionResult> AddDeviceData(DeviceData deviceData)
        {
            await _unitOfWork.DeviceDataRepository.Add(deviceData);
            await _unitOfWork.CommitAsync();
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
            var meters = await _unitOfWork.MeterDataRepository.GetAll(query);
            return Ok(meters);
        }

        [HttpGet]
        [Route("GetMeterLogs")]
        public async Task<IActionResult> GetMeterLogs(string meterSn)
        {
            var filter = Builders<TimeData>.Filter;
            var query = filter.Eq(x => x.meterSn,meterSn);
            var timeDatas = await _unitOfWork.TimeDataRepository.GetAll(query);
            return Ok(timeDatas);
        }


    }
}
