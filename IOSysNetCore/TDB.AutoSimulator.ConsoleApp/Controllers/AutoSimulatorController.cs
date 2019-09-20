using IOSys.DTO;
using IOSys.DTO.Account;
using IOSys.DTO.Basic;
using IOSys.DTO.InOut;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDB.AutoSimulator.ConsoleApp.Configs;
using TDB.AutoSimulator.ConsoleApp.DTO;
using TDB.AutoSimulator.ConsoleApp.Helper;
using TDB.AutoSimulator.ConsoleApp.Http;

namespace TDB.AutoSimulator.ConsoleApp.Controllers
{
    /// <summary>
    /// 自动/模拟
    /// </summary>
    [Route("api/[controller]")]
    public class AutoSimulatorController : ControllerBase
    {
        #region 接口

        /// <summary>
        /// 模拟收入
        /// </summary>
        /// <param name="req"></param>
        [HttpPost]
        [Route("SimulateInCome")]
        public void SimulateInCome([FromBody]SimulateInCome req)
        {
            //登录
            var user = this.Login();
            //登录失败
            if (user == null)
            {
                return;
            }

            //查询收入列表
            var list = this.QueryInCome(user, req.Date);
            //已有收入信息，不再模拟收入
            if (list.Count > 0)
            {
                return;
            }

            //模拟收入
            var lstNew = this.SimulateInCome(user, req.Date);

            //保存收入信息
            foreach (var item in lstNew)
            {
                Task.Factory.StartNew(() => { this.SaveInCome(user, item); });
            }
        }

        /// <summary>
        /// 模拟支出
        /// </summary>
        /// <param name="req"></param>
        [HttpPost]
        [Route("SimulateOutPut")]
        public void SimulateOutPut([FromBody]SimulateOutPutReq req)
        {
            //登录
            var user = this.Login();
            //登录失败
            if (user == null)
            {
                return;
            }

            //查询支出列表
            var list = this.QueryOutPut(user, req.Date);
            //已有支出信息，不再模拟支出
            if (list.Count > 0)
            {
                return;
            }

            //模拟支出
            var lstNew = this.SimulateOutPut(user, req.Date);

            //保存支出信息
            foreach (var item in lstNew)
            {
                Task.Factory.StartNew(() => { this.SaveOutPut(user, item); });
            }
        }

        /// <summary>
        /// 模拟收支
        /// </summary>
        /// <param name="req"></param>
        [HttpPost]
        [Route("SimulateInOut")]
        public void SimulateInOut([FromBody]SimulateInOutReq req)
        {
            var date = req.StartDate.Date;
            while(date < req.EndDate.Date)
            {
                //模拟收入
                var reqIn = new SimulateInCome() { Date = date };
                this.SimulateInCome(reqIn);

                //模拟支出
                var reqOut = new SimulateOutPutReq() { Date = date };
                this.SimulateOutPut(reqOut);

                date = date.AddDays(1);
            }
        }

        #endregion

        #region 变量

        /// <summary>
        /// 登录信息
        /// </summary>
        private LoginInfo user = null;

