using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using EzIRCustomerAPI.Commons;
using EzIRCustomerAPI.DataAccess.Interfaces;
using EzIRCustomerAPI.Models.ViewModels;
using EzIRCustomerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EzIRCustomerAPI.Controllers
{    
    [ApiController]
    
    public class InfoSheetAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IInfoSheetContext _infoSheetContext;
        private readonly IChangeInfoContext _changeInfoContext;
        private readonly IAppLogger _appLogger;
        private readonly IEmailSender _emailSender;

        public InfoSheetAPIController(IConfiguration configuration, IInfoSheetContext infoSheetContext, IAppLogger appLogger, IEmailSender emailSender, IChangeInfoContext changeInfoContext)
        {
            _configuration = configuration;
            _infoSheetContext = infoSheetContext;
            _appLogger = appLogger;
            _emailSender = emailSender;
            _changeInfoContext = changeInfoContext;
        }

        [HttpGet]
        [Route(ApiRoute.GET_INFOSHEET)]
        public IActionResult GetInfoSheet([FromHeader] InfoSheetViewModel infoSheetViewModel)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion
                
                var listInfoSheetEzIR = (List<InfoSheet>) _infoSheetContext.SelectEzIR(infoSheetViewModel);
                var listInfoSheetEzSearch = (List<InfoSheet>) _infoSheetContext.SelectEzSearch(infoSheetViewModel);

                if (listInfoSheetEzIR.Count > 0)
                {                    
                    if (listInfoSheetEzSearch.Count == 0) return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = listInfoSheetEzIR });

                    //listInfoSheetEzIR.ForEach(infosheet => {
                    //    // tìm và xóa bỏ data ở db EzSearch rồi add lại data ở EzIR vào --- (trường hợp update)
                    //    var index = listInfoSheetEzSearch.FindIndex(a => a.ID == infosheet.ID && a.CpnyID == infosheet.CpnyID);

                    //    if (index >= 0)
                    //    {
                    //        listInfoSheetEzSearch.RemoveAt(index);
                    //        listInfoSheetEzSearch.Add(infosheet);
                    //    }

                    //    // nếu aid == 0 || aid == null => add data vào luôn --- (trường hợp insert)
                    //    if (infosheet.ID == 0 || infosheet.ID == null) listInfoSheetEzSearch.Add(infosheet);                        
                    //});

                    // so sánh data giữa 2 db 
                    foreach (var info in listInfoSheetEzIR)
                    {                       
                        if (info.ID == 0)
                        {
                            //(trường hợp insert)
                            listInfoSheetEzSearch.Add(info);
                        }
                        else
                        {
                            //(trường hợp update)
                            var index = listInfoSheetEzSearch.FindIndex(a => a.ID == info.ID && a.CpnyID == info.CpnyID);
                            if (index >= 0)
                            {
                                listInfoSheetEzSearch.RemoveAt(index);
                                listInfoSheetEzSearch.Add(info);
                            }
                        }
                    }

                    return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = listInfoSheetEzSearch });
                }

                return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = listInfoSheetEzSearch });
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        [HttpPost]
        [Route(ApiRoute.INSERT_INFOSHEET)]
        public async Task<ActionResult> InsertInfoSheet([FromBody] List<InfoSheetViewModel> infoSheetViewModels, [FromHeader] string roleCode, [FromHeader] string username)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion

                if (infoSheetViewModels == null || infoSheetViewModels.Count < 1)
                    return Ok(new CResponseMessage { Code = -1, Message = "InvalidInputData" });

                CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "UnknowError" };

                foreach (var infoSheetViewModel in infoSheetViewModels)
                {
                    if (infoSheetViewModel == null || infoSheetViewModel.CpnyID == null || infoSheetViewModel.MenuID == null)
                        return Ok(new CResponseMessage { Code = -1, Message = "InvalidInputData" });

                    _infoSheetContext.SetRoleCode(roleCode);
                    _infoSheetContext.SetUsername(username);

                    if (!string.IsNullOrEmpty(infoSheetViewModel.Content)) infoSheetViewModel.HasContent = 1;

                    if (infoSheetViewModel.ID == null || infoSheetViewModel.ID == 0)
                    {
                        cResponseMessage = _infoSheetContext.Insert(infoSheetViewModel);

                        if(infoSheetViewModel.ChangeInfos.Count > 0) _changeInfoContext.Insert(infoSheetViewModel.ChangeInfos[0]);                        
                    } 
                    else
                    {
                        cResponseMessage = _infoSheetContext.Update(infoSheetViewModel);

                        if (infoSheetViewModel.ChangeInfos.Count > 0) _changeInfoContext.Insert(infoSheetViewModel.ChangeInfos[0]);                        
                    }

                }

                if (cResponseMessage.Code == 0)
                {
                    var response = await this._emailSender.SendEmailAsync(
                                                            infoSheetViewModels[0].EmailSettings.Mail,
                                                            infoSheetViewModels[0].EmailSettings.Subject,
                                                            infoSheetViewModels[0].EmailSettings.Message,
                                                            this._configuration["EmailSettings:Sender"],
                                                            this._configuration["EmailSettings:MailServer"],
                                                            Convert.ToInt32(this._configuration["EmailSettings:MailPort"]),
                                                            this._configuration["EmailSettings:Username"],
                                                            this._configuration["EmailSettings:Password"]
                                                           );

                    _appLogger.InfoLogger.LogInfo($"Mail: {infoSheetViewModels[0].EmailSettings.Mail} - Subject: {infoSheetViewModels[0].EmailSettings.Subject} - Status: {response.Message}");
                }

                return Ok(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        [HttpPut]
        [Route(ApiRoute.UPDATE_INFOSHEET)]
        public async Task<ActionResult> UpdateInfoSheet([FromBody] List<InfoSheetViewModel> infoSheetViewModels, [FromHeader] string roleCode, [FromHeader] string username)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion

                if (infoSheetViewModels == null || infoSheetViewModels.Count < 1)
                    return Ok(new CResponseMessage { Code = -1, Message = "InvalidInputData" });

                CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "UnknowError" };

                foreach (var infoSheetViewModel in infoSheetViewModels)
                {
                    if (infoSheetViewModel == null || infoSheetViewModel.ID == null || infoSheetViewModel.CpnyID == null || infoSheetViewModel.MenuID == null)
                        return Ok(new CResponseMessage { Code = -1, Message = "InvalidInputData" });

                    _infoSheetContext.SetRoleCode(roleCode);
                    _infoSheetContext.SetUsername(username);

                    if (!string.IsNullOrEmpty(infoSheetViewModel.Content)) infoSheetViewModel.HasContent = 1;

                    cResponseMessage = _infoSheetContext.Update(infoSheetViewModel);                    

                    if (infoSheetViewModel.ChangeInfos.Count > 0) _changeInfoContext.Update(infoSheetViewModel.ChangeInfos[0]);                    
                }

                if (cResponseMessage.Code == 0)
                {
                    var response = await this._emailSender.SendEmailAsync(
                                                            infoSheetViewModels[0].EmailSettings.Mail,
                                                            infoSheetViewModels[0].EmailSettings.Subject,
                                                            infoSheetViewModels[0].EmailSettings.Message,
                                                            this._configuration["EmailSettings:Sender"],
                                                            this._configuration["EmailSettings:MailServer"],
                                                            Convert.ToInt32(this._configuration["EmailSettings:MailPort"]),
                                                            this._configuration["EmailSettings:Username"],
                                                            this._configuration["EmailSettings:Password"]
                                                           );

                    _appLogger.InfoLogger.LogInfo($"Mail: {infoSheetViewModels[0].EmailSettings.Mail} - Subject: {infoSheetViewModels[0].EmailSettings.Subject} - Status: {response.Message}");
                }

                return Ok(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
    }
}
