using eSya.GenerateToken.DL.Entities;
using eSya.GenerateToken.DO;
using eSya.GenerateToken.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.GenerateToken.DL.Repository
{
    public class AddonRepository : IAddonRepository
    {
        private readonly IStringLocalizer<AddonRepository> _localizer;
        public AddonRepository(IStringLocalizer<AddonRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Add on Token Type
        public async Task<List<DO_CounterMapping>> GetMappedCounterbyBusinessKey(int businesskey)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {

                    var ds = await db.GtTokm03s.Where(k => k.BusinessKey == businesskey && k.ActiveStatus).Join(db.GtTokm01s.Where(x => x.ActiveStatus),
                               x => x.TokenPrefix.ToUpper().Replace(" ", ""),
                               y => y.TokenPrefix.ToUpper().Replace(" ", ""),
                               (x, y) => new { x, y }).Join(db.GtTokm02s.Where(x => x.ActiveStatus),
                               a => new { a.x.CounterNumber, a.x.FloorId },
                               p => new { p.CounterNumber, p.FloorId },
                               (a, p) => new { a, p }).
                               Join(db.GtEcapcds.Where(x => x.ActiveStatus),
                                b => new { b.p.FloorId },
                               c => new { FloorId = c.ApplicationCode }, (b, c) => new { b, c }).
                               Select(r => new DO_CounterMapping
                               {
                                   BusinessKey = r.b.a.x.BusinessKey,
                                   CounterNumber = r.b.a.x.CounterNumber,
                                   TokenPrefix = r.b.a.x.TokenPrefix,
                                   CounterKey = r.b.a.x.CounterKey,
                                   ActiveStatus = r.b.a.x.ActiveStatus,
                                   TokenType = r.b.a.y.TokenType,
                                   TokenDesc = r.b.a.y.TokenDesc,
                                   FloorId = r.b.a.x.FloorId,
                                   CounterNumberdesc = r.b.a.x.CounterNumber,
                                   FloorName = r.c.CodeDesc,

                               })
                                 .OrderBy(x => x.FloorName).ThenBy(x => x.CounterNumber).ToListAsync();
                    return ds;

                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<List<DO_CounterMapping>> GetAddOnMappedCounters(int businesskey, int floorId, string tokenprefix, string counterNo)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {

                    var ds = await db.GtTokm03s.Where(k => k.BusinessKey == businesskey && k.FloorId == floorId && k.ActiveStatus
                    && k.TokenPrefix.ToUpper().Replace(" ", "") != tokenprefix.ToUpper().Replace(" ", "")).Join(db.GtTokm01s.Where(x => x.ActiveStatus),
                               x => x.TokenPrefix.ToUpper().Replace(" ", ""),
                               y => y.TokenPrefix.ToUpper().Replace(" ", ""),
                               (x, y) => new { x, y }).Join(db.GtTokm02s.Where(x => x.ActiveStatus),
                               a => new { a.x.CounterNumber, a.x.FloorId },
                               p => new { p.CounterNumber, p.FloorId },
                               (a, p) => new { a, p }).
                               Join(db.GtEcapcds.Where(x => x.ActiveStatus),
                                b => new { b.p.FloorId },
                               c => new { FloorId = c.ApplicationCode }, (b, c) => new { b, c }).
                               Select(r => new DO_CounterMapping
                               {
                                   BusinessKey = r.b.a.x.BusinessKey,
                                   CounterNumber = r.b.a.x.CounterNumber,
                                   TokenPrefix = r.b.a.x.TokenPrefix,
                                   CounterKey = r.b.a.x.CounterKey,
                                   ActiveStatus = r.b.a.x.ActiveStatus,
                                   TokenType = r.b.a.y.TokenType,
                                   TokenDesc = r.b.a.y.TokenDesc,
                                   FloorId = r.b.a.x.FloorId,
                                   CounterNumberdesc = r.b.a.x.CounterNumber,
                                   FloorName = r.c.CodeDesc,

                               }).ToListAsync();
                    foreach (var obj in ds)
                    {
                        //GtTokm04 linked = db.GtTokm04s.Where(c => c.BusinessKey == businesskey && c.CounterKey.ToUpper().Replace(" ", "") == obj.CounterKey.ToUpper().Replace(" ", "")
                        //&& c.AddOn.ToUpper().Replace(" ", "") == obj.TokenPrefix.ToUpper().Replace(" ", "")).FirstOrDefault();

                        var existinkey = (tokenprefix + "-" + floorId + "-" + counterNo).ToString();

                        GtTokm04 linked = db.GtTokm04s.Where(c => c.BusinessKey == businesskey && c.CounterKey.ToUpper().Replace(" ", "") == existinkey.ToUpper().Replace(" ", "")
                       && c.AddOn.ToUpper().Replace(" ", "") == obj.TokenPrefix.ToUpper().Replace(" ", "")).FirstOrDefault();

                        if (linked != null)
                        {
                            obj.ActiveStatus = linked.ActiveStatus;
                        }
                        else
                        {
                            obj.ActiveStatus = false;
                        }
                    }
                    ds = ds.OrderBy(x => x.FloorName).ThenBy(x => x.CounterNumber).ToList();
                    return ds;

                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateAddOnCounters(List<DO_CounterAddOn> obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var _link in obj)
                        {
                            var _linkExist = db.GtTokm04s.Where(w => w.BusinessKey == _link.BusinessKey && w.CounterKey.ToUpper().Replace(" ", "") == _link.CounterKey.ToUpper().Replace(" ", "")
                            && w.AddOn.ToUpper().Replace(" ", "") == _link.AddOn.ToUpper().Replace(" ", "")).FirstOrDefault();
                            if (_linkExist != null)
                            {

                                _linkExist.ActiveStatus = _link.ActiveStatus;
                                _linkExist.ModifiedBy = _link.UserID;
                                _linkExist.ModifiedOn = System.DateTime.Now;
                                _linkExist.ModifiedTerminal = _link.TerminalID;


                            }
                            else
                            {

                                var _objlink = new GtTokm04
                                {
                                    BusinessKey = _link.BusinessKey,
                                    CounterKey = _link.CounterKey,
                                    AddOn = _link.AddOn,
                                    FormId = _link.FormId,
                                    ActiveStatus = _link.ActiveStatus,
                                    CreatedBy = _link.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = _link.TerminalID
                                };
                                db.GtTokm04s.Add(_objlink);


                            }
                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        return new DO_ReturnParameter() { Status = false, Message = ex.Message };
                    }
                }
            }
        }
        #endregion

    }
}
