using IOSys.Helper.Config;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TDB.DB
{
    /// <summary>
    /// mysql数据库操作
    /// </summary>
    public class MysqlDAL
    {
        #region 单例

        /// <summary>
        /// 构造函数
        /// </summary>
        private MysqlDAL()
        {
        }

        private static object _lock = new object();
        private static MysqlDAL _inst = null;
        /// <summary>
        /// 单例
        /// </summary>
        public static MysqlDAL Inst
        {
            get
            {
                if (_inst == null)
                {
                    lock (_lock)
                    {
                        if (_inst == null)
                        {
                            _inst = new MysqlDAL();
                        }
                    }
                }

                return _inst;
            }
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <returns>备份文件完整文件名</returns>
        public string Backup()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), IOSysJson.Inst.AppConfig.DBBackupPath);
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            var file = Path.Combine(path, $"IO_DBBackup_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.sql");
            using (var conn = new MySqlConnection(IOSysJson.Inst.AppConfig.DBConnStr))
            {
                using (var cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportToFile(file);
                        conn.Close();
                    }
                }
            }

            return file;
        }

        #endregion
    }
}
