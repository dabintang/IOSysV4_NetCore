using IOSys.DAL;
using IOSys.DTO;
using IOSys.DTO.Account;
using IOSys.Helper;
using IOSys.Helper.Config;
using IOSys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOSys.BLL
{
    /// <summary>
    /// 账号 业务逻辑层
    /// </summary>
    public class AccountBLL : BaseBLL
    {
        #region 公开方法

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="req">条件</param>
        /// <param name="clientIP">客户端IP</param>
        /// <returns></returns>
        public async Task<ResultInfo<LoginInfo>> LoginAsync(LoginReq req, string clientIP)
        {
            //验空
            if (req == null || string.IsNullOrEmpty(req.LoginName) || string.IsNullOrEmpty(req.Password))
            {
                return new ResultInfo<LoginInfo>(false, this.Res.Acc.LoginParamEmpty, null);
            }

            //密码DES解密
            var pwd = EncryptHelper.DecryptAES(req.Password);

            //获取用户信息
            var user = await AccountDAL.Inst.GetUserAsync(req.LoginName, EncryptHelper.Md5(pwd));
            if (user == null)
            {
                return new ResultInfo<LoginInfo>(false, this.Res.Acc.LoginParamIncorrect, null);
            }

            //获取家庭信息
            var family = await BasicDAL.Inst.GetFamilyAsync(user.FamilyID);
            if (family == null)
            {
                return new ResultInfo<LoginInfo>(false, this.Res.Acc.UserNoFamily, null);
            }

            //token里存放的信息
            var tokenInfo = new TokenInfo();
            tokenInfo.UserID = user.ID;
            tokenInfo.FamilyID = family.ID;

            //返回的登录信息
            var loginInfo = new LoginInfo();
            loginInfo.NickName = user.NickName;
            loginInfo.FamilyName = family.Name;
            loginInfo.Token = JWTHelper.CreateToken(tokenInfo, clientIP);

            //登录日志
            var logLogin = new LoginLog();
            logLogin.UserID = user.ID;
            logLogin.Token = loginInfo.Token;
            logLogin.IP = clientIP;
            logLogin.LoginTime = DateTime.Now;
            await LoginLogDAL.Inst.AddLogAsync(logLogin);

            return new ResultInfo<LoginInfo>(true, this.Res.Acc.LoginOK, loginInfo);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<UserInfo> GetUserAsync(int id)
        {
            //获取数据库信息
            var model = await AccountDAL.Inst.GetUserAsync(this.LoginInfo.FamilyID, id);
            if (model == null)
            {
                return null;
            }

            //结果
            var info = new UserInfo();
            info.ID = model.ID;
            info.LoginName = model.LoginName;
            info.NickName = model.NickName;
            info.Mobile = model.Mobile;
            info.Email = model.Email;
            info.Remark = model.Remark;

            return info;
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="info">用户</param>
        /// <returns>主键ID</returns>
        public async Task<ResultInfo<int>> SaveUserAsync(UserInfo info)
        {
            //昵称不能为空
            if (string.IsNullOrWhiteSpace(info.NickName))
            {
                return new ResultInfo<int>(false, this.Res.Acc.NickNameEmpty, -1);
            }

            //获取原有列表
            var list = await AccountDAL.Inst.QueryUserAsync(this.LoginInfo.FamilyID);

            //判断登录名是否已被使用
            if (list.Exists(m => m.ID != info.ID && m.LoginName == info.LoginName))
            {
                return new ResultInfo<int>(false, this.Res.Acc.LoginNameExisted, -1);
            }

            //尝试查找原数据
            var model = list.Find(m => m.ID == info.ID);

            //新建
            if (model == null)
            {
                model = new User();
                model.FamilyID = this.LoginInfo.FamilyID;
                model.LoginName = info.LoginName;
                model.Password = EncryptHelper.Md5("888888");
                model.IsActive = true;
                model.IsDelete = false;
            }

            model.NickName = info.NickName;
            model.Mobile = info.Mobile;
            model.Email = info.Email;
            model.Remark = info.Remark;

            //设置创建者/更新者字段值
            this.SetCreateUpdateFields(model);

            //保存到数据库
            await AccountDAL.Inst.SaveUserAsync(model);

            return new ResultInfo<int>(true, Res.Gen.OK, model.ID);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="req">参数</param>
        /// <returns></returns>
        public async Task<ResultInfo<bool>> ChangePassword(ChangePwdReq req)
        {
            //新密码不能为空
            if (string.IsNullOrEmpty(req.NewPassword))
            {
                return new ResultInfo<bool>(false, this.Res.Acc.PasswordEmpty, false);
            }

            //新密码DES解密
            var newPwd = EncryptHelper.DecryptAES(req.NewPassword);

            //获取数据库信息
            var model = await AccountDAL.Inst.GetUserAsync(this.LoginInfo.FamilyID, this.LoginInfo.UserID);

            //用户不存在
            if (model == null)
            {
                return new ResultInfo<bool>(false, this.Res.Acc.UserUnexist, false);
            }

            //旧密码DES解密
            var oldPwd = EncryptHelper.DecryptAES(req.OldPassword);

            //原密码不对
            if (EncryptHelper.Md5(oldPwd) != model.Password)
            {
                return new ResultInfo<bool>(false, this.Res.Acc.OldPasswordIncorrect, false);
            }

            //设置新密码
            model.Password = EncryptHelper.Md5(newPwd);

            //保存
            await AccountDAL.Inst.SaveUserAsync(model);

            return new ResultInfo<bool>(true, Res.Gen.OK, true);
        }

        #endregion
    }
}
