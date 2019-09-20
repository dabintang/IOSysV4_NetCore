using IOSys.Helper.Config;
using IOSys.DTO;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.Helper
{
    /// <summary>
    /// JWT帮助类
    /// </summary>
    public class JWTHelper
    {
        #region 常量

        /// <summary>
        /// 秘钥
        /// </summary>
        private const string Cst_Secret = "tangdabin_";

        #endregion

        /// <summary>
        /// 生成token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="user">用户信息</param>
        /// <param name="ip">IP地址</param>
        /// <returns></returns>
        public static string CreateToken<T>(T user, string ip)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            //用户信息转字段
            var dic = CvtHelper.ToDictionary(user, false);
            //超时时间
            var timeOut = CvtHelper.ToUTCSecond(DateTime.Now) + CvtHelper.DayToSecond(IOSysJson.Inst.AppConfig.TokenTimeOutDay);
            dic["exp"] = timeOut;

            var token = encoder.Encode(dic, Cst_Secret + ip);
            return token;
        }

        /// <summary>
        /// 验证token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">签名</param>
        /// <param name="ip">IP地址</param>
        /// <returns></returns>
        public static ResultInfo<T> CheckToken<T>(string token, string ip)
        {
            var msgCfg = ConfigHelper.GetMsg("zh");

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var json = decoder.Decode(token, Cst_Secret + ip, true);
                var result = JsonConvert.DeserializeObject<T>(json);

                return new ResultInfo<T>(true, msgCfg.Gen.TokenOK, result);
            }
            //catch (TokenExpiredException ex)
            //{
            //    return new ResultView<T>(false, string.Format("签名已过期：{0}", ex.Message), default(T));
            //}
            //catch (SignatureVerificationException ex)
            //{
            //    return new ResultView<T>(false, string.Format("无效签名：{0}", ex.Message), default(T));
            //}
            catch (Exception ex)
            {
                //LogHelper.Error(ex, "验证token异常");

                return new ResultInfo<T>(false, msgCfg.Gen.TokenError.FormatMsg(token, ex.Message), default(T));
            }
        }
    }
}
