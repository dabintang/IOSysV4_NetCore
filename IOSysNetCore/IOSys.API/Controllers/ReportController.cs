using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOSys.BLL;
using IOSys.DTO;
using IOSys.DTO.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IOSys.API.Controllers
{
    /// <summary>
    /// 报表
    /// </summary>
    public class ReportController : BaseController
    {
        #region 变量

        private ReportBLL _bll;
        /// <summary>
        /// 报表 逻辑层
        /// </summary>
        private ReportBLL bll
        {
            get
            {
                if (this._bll == null)
                {
                    this._bll = new ReportBLL();
                    this._bll.LoginInfo = this.LoginInfo;
                    this._bll.Lang = this.Lang;
                }

                return this._bll;
            }
        }

        #endregion

        #region 明细

        /// <summary>
        /// 查询收入明细列表
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryInRecord")]
        public async Task<ResultPageAmountList<InRecordListInfo>> QueryInRecordAsync([FromBody]InRecordReq req)
        {
            var list = await this.bll.QueryInRecordAsync(req);
            return new ResultPageAmountList<InRecordListInfo>(true, this.bll.Res.Gen.OK, list, (req.GetSkip() + 1), list.TotalRecord, list.TotalAmount);
        }

        /// <summary>
        /// 查询支出明细列表
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryOutRecord")]
        public async Task<ResultPageAmountList<OutRecordListInfo>> QueryOutRecordAsync([FromBody]OutRecordReq req)
        {
            var list = await this.bll.QueryOutRecordAsync(req);
            return new ResultPageAmountList<OutRecordListInfo>(true, this.bll.Res.Gen.OK, list, (req.GetSkip() + 1), list.TotalRecord, list.TotalAmount);
        }

        /// <summary>
        /// 查询转账明细列表
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryTransferRecord")]
        public async Task<ResultPageList<TransferRecordListInfo>> QueryTransferRecord([FromBody]TransferRecordReq req)
        {
            var list = await this.bll.QueryTransferRecordAsync(req);
            return new ResultPageList<TransferRecordListInfo>(true, this.bll.Res.Gen.OK, list, (req.GetSkip() + 1), list.TotalRecord);
        }

        /// <summary>
        /// 查询借还明细列表
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryBorrowRepayRecord")]
        public async Task<ResultPageList<BorrowRepayRecordListInfo>> QueryBorrowRepayRecordAsync([FromBody]BorrowRepayRecordReq req)
        {
            var list = await this.bll.QueryBorrowRepayRecordAsync(req);
            return new ResultPageList<BorrowRepayRecordListInfo>(true, this.bll.Res.Gen.OK, list, (req.GetSkip() + 1), list.TotalRecord);
        }

        /// <summary>
        /// 查询账户流水明细列表
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryAccountTurnover")]
        public async Task<ResultPageMonthAmountList<AccountTurnoverListInfo>> QueryAccountTurnoverAsync([FromBody]AccountTurnoverListReq req)
        {
            var list = await this.bll.QueryAccountTurnoverAsync(req);
            return new ResultPageMonthAmountList<AccountTurnoverListInfo>(true, this.bll.Res.Gen.OK, list, req.Month, list.TotalAmount, list.HasPreMonth);
        }

        #endregion

        #region 统计

        /// <summary>
        /// 收入统计
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        [HttpGet]
        [Route("SumInCome")]
        public async Task<ResultList<SumListInfo<string>>> SumInComeAsync([FromQuery]InSumReq req)
        {
            var list = await this.bll.SumInComeAsync(req);
            return new ResultList<SumListInfo<string>>(true, this.bll.Res.Gen.OK, list);
        }

        /// <summary>
        /// 支出统计
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        [HttpGet]
        [Route("SumOutPut")]
        public async Task<ResultList<SumListInfo<string>>> SumOutPutAsync([FromQuery]OutSumReq req)
        {
            var list = await this.bll.SumOutPutAsync(req);
            return new ResultList<SumListInfo<string>>(true, this.bll.Res.Gen.OK, list);
        }

        /// <summary>
        /// 按借还者统计借还信息
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        [HttpGet]
        [Route("SumBorrowRepayTarget")]
        public async Task<ResultList<SumListInfo<string>>> SumBorrowRepayTargetAsync([FromQuery]BorrowRepaySumReq req)
        {
            var list = await this.bll.SumBorrowRepayTargetAsync(req);
            return new ResultList<SumListInfo<string>>(true, this.bll.Res.Gen.OK, list);
        }

        /// <summary>
        /// 用户收支统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SumUserTotal")]
        public async Task<ResultInfo<UserTotalInfo>> SumUserTotal()
        {
            var info = await this.bll.SumUserTotal();
            return new ResultInfo<UserTotalInfo>(true, this.bll.Res.Gen.OK, info);
        }

        /// <summary>
        /// 月份统计
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SumMonth")]
        public async Task<ResultList<MonthSumListInfo<string>>> SumMonthAsync([FromQuery]MonthSumReq req)
        {
            var list = await this.bll.SumMonthAsync(req);
            return new ResultList<MonthSumListInfo<string>>(true, this.bll.Res.Gen.OK, list);
        }

        /// <summary>
        /// 月份支出统计
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SumMonthOut")]
        public async Task<ResultList<MonthOutCategorySumListInfo>> SumMonthOutAsync([FromQuery]MonthOutSumReq req)
        {
            var list = await this.bll.SumMonthOutAsync(req);
            return new ResultList<MonthOutCategorySumListInfo>(true, this.bll.Res.Gen.OK, list);
        }

        #endregion
    }
}