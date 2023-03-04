using AuthenticateInternal;
using CommonLib.Interfaces.Logger;
using CoreLib.Entities;
using CoreLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.Admin;
using EzIRSpecialist.Models.ModelView;
using EzIRSpecialist.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Authentication
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class EzAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string RoleCode { get; }

        /// <summary>
        /// Quyền mà action yêu cầu để truy cập vào.
        /// </summary>
        private readonly RoleGroup _requiredRoleGroup;

        private readonly string _type;
        public static bool isAccessed = false;
        private ISystemManagementContext _systemManagementContext;
        private IHttpContextAccessor _contextAccessor;
        private IConfiguration _configuration;
        private IStringLocalizer<SharedResource> _sharedLocalizer;
        private IAppLogger _appLogger;
        private readonly IErrorHandler _errorHandler;

        public EzAuthorizeAttribute(string roleCode, bool view = false, bool edit = false, bool delete = false, bool special = false)
        {
            this.RoleCode = roleCode;

            this._requiredRoleGroup = new RoleGroup()
            {
                RoleCode = roleCode,
                View = view,
                Edit = edit,
                Delete = delete,
                Special = special,
            };
        }

        /// <inheritdoc/>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                // check login qua internal
               
                _contextAccessor = (IHttpContextAccessor)context.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor));
                _configuration = (IConfiguration)context.HttpContext.RequestServices.GetService(typeof(IConfiguration));
                _systemManagementContext = (ISystemManagementContext)context.HttpContext.RequestServices.GetService(typeof(ISystemManagementContext));
                _sharedLocalizer = (IStringLocalizer<SharedResource>)context.HttpContext.RequestServices.GetService(typeof(IStringLocalizer<SharedResource>));

                var loginName = string.Empty;
                var username = string.Empty;
                var returnMessage = string.Empty;

                if (this._configuration["MoiTruong"] == "1")
                {
                    // bỏ comment khi chạy trên iis và internal

                    var authenticator = new Authenticate();
                    loginName = string.Empty;
                    username = string.Empty;
                    returnMessage = string.Empty;


                    if (authenticator.Check_Session(_contextAccessor.HttpContext.Request, ref loginName, ref username, ref returnMessage) != 0)
                    {

                        context.Result = new RedirectResult(_configuration["LoginUrl"]);

                        return;

                    }
                }
                else
                {
                    loginName = "TungLX";
                    username = "TungLX";
                    returnMessage = string.Empty;
                }
                

                

                //lấy loginName thay cho username truyền vảo Username khi Login qua Internal

                var user = _systemManagementContext.GetListUser(new UserViewModel { Username = loginName }).FirstOrDefault(x => x.Username.ToUpper() == loginName.ToUpper());

                if (user == null)
                {
                    context.Result = new RedirectResult("/");

                    return;

                } else
                {                    
                    if ((_contextAccessor.HttpContext.Session.GetString(ConstantsSessionName.USERNAME) ?? null) == null)
                    {
                        _contextAccessor.HttpContext.Session.SetString(ConstantsSessionName.USERNAME, loginName);
                        _contextAccessor.HttpContext.Session.SetString(ConstantsSessionName.FULL_NAME, user.FullName ?? "");
                        _contextAccessor.HttpContext.Session.SetString(ConstantsSessionName.EMAIL, user.Email ?? "");
                        _contextAccessor.HttpContext.Session.SetString(ConstantsSessionName.EMAIL_CC, user.EmailCc ?? "");
                        _contextAccessor.HttpContext.Session.SetString(ConstantsSessionName.PHONE, user.Phone ?? "");
                        _contextAccessor.HttpContext.Session.SetInt32(ConstantsSessionName.REGION_ID, user.RegionID ?? 0);
                    }                    
                }

                context.HttpContext.Session.SetString(ConstantsSessionName.USERNAME, user.Username);

                // lấy toàn bộ role của người dùng - RoleType = '0' là roles của Chuyên viên 
                var cResponse = _systemManagementContext.GetListRoleByUser(new RoleGroupViewModel { UserName = loginName, RoleType = ConstantsCommon.SPECIALIST_ROLE_TYPE });

                if (cResponse.Code != 0)
                {
                    context.Result = new RedirectResult("/");
                   
                    return;
                }

                // check xem người dùng có quyền của ACTION yêu cầu không
                if (_requiredRoleGroup != null)
                {
                    List<RoleModelView> roleOfUserOfUser = (List<RoleModelView>)cResponse.Data;
                    var isUnAuthorize = true;

                    _contextAccessor.HttpContext.Session.SetString(ConstantsSessionName.GROUP_CODE, roleOfUserOfUser.FirstOrDefault().GroupCode ?? "");

                    roleOfUserOfUser.ToList().ForEach(role =>
                    {
                        // check tên role
                        if (_requiredRoleGroup.RoleCode != role.RoleCode)
                        {
                            return;
                        }
                      
                        // check quyền
                        if ((_requiredRoleGroup.View ?? false) && role.View.Value)
                        {
                            isUnAuthorize = false;
                        }
                      
                        if ((_requiredRoleGroup.Edit ?? false) && !role.Edit.Value)
                        {
                            isUnAuthorize = true;
                        }
                       
                        if ((_requiredRoleGroup.Delete ?? false) && !role.Delete.Value)
                        {
                            isUnAuthorize = true;
                        }
                      
                        if ((_requiredRoleGroup.Special ?? false) && !role.Special.Value)
                        {
                            isUnAuthorize = true;
                        }
                    });

                    if (isUnAuthorize)
                    {
                        CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
                        context.Result = new JsonResult(cResponseMessage);
                    }
                   
                }
            }
            catch (Exception e)
            {
                var errorHandler = (IErrorHandler)context.HttpContext.RequestServices.GetService(typeof(IErrorHandler));

                errorHandler.WriteToFile(e);               
            }            
        }
    }
}
