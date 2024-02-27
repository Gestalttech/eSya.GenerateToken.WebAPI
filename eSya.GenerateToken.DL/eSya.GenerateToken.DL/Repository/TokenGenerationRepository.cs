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
    public class TokenGenerationRepository: ITokenGenerationRepository
    {
        #region KIOSK/Tab based
        public async Task<List<DO_TokenConfiguration>> GetAllConfigureTokens()

        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var tokens = await db.GtTokm01s.Where(x => x.ActiveStatus)
                                    .Select(t => new DO_TokenConfiguration
                                    {
                                        TokenType = t.TokenType,
                                        TokenPrefix = t.TokenPrefix,
                                        TokenDesc = t.TokenDesc,
                                        TokenNumberLength = t.TokenNumberLength,
                                        ConfirmationUrl = t.ConfirmationUrl,
                                        QrcodeUrl = t.QrcodeUrl,
                                        DisplaySequence = t.DisplaySequence,
                                        IsEnCounter = t.IsEnCounter,
                                        ActiveStatus = t.ActiveStatus
                                    }).OrderBy(o => o.DisplaySequence).ToListAsync();
                    return tokens;
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<DO_ReturnParameter> GenerateToken(DO_TokenGeneration obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {

                    try
                    {

                        if (!string.IsNullOrEmpty(obj.TokenPrefix))
                        {

                            var RecordExist = db.GtTokm05s.Where(w => w.TokenKey == obj.TokenPrefix && obj.TokenPrefix != "0" && w.BusinessKey == obj.BusinessKey && w.TokenDate == System.DateTime.Now.Date).Count();
                            if (RecordExist > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Token already generated" };
                            }
                        }

                        var token_type = db.GtTokm01s.Where(w => w.TokenPrefix == obj.TokenPrefix).FirstOrDefault();
                        if (token_type == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Token type is not defined. Can't generat token !" };
                        }
                        var tSqNo = db.GtTokm05s.Where(w => w.BusinessKey == obj.BusinessKey
                              && w.TokenPrefix == obj.TokenPrefix
                              && w.TokenDate.Date == System.DateTime.Now.Date)
                          .Select(s => s.SequeueNumber)
                          .DefaultIfEmpty().Max();
                        tSqNo = tSqNo + 1;

                        var sr_token = token_type.TokenPrefix + (Convert.ToInt32(tSqNo)).ToString().PadLeft(token_type.TokenNumberLength, '0');

                        var token = new GtTokm05
                        {
                            BusinessKey = obj.BusinessKey,
                            TokenDate = System.DateTime.Now,
                            TokenKey = sr_token,
                            TokenPrefix = obj.TokenPrefix,
                            SequeueNumber = tSqNo,
                            Isdcode = obj.Isdcode ?? 0, 
                            MobileNumber = obj.MobileNumber!=null ? obj.MobileNumber:null,
                            TokenCalling = false,
                            TokenCallingTime= obj.TokenCallingTime != null? obj.TokenCallingTime : null,
                            CallingCounter =(!string.IsNullOrEmpty(obj.CallingCounter)) ? obj.CallingCounter:null,
                            CounterKey= obj.CounterKey != null ? obj.CounterKey : null,
                            HoldOccurrence = 0,
                            ReCallOccurrence=0,
                            ConfirmationTime=obj.ConfirmationTime!= null? obj.ConfirmationTime:null,
                            TokenStatus="UC",
                            CompletedTime=obj.CompletedTime != null? obj.CompletedTime:null,
                            ActiveStatus = true,
                            FormId = obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };


                        db.GtTokm05s.Add(token);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter { Status = true, Key = sr_token };
                    }


                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                }
            }


        }
        #endregion

        #region Mobile based
        public async Task<DO_ReturnParameter> GenerateOTP(DO_OTP obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {

                    try
                    {

                        Random rnd = new Random();
                        var OTP = rnd.Next(1000, 9999).ToString();
                        var _id = db.GtEcmotps.Select(s => s.Id).DefaultIfEmpty().Max();
                        _id = _id + 1;

                        var otp = new GtEcmotp
                        {
                            Id = _id,
                            MobileNumber = obj.MobileNumber,
                            Otptype = obj.Otptype,
                            Otp = Convert.ToDecimal(OTP),
                            GeneratedOn = System.DateTime.Now
                        };


                        db.GtEcmotps.Add(otp);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter { Status = true, Key = OTP };
                    }


                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                }

            }


        }
        public async Task<DO_ReturnParameter> ConfirmOTP(DO_OTP obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {

                    try
                    {

                        var _exist = db.GtEcmotps
                            .Where(w => w.Otptype == obj.Otptype && w.MobileNumber == obj.MobileNumber && w.ConfirmedOn == null)
                            .OrderByDescending(o => o.GeneratedOn).FirstOrDefault();
                        if (_exist == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Invalid OTP" };
                        }
                        else
                        {
                            if (_exist.Otp != obj.Otp)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Invalid OTP" };
                            }
                            else
                            {
                                _exist.ConfirmedOn = System.DateTime.Now;
                                await db.SaveChangesAsync();
                                dbContext.Commit();
                                return new DO_ReturnParameter() { Status = true, Message = "Confirmed" };
                            }

                        }

                    }


                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                }
            }


        }

        public async Task<List<DO_TokenGeneration>> GetTokenDetailByMobile(int businessKey, int isdCode, string mobileNumber)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                try
                {
                     //businessKey = 11;

                    var ds = db.GtTokm05s
                        .Join(db.GtTokm01s,
                        m => m.TokenPrefix,
                        t => t.TokenPrefix,
                        (m, t) => new { m, t })
                        .Where(w => w.m.BusinessKey == businessKey
                                    && w.m.TokenDate.Date == System.DateTime.Now.Date
                                    && w.m.Isdcode == isdCode
                                    && w.m.MobileNumber == mobileNumber
                                    && w.m.TokenStatus == "RG"
                                    && w.m.ActiveStatus)
                        .Select(r => new DO_TokenGeneration
                        {
                            TokenKey = r.m.TokenKey,
                            TokenType = r.t.TokenDesc,
                            TokenStatus = r.m.TokenStatus == "CM" ? "Completed" : "Inprogress",
                            CreatedOn = r.m.CreatedOn
                        }).OrderBy(o => o.CreatedOn).ToListAsync();

                    return await ds;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

    }
}
