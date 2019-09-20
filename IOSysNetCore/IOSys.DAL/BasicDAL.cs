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
    /// 基础信息 数据层
    /// </summary>
    public class BasicDAL : BaseDAL
    {
        #region 单例

        /// <summary>
        /// 构造函数
        /// </summary>
        private BasicDAL()
        {
        }

        private static object _lock = new object();
        private static BasicDAL _inst = null;
        /// <summary>
        /// 单例
        /// </summary>
        public static BasicDAL Inst
        {
            get
            {
                if (_inst == null)
                {
                    lock (_lock)
                    {
                        if (_inst == null)
                        {
                            _inst = new BasicDAL();
                        }
                    }
                }

                return _inst;
            }
        }

        #endregion

        #region 家庭

        /// <summary>
        /// 获取家庭信息
        /// </summary>
        /// <param name="id">家庭ID</param>
        /// <returns></returns>
        public async Task<Family> GetFamilyAsync(int id)
        {
            //判断缓存是否存在
            var key = string.Format(SysConst.Cache.Family_ID, id);
            var family = CacheHelper.Get<Family>(key);
            if (family != null)
            {
                return family;
            }

            using (IOSysContext db = new IOSysContext())
            {
                //从数据库查找
                family = await db.Familys.FindAsync(id);

                //写入缓存
                if (family != null)
                {
                    CacheHelper.Set(key, family);
                }

                return family;
            }
        }

        #endregion

        #region 账户

        /// <summary>
        /// 查询账户
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <returns></returns>
        public async Task<List<AmountAccount>> QueryAmountAccountAsync(int familyID)
        {
            //判断缓存是否存在
            var key = string.Format(SysConst.Cache.AmountAccountList_FamilyID, familyID);
            var list = CacheHelper.Get<List<AmountAccount>>(key);
            if (list != null)
            {
                return list;
            }

            using (IOSysContext db = new IOSysContext())
            {
                //从数据库查找
                list = await db.AmountAccounts.Where(m => m.FamilyID == familyID).OrderByDescending(m => m.IsActive).ThenByDescending(m => m.SortWeight).ToListAsync();

                //写入缓存
                CacheHelper.Set(key, list);

                return list;
            }
        }

        /// <summary>
        /// 查询所有账户
        /// </summary>
        /// <returns></returns>
        public async Task<List<AmountAccount>> QueryAllAmountAccountAsync()
        {
            using (IOSysContext db = new IOSysContext())
            {
                //从数据库查找
                var list = await db.AmountAccounts.OrderByDescending(m => m.IsActive).ThenByDescending(m => m.SortWeight).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 获取账户
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="amountAccountID">账户ID</param>
        /// <returns></returns>
        public async Task<AmountAccount> GetAmountAccountAsync(int familyID, int amountAccountID)
        {
            var list = await this.QueryAmountAccountAsync(familyID);
            var model = list.Find(m => m.ID == amountAccountID);

            return model;
        }

        /// <summary>
        /// 保存账户
        /// </summary>
        /// <param name="model">账户</param>
        /// <returns>主键ID</returns>
        public async Task<int> SaveAmountAccountAsync(AmountAccount model)
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
                    await db.AmountAccounts.AddAsync(model);
                }

                await db.SaveChangesAsync();
            }

            //移除缓存
            var key = string.Format(SysConst.Cache.AmountAccountList_FamilyID, model.FamilyID);
            CacheHelper.Remove(key);

            return model.ID;
        }

        /// <summary>
        /// 保存更新账户排序权重字段
        /// </summary>
        /// <param name="list">账户</param>
        /// <returns>修改条数</returns>
        public async Task<int> UpdateAmountAccountSortWeightAsync(List<AmountAccount> list)
        {
            int retCount = 0;
            using (IOSysContext db = new IOSysContext())
            {
                foreach(var model in list)
                {
                    //更新
                    if (model.ID > 0)
                    {
                        db.Entry(model).Property(m => m.SortWeight).IsModified = true;
                    }
                }

                retCount = await db.SaveChangesAsync();
            }

            //家庭ID
            var lstFamilyID = list.Select(m => m.FamilyID).Distinct().ToList();
            foreach(var familyID in lstFamilyID)
            {
                //移除缓存
                var key = string.Format(SysConst.Cache.AmountAccountList_FamilyID, familyID);
                CacheHelper.Remove(key);
            }

            return retCount;
        }

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="model">账户</param>
        public async Task DeleteAmountAccountAsync(AmountAccount model)
        {
            using (IOSysContext db = new IOSysContext())
            {
                db.AmountAccounts.Remove(model);
                await db.SaveChangesAsync();
            }

            //移除缓存
            var key = string.Format(SysConst.Cache.AmountAccountList_FamilyID, model.FamilyID);
            CacheHelper.Remove(key);
        }

        /// <summary>
        /// 检查账户是否已使用
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<bool> IsAmountAccountUsedAsync(int id)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //收入表是否有使用
                var exists = await db.InComes.AnyAsync(m => m.AmountAccountID == id);
                if (exists)
                {
                    return exists;
                }

                //支出表是否有使用
                exists = await db.OutPuts.AnyAsync(m => m.AmountAccountID == id);
                if (exists)
                {
                    return exists;
                }

                //收入类型表是否有使用
                exists = await db.InTypes.AnyAsync(m => m.AmountAccountID == id);
                if (exists)
                {
                    return exists;
                }

                //支出类型表是否有使用
                exists = await db.OutTypes.AnyAsync(m => m.AmountAccountID == id);
                if (exists)
                {
                    return exists;
                }

                //转账表是否有使用
                exists = await db.AmountAccountTransfers.AnyAsync(m => m.FromAmountAccountID == id || m.ToAmountAccountID == id);
                if (exists)
                {
                    return exists;
                }

                return exists;
            }
        }

        /// <summary>
        /// 修改使用的账户
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="newAmountAccountID">新账户ID</param>
        /// <param name="newAmount">新金额</param>
        /// <param name="oldAmountAccountID">原账户ID</param>
        /// <param name="oldAmount">原金额</param>
        /// <param name="db">数据库上下文</param>
        public async Task ChangeAmountAccount(int familyID, int newAmountAccountID, decimal newAmount, int? oldAmountAccountID, decimal? oldAmount, IOSysContext db)
        {
            //新增使用账户
            if (oldAmountAccountID.HasValue == false)
            {
                await this.UseAmountAccount(familyID, newAmountAccountID, newAmount, db);
            }
            //只修改了金额
            else if (newAmountAccountID == oldAmountAccountID.Value)
            {
                var model = await this.GetAmountAccountAsync(familyID, newAmountAccountID);
                model.Amount += (newAmount - oldAmount.Value);
                db.Entry(model).State = EntityState.Modified;
            }
            //修改了账户
            else
            {
                var taskUse = this.UseAmountAccount(familyID, newAmountAccountID, newAmount, db);
                var taskUnuse = this.UnUseAmountAccount(familyID, oldAmountAccountID.Value, oldAmount.Value, db);

                await taskUse;
                await taskUnuse;
            }
        }

        /// <summary>
        /// 使用账户
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="amountAccountID">账户ID</param>
        /// <param name="amount">金额</param>
        /// <param name="db">数据库上下文</param>
        public async Task UseAmountAccount(int familyID, int amountAccountID, decimal amount, IOSysContext db)
        {
            //获取账户
            var model = await this.GetAmountAccountAsync(familyID, amountAccountID);
            model.Amount += amount;
            //model.SortWeight += 1;
            db.Entry(model).State = EntityState.Modified;
        }

        /// <summary>
        /// 反使用账户
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="amountAccountID">账户ID</param>
        /// <param name="amount">金额</param>
        /// <param name="db">数据库上下文</param>
        public async Task UnUseAmountAccount(int familyID, int amountAccountID, decimal amount, IOSysContext db)
        {
            //获取账户
            var model = await this.GetAmountAccountAsync(familyID, amountAccountID);
            model.Amount -= amount;
            //model.SortWeight -= 1;
            db.Entry(model).State = EntityState.Modified;
        }

        #endregion

        #region 收入类型

        /// <summary>
        /// 查询收入类型
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <returns></returns>
        public async Task<List<InType>> QueryInTypeAsync(int familyID)
        {
            //判断缓存是否存在
            var key = string.Format(SysConst.Cache.InTypeList_FamilyID, familyID);
            var list = CacheHelper.Get<List<InType>>(key);
            if (list != null)
            {
                return list;
            }

            using (IOSysContext db = new IOSysContext())
            {
                //从数据库查找
                list = await db.InTypes.Where(m => m.FamilyID == familyID).OrderByDescending(m => m.IsActive).ThenByDescending(m => m.SortWeight).ToListAsync();

                //写入缓存
                CacheHelper.Set(key, list);

                return list;
            }
        }

        /// <summary>
        /// 查询所有收入类型
        /// </summary>
        /// <returns></returns>
        public async Task<List<InType>> QueryAllInTypeAsync()
        {
            using (IOSysContext db = new IOSysContext())
            {
                //从数据库查找
                var list = await db.InTypes.OrderByDescending(m => m.IsActive).ThenByDescending(m => m.SortWeight).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 获取收入类型
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="inTypeID">收入类型ID</param>
        /// <returns></returns>
        public async Task<InType> GetInTypeAsync(int familyID, int inTypeID)
        {
            var list = await this.QueryInTypeAsync(familyID);
            var model = list.Find(m => m.ID == inTypeID);

            return model;
        }

        /// <summary>
        /// 保存收入类型
        /// </summary>
        /// <param name="model">收入类型</param>
        /// <returns>主键ID</returns>
        public async Task<int> SaveInTypeAsync(InType model)
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
                    await db.InTypes.AddAsync(model);
                }

                await db.SaveChangesAsync();
            }

            //移除缓存
            var key = string.Format(SysConst.Cache.InTypeList_FamilyID, model.FamilyID);
            CacheHelper.Remove(key);

            return model.ID;
        }

        /// <summary>
        /// 保存收入类型
        /// </summary>
        /// <param name="list">收入类型</param>
        /// <returns>改动条数</returns>
        public async Task<int> SaveInTypesAsync(List<InType> list)
        {
            int retCount = 0;
            using (IOSysContext db = new IOSysContext())
            {
                foreach(var model in list)
                {
                    //更新
                    if (model.ID > 0)
                    {
                        db.Entry(model).State = EntityState.Modified;
                    }
                    //新增
                    else
                    {
                        await db.InTypes.AddAsync(model);
                    }
                }

                retCount = await db.SaveChangesAsync();
            }

            //家庭ID
            var lstFamilyID = list.Select(m => m.FamilyID).Distinct().ToList();
            foreach (var familyID in lstFamilyID)
            {
                //移除缓存
                var key = string.Format(SysConst.Cache.InTypeList_FamilyID, familyID);
                CacheHelper.Remove(key);
            }

            return retCount;
        }

        /// <summary>
        /// 删除收入类型
        /// </summary>
        /// <param name="model">收入类型</param>
        public async Task DeleteInTypeAsync(InType model)
        {
            using (IOSysContext db = new IOSysContext())
            {
                db.InTypes.Remove(model);
                await db.SaveChangesAsync();
            }

            //移除缓存
            var key = string.Format(SysConst.Cache.InTypeList_FamilyID, model.FamilyID);
            CacheHelper.Remove(key);
        }

        /// <summary>
        /// 检查收入类型是否已使用
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<bool> IsInTypeUsedAsync(int id)
        {
            using (IOSysContext db = new IOSysContext())
            {
                return await db.InComes.AnyAsync(m => m.InTypeID == id);
            }
        }

        ///// <summary>
        ///// 修改使用的收入类型
        ///// </summary>
        ///// <param name="familyID">家庭ID</param>
        ///// <param name="newOutTypeID">新收入类型ID</param>
        ///// <param name="oldOutTypeID">原收入类型ID</param>
        ///// <param name="db">数据库上下文</param>
        //public async Task ChangeInType(int familyID, int newInTypeID, int? oldInTypeID, IOSysContext db)
        //{
        //    //新增使用收入类型
        //    if (oldInTypeID.HasValue == false)
        //    {
        //        await this.UseInType(familyID, newInTypeID, db);
        //    }
        //    //修改了收入类型
        //    else if (newInTypeID != oldInTypeID)
        //    {
        //        var taskUse = this.UseInType(familyID, newInTypeID, db);
        //        var taskUnuse = this.UnUseInType(familyID, oldInTypeID.Value, db);

        //        await taskUse;
        //        await taskUnuse;
        //    }
        //}

        ///// <summary>
        ///// 使用收入类型
        ///// </summary>
        ///// <param name="familyID">家庭ID</param>
        ///// <param name="inTypeID">收入类型ID</param>
        ///// <param name="db">数据库上下文</param>
        //public async Task UseInType(int familyID, int inTypeID, IOSysContext db)
        //{
        //    //获取收入类型
        //    var model = await this.GetInTypeAsync(familyID, inTypeID);
        //    model.SortWeight += 1;
        //    db.Entry(model).State = EntityState.Modified;
        //}

        ///// <summary>
        ///// 反使用收入类型
        ///// </summary>
        ///// <param name="familyID">家庭ID</param>
        ///// <param name="inTypeID">收入类型ID</param>
        ///// <param name="db">数据库上下文</param>
        //public async Task UnUseInType(int familyID, int inTypeID, IOSysContext db)
        //{
        //    //获取收入类型
        //    var model = await this.GetInTypeAsync(familyID, inTypeID);
        //    model.SortWeight -= 1;
        //    db.Entry(model).State = EntityState.Modified;
        //}

        #endregion

        #region 支出分类

        /// <summary>
        /// 查询支出分类
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <returns></returns>
        public async Task<List<OutCategory>> QueryOutCategoryAsync(int familyID)
        {
            //判断缓存是否存在
            var key = string.Format(SysConst.Cache.OutCategoryList_FamilyID, familyID);
            var list = CacheHelper.Get<List<OutCategory>>(key);
            if (list != null)
            {
                return list;
            }

            using (IOSysContext db = new IOSysContext())
            {
                //从数据库查找
                list = await db.OutCategorys.Where(m => m.FamilyID == familyID).OrderByDescending(m => m.IsActive).ThenByDescending(m => m.SortWeight).ToListAsync();

                //写入缓存
                CacheHelper.Set(key, list);

                return list;
            }
        }

        /// <summary>
        /// 查询所有支出分类
        /// </summary>
        /// <returns></returns>
        public async Task<List<OutCategory>> QueryAllOutCategoryAsync()
        {
            using (IOSysContext db = new IOSysContext())
            {
                //从数据库查找
                var list = await db.OutCategorys.OrderByDescending(m => m.IsActive).ThenByDescending(m => m.SortWeight).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 获取支出分类
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="outCategoryID">支出分类ID</param>
        /// <returns></returns>
        public async Task<OutCategory> GetOutCategoryAsync(int familyID, int outCategoryID)
        {
            var list = await this.QueryOutCategoryAsync(familyID);
            var model = list.Find(m => m.ID == outCategoryID);

            return model;
        }

        /// <summary>
        /// 获取支出分类
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="outTypeID">支出类型ID</param>
        /// <returns></returns>
        public async Task<OutCategory> GetOutCategoryByOutTypeIDAsync(int familyID, int outTypeID)
        {
            var lstOutType = await this.QueryOutTypeAsync(familyID);
            var outType = lstOutType.Find(m => m.ID == outTypeID);
            if (outType == null)
            {
                return null;
            }

            var outCategory = await this.GetOutCategoryAsync(familyID, outType.OutCategoryID);

            return outCategory;
        }

        /// <summary>
        /// 保存支出分类
        /// </summary>
        /// <param name="model">支出分类</param>
        /// <returns>主键ID</returns>
        public async Task<int> SaveOutCategoryAsync(OutCategory model)
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
                    await db.OutCategorys.AddAsync(model);
                }

                await db.SaveChangesAsync();
            }

            //移除缓存
            var key = string.Format(SysConst.Cache.OutCategoryList_FamilyID, model.FamilyID);
            CacheHelper.Remove(key);

            return model.ID;
        }

        /// <summary>
        /// 保存支出分类
        /// </summary>
        /// <param name="list">支出分类</param>
        /// <returns>主键ID</returns>
        public async Task<int> SaveOutCategorysAsync(List<OutCategory> list)
        {
            int retCount = 0;
            using (IOSysContext db = new IOSysContext())
            {
                foreach (var model in list)
                {
                    //更新
                    if (model.ID > 0)
                    {
                        db.Entry(model).State = EntityState.Modified;
                    }
                    //新增
                    else
                    {
                        await db.OutCategorys.AddAsync(model);
                    }
                }

                retCount = await db.SaveChangesAsync();
            }

            //家庭ID
            var lstFamilyID = list.Select(m => m.FamilyID).Distinct().ToList();
            foreach (var familyID in lstFamilyID)
            {
                //移除缓存
                var key = string.Format(SysConst.Cache.OutCategoryList_FamilyID, familyID);
                CacheHelper.Remove(key);
            }

            return retCount;
        }

        /// <summary>
        /// 删除支出分类
        /// </summary>
        /// <param name="model">支出分类</param>
        public async Task DeleteOutCategoryAsync(OutCategory model)
        {
            using (IOSysContext db = new IOSysContext())
            {
                db.OutCategorys.Remove(model);
                await db.SaveChangesAsync();
            }

            //移除缓存
            var key = string.Format(SysConst.Cache.OutCategoryList_FamilyID, model.FamilyID);
            CacheHelper.Remove(key);
        }

        /// <summary>
        /// 检查支出分类是否已使用
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<bool> IsOutCategoryUsedAsync(int id)
        {
            using (IOSysContext db = new IOSysContext())
            {
                return await db.OutTypes.AnyAsync(m => m.OutCategoryID == id);
            }
        }

        ///// <summary>
        ///// 修改使用的支出分类
        ///// </summary>
        ///// <param name="familyID">家庭ID</param>
        ///// <param name="newOutTypeID">新支出分类ID</param>
        ///// <param name="oldOutTypeID">原支出分类ID</param>
        ///// <param name="db">数据库上下文</param>
        //public async Task ChangeOutCategory(int familyID, int newOutCategoryID, int? oldOutCategoryID, IOSysContext db)
        //{
        //    //新增使用支出分类
        //    if (oldOutCategoryID.HasValue == false)
        //    {
        //        await this.UseOutCategory(familyID, newOutCategoryID, db);
        //    }
        //    //修改了支出分类
        //    else if (newOutCategoryID != oldOutCategoryID)
        //    {
        //        var taskUse = this.UseOutCategory(familyID, newOutCategoryID, db);
        //        var taskUnuse = this.UnUseOutCategory(familyID, oldOutCategoryID.Value, db);

        //        await taskUse;
        //        await taskUnuse;
        //    }
        //}

        ///// <summary>
        ///// 使用支出分类
        ///// </summary>
        ///// <param name="familyID">家庭ID</param>
        ///// <param name="outCategoryID">支出分类ID</param>
        ///// <param name="db">数据库上下文</param>
        //public async Task UseOutCategory(int familyID, int outCategoryID, IOSysContext db)
        //{
        //    //获取支出分类
        //    var model = await this.GetOutCategoryAsync(familyID, outCategoryID);
        //    model.SortWeight += 1;
        //    db.Entry(model).State = EntityState.Modified;
        //}

        ///// <summary>
        ///// 反使用支出分类
        ///// </summary>
        ///// <param name="familyID">家庭ID</param>
        ///// <param name="outCategoryID">支出分类ID</param>
        ///// <param name="db">数据库上下文</param>
        //public async Task UnUseOutCategory(int familyID, int outCategoryID, IOSysContext db)
        //{
        //    //获取支出分类
        //    var model = await this.GetOutCategoryAsync(familyID, outCategoryID);
        //    model.SortWeight -= 1;
        //    db.Entry(model).State = EntityState.Modified;
        //}

        #endregion

        #region 支出类型

        /// <summary>
        /// 查询支出类型
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <returns></returns>
        public async Task<List<OutType>> QueryOutTypeAsync(int familyID)
        {
            //判断缓存是否存在
            var key = string.Format(SysConst.Cache.OutTypeList_FamilyID, familyID);
            var list = CacheHelper.Get<List<OutType>>(key);
            if (list != null)
            {
                return list;
            }

            using (IOSysContext db = new IOSysContext())
            {
                //从数据库查找
                list = await db.OutTypes.Where(m => m.FamilyID == familyID).OrderByDescending(m => m.IsActive).ThenByDescending(m => m.SortWeight).ToListAsync();

                //写入缓存
                CacheHelper.Set(key, list);

                return list;
            }
        }

        /// <summary>
        /// 查询所有支出类型
        /// </summary>
        /// <returns></returns>
        public async Task<List<OutType>> QueryAllOutTypeAsync()
        {
            using (IOSysContext db = new IOSysContext())
            {
                //从数据库查找
                var list = await db.OutTypes.OrderByDescending(m => m.IsActive).ThenByDescending(m => m.SortWeight).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 获取支出类型
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="outTypeID">支出类型ID</param>
        /// <returns></returns>
        public async Task<OutType> GetOutTypeAsync(int familyID, int outTypeID)
        {
            var list = await this.QueryOutTypeAsync(familyID);
            var model = list.Find(m => m.ID == outTypeID);

            return model;
        }

        /// <summary>
        /// 保存支出类型
        /// </summary>
        /// <param name="model">支出类型</param>
        /// <returns>主键ID</returns>
        public async Task<int> SaveOutTypeAsync(OutType model)
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
                    await db.OutTypes.AddAsync(model);
                }

                await db.SaveChangesAsync();
            }

            //移除缓存
            var key = string.Format(SysConst.Cache.OutTypeList_FamilyID, model.FamilyID);
            CacheHelper.Remove(key);

            return model.ID;
        }

        /// <summary>
        /// 保存支出类型
        /// </summary>
        /// <param name="list">支出类型</param>
        /// <returns>改动条数</returns>
        public async Task<int> SaveOutTypesAsync(List<OutType> list)
        {
            int retCount = 0;
            using (IOSysContext db = new IOSysContext())
            {
                foreach (var model in list)
                {
                    //更新
                    if (model.ID > 0)
                    {
                        db.Entry(model).State = EntityState.Modified;
                    }
                    //新增
                    else
                    {
                        await db.OutTypes.AddAsync(model);
                    }
                }

                retCount = await db.SaveChangesAsync();
            }

            //家庭ID
            var lstFamilyID = list.Select(m => m.FamilyID).Distinct().ToList();
            foreach (var familyID in lstFamilyID)
            {
                //移除缓存
                var key = string.Format(SysConst.Cache.OutTypeList_FamilyID, familyID);
                CacheHelper.Remove(key);
            }

            return retCount;
        }

        /// <summary>
        /// 删除支出类型
        /// </summary>
        /// <param name="model">支出类型</param>
        public async Task DeleteOutTypeAsync(OutType model)
        {
            using (IOSysContext db = new IOSysContext())
            {
                db.OutTypes.Remove(model);
                await db.SaveChangesAsync();
            }

            //移除缓存
            var key = string.Format(SysConst.Cache.OutTypeList_FamilyID, model.FamilyID);
            CacheHelper.Remove(key);
        }

        /// <summary>
        /// 检查支出类型是否已使用
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<bool> IsOutTypeUsedAsync(int id)
        {
            using (IOSysContext db = new IOSysContext())
            {
                return await db.OutPuts.AnyAsync(m => m.OutTypeID == id);
            }
        }

        ///// <summary>
        ///// 修改使用的支出类型
        ///// </summary>
        ///// <param name="familyID">家庭ID</param>
        ///// <param name="newOutTypeID">新支出类型ID</param>
        ///// <param name="oldOutTypeID">原支出类型ID</param>
        ///// <param name="db">数据库上下文</param>
        //public async Task ChangeOutType(int familyID, int newOutTypeID, int? oldOutTypeID, IOSysContext db)
        //{
        //    //新增使用支出类型
        //    if (oldOutTypeID.HasValue == false)
        //    {
        //        await this.UseOutType(familyID, newOutTypeID, db);
        //    }
        //    //修改了支出类型
        //    else if (newOutTypeID != oldOutTypeID)
        //    {
        //        var taskUse = this.UseOutType(familyID, newOutTypeID, db);
        //        var taskUnuse = this.UnUseOutType(familyID, oldOutTypeID.Value, db);

        //        await taskUse;
        //        await taskUnuse;
        //    }
        //}

        ///// <summary>
        ///// 使用支出类型
        ///// </summary>
        ///// <param name="familyID">家庭ID</param>
        ///// <param name="outTypeID">支出类型ID</param>
        ///// <param name="db">数据库上下文</param>
        //public async Task UseOutType(int familyID, int outTypeID, IOSysContext db)
        //{
        //    //获取支出类型
        //    var model = await this.GetOutTypeAsync(familyID, outTypeID);
        //    model.SortWeight += 1;
        //    db.Entry(model).State = EntityState.Modified;

        //    //获取支出分类
        //    var outCategory = await this.GetOutCategoryByOutTypeIDAsync(familyID, outTypeID);
        //    if (outCategory != null)
        //    {
        //        await this.UseOutCategory(familyID, outCategory.ID, db);
        //    }
        //}

        ///// <summary>
        ///// 反使用支出类型
        ///// </summary>
        ///// <param name="familyID">家庭ID</param>
        ///// <param name="outTypeID">支出类型ID</param>
        ///// <param name="db">数据库上下文</param>
        //public async Task UnUseOutType(int familyID, int outTypeID, IOSysContext db)
        //{
        //    //获取支出类型
        //    var model = await this.GetOutTypeAsync(familyID, outTypeID);
        //    model.SortWeight -= 1;
        //    db.Entry(model).State = EntityState.Modified;

        //    //获取支出分类
        //    var outCategory = await this.GetOutCategoryByOutTypeIDAsync(familyID, outTypeID);
        //    if (outCategory != null)
        //    {
        //        await this.UnUseOutCategory(familyID, outCategory.ID, db);
        //    }
        //}

        #endregion
    }
}
