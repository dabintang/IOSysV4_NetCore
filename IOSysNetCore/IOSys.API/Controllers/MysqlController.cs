using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOSys.BLL;
using IOSys.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IOSys.API.Controllers
{
    /// <summary>
    /// mysql
    /// </summary>
    public class MysqlController : BaseController
    {
        #region 变量

        private MysqlBLL _bll;
        /// <summary>
        /// APP管理 逻辑层
        /// </summary>
        private MysqlBLL bll
        {
            get
            {
                if (this._bll == null)
                {
                    this._bll = new MysqlBLL();
                    this._bll.LoginInfo = this.LoginInfo;
                    this._bll.Lang = this.Lang;
                }

                return this._bll;
            }
        }

        #endregion

        #region 接口

        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Backup")]
        public ResultInfo<bool> Backup()
        {
            this.bll.Backup();

            return new ResultInfo<bool>(true, this.bll.Res.Gen.OK, true);
        }

        #endregion
    }
}