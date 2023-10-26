using MeterPro.DATA.DAL;
using MeterPro.DATA.Models;
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
        private readonly UnitOfWork _unitOfWork;

        public WebHooksController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                   
                    var device = _unitOfWork.MeterDataRepository.GetAll(query).Result.FirstOrDefault();
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
                            PowerStatus = "ON"

                        };

                        await _unitOfWork.MeterDataRepository.Add(newDevice);
                        await _unitOfWork.CommitAsync();
                    }
                    else
                    {
                        device.LastUpdated = DateTime.Now;
                        var update = Builders<Meter>.Update
                                        .Set("LastUpdated", device.LastUpdated);
                        await _unitOfWork.MeterDataRepository.Update(update, "MeterSn", device.MeterSn!);
                    }

                    await _unitOfWork.TimeDataRepository.AddBulk(model!);
                    await _unitOfWork.CommitAsync();
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

    }
}
