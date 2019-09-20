using IOSys.Helper;
using IOSys.Helper.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDB.DB;

namespace IOSys.BLL
{
    /// <summary>
    /// mysql
    /// </summary>
    public class MysqlBLL : BaseBLL
    {
        #region 公开方法

        /// <summary>
        /// 备份数据库
        /// </summary>
        public void Backup()
        {
            //备份数据库
            var file = MysqlDAL.Inst.Backup();

            //发送邮件
            var content = $"收支数据库备份,{DateTime.Now.ToString("yyyyMMdd")}";
            EmailHelper.Send(content, content, false, IOSysJson.Inst.AppConfig.DBBackupToEmail, file);
        }

        #endregion
    }
}
