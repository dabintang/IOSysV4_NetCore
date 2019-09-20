using IOSys.DTO.Enum;
using IOSys.DTO.InOut;
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
    /// 收支操作 数据层
    /// </summary>
    public class InOutDAL : BaseDAL
    {
        #region 单例

        /// <summary>
        /// 构造函数
        /// </summary>
        private InOutDAL()
        {
        }

        private static object _lock = new object();
        private static InOutDAL _inst = null;
        /// <summary>
        /// 单例
        /// </summary>
        public static InOutDAL Inst
        {
            get
            {
                if (_inst == null)
                {
                    lock (_lock)
                    {
                        if (_inst == null)
                        {
                            _inst = new InOutDAL();
                        }
                    }
                }

                return _inst;
            }
        }

        #endregion

        #region 收入

        /// <summary>
        /// 查询指定日期的收入信息
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public async Task<List<InCome>> QueryInComeAsync(int familyID, DateTime date)
        {
            using (IOSysContext db = new IOSysContext())
            {
                var list = await db.InComes.AsNoTracking().Where(m => m.FamilyID == familyID && m.InDate == date.Date).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 获取收入信息
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="inComeID">收入ID</param>
        /// <returns></returns>
        public async Task<InCome> GetInComeAsync(int familyID, int inComeID)
        {
            if (inComeID <= 0)
            {
                return null;
            }

            using (IOSysContext db = new IOSysContext())
            {
                var model = await db.InComes.AsNoTracking().Where(m => m.ID == inComeID && m.FamilyID == familyID).FirstOrDefaultAsync();

                return model;
            }
        }

        /// <summary>
        /// 保存收入信息
        /// </summary>
        /// <param name="model">收入信息</param>
        /// <returns>主键ID</returns>
        public async Task<int> SaveInComeAsync(InCome model)
        {
            using (IOSysContext db = new IOSysContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    InCome oldModel = null;

                    //尝试查找原数据
                    if (model.ID > 0)
                    {
                        oldModel = db.InComes.AsNoTracking().Where(m => m.ID == model.ID && m.FamilyID == model.FamilyID).FirstOrDefault();

                        if (oldModel == null)
                        {
                            model.CreatorID = model.UpdateID.Value;
                            model.CreateTime = model.UpdateTime.Value;
                        }
                    }

                    //新增
                    if (oldModel == null)
                    {
                        //新增
                        await db.InComes.AddAsync(model);

                        //修改账户信息
                        await BasicDAL.Inst.UseAmountAccount(model.FamilyID, model.AmountAccountID, model.Amount, db);
                        ////修改收入类型信息
                        //await BasicDAL.Inst.UseInType(model.FamilyID, model.InTypeID, db);
                    }
                    //更新
                    else
                    {
                        //设置更新者信息
                        model.UpdateID = model.UpdateID;
                        model.UpdateTime = model.UpdateTime;
                        model.CreatorID = oldModel.CreatorID;
                        model.CreateTime = oldModel.CreateTime;

                        //更新
                        db.Entry(model).State = EntityState.Modified;

                        //修改账户信息
                        await BasicDAL.Inst.ChangeAmountAccount(model.FamilyID, model.AmountAccountID, model.Amount, oldModel.AmountAccountID, oldModel.Amount, db);
                        ////修改收入类型信息
                        //await BasicDAL.Inst.ChangeInType(model.FamilyID, model.InTypeID, oldModel.InTypeID, db);
                    }

                    //保存修改
                    await db.SaveChangesAsync();

                    //提交事务
                    trans.Commit();
                }
            }

            //更新最小交易流水日期缓存
            this.UpdateMinTurnoverDate(model.FamilyID, model.InDate);

            return model.ID;
        }

        /// <summary>
        /// 删除收入信息
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="inComeID">收入ID</param>
        public async Task<bool> DeleteInComeAsync(int familyID, int inComeID)
        {
            using (IOSysContext db = new IOSysContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    //获取原数据
                    var model = db.InComes.Where(m => m.ID == inComeID && m.FamilyID == familyID).FirstOrDefault();
                    if (model == null)
                    {
                        return false;
                    }

                    //删除
                    db.InComes.Remove(model);

                    //修改账户信息
                    await BasicDAL.Inst.UnUseAmountAccount(model.FamilyID, model.AmountAccountID, model.Amount, db);
                    ////修改收入类型信息
                    //await BasicDAL.Inst.UnUseInType(model.FamilyID, model.InTypeID, db);

                    await db.SaveChangesAsync();

                    //提交事务
                    trans.Commit();

                    //移除最小交易流水日期缓存
                    this.RemoveMinTurnoverDate(familyID, model.InDate);

                    return true;
                }
            }
        }

        /// <summary>
        /// 统计账户在收入表中的使用次数
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">截止时间</param>
        /// <returns></returns>
        public async Task<List<UseTimesTemp>> SumAmountAccountTimesInInComeAsync(DateTime? start, DateTime? end)
        {
            using (IOSysContext db = new IOSysContext())
            {
                var q = db.InComes.AsNoTracking();

                //开始时间
                if (start.HasValue)
                {
                    q = q.Where(m => m.InDate >= start.Value);
                }

                //截止时间
                if (end.HasValue)
                {
                    q = q.Where(m => m.InDate <= end.Value);
                }

                var list = await (from m in q
                                  group m by m.AmountAccountID into g
                                  select new UseTimesTemp()
                                  {
                                      ID = g.Key,
                                      Times = g.Count()
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 统计收入类型在收入表中的使用次数
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">截止时间</param>
        /// <returns></returns>
        public async Task<List<UseTimesTemp>> SumInTypeTimesAsync(DateTime? start, DateTime? end)
        {
            using (IOSysContext db = new IOSysContext())
            {
                var q = db.InComes.AsNoTracking();

                //开始时间
                if (start.HasValue)
                {
                    q = q.Where(m => m.InDate >= start.Value);
                }

                //截止时间
                if (end.HasValue)
                {
                    q = q.Where(m => m.InDate <= end.Value);
                }

                var list = await (from m in q
                                  group m by m.InTypeID into g
                                  select new UseTimesTemp()
                                  {
                                      ID = g.Key,
                                      Times = g.Count()
                                  }).ToListAsync();

                return list;
            }
        }

        #endregion

        #region 支出

        /// <summary>
        /// 查询指定日期的支出信息
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public async Task<List<OutPut>> QueryOutPutAsync(int familyID, DateTime date)
        {
            using (IOSysContext db = new IOSysContext())
            {
                var list = await db.OutPuts.AsNoTracking().Where(m => m.FamilyID == familyID && m.OutDate == date.Date).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 获取支出信息
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="outPutID">支出ID</param>
        /// <returns></returns>
        public async Task<OutPut> GetOutPutAsync(int familyID, int outPutID)
        {
            if (outPutID <= 0)
            {
                return null;
            }

            using (IOSysContext db = new IOSysContext())
            {
                var model = await db.OutPuts.AsNoTracking().Where(m => m.ID == outPutID && m.FamilyID == familyID).FirstOrDefaultAsync();

                return model;
            }
        }

        /// <summary>
        /// 保存支出信息
        /// </summary>
        /// <param name="model">支出信息</param>
        /// <returns>主键ID</returns>
        public async Task<int> SaveOutPutAsync(OutPut model)
        {
            using (IOSysContext db = new IOSysContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    OutPut oldModel = null;
                    
                    //尝试查找原数据
                    if (model.ID > 0)
                    {
                        oldModel = db.OutPuts.AsNoTracking().Where(m => m.ID == model.ID && m.FamilyID == model.FamilyID).FirstOrDefault();

                        if (oldModel == null)
                        {
                            model.CreatorID = model.UpdateID.Value;
                            model.CreateTime = model.UpdateTime.Value;
                        }
                    }
                        
                    //新增
                    if (oldModel == null)
                    {
                        //新增
                        await db.OutPuts.AddAsync(model);

                        //修改账户信息
                        await BasicDAL.Inst.UseAmountAccount(model.FamilyID, model.AmountAccountID, -model.Amount, db);
                        ////修改支出类型信息
                        //await BasicDAL.Inst.UseOutType(model.FamilyID, model.OutTypeID, db);
                    }
                    //更新
                    else
                    {
                        //设置更新者信息
                        model.UpdateID = model.UpdateID;
                        model.UpdateTime = model.UpdateTime;
                        model.CreatorID = oldModel.CreatorID;
                        model.CreateTime = oldModel.CreateTime;

                        //更新
                        db.Entry(model).State = EntityState.Modified;

                        //修改账户信息
                        await BasicDAL.Inst.ChangeAmountAccount(model.FamilyID, model.AmountAccountID, -model.Amount, oldModel.AmountAccountID, -oldModel.Amount, db);
                        ////修改支出类型信息
                        //await BasicDAL.Inst.ChangeOutType(model.FamilyID, model.OutTypeID, oldModel.OutTypeID, db);
                    }

                    //保存修改
                    await db.SaveChangesAsync();

                    //提交事务
                    trans.Commit();
                }
            }

            //更新最小交易流水日期缓存
            this.UpdateMinTurnoverDate(model.FamilyID, model.OutDate);

            return model.ID;
        }

        /// <summary>
        /// 删除支出信息
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="outPutID">支出ID</param>
        public async Task<bool> DeleteOutPutAsync(int familyID, int outPutID)
        {
            using (IOSysContext db = new IOSysContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    //获取原数据
                    var model = db.OutPuts.Where(m => m.ID == outPutID && m.FamilyID == familyID).FirstOrDefault();
                    if (model == null)
                    {
                        return false;
                    }

                    //删除
                    db.OutPuts.Remove(model);

                    //修改账户信息
                    await BasicDAL.Inst.UnUseAmountAccount(model.FamilyID, model.AmountAccountID, -model.Amount, db);
                    ////修改支出类型信息
                    //await BasicDAL.Inst.UnUseOutType(model.FamilyID, model.OutTypeID, db);

                    await db.SaveChangesAsync();

                    //提交事务
                    trans.Commit();

                    //移除最小交易流水日期缓存
                    this.RemoveMinTurnoverDate(familyID, model.OutDate);

                    return true;
                }
            }
        }

        /// <summary>
        /// 统计账户在支出表中的使用次数
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">截止时间</param>
        /// <returns></returns>
        public async Task<List<UseTimesTemp>> SumAmountAccountTimesInOutPutAsync(DateTime? start, DateTime? end)
        {
            using (IOSysContext db = new IOSysContext())
            {
                var q = db.OutPuts.AsNoTracking();

                //开始时间
                if (start.HasValue)
                {
                    q = q.Where(m => m.OutDate >= start.Value);
                }

                //截止时间
                if (end.HasValue)
                {
                    q = q.Where(m => m.OutDate <= end.Value);
                }

                var list = await (from m in q
                                  group m by m.AmountAccountID into g
                                  select new UseTimesTemp()
                                  {
                                      ID = g.Key,
                                      Times = g.Count()
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 统计支出类型在支出表中的使用次数
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">截止时间</param>
        /// <returns></returns>
        public async Task<List<UseTimesTemp>> SumOutTypeTimesAsync(DateTime? start, DateTime? end)
        {
            using (IOSysContext db = new IOSysContext())
            {
                var q = db.OutPuts.AsNoTracking();

                //开始时间
                if (start.HasValue)
                {
                    q = q.Where(m => m.OutDate >= start.Value);
                }

                //截止时间
                if (end.HasValue)
                {
                    q = q.Where(m => m.OutDate <= end.Value);
                }

                var list = await (from m in q
                                  group m by m.OutTypeID into g
                                  select new UseTimesTemp()
                                  {
                                      ID = g.Key,
                                      Times = g.Count()
                                  }).ToListAsync();

                return list;
            }
        }

        #endregion

        #region 转账

        /// <summary>
        /// 查询指定日期的转账信息
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public async Task<List<AmountAccountTransfer>> QueryTransferAsync(int familyID, DateTime date)
        {
            using (IOSysContext db = new IOSysContext())
            {
                var list = await db.AmountAccountTransfers.AsNoTracking().Where(m => m.FamilyID == familyID && m.TransferDate == date.Date).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 获取转账信息
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="transferID">转账ID</param>
        /// <returns></returns>
        public async Task<AmountAccountTransfer> GetTransferAsync(int familyID, int transferID)
        {
            if (transferID <= 0)
            {
                return null;
            }

            using (IOSysContext db = new IOSysContext())
            {
                var model = await db.AmountAccountTransfers.AsNoTracking().Where(m => m.ID == transferID && m.FamilyID == familyID).FirstOrDefaultAsync();

                return model;
            }
        }

        /// <summary>
        /// 保存转账信息
        /// </summary>
        /// <param name="model">转账信息</param>
        /// <returns>主键ID</returns>
        public async Task<int> SaveTransferAsync(AmountAccountTransfer model)
        {
            using (IOSysContext db = new IOSysContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    AmountAccountTransfer oldModel = null;

                    //尝试查找原数据
                    if (model.ID > 0)
                    {
                        oldModel = db.AmountAccountTransfers.AsNoTracking().Where(m => m.ID == model.ID && m.FamilyID == model.FamilyID).FirstOrDefault();

                        if (oldModel == null)
                        {
                            model.CreatorID = model.UpdateID.Value;
                            model.CreateTime = model.UpdateTime.Value;
                        }
                    }

                    //新增
                    if (oldModel == null)
                    {
                        //新增
                        await db.AmountAccountTransfers.AddAsync(model);

                        //修改账户信息
                        await BasicDAL.Inst.UseAmountAccount(model.FamilyID, model.FromAmountAccountID, -model.Amount, db);
                        await BasicDAL.Inst.UseAmountAccount(model.FamilyID, model.ToAmountAccountID, model.Amount, db);
                    }
                    //更新
                    else
                    {
                        //设置更新者信息
                        model.UpdateID = model.UpdateID;
                        model.UpdateTime = model.UpdateTime;
                        model.CreatorID = oldModel.CreatorID;
                        model.CreateTime = oldModel.CreateTime;

                        //更新
                        db.Entry(model).State = EntityState.Modified;

                        //修改账户信息
                        await BasicDAL.Inst.ChangeAmountAccount(model.FamilyID, model.FromAmountAccountID, -model.Amount, oldModel.FromAmountAccountID, -oldModel.Amount, db);
                        await BasicDAL.Inst.ChangeAmountAccount(model.FamilyID, model.ToAmountAccountID, model.Amount, oldModel.ToAmountAccountID, oldModel.Amount, db);
                    }

                    //保存修改
                    await db.SaveChangesAsync();

                    //提交事务
                    trans.Commit();
                }
            }

            //更新最小交易流水日期缓存
            this.UpdateMinTurnoverDate(model.FamilyID, model.TransferDate);

            return model.ID;
        }

        /// <summary>
        /// 删除转账信息
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="transferID">转账ID</param>
        public async Task<bool> DeleteTransferAsync(int familyID, int transferID)
        {
            using (IOSysContext db = new IOSysContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    //获取原数据
                    var model = db.AmountAccountTransfers.Where(m => m.ID == transferID && m.FamilyID == familyID).FirstOrDefault();
                    if (model == null)
                    {
                        return false;
                    }

                    //删除
                    db.AmountAccountTransfers.Remove(model);

                    //修改账户信息
                    await BasicDAL.Inst.UnUseAmountAccount(model.FamilyID, model.FromAmountAccountID, -model.Amount, db);
                    await BasicDAL.Inst.UnUseAmountAccount(model.FamilyID, model.ToAmountAccountID, model.Amount, db);

                    await db.SaveChangesAsync();

                    //提交事务
                    trans.Commit();

                    //移除最小交易流水日期缓存
                    this.RemoveMinTurnoverDate(familyID, model.TransferDate);

                    return true;
                }
            }
        }

        /// <summary>
        /// 统计账户在转账表中的使用次数
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">截止时间</param>
        /// <returns></returns>
        public async Task<List<UseTimesTemp>> SumAmountAccountTimesInTransferAsync(DateTime? start, DateTime? end)
        {
            using (IOSysContext db = new IOSysContext())
            {
                var q = db.AmountAccountTransfers.AsNoTracking();

                //开始时间
                if (start.HasValue)
                {
                    q = q.Where(m => m.TransferDate >= start.Value);
                }

                //截止时间
                if (end.HasValue)
                {
                    q = q.Where(m => m.TransferDate <= end.Value);
                }

                //原账户统计
                var lstFrom = await (from m in q
                                  group m by m.FromAmountAccountID into g
                                  select new UseTimesTemp()
                                  {
                                      ID = g.Key,
                                      Times = g.Count()
                                  }).ToListAsync();

                //目标账户统计
                var lstTo = await (from m in q
                                     group m by m.ToAmountAccountID into g
                                     select new UseTimesTemp()
                                     {
                                         ID = g.Key,
                                         Times = g.Count()
                                     }).ToListAsync();

                //合计
                var list = (from m in lstFrom.Union(lstTo)
                            group m by m.ID into g
                            select new UseTimesTemp()
                            {
                                ID = g.Key,
                                Times = g.Sum(m => m.Times)
                            }).ToList();

                return list;
            }
        }

        #endregion

        #region 借还

        /// <summary>
        /// 查询指定日期的借还信息
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public async Task<List<BorrowRepay>> QueryBorrowRepayAsync(int familyID, DateTime date)
        {
            using (IOSysContext db = new IOSysContext())
            {
                var list = await db.BorrowRepays.AsNoTracking().Where(m => m.FamilyID == familyID && m.BRDate == date.Date).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 获取借还信息
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="borrowRepayID">借还ID</param>
        /// <returns></returns>
        public async Task<BorrowRepay> GetBorrowRepayAsync(int familyID, int borrowRepayID)
        {
            if (borrowRepayID <= 0)
            {
                return null;
            }

            using (IOSysContext db = new IOSysContext())
            {
                var model = await db.BorrowRepays.AsNoTracking().Where(m => m.ID == borrowRepayID && m.FamilyID == familyID).FirstOrDefaultAsync();

                return model;
            }
        }

        /// <summary>
        /// 保存借还信息
        /// </summary>
        /// <param name="model">借还信息</param>
        /// <returns>主键ID</returns>
        public async Task<int> SaveBorrowRepayAsync(BorrowRepay model)
        {
            using (IOSysContext db = new IOSysContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    BorrowRepay oldModel = null;

                    //尝试查找原数据
                    if (model.ID > 0)
                    {
                        oldModel = db.BorrowRepays.AsNoTracking().Where(m => m.ID == model.ID && m.FamilyID == model.FamilyID).FirstOrDefault();

                        if (oldModel == null)
                        {
                            model.CreatorID = model.UpdateID.Value;
                            model.CreateTime = model.UpdateTime.Value;
                        }
                    }

                    //新增
                    if (oldModel == null)
                    {
                        //新增
                        await db.BorrowRepays.AddAsync(model);

                        //账户金额变动值
                        var amount = this.GetAmountDirectionForBorrowRepay(model.BRType, model.Amount);

                        //修改账户信息
                        await BasicDAL.Inst.UseAmountAccount(model.FamilyID, model.AmountAccountID, amount, db);
                    }
                    //更新
                    else
                    {
                        //设置更新者信息
                        model.UpdateID = model.UpdateID;
                        model.UpdateTime = model.UpdateTime;
                        model.CreatorID = oldModel.CreatorID;
                        model.CreateTime = oldModel.CreateTime;

                        //更新
                        db.Entry(model).State = EntityState.Modified;

                        //账户金额变动值
                        var newAmount = this.GetAmountDirectionForBorrowRepay(model.BRType, model.Amount);
                        var oldAmount = this.GetAmountDirectionForBorrowRepay(oldModel.BRType, oldModel.Amount);

                        //修改账户信息
                        await BasicDAL.Inst.ChangeAmountAccount(model.FamilyID, model.AmountAccountID, newAmount, oldModel.AmountAccountID, oldAmount, db);
                    }

                    //保存修改
                    await db.SaveChangesAsync();

                    //提交事务
                    trans.Commit();
                }
            }

            //更新最小交易流水日期缓存
            this.UpdateMinTurnoverDate(model.FamilyID, model.BRDate);

            return model.ID;
        }

        /// <summary>
        /// 删除借还信息
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="borrowRepayID">借还ID</param>
        public async Task<bool> DeleteBorrowRepayAsync(int familyID, int borrowRepayID)
        {
            using (IOSysContext db = new IOSysContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    //获取原数据
                    var model = db.BorrowRepays.Where(m => m.ID == borrowRepayID && m.FamilyID == familyID).FirstOrDefault();
                    if (model == null)
                    {
                        return false;
                    }

                    //删除
                    db.BorrowRepays.Remove(model);

                    //账户金额变动值
                    var amount = this.GetAmountDirectionForBorrowRepay(model.BRType, model.Amount);

                    //修改账户信息
                    await BasicDAL.Inst.UnUseAmountAccount(model.FamilyID, model.AmountAccountID, amount, db);

                    await db.SaveChangesAsync();

                    //提交事务
                    trans.Commit();

                    //移除最小交易流水日期缓存
                    this.RemoveMinTurnoverDate(familyID, model.BRDate);

                    return true;
                }
            }
        }

        /// <summary>
        /// 获取借还金额变动方向
        /// </summary>
        /// <param name="brType">借还类型</param>
        /// <param name="amount">借还金额</param>
        /// <returns></returns>
        private decimal GetAmountDirectionForBorrowRepay(int brType, decimal amount)
        {
            //账户金额变动值
            var amountDirection = 0M;
            switch ((EnmBorrowRepayType)brType)
            {
                case EnmBorrowRepayType.BorrowIn:
                case EnmBorrowRepayType.RepayIn:
                    amountDirection = amount;
                    break;
                case EnmBorrowRepayType.BorrowOut:
                case EnmBorrowRepayType.RepayOut:
                    amountDirection = -amount;
                    break;
                default:
                    throw new Exception("借还类型不正确");
            }

            return amountDirection;
        }

        /// <summary>
        /// 统计账户在借还表中的使用次数
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">截止时间</param>
        /// <returns></returns>
        public async Task<List<UseTimesTemp>> SumAmountAccountTimesInBorrowRepayAsync(DateTime? start, DateTime? end)
        {
            using (IOSysContext db = new IOSysContext())
            {
                var q = db.BorrowRepays.AsNoTracking();

                //开始时间
                if (start.HasValue)
                {
                    q = q.Where(m => m.BRDate >= start.Value);
                }

                //截止时间
                if (end.HasValue)
                {
                    q = q.Where(m => m.BRDate <= end.Value);
                }

                var list = await (from m in q
                                  group m by m.AmountAccountID into g
                                  select new UseTimesTemp()
                                  {
                                      ID = g.Key,
                                      Times = g.Count()
                                  }).ToListAsync();

                return list;
            }
        }

        #endregion

        #region 其他

        /// <summary>
        /// 获取最小流水日期
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <returns></returns>
        public async Task<DateTime> GetMinTurnoverDate(int familyID)
        {
            //判断缓存是否存在
            var key = string.Format(SysConst.Cache.MinTurnoverDate_FamilyID, familyID);
            var minDate = CacheHelper.Get<DateTime?>(key);
            if (minDate != null)
            {
                return minDate.Value;
            }

            using (IOSysContext db = new IOSysContext())
            {
                //收入
                var taskIn = db.InComes.Where(m => m.FamilyID == familyID).OrderBy(m => m.InDate).Take(1).FirstOrDefaultAsync();
                //支出
                var taskOut = db.OutPuts.Where(m => m.FamilyID == familyID).OrderBy(m => m.OutDate).Take(1).FirstOrDefaultAsync();
                //转账
                var taskTransfer = db.AmountAccountTransfers.Where(m => m.FamilyID == familyID).OrderBy(m => m.TransferDate).Take(1).FirstOrDefaultAsync();
                //借还
                var taskBR = db.BorrowRepays.Where(m => m.FamilyID == familyID).OrderBy(m => m.BRDate).Take(1).FirstOrDefaultAsync();

                minDate = DateTime.Today;
                //收入
                var income = await taskIn;
                minDate = (income != null && minDate > income.InDate) ? income.InDate : minDate;
                //支出
                var output = await taskOut;
                minDate = (output != null && minDate > output.OutDate) ? output.OutDate : minDate;
                //转账
                var transfer = await taskTransfer;
                minDate = (transfer != null && minDate > transfer.TransferDate) ? transfer.TransferDate : minDate;
                //借还
                var br = await taskBR;
                minDate = (br != null && minDate > br.BRDate) ? br.BRDate : minDate;

                //写入缓存
                CacheHelper.Set(key, minDate);

                return minDate ?? DateTime.Today;
            }
        }

        /// <summary>
        /// 更新最小交易流水日期
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="minDate">交易流水日期</param>
        public void UpdateMinTurnoverDate(int familyID, DateTime date)
        {
            var minDate = this.GetMinTurnoverDate(familyID).Result;
            if (minDate > date)
            {
                var key = string.Format(SysConst.Cache.MinTurnoverDate_FamilyID, familyID);

                //写入缓存
                CacheHelper.Set(key, date);
            }
        }

        /// <summary>
        /// 移除最小交易流水日期缓存
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="minDate">交易流水日期</param>
        public void RemoveMinTurnoverDate(int familyID, DateTime date)
        {
            var minDate = this.GetMinTurnoverDate(familyID).Result;
            if (minDate >= date)
            {
                //key
                var key = string.Format(SysConst.Cache.MinTurnoverDate_FamilyID, familyID);

                //移除缓存
                CacheHelper.Remove(key);
            }
        }

        #endregion
    }
}
