using eSya.GenerateToken.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.GenerateToken.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonDataController : ControllerBase
    {
        private readonly ICommonDataRepository _addonRepository;

        public CommonDataController(ICommonDataRepository addonRepository)
        {
            _addonRepository = addonRepository;

        }
        [HttpGet]
        public async Task<IActionResult> GetApplicationCodesByCodeType(int codetype)
        {
            var floors = await _addonRepository.GetApplicationCodesByCodeType(codetype);
            return Ok(floors);
        }
       
        [HttpGet]
        public async Task<IActionResult> GetBusinessKey()
        {
            var locs = await _addonRepository.GetBusinessKey();
            return Ok(locs);
        }
    }
}
