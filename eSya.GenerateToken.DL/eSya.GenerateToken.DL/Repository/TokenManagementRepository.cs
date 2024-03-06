using eSya.GenerateToken.DL.Entities;
using eSya.GenerateToken.DO;
using eSya.GenerateToken.IF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.GenerateToken.DL.Repository
{
    public class TokenManagementRepository: ITokenManagementRepository
    {
        #region Token Calling
        public async Task<List<DO_Floor>> GetFloorsbyFloorId(int codetype)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var ds = await db.GtEcapcds.Where(x => x.CodeType == codetype && x.ActiveStatus)
                        .Select(
                          f => new DO_Floor
                          {
                              FloorId = f.ApplicationCode,
                              FloorName = f.CodeDesc
                          }).ToListAsync();

                    return ds;
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public async Task<List<DO_CounterMapping>> GetCounterNumbersbyFloorId(int floorId)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var ds = await db.GtTokm02s.Where(x => x.FloorId == floorId && x.ActiveStatus).Select(
                          f => new DO_CounterMapping
                          {
                              CounterNumber = f.CounterNumber
                          }).ToListAsync();

                    return ds;
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<List<DO_CounterMapping>> GetTokenTypeByCounter(int businessKey, string counterNumber)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {

                    var ds = db.GtTokm03s
                        .Join(db.GtTokm01s,
                        m => m.TokenPrefix,
                        t => t.TokenPrefix,
                        (m, t) => new { m, t })
                        .Where(w => w.m.BusinessKey == businessKey
                                    && w.m.CounterNumber == counterNumber
                                    && w.m.ActiveStatus)
                        .Select(r => new DO_CounterMapping
                        {
                            CounterNumber = r.m.CounterNumber,
                            //TokenType = r.m.TokenType,
                            TokenPrefix = r.m.TokenPrefix,
                            TokenDesc = r.t.TokenDesc,
                            DisplaySequence = r.t.DisplaySequence,

                        }).OrderBy(o => o.DisplaySequence).ToListAsync();

                    return await ds;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<List<DO_TokenGeneration>> GetTokenDetailByTokenType(int businessKey, string tokenprefix)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {

                    var ds = db.GtTokm05s
                    .Join(db.GtTokm01s,
                    d => d.TokenPrefix,
                    m => m.TokenPrefix,
                    (d, m) => new { d, m })
                        .Where(w => w.d.BusinessKey == businessKey
                                    && w.d.TokenDate.Date == System.DateTime.Now.Date
                                    && (w.d.TokenPrefix == tokenprefix || tokenprefix == "A")
                                    && w.d.TokenStatus == "UC"
                                    && w.d.ActiveStatus)
                        //&& !w.d.CallingConfirmation)
                        .Select(r => new DO_TokenGeneration
                        {
                            TokenKey = r.d.TokenKey,
                            //TokenType = r.d.TokenType,
                            TokenPrefix = r.d.TokenPrefix,
                            TokenDate = r.d.TokenDate,
                            SequeueNumber = r.d.SequeueNumber,
                            //TokenHold = r.d.TokenHold,
                            CreatedOn = r.d.CreatedOn,
                            TokenCalling = r.d.TokenCalling,
                            //CallingConfirmation = r.d.CallingConfirmation,
                            ConfirmationUrl = r.m.ConfirmationUrl,
                           QrcodeUrl=r.m.QrcodeUrl,
                        }).OrderBy(o => o.CreatedOn).ToListAsync();

                    return await ds;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateCallingToken(DO_TokenGeneration obj)
        {

            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var dc = db.GtTokm05s.Where(w => w.BusinessKey == obj.BusinessKey
                            && w.TokenDate.Date == System.DateTime.Now.Date
                            && w.TokenPrefix == obj.TokenPrefix
                            && w.TokenKey == obj.TokenKey).FirstOrDefault();

                        if (dc != null)
                        {
                            if (dc.TokenCalling && dc.CallingCounter != obj.CallingCounter)
                            {
                                dbContext.Rollback();
                                return new DO_ReturnParameter { Status = false, Message = "Token No. " + dc.TokenKey + " is Serving by " + dc.CallingCounter };
                            }
                            //dc.TokenHold = false;
                            dc.TokenCalling = true;
                            dc.CallingCounter = obj.CallingCounter;
                            //dc.CallingOccurence = dc.CallingOccurence + 1;

                            //if (dc.FirstCallingTime == null)
                            //    dc.FirstCallingTime = System.DateTime.Now;
                            dc.TokenCallingTime = System.DateTime.Now;
                            dc.CounterKey = obj.CounterKey;
                            dc.ModifiedBy = obj.UserID;
                            dc.ModifiedOn = System.DateTime.Now;
                            dc.ModifiedTerminal = obj.TerminalID;
                        }

                        var remainingToken = db.GtTokm05s
                           .Where(w => w.BusinessKey == obj.BusinessKey
                               && w.TokenPrefix == obj.TokenPrefix
                               && w.TokenKey != obj.TokenKey
                               && w.TokenDate.Date == System.DateTime.Now.Date
                               && w.CallingCounter == obj.CallingCounter
                               && w.TokenCalling == true);

                        foreach (var t in remainingToken)
                        {
                            t.TokenCalling = false;
                        }




                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter { Status = true, Key = obj.TokenKey };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<DO_ReturnParameter> UpdateTokenToHold(DO_TokenGeneration obj)
        {

            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var dc = db.GtTokm05s.Where(w => w.BusinessKey == obj.BusinessKey
                            && w.TokenDate.Date == System.DateTime.Now.Date
                            && w.TokenPrefix == obj.TokenPrefix
                            && w.TokenKey == obj.TokenKey
                            ).FirstOrDefault();

                        dc.TokenCalling = false;
                        dc.TokenStatus = "HL";
                        dc.HoldOccurrence = dc.HoldOccurrence + 1;

                        //dc.TokenHold = true;
                        //dc.TokenHoldOccurence = dc.TokenHoldOccurence + 1;
                        //dc.TokenHoldingTime = System.DateTime.Now;
                        dc.ModifiedBy = obj.UserID;
                        dc.ModifiedOn = System.DateTime.Now;
                        dc.ModifiedTerminal = obj.TerminalID;

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter { Status = true, Key = obj.TokenKey };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }

        }
        public async Task<DO_ReturnParameter> UpdateTokenToRelease(DO_TokenGeneration obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var dc = db.GtTokm05s.Where(w => w.BusinessKey == obj.BusinessKey
                            && w.TokenDate.Date == System.DateTime.Now.Date
                            && w.TokenPrefix == obj.TokenPrefix
                            && w.TokenKey == obj.TokenKey).FirstOrDefault();

                        //dc.TokenHold = false;
                       //dc.TokenCalling = true; //need to uncomment
                        dc.TokenStatus = "UC";
                        dc.ModifiedBy = obj.UserID;
                        dc.ModifiedOn = System.DateTime.Now;
                        dc.ModifiedTerminal = obj.TerminalID;

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter { Status = true, Key = obj.TokenKey };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }

        }
        public async Task<DO_ReturnParameter> UpdateTokenStatusToCompleted(DO_TokenGeneration obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var dc = db.GtTokm05s.Where(w => w.BusinessKey == obj.BusinessKey
                            && w.TokenDate.Date == System.DateTime.Now.Date
                            && w.TokenPrefix == obj.TokenPrefix
                            && w.TokenKey == obj.TokenKey).FirstOrDefault();

                        dc.TokenCalling = false;
                        dc.TokenStatus = "CL";
                        dc.CompletedTime = System.DateTime.Now;
                        dc.ModifiedBy = obj.UserID;
                        dc.ModifiedOn = System.DateTime.Now;
                        dc.ModifiedTerminal = obj.TerminalID;



                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter { Status = true, Key = obj.TokenKey };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }

        }
        public async Task<DO_ReturnParameter> UpdateToCallingNextToken(DO_TokenGeneration obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var dc = db.GtTokm05s.Where(w => w.BusinessKey == obj.BusinessKey
                            && w.TokenDate.Date == System.DateTime.Now.Date
                            && w.TokenPrefix == obj.TokenPrefix
                            && w.TokenKey == obj.TokenKey).FirstOrDefault();

                        int sq_No = -1;
                        if (dc != null)
                        {
                            dc.TokenCalling = false;
                            dc.ModifiedBy = obj.UserID;
                            dc.ModifiedOn = System.DateTime.Now;
                            dc.ModifiedTerminal = obj.TerminalID;

                            sq_No = dc.SequeueNumber;
                        }

                        var nextToken = db.GtTokm05s
                            .Where(w => w.BusinessKey == obj.BusinessKey
                                && w.TokenPrefix == obj.TokenPrefix
                                && w.TokenDate.Date == System.DateTime.Now.Date
                                && w.TokenStatus == "UC"
                                //&& w.TokenHold == false
                                && w.SequeueNumber > sq_No)
                            .OrderBy(o => o.SequeueNumber).FirstOrDefault();

                        string strTokenKey = "";
                        if (nextToken != null)
                        {
                            nextToken.TokenCalling = true;
                            nextToken.CallingCounter = obj.CallingCounter;
                            //nextToken.CallingOccurence = nextToken.CallingOccurence + 1;
                            nextToken.TokenCallingTime = System.DateTime.Now;
                            //if (nextToken.FirstCallingTime == null)
                            //    nextToken.FirstCallingTime = System.DateTime.Now;
                            nextToken.ModifiedBy = obj.UserID;
                            nextToken.ModifiedOn = System.DateTime.Now;
                            nextToken.ModifiedTerminal = obj.TerminalID;

                            strTokenKey = nextToken.TokenKey;
                        }
                        else
                        {
                            nextToken = db.GtTokm05s
                            .Where(w => w.BusinessKey == obj.BusinessKey
                                && w.TokenPrefix == obj.TokenPrefix
                                && w.TokenDate.Date == System.DateTime.Now.Date
                                && w.TokenStatus == "UC"
                                //&& w.TokenHold == false
                                )
                            .OrderBy(o => o.SequeueNumber).FirstOrDefault();

                            if (nextToken != null)
                            {
                                nextToken.TokenCalling = true;
                                //nextToken.CallingOccurence = obj.CallingOccurence;
                                //nextToken.CallingOccurence = nextToken.CallingOccurence + 1;
                                nextToken.TokenCallingTime = System.DateTime.Now;
                                //if (nextToken.FirstCallingTime == null)
                                //    nextToken.FirstCallingTime = System.DateTime.Now;
                                nextToken.ModifiedBy = obj.UserID;
                                nextToken.ModifiedOn = System.DateTime.Now;
                                nextToken.ModifiedTerminal = obj.TerminalID;

                                strTokenKey = nextToken.TokenKey;
                            }
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter { Status = true, Key = strTokenKey };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }

        }

        public async Task<DO_ReturnParameter> UpdateCallingConfirmation(DO_TokenGeneration obj)
        {

            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var dc = db.GtTokm05s.Where(w => w.BusinessKey == obj.BusinessKey
                            && w.TokenDate.Date == System.DateTime.Now.Date
                            && w.TokenPrefix == obj.TokenPrefix
                            && w.TokenKey == obj.TokenKey).FirstOrDefault();

                        if (dc != null)
                        {
                            //if (dc.CallingConfirmation)
                            //{
                            //    dbContext.Rollback();
                            //    return new DO_ReturnParameter { Status = false, Message = "Token No. " + dc.TokenKey + " is already confirmed" };
                            //}
                            //dc.CallingConfirmation = true;
                            //dc.CallingConfirmationTime = System.DateTime.Now;
                            if (dc.TokenStatus=="CL")
                            {
                                dbContext.Rollback();
                                return new DO_ReturnParameter { Status = false, Message = "Token No. " + dc.TokenKey + " is already confirmed" };
                            }
                            dc.TokenStatus = "CL";
                            dc.ConfirmationTime = System.DateTime.Now;
                            dc.ModifiedBy = obj.UserID;
                            dc.ModifiedOn = System.DateTime.Now;
                            dc.ModifiedTerminal = obj.TerminalID;
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter { Status = true, Key = obj.TokenKey };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }

        }

        #endregion
    }
}
