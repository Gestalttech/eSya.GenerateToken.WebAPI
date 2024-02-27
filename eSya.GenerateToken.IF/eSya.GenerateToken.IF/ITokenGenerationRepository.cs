using eSya.GenerateToken.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.GenerateToken.IF
{
    public interface ITokenGenerationRepository
    {
        #region KIOSK/Tab based
        Task<List<DO_TokenConfiguration>> GetAllConfigureTokens();
        Task<DO_ReturnParameter> GenerateToken(DO_TokenGeneration obj);
        #endregion
        #region Mobile based
        Task<DO_ReturnParameter> GenerateOTP(DO_OTP obj);
        Task<DO_ReturnParameter> ConfirmOTP(DO_OTP obj);
        Task<List<DO_TokenGeneration>> GetTokenDetailByMobile(int businessKey, int isdCode, string mobileNumber);
        #endregion
    }
}
