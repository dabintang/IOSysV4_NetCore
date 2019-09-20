using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOSys.API.Filters;
using IOSys.BLL;
using IOSys.DTO;
using IOSys.DTO.Basic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IOSys.API.Controllers
{
    /// <summary>
    /// 基础数据
    /// </summary>
    public class BasicController : BaseController
    {
        #region 变量

        private BasicBLL _bll;
        /// <summary>
        /// 基础数据 逻辑层
        /// </summary>
        private BasicBLL bll
        {
            get
            {
                if (this._bll == null)
                {
                    this._bll = new BasicBLL();
                    this._bll.LoginInfo = this.LoginInfo;
                    this._bll.Lang = this.Lang;
                }

                return this._bll;
            }
        }

        #endregion

        #region 账户

        /// <summary>
        /// 查询账户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("QueryAmountAccount")]
        public async Task<ResultList<AmountAccountListInfo>> QueryAmountAccountAsync()
        {
            var list = await this.bll.QueryAmountAccountAsync();
            return new ResultList<AmountAccountListInfo>(true, this.bll.Res.Gen.OK, list);
        }

        /// <summary>
        /// 获取账户
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAmountAccount/{id}")]
        public async Task<ResultInfo<AmountAccountInfo>> GetAmountAccountAsync(int id)
        {
            var item = await this.bll.GetAmountAccountAsync(id);
            return new ResultInfo<AmountAccountInfo>(true, this.bll.Res.Gen.OK, item);
        }

        /// <summary>
        /// 保存账户
        /// </summary>
        /// <param name="info">账户</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveAmountAccount")]
        [ParamsLog("保存账户")]
        public async Task<ResultInfo<int>> SaveAmountAccountAsync([FromBody]AmountAccountInfo info)
        {
            var result = await this.bll.SaveAmountAccountAsync(info);
            return result;
        }

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="req">条件</param>
        [HttpPost]
        [Route("DeleteAmountAccount")]
        [ParamsLog("删除账户")]
        public async Task<ResultInfo<bool>> DeleteAmountAccountAsync([FromBody]IDReq req)
        {
            var result = await this.bll.DeleteAmountAccountAsync(req.ID);
            return result;
        }

        #endregion

        #region 收入类型

        /// <summary>
        /// 查询收入类型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("QueryInType")]
        public async Task<ResultList<InTypeListInfo>> QueryInTypeAsync()
        {
            var list = await this.bll.QueryInTypeAsync();
            return new ResultList<InTypeListInfo>(true, this.bll.Res.Gen.OK, list);
        }

        /// <summary>
        /// 获取收入类型
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetInType/{id}")]
        public async Task<ResultInfo<InTypeInfo>> GetInTypeAsync(int id)
        {
            var item = await this.bll.GetInTypeAsync(id);
            return new ResultInfo<InTypeInfo>(true, this.bll.Res.Gen.OK, item);
        }

        /// <summary>
        /// 保存收入类型
        /// </summary>
        /// <param name="info">收入类型</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveInType")]
        [ParamsLog("保存收入类型")]
        public async Task<ResultInfo<int>> SaveInTypeAsync([FromBody]InTypeInfo info)
        {
            var result = await this.bll.SaveInTypeAsync(info);
            return result;
        }

        /// <summary>
        /// 删除收入类型
        /// </summary>
        /// <param name="req">条件</param>
        [HttpPost]
        [Route("DeleteInType")]
        [ParamsLog("删除收入类型")]
        public async Task<ResultInfo<bool>> DeleteInTypeAsync([FromBody]IDReq req)
        {
            var result = await this.bll.DeleteInTypeAsync(req.ID);
            return result;
        }

        #endregion

        #region 支出分类

        /// <summary>
        /// 查询支出分类列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("QueryOutCategory")]
        public async Task<ResultList<OutCategoryListInfo>> QueryOutCategoryAsync()
        {
            var list = await this.bll.QueryOutCategoryAsync();
            return new ResultList<OutCategoryListInfo>(true, this.bll.Res.Gen.OK, list);
        }

        /// <summary>
        /// 获取支出分类
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOutCategory/{id}")]
        public async Task<ResultInfo<OutCategoryInfo>> GetOutCategoryAsync(int id)
        {
            var item = await this.bll.GetOutCategoryAsync(id);
            return new ResultInfo<OutCategoryInfo>(true, this.bll.Res.Gen.OK, item);
        }

        /// <summary>
        /// 保存支出分类
        /// </summary>
        /// <param name="info">支出分类</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveOutCategory")]
        [ParamsLog("保存支出分类")]
        public async Task<ResultInfo<int>> SaveOutCategoryAsync([FromBody]OutCategoryInfo info)
        {
            var result = await this.bll.SaveOutCategoryAsync(info);
            return result;
        }

        /// <summary>
        /// 删除支出分类
        /// </summary>
        /// <param name="req">条件</param>
        [HttpPost]
        [Route("DeleteOutCategory")]
        [ParamsLog("删除支出分类")]
        public async Task<ResultInfo<bool>> DeleteOutCategoryAsync([FromBody]IDReq req)
        {
            var result = await this.bll.DeleteOutCategoryAsync(req.ID);
            return result;
        }

        #endregion

        #region 支出类型

        /// <summary>
        /// 查询支出类型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("QueryOutCategoryType")]
        public async Task<ResultList<OutCategoryTypeListInfo>> QueryOutCategoryTypeAsync()
        {
            var list = await this.bll.QueryOutCategoryTypeAsync();
            return new ResultList<OutCategoryTypeListInfo>(true, this.bll.Res.Gen.OK, list);
        }

        /// <summary>
        /// 获取支出类型
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOutType/{id}")]
        public async Task<ResultInfo<OutTypeInfo>> GetOutTypeAsync(int id)
        {
            var item = await this.bll.GetOutTypeAsync(id);
            return new ResultInfo<OutTypeInfo>(true, this.bll.Res.Gen.OK, item);
        }

        /// <summary>
        /// 保存支出类型
        /// </summary>
        /// <param name="info">支出类型</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveOutType")]
        [ParamsLog("保存支出类型")]
        public async Task<ResultInfo<int>> SaveOutTypeAsync([FromBody]OutTypeInfo info)
        {
            var result = await this.bll.SaveOutTypeAsync(info);
            return result;
        }

        /// <summary>
        /// 删除支出类型
        /// </summary>
        /// <param name="req">条件</param>
        [HttpPost]
        [Route("DeleteOutType")]
        [ParamsLog("删除支出类型")]
        public async Task<ResultInfo<bool>> DeleteOutTypeAsync([FromBody]IDReq req)
        {
            var result = await this.bll.DeleteOutTypeAsync(req.ID);
            return result;
        }

        #endregion

        #region 下拉框

        /// <summary>
        /// 获取借还类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("QueryBorrowRepayType")]
        public ResultList<IDNameInfo> QueryBorrowRepayType()
        {
            var list = this.bll.QueryBorrowRepayType();
            return new ResultList<IDNameInfo>(true, this.bll.Res.Gen.OK, list);
        }

        /// <summary>
        /// 获取收入统计方式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("QueryInGroupType")]
        public ResultList<IDNameInfo> QueryInGroupType()
        {
            var list = this.bll.QueryInGroupType();
            return new ResultList<IDNameInfo>(true, this.bll.Res.Gen.OK, list);
        }

        /// <summary>
        /// 获取支出统计方式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("QueryOutGroupType")]
        public ResultList<IDNameInfo> QueryOutGroupType()
        {
            var list = this.bll.QueryOutGroupType();
            return new ResultList<IDNameInfo>(true, this.bll.Res.Gen.OK, list);
        }

        #endregion
    }
}