using MeterPro.DATA.DAL;
using MeterPro.DATA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
               
               await _unitOfWork.TimeDataRepository.AddBulk(model);
                await _unitOfWork.CommitAsync();
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
