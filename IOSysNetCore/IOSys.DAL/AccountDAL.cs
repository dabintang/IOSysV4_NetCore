using IOSys.Helper;
using IOSys.Helper.Const;
using IOSys.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOSys.DAL
{
    /// <summary>
    /// 账号 数据层
    /// </summary>
    public class AccountDAL : BaseDAL
    {
        #region 单例

        /// <summary>
        /// 构造函数
        /// </summary>
        private AccountDAL()
        {
        }

        private static object _lock = new object();
        private static AccountDAL _inst = null;
        /// <summary>
        /// 单例
        /// </summary>
        public static AccountDAL Inst
        {
            get
            {
                if (_inst == null)
                {
                    lock (_lock)
                    {
                        if (_inst == null)
                        {
                            _inst = new AccountDAL();
                        }
                    }
                }

                return _inst;
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(string loginName, string password)
        {
            using (IOSysContext db = new IOSysContext())
            {
                var model = await db.Users.Where(m => m.LoginName == loginName && m.Password == password).FirstOrDefaultAsync();
                return model;
            }
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <returns></returns>
        public async Task<List<User>> QueryUserAsync(int familyID)
        {
            //判断缓存是否存在
            var key = string.Format(SysConst.Cache.UserList_FamilyID, familyID);
            var list = CacheHelper.Get<List<User>>(key);
            if (list != null)
            {
                return list;
            }

            using (IOSysContext db = new IOSysContext())
            {
                //从数据库查找
                list = await db.Users.Where(m => m.FamilyID == familyID && m.IsDelete == false).OrderByDescending(m => m.IsActive).ThenByDescending(m => m.LoginName).ToListAsync();

                //写入缓存
                CacheHelper.Set(key, list);

                return list;
            }
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(int familyID, int userID)
        {
            var list = await this.QueryUserAsync(familyID);
            var model = list.Find(m => m.ID == userID);

            return model;
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="model">用户</param>
        /// <returns>主键ID</returns>
        public async Task<int> SaveUserAsync(User model)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //更新
                if (model.ID > 0)
                {
                    db.Entry(model).State = EntityState.Modified;
                }
                //新增
                else
                {
                    await db.Users.AddAsync(model);
                }

                await db.SaveChangesAsync();
            }

            //移除缓存
            var key = string.Format(SysConst.Cache.UserList_FamilyID, model.FamilyID);
            CacheHelper.Remove(key);

            return model.ID;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="userID">用户ID</param>
        public async Task DeleteUserAsync(int familyID, int userID)
        {
            //获取原数据
            var model = await this.GetUserAsync(familyID, userID);
            if (model == null)
            {
                return;
            }

            //设删除标识
            model.IsDelete = true;

            using (IOSysContext db = new IOSysContext())
            {
                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            //移除缓存
            var key = string.Format(SysConst.Cache.UserList_FamilyID, familyID);
            CacheHelper.Remove(key);
        }

        #endregion
    }
}
