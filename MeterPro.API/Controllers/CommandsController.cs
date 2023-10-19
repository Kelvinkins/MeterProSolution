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

namespace MeterPro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {

        private readonly HttpClient _httpClient = new HttpClient();

        [HttpPost]
        [Route("Fire")]
        public async Task<IActionResult> Fire([FromBody] Command? command)
        {
            string apiUrl = "https://iot.acrel.cn/basic/currency/auth_user/login";
            string password = "1234567890123456";
            string offset = "1234567890123456";
            var login = new LoginModel()
            {
                LoginName = "admin",
                Password = "acrel001"
            };
            string jsonBody = JsonConvert.SerializeObject(login);

            var encryptedCred =Cryptography.Encrypt(jsonBody, password, offset);
            // Create form data
            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("params", encryptedCred)
            });

            // Send the POST request
            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, formData);

            if (response.IsSuccessStatusCode)
            {
                // Request was successful, you can handle the response here
                string responseContent = await response.Content.ReadAsStringAsync();
                // You can parse or process the response as needed.
                return Ok(responseContent);

                
            }
            else
            {
                // Request failed, handle the error
                return BadRequest("Request failed with status code: " + response.StatusCode);
            }
        }
    }
}