        private Random _random = null;
        /// <summary>
        /// 随机数生成器
        /// </summary>
        private Random random
        {
            get
            {
                if (_random == null)
                {
                    _random = new Random();
                }

                return _random;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 用默认账号登录
        /// </summary>
        /// <returns></returns>
        private LoginInfo Login()
        {
            if (user != null)
            {
                return user;
            }

            var req = new LoginReq();
            req.LoginName = AppConfig.Inst.App.IOLoginName;
            req.Password = EncryptHelper.EncryptAES(AppConfig.Inst.App.IOLoginPwd);

            //请求接口
            var client = new TdbHttpClient(AppConfig.Inst.App.IOApiUrl);
            var res = client.ExecPost<ResultInfo<LoginInfo>>("api/Account/Login", req);

            if (res.StatusCode == System.Net.HttpStatusCode.OK && res.Result.IsOK)
            {
                user = res.Result.Info;
            }

            return res.Result.Info;
        }

        /// <summary>
        /// 查询账户列表
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        private List<AmountAccountListInfo> QueryAmountAccount(LoginInfo user)
        {
            //token
            var dicHeader = new Dictionary<string, string>();
            dicHeader["Authorization"] = user.Token;

            //请求接口
            var client = new TdbHttpClient(AppConfig.Inst.App.IOApiUrl);
            var res = client.ExecGet<ResultList<AmountAccountListInfo>>("api/Basic/QueryAmountAccount", null, dicHeader);

            return res.Result.LstInfo;
        }

        /// <summary>
        /// 查询收入类型列表
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        private List<InTypeListInfo> QueryInType(LoginInfo user)
        {
            //token
            var dicHeader = new Dictionary<string, string>();
            dicHeader["Authorization"] = user.Token;

            //请求接口
            var client = new TdbHttpClient(AppConfig.Inst.App.IOApiUrl);
            var res = client.ExecGet<ResultList<InTypeListInfo>>("api/Basic/QueryInType", null, dicHeader);

            return res.Result.LstInfo;
        }

        /// <summary>
        /// 查询收入列表
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="date">收入日期</param>
        /// <returns></returns>
        private List<InComeListInfo> QueryInCome(LoginInfo user, DateTime date)
        {
            //token
            var dicHeader = new Dictionary<string, string>();
            dicHeader["Authorization"] = user.Token;

            //请求接口
            var client = new TdbHttpClient(AppConfig.Inst.App.IOApiUrl);
            var res = client.ExecGet<ResultList<InComeListInfo>>("api/InOut/QueryInCome/" + date.ToString("yyyy-MM-dd"), null, dicHeader);

            return res.Result.LstInfo;
        }

        /// <summary>
        /// 保存收入信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="info">收入信息</param>
        /// <returns>主键ID</returns>
        private int SaveInCome(LoginInfo user, InComeInfo info)
        {
            //token
            var dicHeader = new Dictionary<string, string>();
            dicHeader["Authorization"] = user.Token;

            //条件
            var req = new LoginReq();
            req.LoginName = AppConfig.Inst.App.IOLoginName;
            req.Password = AppConfig.Inst.App.IOLoginPwd;

            //请求接口
            var client = new TdbHttpClient(AppConfig.Inst.App.IOApiUrl);
            var res = client.ExecPost<ResultInfo<int>>("api/InOut/SaveInCome", info, dicHeader);

            return res.Result.Info;
        }

        /// <summary>
        /// 模拟收入
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        private IEnumerable<InComeInfo> SimulateInCome(LoginInfo user, DateTime date)
        {
            //限制收入生成次数
            if (random.NextDouble() < 0.95)
            {
                yield break;
            }

            //条数
            int num = 3;
            //最大金额
            decimal maxAmount = 15000;
            //年份偏移量
            var offset = (date.Year - 2019) * 2000;
            maxAmount += offset;

            //查询账户列表
            var lstAmountAccount = this.QueryAmountAccount(user);

            //查询收入类型列表
            var lstInType = this.QueryInType(user);

            for (int i = 0; i < num; i++)
            {
                var item = new InComeInfo();
                item.InDate = date.Date;
                item.InTypeID = lstInType[random.Next(0, lstInType.Count)].ID;
                item.AmountAccountID = lstAmountAccount[random.Next(0, lstAmountAccount.Count)].ID;

                var amountRandom = random.NextDouble();
                item.Amount = (Convert.ToDecimal(amountRandom) * maxAmount) + 1;

                item.Remark = "模拟收入";

                yield return item;

                //限制收入生成次数
                if (amountRandom > 0.5)
                {
                    yield break;
                }
            }
        }

        /// <summary>
        /// 查询支出类型列表
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        private List<OutTypeListInfo> QueryOutType(LoginInfo user)
        {
            //token
            var dicHeader = new Dictionary<string, string>();
            dicHeader["Authorization"] = user.Token;

            //请求接口
            var client = new TdbHttpClient(AppConfig.Inst.App.IOApiUrl);
            var res = client.ExecGet<ResultList<OutCategoryTypeListInfo>>("api/Basic/QueryOutCategoryType", null, dicHeader);

            var list = res.Result.LstInfo.SelectMany(m => m.LstOutType).ToList();

            return list;
        }

        /// <summary>
        /// 查询支出列表
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="date">收入日期</param>
        /// <returns></returns>
        private List<OutPutListInfo> QueryOutPut(LoginInfo user, DateTime date)
        {
            //token
            var dicHeader = new Dictionary<string, string>();
            dicHeader["Authorization"] = user.Token;

            //请求接口
            var client = new TdbHttpClient(AppConfig.Inst.App.IOApiUrl);
            var res = client.ExecGet<ResultList<OutPutListInfo>>("api/InOut/QueryOutPut/" + date.ToString("yyyy-MM-dd"), null, dicHeader);

            return res.Result.LstInfo;
        }

        /// <summary>
        /// 保存支出信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="info">支出信息</param>
        /// <returns>主键ID</returns>
        private int SaveOutPut(LoginInfo user, OutPutInfo info)
        {
            //token
            var dicHeader = new Dictionary<string, string>();
            dicHeader["Authorization"] = user.Token;

            //条件
            var req = new LoginReq();
            req.LoginName = AppConfig.Inst.App.IOLoginName;
            req.Password = AppConfig.Inst.App.IOLoginPwd;

            //请求接口
            var client = new TdbHttpClient(AppConfig.Inst.App.IOApiUrl);
            var res = client.ExecPost<ResultInfo<int>>("api/InOut/SaveOutPut", info, dicHeader);

            return res.Result.Info;
        }

        /// <summary>
        /// 模拟支出
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        private IEnumerable<OutPutInfo> SimulateOutPut(LoginInfo user, DateTime date)
        {
            //条数
            int num = 6;
            //最大金额
            decimal maxAmount = 2000;
            //年份偏移量
            var offset = (date.Year - 2019) * 300;
            maxAmount += offset;

            //查询账户列表
            var lstAmountAccount = this.QueryAmountAccount(user);

            //查询支出类型列表
            var lstOutType = this.QueryOutType(user);

            for (int i = 0; i < num; i++)
            {
                var item = new OutPutInfo();
                item.OutDate = date.Date;
                item.OutTypeID = lstOutType[random.Next(0, lstOutType.Count)].ID;
                item.AmountAccountID = lstAmountAccount[random.Next(0, lstAmountAccount.Count)].ID;
                item.Amount = (Convert.ToDecimal(random.NextDouble()) * maxAmount) + 1;
                item.Remark = "模拟支出";

                //随机出来的支出太高了，控制下...
                if (Convert.ToInt32(item.Amount) % 25 != 1)
                {
                    item.Amount = item.Amount / 30;
                }

                yield return item;
            }
        }

        #endregion
    }
}
