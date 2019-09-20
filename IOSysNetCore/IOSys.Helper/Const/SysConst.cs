using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.Helper.Const
{
    /// <summary>
    /// 常量
    /// </summary>
    public class SysConst
    {
        /// <summary>
        /// 缓存常量
        /// </summary>
        public class Cache
        {
            /// <summary>
            /// 家庭
            /// </summary>
            public const string Family_ID = "Family_ID-{0}";

            /// <summary>
            /// 家庭收入类型列表
            /// </summary>
            public const string InTypeList_FamilyID = "InTypeList_FamilyID-{0}";

            /// <summary>
            /// 家庭支出分类列表
            /// </summary>
            public const string OutCategoryList_FamilyID = "OutCategoryList_FamilyID-{0}";

            /// <summary>
            /// 家庭账户列表
            /// </summary>
            public const string AmountAccountList_FamilyID = "AmountAccountList_FamilyID-{0}";

            /// <summary>
            /// 家庭支出类型列表
            /// </summary>
            public const string OutTypeList_FamilyID = "OutTypeList_FamilyID-{0}";

            /// <summary>
            /// 家庭用户列表
            /// </summary>
            public const string UserList_FamilyID = "UserList_FamilyID-{0}";

            /// <summary>
            /// 最小流水日期
            /// </summary>
            public const string MinTurnoverDate_FamilyID = "MinTurnoverDate_FamilyID-{0}";
        }

        /// <summary>
        /// 键值
        /// </summary>
        public class Key
        {
            /// <summary>
            /// 使用语音
            /// </summary>
            public const string Lang = "Lang";
        }

        /// <summary>
        /// 值
        /// </summary>
        public class Value
        {
            /// <summary>
            /// 默认使用语音（zh：中文）
            /// </summary>
            public const string DefaultLang = "zh";
        }
    }
}
