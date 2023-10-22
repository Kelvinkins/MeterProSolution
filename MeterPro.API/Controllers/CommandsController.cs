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

namespace MeterPro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {

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
                    // Parse and return the response content
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
        [Route("Fire")]
        public async Task<IActionResult> Fire([FromBody] Command? command)
        {
            string apiUrl = "https://iot.acrel-eem.com/basic/currency/auth_user/login";
            string password = "1234567890123456";
            string offset = "1234567890123456";
            //var login = new LoginModel()
            //{
            //    LoginName = "Myboard",
            //    PassWord = "Tobe2023"
            //};
            //string jsonBody = "{\"LoginName\": \"Myboard\",\"PassWord\": \"Tobe2023\"}";

            //var encryptedCred =Cryptography.Encrypt(jsonBody, password, offset);
            // Create form data
            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("params", "cuwwbtodRuK/Zv1QiWvH2qVkUmZTX1+X1ieesX4QxCKnkgMZw8t/hppPj2kwiCXx")
            });

            // Send the POST request
            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, formData);

            if (response.IsSuccessStatusCode)
            {
                // Request was successful, you can handle the response here
                string responseContent = await response.Content.ReadAsStringAsync();
                // You can parse or process the response as needed.
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                var result= await SendPostRequestAsync(tokenResponse.data.token, command);
                return Ok(result);
                



            }
            else
            {
                // Request failed, handle the error
                return BadRequest("Request failed with status code: " + response.StatusCode);
            }
        }
    }
}
