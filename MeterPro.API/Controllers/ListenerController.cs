using MeterPro.DATA.DAL;
using MeterPro.DATA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using Newtonsoft.Json;

namespace MeterPro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListenerController : ControllerBase
    {
        private static UnitOfWork unitOfWork;
        ListenerController(UnitOfWork unitOfWork)
        {
            unitOfWork = unitOfWork;
            
        }

        public static void StartListening(MqttClient client)
        {
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceivedAsync;

        }
        static void client_MqttMsgPublishReceivedAsync(object sender, MqttMsgPublishEventArgs e)
        {
            string meterSn = e.Topic.ToString().Split("/")[3];
            string payload = System.Text.Encoding.Default.GetString(e.Message).Replace(meterSn, "DeviceData");
            if (payload.Contains("login") || e.Topic.ToString().Contains("data/up/NzMyNDQ5MDQ1ODk4MTI5NDA4"))
            {
                //ProcessAck(meterSn);
            }
            if (e.Topic.ToString().Contains("data/up/NzMyNDQ5MDQ1ODk4MTI5NDA4"))
            {
                //string payloadData = payload.Replace(meterSn, "DeviceData");
                //ProcessAck(meterSn);

                Console.WriteLine("Received `{0}` from `{1}` topic", payload, e.Topic.ToString());
                try
                {
                    var data = JsonConvert.DeserializeObject<UpdateMessage>(payload);
                    var deviceData = data!.Reported.DeviceData;
                    unitOfWork.DeviceDataRepository.Add(deviceData);
                    unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {

                }


            }
        }

    }
}
