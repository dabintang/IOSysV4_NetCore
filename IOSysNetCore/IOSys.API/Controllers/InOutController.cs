using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOSys.API.Filters;
using IOSys.BLL;
using IOSys.DTO;
using IOSys.DTO.InOut;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IOSys.API.Controllers
{
    /// <summary>
    /// 收入支出
    /// </summary>
    public class InOutController : BaseController
    {
        #region 变量

        private InOutBLL _bll;
        /// <summary>
        /// 收支操作 逻辑层
        /// </summary>
        private InOutBLL bll
        {
            get
            {
                if (this._bll == null)
                {
                    this._bll = new InOutBLL();
                    this._bll.LoginInfo = this.LoginInfo;
                    this._bll.Lang = this.Lang;
                }

                return this._bll;
            }
        }

        #endregion

        #region 收入

        /// <summary>
        /// 查询收入列表
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        [HttpGet]
        [Route("QueryInCome/{date}")]
        public async Task<ResultList<InComeListInfo>> QueryInComeAsync(DateTime date)
        {
            var list = await this.bll.QueryInComeAsync(date);
            return new ResultList<InComeListInfo>(true, this.bll.Res.Gen.OK, list);
        }

        /// <summary>
        /// 获取收入信息
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetInCome/{id}")]
        public async Task<ResultInfo<InComeInfo>> GetInComeAsync(int id)
        {
            var item = await this.bll.GetInComeAsync(id);
            return new ResultInfo<InComeInfo>(true, this.bll.Res.Gen.OK, item);
        }

        /// <summary>
        /// 保存收入信息
        /// </summary>
        /// <param name="info">收入信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveInCome")]
        [ParamsLog("保存收入信息")]
        public async Task<ResultInfo<int>> SaveInComeAsync([FromBody]InComeInfo info)
        {
            var result = await this.bll.SaveInComeAsync(info);
            return result;
        }

        /// <summary>
        /// 删除收入信息
        /// </summary>
        /// <param name="req">条件</param>
        [HttpPost]
        [Route("DeleteInCome")]
        [ParamsLog("删除收入信息")]
        public async Task<ResultInfo<bool>> DeleteInComeAsync([FromBody]IDReq req)
        {
            var result = await this.bll.DeleteInComeAsync(req.ID);
            return result;
        }

        #endregion

        #region 支出

        /// <summary>
        /// 查询支出列表
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        [HttpGet]
        [Route("QueryOutPut/{date}")]
        public async Task<ResultList<OutPutListInfo>> QueryOutPutAsync(DateTime date)
        {
            var list = await this.bll.QueryOutPutAsync(date);
            return new ResultList<OutPutListInfo>(true, this.bll.Res.Gen.OK, list);
        }

        /// <summary>
        /// 获取支出信息
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOutPut/{id}")]
        public async Task<ResultInfo<OutPutInfo>> GetOutPutAsync(int id)
        {
            var item = await this.bll.GetOutPutAsync(id);
            return new ResultInfo<OutPutInfo>(true, this.bll.Res.Gen.OK, item);
        }

        /// <summary>
        /// 保存支出信息
        /// </summary>
        /// <param name="info">支出信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveOutPut")]
        [ParamsLog("保存支出信息")]
        public async Task<ResultInfo<int>> SaveOutPutAsync([FromBody]OutPutInfo info)
        {
            var result = await this.bll.SaveOutPutAsync(info);
            return result;
        }

        /// <summary>
        /// 删除支出信息
        /// </summary>
        /// <param name="req">条件</param>
        [HttpPost]
        [Route("DeleteOutPut")]
        [ParamsLog("删除支出信息")]
        public async Task<ResultInfo<bool>> DeleteOutPutAsync([FromBody]IDReq req)
        {
            var result = await this.bll.DeleteOutPutAsync(req.ID);
            return result;
        }

        #endregion

        #region 转账

        /// <summary>
        /// 查询转账列表
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        [HttpGet]
        [Route("QueryTransfer/{date}")]
        public async Task<ResultList<TransferListInfo>> QueryTransferAsync(DateTime date)
        {
            var list = await this.bll.QueryTransferAsync(date);
            return new ResultList<TransferListInfo>(true, this.bll.Res.Gen.OK, list);
        }

        /// <summary>
        /// 获取转账信息
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTransfer/{id}")]
        public async Task<ResultInfo<TransferInfo>> GetTransferAsync(int id)
        {
            var item = await this.bll.GetTransferAsync(id);
            return new ResultInfo<TransferInfo>(true, this.bll.Res.Gen.OK, item);
        }

        /// <summary>
        /// 保存转账信息
        /// </summary>
        /// <param name="info">转账信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveTransfer")]
        [ParamsLog("保存转账信息")]
        public async Task<ResultInfo<int>> SaveTransferAsync([FromBody]TransferInfo info)
        {
            var result = await this.bll.SaveTransferAsync(info);
            return result;
        }

        /// <summary>
        /// 删除转账信息
        /// </summary>
        /// <param name="req">条件</param>
        [HttpPost]
        [Route("DeleteTransfer")]
        [ParamsLog("删除转账信息")]
        public async Task<ResultInfo<bool>> DeleteTransferAsync([FromBody]IDReq req)
        {
            var result = await this.bll.DeleteTransferAsync(req.ID);
            return result;
        }

        #endregion

        #region 借还

        /// <summary>
        /// 查询借还列表
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        [HttpGet]
        [Route("QueryBorrowRepay/{date}")]
        public async Task<ResultList<BorrowRepayListInfo>> QueryBorrowRepayAsync(DateTime date)
        {
            var list = await this.bll.QueryBorrowRepayAsync(date);
            return new ResultList<BorrowRepayListInfo>(true, this.bll.Res.Gen.OK, list);
        }

        /// <summary>
        /// 获取借还信息
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBorrowRepay/{id}")]
        public async Task<ResultInfo<BorrowRepayInfo>> GetBorrowRepayAsync(int id)
        {
            var item = await this.bll.GetBorrowRepayAsync(id);
            return new ResultInfo<BorrowRepayInfo>(true, this.bll.Res.Gen.OK, item);
        }

        /// <summary>
        /// 保存借还信息
        /// </summary>
        /// <param name="info">借还信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveBorrowRepay")]
        [ParamsLog("保存借还信息")]
        public async Task<ResultInfo<int>> SaveBorrowRepayAsync([FromBody]BorrowRepayInfo info)
        {
            var result = await this.bll.SaveBorrowRepayAsync(info);
            return result;
        }

        /// <summary>
        /// 删除借还信息
        /// </summary>
        /// <param name="req">条件</param>
        [HttpPost]
        [Route("DeleteBorrowRepay")]
        [ParamsLog("删除借还信息")]
        public async Task<ResultInfo<bool>> DeleteBorrowRepayAsync([FromBody]IDReq req)
        {
            var result = await this.bll.DeleteBorrowRepayAsync(req.ID);
            return result;
        }

        #endregion

        #region 其他

        /// <summary>
        /// 更新账户的排序权重
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateAmountAccountSortWeight")]
        public async Task<ResultInfo<bool>> UpdateAmountAccountSortWeightAsync()
        {
            var result = await this.bll.UpdateAmountAccountSortWeightAsync();
            return new ResultInfo<bool>(result, result ? this.bll.Res.Gen.OK : this.bll.Res.Gen.UnkonwErr.FormatMsg("失败"), result);
        }

        /// <summary>
        /// 更新收入类型的排序权重
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateInTypeSortWeight")]
        public async Task<ResultInfo<bool>> UpdateInTypeSortWeightAsync()
        {
            var result = await this.bll.UpdateInTypeSortWeightAsync();
            return new ResultInfo<bool>(result, result ? this.bll.Res.Gen.OK : this.bll.Res.Gen.UnkonwErr.FormatMsg("失败"), result);
        }

        /// <summary>
        /// 更新支出类型的排序权重
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateOutTypeSortWeight")]
        public async Task<ResultInfo<bool>> UpdateOutTypeSortWeightAsync()
        {
            var result = await this.bll.UpdateOutTypeSortWeightAsync();
            return new ResultInfo<bool>(result, result ? this.bll.Res.Gen.OK : this.bll.Res.Gen.UnkonwErr.FormatMsg("失败"), result);
        }

        #endregion
    }
}