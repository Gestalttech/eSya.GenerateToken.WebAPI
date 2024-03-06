using eSya.GenerateToken.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.GenerateToken.IF
{
    public interface ITokenManagementRepository
    {
        #region Token Calling
        Task<List<DO_Floor>> GetFloorsbyFloorId(int codetype);
        Task<List<DO_CounterMapping>> GetCounterNumbersbyFloorId(int floorId);
        Task<List<DO_CounterMapping>> GetTokenTypeByCounter(int businessKey, string counterNumber);
        Task<List<DO_TokenGeneration>> GetTokenDetailByTokenType(int businessKey, string tokenprefix);
        Task<DO_ReturnParameter> UpdateCallingToken(DO_TokenGeneration obj);
        Task<DO_ReturnParameter> UpdateTokenToHold(DO_TokenGeneration obj);
        Task<DO_ReturnParameter> UpdateTokenToRelease(DO_TokenGeneration obj);
        Task<DO_ReturnParameter> UpdateTokenStatusToCompleted(DO_TokenGeneration obj);
        Task<DO_ReturnParameter> UpdateToCallingNextToken(DO_TokenGeneration obj);
        Task<DO_ReturnParameter> UpdateCallingConfirmation(DO_TokenGeneration obj);
        #endregion
    }
}
