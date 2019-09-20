using IOSys.DTO.Account;
using IOSys.DTO.Enum;
using IOSys.Helper;
using IOSys.Helper.Config;
using IOSys.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.BLL
{
    /// <summary>
    /// 业务逻辑层基类
    /// </summary>
    public class BaseBLL
    {
        #region 变量

        /// <summary>
        /// token附带的登录信息
        /// </summary>
        public TokenInfo LoginInfo { get; set; }

        /// <summary>
        /// 使用语音
        /// </summary>
        public string Lang { get; set; }

        private ResourceConfig _Msg;
        /// <summary>
        /// 提示信息
        /// </summary>
        public ResourceConfig Res
        {
            get
            {
                if (this._Msg == null)
                {
                    this._Msg = ConfigHelper.GetMsg(this.Lang);
                }

                return this._Msg;
            }
        }

        #endregion

        #region 受保护方法

        /// <summary>
        /// 设置创建者/更新者字段值
        /// </summary>
        /// <param name="model">表对象</param>
        protected void SetCreateUpdateFields(IBaseIDModel model)
        {
            //新建
            if (model.ID == 0)
            {
                model.CreatorID = this.LoginInfo.UserID;
                model.CreateTime = DateTime.Now;
            }
            //更新
            else
            {
                model.UpdateID = this.LoginInfo.UserID;
                model.UpdateTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 获取借还类型名称
        /// </summary>
        /// <param name="brType">借还类型</param>
        /// <returns></returns>
        protected string GetBorrowRepayTypeName(int brType)
        {
            switch ((EnmBorrowRepayType)brType)
            {
                case EnmBorrowRepayType.BorrowIn:
                    return this.Res.Cst.BRT.BorrowIn;
                case EnmBorrowRepayType.RepayIn:
                    return this.Res.Cst.BRT.RepayIn;
                case EnmBorrowRepayType.BorrowOut:
                    return this.Res.Cst.BRT.BorrowOut;
                case EnmBorrowRepayType.RepayOut:
                    return this.Res.Cst.BRT.RepayOut;
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}
