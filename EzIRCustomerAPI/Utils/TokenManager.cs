using CommonLib.Implementations.Logger;
using CommonLib.Interfaces.Logger;
using EzIRCustomerAPI.Models;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Utils
{
    public class TokenManager
    {
        private static readonly string _secret = "QirJsqTK22Q87CYnqoFHoz5GN2-zAEtVNI8sB2_9FsFmCjIAsLbVIIQhWMNqnu1RKcg_tNZ8ZF1W";//key
        private static readonly int _expire = 60 * 60 * 24; //seconds --- 86400 seconds  = 1 day

        public static string GenerateToken(UserInfo userInfo, int expire, IAppLogger _appLogger)
        {
            try
            {
                var keySec = _secret;
                if (expire <= 0) expire = _expire;
                var provider = new UtcDateTimeProvider();
                var createTime = provider.GetNow();
                var expiredTime = provider.GetNow().AddSeconds(expire);
                var secondsSinceEpoch = UnixEpoch.GetSecondsSince(expiredTime);

                var payload = new Dictionary<string, object>
                {
                    { "UserInfo", userInfo },
                    { "exp", secondsSinceEpoch }
                };

                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                JWT.IJsonSerializer serializer = new JsonNetSerializer();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

                var token = encoder.Encode(payload, keySec);

                return token;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);                
            }
            return null;
        }

        public static bool ValidateToken(string token, out UserInfo userInfo, IAppLogger _appLogger)
        {
            userInfo = null;
            try
            {
                var keySec = _secret;
                
                JWT.IJsonSerializer serializer = new JsonNetSerializer();
                var provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var stringToken = decoder.Decode(token, keySec, verify: true);

                userInfo = JsonConvert.DeserializeObject<UserInfo>(stringToken.ToString());

                return true;
            }
            catch (TokenExpiredException)
            {
                _appLogger.InfoLogger.LogInfo("Token has expired: " + token);                
            }
            catch (SignatureVerificationException)
            {
                _appLogger.InfoLogger.LogInfo("Token has invalid signature: " + token);                
            }

            return false;
        }
    }
}
