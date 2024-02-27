using eSya.GenerateToken.DL.Repository;
using eSya.GenerateToken.DO;
using eSya.GenerateToken.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.GenerateToken.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenGenerationController : ControllerBase
    {
        private readonly ITokenGenerationRepository _tokenGenerationRepository;

        public TokenGenerationController(ITokenGenerationRepository tokenGenerationRepository)
        {
            _tokenGenerationRepository = tokenGenerationRepository;

        }
        #region KIOSK/Tab based
        /// <summary>
        /// Getting All ActiveToken Types.
        /// UI Reffered - Token Generation
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllConfigureTokens()
        {
            var tokens = await _tokenGenerationRepository.GetAllConfigureTokens();
            return Ok(tokens);
        }

        /// <summary>
        /// Generate Generate Token.
        /// UI Reffered - Token Generation
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GenerateToken(DO_TokenGeneration obj)
        {
            var msg = await _tokenGenerationRepository.GenerateToken(obj);
            return Ok(msg);
        }
        #endregion

        #region Mobile based
        /// <summary>
        /// Generate OTP .
        /// UI Reffered - Token Generation Mobile based
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GenerateOTP(DO_OTP obj)
        {
            var msg = await _tokenGenerationRepository.GenerateOTP(obj);
            return Ok(msg);
        }
        /// <summary>
        /// Validate OTP .
        /// UI Reffered - Token Generation Mobile based
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ConfirmOTP(DO_OTP obj)
        {
            var msg = await _tokenGenerationRepository.ConfirmOTP(obj);
            return Ok(msg);
        }
        /// <summary>
        /// Getting Token Detail By Mobile
        /// UI Reffered - Token Generation Mobile based
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetTokenDetailByMobile(int businessKey, int isdCode, string mobileNumber)
        {
            var tdetails = await _tokenGenerationRepository.GetTokenDetailByMobile(businessKey, isdCode, mobileNumber);
            return Ok(tdetails);
        }
        #endregion
    }
}
