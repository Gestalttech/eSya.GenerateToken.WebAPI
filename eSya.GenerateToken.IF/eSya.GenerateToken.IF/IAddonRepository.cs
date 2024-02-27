using eSya.GenerateToken.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.GenerateToken.IF
{
    public interface IAddonRepository
    {
        #region Add on Token Type
        Task<List<DO_CounterMapping>> GetMappedCounterbyBusinessKey(int businesskey);
        Task<List<DO_CounterMapping>> GetAddOnMappedCounters(int businesskey, int floorId, string tokenprefix, string counterNo);
        Task<DO_ReturnParameter> InsertOrUpdateAddOnCounters(List<DO_CounterAddOn> obj);
        #endregion
    }
}
