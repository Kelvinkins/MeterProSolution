using MeterPro.DATA.CommandModels;
using MeterPro.DATA.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using MongoDB.Libmongocrypt;
using System.Text;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;
using MeterPro.DATA.AuthModels;
using System.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using MeterPro.DATA.DAL;
using MongoDB.Driver;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MeterPro.DATA.Models;
using MeterPro.DATA.Enums;
using MeterPro.MQTT.Logics;
using MeterPro.DATA.ViewModels;
using System.ComponentModel.Design;

namespace MeterPro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;

        public CommandsController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        private readonly HttpClient _httpClient = new HttpClient();

        //public MyHttpClient()
        //{
        //    // Initialize HttpClient
        //    client = new HttpClient();
        //    client.BaseAddress = new Uri("https://iot.acrel.cn/basic/currency/entry/home/control");
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //}

        private async Task<string> SendPostRequestAsync(string token, Command command)
        {
            try
            {
                // Prepare the request payload if needed
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var json=JsonConvert.SerializeObject(command,settings: serializerSettings);
                var requestContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Add the token to the request headers
                _httpClient.DefaultRequestHeaders.Add("token", token);

                // Send the POST request
                HttpResponseMessage response = await _httpClient.PostAsync("https://iot.acrel-eem.com/basic/currency/entry/home/control", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Handle the error or return an appropriate response
                    return "Request failed with status code: " + response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return "An error occurred: " + ex.Message;
            }
        }


        [HttpPost]
        [Route("FireV2")]
        public async Task<IActionResult> FireV2([FromBody] Command? command)
        {
            try
            {
                var filter = Builders<Meter>.Filter;
                var query = filter.Eq(x => x.MeterSn, command!.MeterSn);

                var device = unitOfWork.MeterDataRepository.GetAll(query).Result.FirstOrDefault();
                if ((device!.ShutOffBy == ShutOffBy.System || device!.ShutOffBy == ShutOffBy.Admin) && command!.Value!.ForceSwitch == 1)
                {
                    return Unauthorized($"Sorry, the device was shutOff by {device.ShutOffBy}");

                }


                var cmd = new SwitchCommand()
                {
                    Method = "operate",
                    msgid = Guid.NewGuid().ToString(),
                    sn = command!.MeterSn,
                    timestamp = DateTime.Now.Ticks,
                    payload = new Payload()
                    {
                        addr = command.MeterSn,
                        do1 = Convert.ToInt32(command.Value!.ForceSwitch),
                        ForceSwitch = Convert.ToInt32(command.Value!.ForceSwitch),
                        meterName = "DDSY1352-IOT",
                        method = "FORCESWITCH"

                    }
                };
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var data = JsonConvert.SerializeObject(cmd, settings: serializerSettings);
                BrokerService.Subscribe(BrokerService.client, $"indicate/server/NzMyNDQ5MDQ1ODk4MTI5NDA4/{command.MeterSn}");//Subscribe to data topic
                BrokerService.SendCommand(BrokerService.client, $"indicate/server/NzMyNDQ5MDQ1ODk4MTI5NDA4/{command.MeterSn}", data);
                var commandResponse = new CommandResponse()
                {
                    data = data,
                    ErrorCode = "No Error",
                    ErrorMsg = "No Errors",
                    Success = "1"

                };

                if (command!.ShutOffBy == null)
                {
                    command.ShutOffBy = ShutOffBy.User;
                }

                if (commandResponse!.Success == "1")
                {


                    var update = Builders<Meter>.Update
                                    .Set("LastUpdated", device!.LastUpdated)
                                    .Set("ShutOffBy", command.ShutOffBy)
                                    .Set("PowerStatus", command.Value!.ForceSwitch == 1 ? "ON" : "OFF");
                    await unitOfWork.MeterDataRepository.Update(update, "MeterSn", device.MeterSn!);
                    await unitOfWork.CommitAsync();

                }
                else
                {

                    if (DateTimeHelper.HasNotReportedInLastTenMinutes(Convert.ToDateTime(device!.LastUpdated)))
                    {
                        var update = Builders<Meter>.Update
                                    .Set("LastUpdated", DateTime.Now)
                                    .Set("Status", "OFFLINE");
                        await unitOfWork.MeterDataRepository.Update(update, "MeterSn", device!.MeterSn!);
                        await unitOfWork.CommitAsync();
                    }
                }
                return Ok(commandResponse);
            }
            catch (Exception ex)
            {
                var response = new CommandResponse()
                {
                    data = ex,
                    ErrorCode = ex.HResult.ToString(),
                    ErrorMsg = ex.Message,
                    Success = "0"

                };
                return Ok(response);

            }
        }
        

        [HttpPost]
        [Route("Fire")]
        public async Task<IActionResult> Fire([FromBody] Command? command)
        {
            string apiUrl = "https://iot.acrel-eem.com/basic/currency/auth_user/login";
            string password = "1234567890123456";
            string offset = "1234567890123456";
          
            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("params", "cuwwbtodRuK/Zv1QiWvH2qVkUmZTX1+X1ieesX4QxCKnkgMZw8t/hppPj2kwiCXx")
            });

            var filter = Builders<Meter>.Filter;
            var query = filter.Eq(x => x.MeterSn, command!.MeterSn);

            var device = unitOfWork.MeterDataRepository.GetAll(query).Result.FirstOrDefault();
            if ((device!.ShutOffBy == ShutOffBy.System || device!.ShutOffBy == ShutOffBy.Admin) && command!.Value!.ForceSwitch==0)
            {
                return Unauthorized($"Sorry, the device was shutOff by {device.ShutOffBy}");

            }
            // Send the POST request
            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, formData);

            if (response.IsSuccessStatusCode)
            {
                if (command!.ShutOffBy == null)
                {
                    command.ShutOffBy=ShutOffBy.User;
                }
                // Request was successful, you can handle the response here
                string responseContent = await response.Content.ReadAsStringAsync();
                // You can parse or process the response as needed.
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                var result= await SendPostRequestAsync(tokenResponse.data.token, command);
                var commandResponse=JsonConvert.DeserializeObject<CommandResponse>(responseContent);
                if (commandResponse!.Success == "1")
                {
                   

                    var update = Builders<Meter>.Update
                                    .Set("LastUpdated", device!.LastUpdated)
                                    .Set("ShutOffBy", command.ShutOffBy)
                                    .Set("PowerStatus", command.Value!.ForceSwitch == 1 ? "ON" : "OFF");
                     await unitOfWork.MeterDataRepository.Update(update, "MeterSn", device.MeterSn!);
                    await unitOfWork.CommitAsync();

                }
                else
                {
         
                    if (DateTimeHelper.HasNotReportedInLastTenMinutes(Convert.ToDateTime(device!.LastUpdated)))
                    {
                        var update = Builders<Meter>.Update
                                    .Set("LastUpdated", DateTime.Now)
                                    .Set("Status", "OFFLINE");
                        await unitOfWork.MeterDataRepository.Update(update, "MeterSn", device!.MeterSn!);
                        await unitOfWork.CommitAsync();
                    }

                }

                return Ok(commandResponse);
                
            }
            else
            {
                // Request failed, handle the error
                return BadRequest("Request failed with status code: " + response.StatusCode);
            }
        }


        [HttpPost]
        [Route("Subscribe")]
        public  IActionResult Subscribe(TopicSubVm model)
        {
            BrokerService.LoginDevice(model.MeterSn!);
            BrokerService.Subscribe(BrokerService.client, $"data/up/NzMyNDQ5MDQ1ODk4MTI5NDA4/{model.MeterSn}");//Subscribe to data topic
            return Ok("Subscribed successfully, please check for data");
        }
    }
}
