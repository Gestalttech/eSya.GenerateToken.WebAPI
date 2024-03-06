using eSya.GenerateToken.DL.Repository;
using eSya.GenerateToken.DO;
using eSya.GenerateToken.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.GenerateToken.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenManagementController : ControllerBase
    {
        private readonly ITokenManagementRepository _tokenManagementRepository;

        public TokenManagementController(ITokenManagementRepository tokenManagementRepository)
        {
            _tokenManagementRepository = tokenManagementRepository;

        }

        #region Token Calling
        /// <summary>
        /// Get Token Details By Type
        /// UI Reffered -Token Management Counter
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetFloorsbyFloorId(int codetype)
        {
            var msg = await _tokenManagementRepository.GetFloorsbyFloorId(codetype);
            return Ok(msg);
        }
        /// <summary>
        /// Get Token Details By Type
        /// UI Reffered -Token Management Counter
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCounterNumbersbyFloorId(int floorId)
        {
            var msg = await _tokenManagementRepository.GetCounterNumbersbyFloorId(floorId);
            return Ok(msg);
        }
        /// <summary>
        /// Get Token Details By Type
        /// UI Reffered -Token Management Counter
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetTokenDetailByTokenType(int businessKey, string tokenprefix)
        {
            var msg = await _tokenManagementRepository.GetTokenDetailByTokenType(businessKey, tokenprefix);
            return Ok(msg);
        }

        /// <summary>
        /// Get Token Types By Counter
        /// UI Reffered -Token Management Counter
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetTokenTypeByCounter(int businessKey, string counterNumber)
        {
            var msg = await _tokenManagementRepository.GetTokenTypeByCounter(businessKey, counterNumber);
            return Ok(msg);
        }

       
        /// <summary>
        /// Token Calling
        /// UI Reffered -Token Management Counter 
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateCallingToken(DO_TokenGeneration obj)
        {
            var msg = await _tokenManagementRepository.UpdateCallingToken(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Token Holding
        /// UI Reffered -Token Management Counter 
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateTokenToHold(DO_TokenGeneration obj)
        {
            var msg = await _tokenManagementRepository.UpdateTokenToHold(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Token Releasing
        /// UI Reffered -Token Management Counter 
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateTokenToRelease(DO_TokenGeneration obj)
        {
            var msg = await _tokenManagementRepository.UpdateTokenToRelease(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Token Completing
        /// UI Reffered -Token Management Counter 
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateTokenStatusToCompleted(DO_TokenGeneration obj)
        {
            var msg = await _tokenManagementRepository.UpdateTokenStatusToCompleted(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Next Token Calling
        /// UI Reffered -Token Management Counter 
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateToCallingNextToken(DO_TokenGeneration obj)
        {
            var msg = await _tokenManagementRepository.UpdateToCallingNextToken(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Calling Cconfirmation
        /// UI Reffered -Token Management Counter 
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateCallingConfirmation(DO_TokenGeneration obj)
        {
            var msg = await _tokenManagementRepository.UpdateCallingConfirmation(obj);
            return Ok(msg);
        }
        #endregion
    }
}
