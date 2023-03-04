using CommonLib.Interfaces.Logger;
using CoreLib.Entities;
using EzIRCustomerAPI.Models;
using EzIRCustomerAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Commons
{
    public class CheckValidToken
    {
        public static CResponseMessage Check(string token, IAppLogger _appLogger)
        {
            if (token == null)
            {
                return new CResponseMessage { Code = 401, Message = "Unauthorized" };
            }

            token = token.Replace("Bearer ", "");

            UserInfo userInfo;

            if (string.IsNullOrWhiteSpace(token) || !TokenManager.ValidateToken(token, out userInfo, _appLogger)) 
            {
                return new CResponseMessage { Code = 401, Message = "Unauthorized" };
            }

            return new CResponseMessage { Code = 200, Message = "Ok" };
        }
    }
}
