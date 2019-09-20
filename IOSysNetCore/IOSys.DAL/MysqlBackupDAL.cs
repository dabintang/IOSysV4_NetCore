//using IOSys.Helper.Config;
//using MySql.Data.MySqlClient;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace IOSys.DAL
//{
//    /// <summary>
//    /// 备份mysql
//    /// </summary>
//    public class MysqlBackupDAL
//    {
//        #region 单例

//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        private MysqlBackupDAL()
//        {
//        }

//        private static object _lock = new object();
//        private static MysqlBackupDAL _inst = null;
//        /// <summary>
//        /// 单例
//        /// </summary>
//        public static MysqlBackupDAL Inst
//        {
//            get
//            {
//                if (_inst == null)
//                {
//                    lock (_lock)
//                    {
//                        if (_inst == null)
//                        {
//                            _inst = new MysqlBackupDAL();
//                        }
//                    }
//                }

//                return _inst;
//            }
//        }

//        #endregion

//        #region 公开方法

//        public void Backup()
//        {
//            string file = "C:\\backup.sql";
//            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(IOSysJson.Inst.AppConfig.DBConnStr))
//            {
//                using (var cmd = new MySql.Data.MySqlClient.MySqlCommand())
//                {
//                    using (MySqlBackup mb = new MySqlBackup(cmd))
//                    {
//                        cmd.Connection = conn;
//                        conn.Open();
//                        mb.ExportToFile(file);
//                        conn.Close();
//                    }
//                }
//            }
//        }

//        #endregion
//    }
//}
