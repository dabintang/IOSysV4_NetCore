using IOSys.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.Helper.Config
{
    /// <summary>
    /// 文本资源配置
    /// </summary>
    public class ResourceConfig
    {
        /// <summary>
        /// 通用的
        /// </summary>
        public General Gen { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        public Account Acc { get; set; }

        /// <summary>
        /// 基础数据
        /// </summary>
        public Basic Bas { get; set; }

        /// <summary>
        /// 收支
        /// </summary>
        public InOut IO { get; set; }

        /// <summary>
        /// 常量
        /// </summary>
        public Constant Cst { get; set; }
    }

    #region 提示消息

    /// <summary>
    /// 账户
    /// </summary>
    public class Account
    {
        /// <summary>
        /// 用户名或密码不能为空
        /// </summary>
        public MsgInfo LoginParamEmpty { get; set; }

        /// <summary>
        /// 用户名或密码不对
        /// </summary>
        public MsgInfo LoginParamIncorrect { get; set; }

        /// <summary>
        /// 该用户尚未指定家庭，请联系管理员
        /// </summary>
        public MsgInfo UserNoFamily { get; set; }

        /// <summary>
        /// 登录成功
        /// </summary>
        public MsgInfo LoginOK { get; set; }

        /// <summary>
        /// 登录名已存在
        /// </summary>
        public MsgInfo LoginNameExisted { get; set; }

        /// <summary>
        /// 用户不存在
        /// </summary>
        public MsgInfo UserUnexist { get; set; }

        /// <summary>
        /// 原密码不对
        /// </summary>
        public MsgInfo OldPasswordIncorrect { get; set; }

        /// <summary>
        /// 密码不能为空
        /// </summary>
        public MsgInfo PasswordEmpty { get; set; }

        /// <summary>
        /// 密码长度不能超过{0}位
        /// </summary>
        public MsgInfo PasswordTooLong { get; set; }

        /// <summary>
        /// 昵称不能为空
        /// </summary>
        public MsgInfo NickNameEmpty { get; set; }
    }

    /// <summary>
    /// 基础数据
    /// </summary>
    public class Basic
    {
        /// <summary>
        /// 名称已存在
        /// </summary>
        public MsgInfo NameExisted { get; set; }

        /// <summary>
        /// 已被使用
        /// </summary>
        public MsgInfo BeenUsed { get; set; }

        /// <summary>
        /// 账户不存在
        /// </summary>
        public MsgInfo AmountAccountUnexist { get; set; }

        /// <summary>
        /// 收入类型不存在
        /// </summary>
        public MsgInfo InTypeUnexist { get; set; }

        /// <summary>
        /// 支出类型不存在
        /// </summary>
        public MsgInfo OutTypeUnexist { get; set; }

        /// <summary>
        /// 借还类型不存在
        /// </summary>
        public MsgInfo BorrowRepayTypeUnexist { get; set; }
    }

    /// <summary>
    /// 收支
    /// </summary>
    public class InOut
    {
        /// <summary>
        /// 对方名称不能为空
        /// </summary>
        public MsgInfo BRTargetEmpty { get; set; }
    }

    /// <summary>
    /// 通用的
    /// </summary>
    public class General
    {
        /// <summary>
        /// 成功
        /// </summary>
        public MsgInfo OK { get; set; }

        /// <summary>
        /// 未知错误：{0}
        /// </summary>
        public MsgInfo UnkonwErr { get; set; }

        /// <summary>
        /// 签名验证通过
        /// </summary>
        public MsgInfo TokenOK { get; set; }

        /// <summary>
        /// 签名验证失败。（Token：{0}，Message：{1}）
        /// </summary>
        public MsgInfo TokenError { get; set; }

        /// <summary>
        /// 无权操作
        /// </summary>
        public MsgInfo NoRight { get; set; }

        /// <summary>
        /// IP不允许匿名访问。（IP：{0}）
        /// </summary>
        public MsgInfo IPError { get; set; }

        /// <summary>
        /// 等锁超时
        /// </summary>
        public MsgInfo WaiteTimeOut { get; set; }
    }

    #endregion

    #region 常量

    /// <summary>
    /// 常量
    /// </summary>
    public class Constant
    {
        /// <summary>
        /// 借还类型
        /// </summary>
        public BorrowRepayType BRT { get; set; }

        /// <summary>
        /// 收入统计类型
        /// </summary>
        public InGroupType IGT { get; set; }

        /// <summary>
        /// 支出统计类型
        /// </summary>
        public OutGroupType OGT { get; set; }

        /// <summary>
        /// 转账方向
        /// </summary>
        public TransferDirection TranDir { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public DataType DT { get; set; }
    }

    /// <summary>
    /// 转账方向
    /// </summary>
    public class TransferDirection
    {
        /// <summary>
        /// 转入
        /// </summary>
        public string TransferIn { get; set; }

        /// <summary>
        /// 转出
        /// </summary>
        public string TransferOut { get; set; }
    }

    /// <summary>
    /// 借还类型
    /// </summary>
    public class BorrowRepayType
    {
        /// <summary>
        /// 借入
        /// </summary>
        public string BorrowIn { get; set; }

        /// <summary>
        /// 还入
        /// </summary>
        public string RepayIn { get; set; }

        /// <summary>
        /// 借出
        /// </summary>
        public string BorrowOut { get; set; }

        /// <summary>
        /// 还出
        /// </summary>
        public string RepayOut { get; set; }
    }

    /// <summary>
    /// 收入统计类型
    /// </summary>
    public class InGroupType
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string InType { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        public string AmountAccount { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public string Year { get; set; }
    }

    /// <summary>
    /// 支出统计类型
    /// </summary>
    public class OutGroupType
    {
        /// <summary>
        /// 分类
        /// </summary>
        public string OutCategory { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string OutType { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        public string AmountAccount { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public string Year { get; set; }
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public class DataType
    {
        /// <summary>
        /// 收入
        /// </summary>
        public string In { get; set; }

        /// <summary>
        /// 支出
        /// </summary>
        public string Out { get; set; }

        /// <summary>
        /// 转账
        /// </summary>
        public string Transfer { get; set; }

        /// <summary>
        /// 借还
        /// </summary>
        public string BorrowRepay { get; set; }
    }

    #endregion
}
