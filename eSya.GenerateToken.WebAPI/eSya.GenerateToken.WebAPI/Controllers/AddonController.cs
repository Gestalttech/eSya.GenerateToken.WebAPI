using eSya.GenerateToken.DO;
using eSya.GenerateToken.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.GenerateToken.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AddonController : ControllerBase
    {
        private readonly IAddonRepository _addonRepository;

        public AddonController(IAddonRepository addonRepository)
        {
            _addonRepository = addonRepository;

        }

        #region Add on Token Type
        /// <summary>
        /// Getting already Mapped Counters.
        /// UI Reffered - Add on
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMappedCounterbyBusinessKey(int businesskey)
        {
            var mcounters = await _addonRepository.GetMappedCounterbyBusinessKey(businesskey);
            return Ok(mcounters);
        }

        /// <summary>
        /// Getting Add On Counters.
        /// UI Reffered - Add on
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAddOnMappedCounters(int businesskey, int floorId, string tokenprefix, string counterNo)
        {
            var addoncounters = await _addonRepository.GetAddOnMappedCounters(businesskey, floorId, tokenprefix, counterNo);
            return Ok(addoncounters);
        }
        /// <summary>
        /// Insert Or Update Add On Counters.
        /// UI Reffered -Add on
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateAddOnCounters(List<DO_CounterAddOn> obj)
        {
            var msg = await _addonRepository.InsertOrUpdateAddOnCounters(obj);
            return Ok(msg);

        }
        #endregion
    }
}
