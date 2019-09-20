using IOSys.Helper.Config;
using IOSys.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DAL
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class IOSysContext : DbContext
    {
        #region 表

        /// <summary>
        /// 账户表
        /// </summary>
        public DbSet<AmountAccount> AmountAccounts { get; set; }

        /// <summary>
        /// 转账表
        /// </summary>
        public DbSet<AmountAccountTransfer> AmountAccountTransfers { get; set; }

        /// <summary>
        /// 家庭表
        /// </summary>
        public DbSet<Family> Familys { get; set; }

        /// <summary>
        /// 收入表
        /// </summary>
        public DbSet<InCome> InComes { get; set; }

        /// <summary>
        /// 收入类型表
        /// </summary>
        public DbSet<InType> InTypes { get; set; }

        /// <summary>
        /// 支出分类表
        /// </summary>
        public DbSet<OutCategory> OutCategorys { get; set; }

        /// <summary>
        /// 支出表
        /// </summary>
        public DbSet<OutPut> OutPuts { get; set; }

        /// <summary>
        /// 支出类型表
        /// </summary>
        public DbSet<OutType> OutTypes { get; set; }

        /// <summary>
        /// 借还表
        /// </summary>
        public DbSet<BorrowRepay> BorrowRepays { get; set; }

        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 登录日志表
        /// </summary>
        public DbSet<LoginLog> LoginLogs { get; set; }

        #endregion

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(IOSysJson.Inst.AppConfig.DBConnStr);//配置连接字符串
            base.OnConfiguring(optionsBuilder);
        }
    }
}
