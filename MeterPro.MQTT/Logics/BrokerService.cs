using MeterPro.DATA.AuthModels;
using MeterPro.DATA.DAL;
using MeterPro.DATA.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MeterPro.MQTT.Logics
{
    public class BrokerService
    {

        private static  UnitOfWork unitOfWork;
        public static MqttClient client;

        BrokerService(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }




        public static void StartMQTT()
        {
            string broker = "kd7e91be.ala.us-east-1.emqxsl.com";
            int port = 8883;
            string clientId = "API-SERVER";
            string username = "testmeter1";
            string password = "test123";
            client = new MqttClient(broker, port, true, MqttSslProtocols.None, null, null);
            try
            {
                client.Connect(clientId, username, password);

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            if (client.IsConnected)
            {


                //var serializerSettings = new JsonSerializerSettings
                //{
                //    ContractResolver = new CamelCasePropertyNamesContractResolver()
                //};

                //var login = new LoginModelV2()
                //{

                //    msgid = 567,
                //    method = "login",
                //    res = 0,
                //    sn = "12204154140174"
                //};
                //var loginData = JsonConvert.SerializeObject(login, settings: serializerSettings);
                //BrokerService.Subscribe(client, "sys/dev/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174");//Subscribe to login ack
                //BrokerService.SendCommand(client, "sys/dev/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174", loginData);//login


                //var loginAck = new LoginModelV2()
                //{

                //    msgid = 567,
                //    method = "login",
                //    res = 1,
                //    sn = "12204154140174"
                //};
                //BrokerService.Subscribe(client, "sys/server/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174");//Subscribe to login ack
                //var loginAckData = JsonConvert.SerializeObject(loginAck, settings: serializerSettings);
                //BrokerService.SendCommand(client, "sys/server/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174", loginAckData);//acknowledge login

                //BrokerService.Subscribe(client, "data/up/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174");//Subscribe to data topic
                //BrokerService.SendCommand(client, "sys/server/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174", loginAckData);//acknowledge login
            }


        }

        public static void SendCommand(MqttClient client, string topic, string msg)
        {

            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg));
            Console.WriteLine("Send `{0}` to topic `{1}`", msg, topic);
        }


        public static void Subscribe(MqttClient client, string topic)
        {
            //client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        }
        public static void LoginDevice(string meterSn)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var login = new LoginModelV2()
            {

                msgid = 567,
                method = "login",
                res = 0,
                sn = meterSn
            };
            var loginData = JsonConvert.SerializeObject(login, settings: serializerSettings);
            BrokerService.Subscribe(client, "sys/dev/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174");//Subscribe to login ack
            BrokerService.SendCommand(client, "sys/dev/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174", loginData);//login


            var loginAck = new LoginModelV2()
            {

                msgid = 567,
                method = "login",
                res = 1,
                sn = meterSn
            };
            BrokerService.Subscribe(client, "sys/server/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174");//Subscribe to login ack
            var loginAckData = JsonConvert.SerializeObject(loginAck, settings: serializerSettings);
            BrokerService.SendCommand(client, "sys/server/NzMyNDQ5MDQ1ODk4MTI5NDA4/12204154140174", loginAckData);//acknowledge login
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
                    SendDeviceData(deviceData,meterSn);
                }
                catch (Exception ex)
                {

                }


            }
        }
        private static HttpClient _httpClient = new HttpClient();


        private static async Task<bool> SendDeviceData(DeviceData data,string meterSn)
        {
            try
            {
                // Prepare the request payload if needed
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var json = JsonConvert.SerializeObject(data, settings: serializerSettings);
                var requestContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Send the POST request
                HttpResponseMessage response = await _httpClient.PostAsync($"https://meterproapi.azurewebsites.net/api/Meters/AddDeviceData?meterSn={meterSn}", requestContent);

                if (response.IsSuccessStatusCode)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        static void ProcessAck(string meterSn)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var loginAck = new LoginModelV2()
            {

                msgid = 567,
                method = "update",
                res = 1,
                sn = meterSn
            };
            var loginAckData = JsonConvert.SerializeObject(loginAck, settings: serializerSettings);
            BrokerService.SendCommand(BrokerService.client, $"sys/server/NzMyNDQ5MDQ1ODk4MTI5NDA4/{meterSn}", loginAckData);//acknowledge login
        }

      

    }
}
