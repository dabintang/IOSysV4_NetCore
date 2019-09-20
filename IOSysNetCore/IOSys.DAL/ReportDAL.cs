using IOSys.DTO;
using IOSys.DTO.Enum;
using IOSys.DTO.Report;
using IOSys.Helper;
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
    /// 报表 数据层
    /// </summary>
    public class ReportDAL : BaseDAL
    {
        #region 单例

        /// <summary>
        /// 构造函数
        /// </summary>
        private ReportDAL()
        {
        }

        private static object _lock = new object();
        private static ReportDAL _inst = null;
        /// <summary>
        /// 单例
        /// </summary>
        public static ReportDAL Inst
        {
            get
            {
                if (_inst == null)
                {
                    lock (_lock)
                    {
                        if (_inst == null)
                        {
                            _inst = new ReportDAL();
                        }
                    }
                }

                return _inst;
            }
        }

        #endregion

        #region 明细

        /// <summary>
        /// 查询收入明细列表
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<PageAmountList<InCome>> QueryInRecordAsync(int familyID, InRecordReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                #region 条件

                //家庭ID
                var q = db.InComes.AsNoTracking().Where(m => m.FamilyID == familyID);

                //开始日期
                if (req.StartDate.HasValue)
                {
                    q = q.Where(m => m.InDate >= req.StartDate.Value);
                }

                //截止日期
                if (req.EndDate.HasValue)
                {
                    q = q.Where(m => m.InDate <= req.EndDate.Value);
                }

                //类型
                if (req.LstInTypeID != null && req.LstInTypeID.Count > 0)
                {
                    q = q.Where(m => req.LstInTypeID.Contains(m.InTypeID));
                }

                //账户
                if (req.LstAmountAccountID != null && req.LstAmountAccountID.Count > 0)
                {
                    q = q.Where(m => req.LstAmountAccountID.Contains(m.AmountAccountID));
                }

                //备注（模糊匹配）
                if (string.IsNullOrWhiteSpace(req.Remark) == false)
                {
                    q = q.Where(m => m.Remark.Contains(req.Remark));
                }

                #endregion

                var totalRecord = 0; //总条数
                var totalAmount = 0M; //总金额

                //是否需要分页
                if (req.IsSetPage())
                {
                    totalAmount = await q.SumAsync(m => m.Amount);
                    totalRecord = await q.CountAsync();
                }

                //设置排序和分页
                this.SetSortPage(ref q, req);

                //查询列表
                var list = await q.ToListAsync();

                //是否需要分页
                if (req.IsSetPage() == false)
                {
                    totalRecord = list.Count;
                    totalAmount = list.Sum(m => m.Amount);
                }

                return new PageAmountList<InCome>(list, totalRecord, totalAmount);
            }
        }

        /// <summary>
        /// 查询支出明细列表
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<PageAmountList<OutPut>> QueryOutRecordAsync(int familyID, OutRecordReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                #region 条件

                //家庭ID
                var q = db.OutPuts.AsNoTracking().Where(m => m.FamilyID == familyID);

                //开始日期
                if (req.StartDate.HasValue)
                {
                    q = q.Where(m => m.OutDate >= req.StartDate.Value);
                }

                //截止日期
                if (req.EndDate.HasValue)
                {
                    q = q.Where(m => m.OutDate <= req.EndDate.Value);
                }

                //类型
                if (req.LstOutTypeID != null && req.LstOutTypeID.Count > 0)
                {
                    q = q.Where(m => req.LstOutTypeID.Contains(m.OutTypeID));
                }

                //账户
                if (req.LstAmountAccountID != null && req.LstAmountAccountID.Count > 0)
                {
                    q = q.Where(m => req.LstAmountAccountID.Contains(m.AmountAccountID));
                }

                //备注（模糊匹配）
                if (string.IsNullOrWhiteSpace(req.Remark) == false)
                {
                    q = q.Where(m => m.Remark.Contains(req.Remark));
                }

                #endregion

                var totalRecord = 0; //总条数
                var totalAmount = 0M; //总金额

                //是否需要分页
                if (req.IsSetPage())
                {
                    totalAmount = await q.SumAsync(m => m.Amount);
                    totalRecord = await q.CountAsync();
                }

                //设置排序和分页
                if (CvtHelper.ToStr(req.Sort).StartsWith("OutCategoryID"))
                {
                    if (CvtHelper.ToStr(req.Sort).EndsWith("asc"))
                    {
                        q = from op in q
                            join otTemp in db.OutTypes on op.OutTypeID equals otTemp.ID into otJoin
                            from ot in otJoin.DefaultIfEmpty()
                            orderby ot.OutCategoryID ascending
                            select op;
                    }
                    else
                    {
                        q = from op in q
                            join otTemp in db.OutTypes on op.OutTypeID equals otTemp.ID into otJoin
                            from ot in otJoin.DefaultIfEmpty()
                            orderby ot.OutCategoryID descending
                            select op;
                    }

                    req.Sort = string.Empty;
                    this.SetSortPage(ref q, req);
                }
                else
                {
                    this.SetSortPage(ref q, req);
                }

                //查询列表
                var list = await q.ToListAsync();

                //是否需要分页
                if (req.IsSetPage() == false)
                {
                    totalRecord = list.Count;
                    totalAmount = list.Sum(m => m.Amount);
                }

                return new PageAmountList<OutPut>(list, totalRecord, totalAmount);
            }
        }

        /// <summary>
        /// 查询转账明细列表
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<PageList<AmountAccountTransfer>> QueryTransferRecordAsync(int familyID, TransferRecordReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                #region 条件

                //家庭ID
                var q = db.AmountAccountTransfers.AsNoTracking().Where(m => m.FamilyID == familyID);

                //开始日期
                if (req.StartDate.HasValue)
                {
                    q = q.Where(m => m.TransferDate >= req.StartDate.Value);
                }

                //截止日期
                if (req.EndDate.HasValue)
                {
                    q = q.Where(m => m.TransferDate <= req.EndDate.Value);
                }

                //源账户
                if (req.LstFromAmountAccountID != null && req.LstFromAmountAccountID.Count > 0)
                {
                    q = q.Where(m => req.LstFromAmountAccountID.Contains(m.FromAmountAccountID));
                }

                //目标账户
                if (req.LstToAmountAccountID != null && req.LstToAmountAccountID.Count > 0)
                {
                    q = q.Where(m => req.LstToAmountAccountID.Contains(m.ToAmountAccountID));
                }

                //备注（模糊匹配）
                if (string.IsNullOrWhiteSpace(req.Remark) == false)
                {
                    q = q.Where(m => m.Remark.Contains(req.Remark));
                }

                #endregion

                var totalRecord = 0; //总条数

                //是否需要分页
                if (req.IsSetPage())
                {
                    totalRecord = await q.CountAsync();
                }

                //设置排序和分页
                this.SetSortPage(ref q, req);

                //查询列表
                var list = await q.ToListAsync();

                //是否需要分页
                if (req.IsSetPage() == false)
                {
                    totalRecord = list.Count;
                }

                return new PageList<AmountAccountTransfer>(list, totalRecord);
            }
        }

        /// <summary>
        /// 查询借还明细列表
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<PageList<BorrowRepay>> QueryBorrowRepayRecordAsync(int familyID, BorrowRepayRecordReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                #region 条件

                //家庭ID
                var q = db.BorrowRepays.AsNoTracking().Where(m => m.FamilyID == familyID);

                //开始日期
                if (req.StartDate.HasValue)
                {
                    q = q.Where(m => m.BRDate >= req.StartDate.Value);
                }

                //截止日期
                if (req.EndDate.HasValue)
                {
                    q = q.Where(m => m.BRDate <= req.EndDate.Value);
                }

                //类型
                if (req.LstBRType != null && req.LstBRType.Count > 0)
                {
                    q = q.Where(m => req.LstBRType.Contains(m.BRType));
                }

                //账户
                if (req.LstAmountAccountID != null && req.LstAmountAccountID.Count > 0)
                {
                    q = q.Where(m => req.LstAmountAccountID.Contains(m.AmountAccountID));
                }

                //对方（模糊匹配）
                if (string.IsNullOrWhiteSpace(req.Target) == false)
                {
                    q = q.Where(m => m.Target.Contains(req.Target));
                }

                //备注（模糊匹配）
                if (string.IsNullOrWhiteSpace(req.Remark) == false)
                {
                    q = q.Where(m => m.Remark.Contains(req.Remark));
                }

                #endregion

                var totalRecord = 0; //总条数

                //是否需要分页
                if (req.IsSetPage())
                {
                    totalRecord = await q.CountAsync();
                }

                //设置排序和分页
                this.SetSortPage(ref q, req);

                //查询列表
                var list = await q.ToListAsync();

                //是否需要分页
                if (req.IsSetPage() == false)
                {
                    totalRecord = list.Count;
                }

                return new PageList<BorrowRepay>(list, totalRecord);
            }
        }

        #endregion

        #region 收入统计

        /// <summary>
        /// 收入统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">截止日期</param>
        /// <returns></returns>
        public async Task<decimal> SumInComeAsync(int familyID, DateTime? startDate, DateTime? endDate)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumInComeWhere(db, familyID, startDate, endDate);

                //统计
                var total = await q.SumAsync(m => m.Amount);

                return total;
            }
        }

        /// <summary>
        /// 按类型统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<SumListInfo<int>>> SumInComeInTypeAsync(int familyID, InSumReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumInComeWhere(db, familyID, req.StartDate, req.EndDate);

                //统计
                var list = await (from m in q
                                  group m by m.InTypeID into g
                                  select new SumListInfo<int>
                                  {
                                      name = g.Key,
                                      value = g.Sum(m => m.Amount)
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 按账户统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<SumListInfo<int>>> SumInComeAmountAccountAsync(int familyID, InSumReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumInComeWhere(db, familyID, req.StartDate, req.EndDate);

                //统计
                var list = await (from m in q
                                  group m by m.AmountAccountID into g
                                  select new SumListInfo<int>
                                  {
                                      name = g.Key,
                                      value = g.Sum(m => m.Amount)
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 按月份统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<SumListInfo<string>>> SumInComeMonthAsync(int familyID, InSumReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumInComeWhere(db, familyID, req.StartDate, req.EndDate);

                //统计
                var list = await (from m in q
                                  group m by new { m.InDate.Year, m.InDate.Month } into g
                                  select new SumListInfo<string>()
                                  {
                                      name = g.First().InDate.ToString("yyyy-MM"),
                                      value = g.Sum(m => m.Amount)
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 按年度统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<SumListInfo<string>>> SumInComeYearAsync(int familyID, InSumReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumInComeWhere(db, familyID, req.StartDate, req.EndDate);

                //统计
                var list = await (from m in q
                                  group m by m.InDate.Year into g
                                  select new SumListInfo<string>()
                                  {
                                      name = CvtHelper.ToStr(g.Key),
                                      value = g.Sum(m => m.Amount)
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 拼接统计条件
        /// </summary>
        /// <param name="db">数据库上下文</param>
        /// <param name="familyID">家庭ID</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">截止日期</param>
        /// <returns></returns>
        private IQueryable<InCome> GetSumInComeWhere(IOSysContext db, int familyID, DateTime? startDate, DateTime? endDate)
        {
            //家庭ID
            var q = db.InComes.AsNoTracking().Where(m => m.FamilyID == familyID);

            //开始日期
            if (startDate.HasValue)
            {
                q = q.Where(m => m.InDate >= startDate.Value);
            }

            //截止日期
            if (endDate.HasValue)
            {
                q = q.Where(m => m.InDate <= endDate.Value);
            }

            return q;
        }

        #endregion

        #region 支出统计

        /// <summary>
        /// 支出统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">截止日期</param>
        /// <returns></returns>
        public async Task<decimal> SumOutPutAsync(int familyID, DateTime? startDate, DateTime? endDate)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumOutPutWhere(db, familyID, startDate, endDate);

                //统计
                var total = await q.SumAsync(m => m.Amount);

                return total;
            }
        }

        /// <summary>
        /// 按分类统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<SumListInfo<int>>> SumOutPutOutCategoryAsync(int familyID, OutSumReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumOutPutWhere(db, familyID, req.StartDate, req.EndDate);

                //统计
                var list = await (from op in q
                                  join otTemp in db.OutTypes on op.OutTypeID equals otTemp.ID into otJoin
                                  from ot in otJoin
                                  group op by ot.OutCategoryID into g
                                  select new SumListInfo<int>
                                  {
                                      name = g.Key,
                                      value = g.Sum(m => m.Amount)
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 按类型统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<SumListInfo<int>>> SumOutPutOutTypeAsync(int familyID, OutSumReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumOutPutWhere(db, familyID, req.StartDate, req.EndDate);

                //统计
                var list = await (from m in q
                                  group m by m.OutTypeID into g
                                  select new SumListInfo<int>
                                  {
                                      name = g.Key,
                                      value = g.Sum(m => m.Amount)
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 按账户统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<SumListInfo<int>>> SumOutPutAmountAccountAsync(int familyID, OutSumReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumOutPutWhere(db, familyID, req.StartDate, req.EndDate);

                //统计
                var list = await (from m in q
                                  group m by m.AmountAccountID into g
                                  select new SumListInfo<int>
                                  {
                                      name = g.Key,
                                      value = g.Sum(m => m.Amount)
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 按月份统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<SumListInfo<string>>> SumOutPutMonthAsync(int familyID, OutSumReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumOutPutWhere(db, familyID, req.StartDate, req.EndDate);

                //统计
                var list = await (from m in q
                                  group m by new { m.OutDate.Year, m.OutDate.Month } into g
                                  select new SumListInfo<string>()
                                  {
                                      name = g.First().OutDate.ToString("yyyy-MM"),
                                      value = g.Sum(m => m.Amount)
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 按年度统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<SumListInfo<string>>> SumOutPutYearAsync(int familyID, OutSumReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumOutPutWhere(db, familyID, req.StartDate, req.EndDate);

                //统计
                var list = await (from m in q
                                  group m by m.OutDate.Year into g
                                  select new SumListInfo<string>()
                                  {
                                      name = CvtHelper.ToStr(g.Key),
                                      value = g.Sum(m => m.Amount)
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 拼接统计条件
        /// </summary>
        /// <param name="db">数据库上下文</param>
        /// <param name="familyID">家庭ID</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">截止日期</param>
        /// <returns></returns>
        private IQueryable<OutPut> GetSumOutPutWhere(IOSysContext db, int familyID, DateTime? startDate, DateTime? endDate)
        {
            //家庭ID
            var q = db.OutPuts.AsNoTracking().Where(m => m.FamilyID == familyID);

            //开始日期
            if (startDate.HasValue)
            {
                q = q.Where(m => m.OutDate >= startDate.Value);
            }

            //截止日期
            if (endDate.HasValue)
            {
                q = q.Where(m => m.OutDate <= endDate.Value);
            }

            return q;
        }

        #endregion

        #region 借还统计

        /// <summary>
        /// 按对方统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<BorrowRepaySumListInfo<string>>> SumBorrowRepayTargetAsync(int familyID, BorrowRepaySumReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumBorrowRepayWhere(db, familyID, req.StartDate, req.EndDate, req.LstBRType);

                //统计
                var list = await (from m in q
                                  group m by new { m.Target, m.BRType } into g
                                  select new BorrowRepaySumListInfo<string>
                                  {
                                      name = g.Key.Target,
                                      BRType = g.Key.BRType,
                                      value = g.Sum(m => m.Amount)
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 按类型统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<BorrowRepaySumListInfo<int>>> SumBorrowRepayBRTypeAsync(int familyID, BorrowRepaySumReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumBorrowRepayWhere(db, familyID, req.StartDate, req.EndDate, req.LstBRType);

                //统计
                var list = await (from m in q
                                  group m by m.BRType into g
                                  select new BorrowRepaySumListInfo<int>
                                  {
                                      name = g.Key,
                                      BRType = g.Key,
                                      value = g.Sum(m => m.Amount)
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 按账户统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<BorrowRepaySumListInfo<int>>> SumBorrowRepayAmountAccountAsync(int familyID, BorrowRepaySumReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumBorrowRepayWhere(db, familyID, req.StartDate, req.EndDate, req.LstBRType);

                //统计
                var list = await (from m in q
                                  group m by new { m.AmountAccountID, m.BRType } into g
                                  select new BorrowRepaySumListInfo<int>
                                  {
                                      name = g.Key.AmountAccountID,
                                      BRType = g.Key.BRType,
                                      value = g.Sum(m => m.Amount)
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 按月份统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<BorrowRepaySumListInfo<string>>> SumBorrowRepayMonthAsync(int familyID, BorrowRepaySumReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumBorrowRepayWhere(db, familyID, req.StartDate, req.EndDate, req.LstBRType);

                //统计
                var list = await (from m in q
                                  group m by new { m.BRDate.Year, m.BRDate.Month, m.BRType } into g
                                  select new BorrowRepaySumListInfo<string>()
                                  {
                                      name = g.First().BRDate.ToString("yyyy-MM"),
                                      BRType = g.Key.BRType,
                                      value = g.Sum(m => m.Amount)
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 按年度统计
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<BorrowRepaySumListInfo<string>>> SumBorrowRepayYearAsync(int familyID, BorrowRepaySumReq req)
        {
            using (IOSysContext db = new IOSysContext())
            {
                //条件
                var q = this.GetSumBorrowRepayWhere(db, familyID, req.StartDate, req.EndDate, req.LstBRType);

                //统计
                var list = await (from m in q
                                  group m by new { m.BRDate.Year, m.BRType } into g
                                  select new BorrowRepaySumListInfo<string>()
                                  {
                                      name = CvtHelper.ToStr(g.Key.Year),
                                      BRType = g.Key.BRType,
                                      value = g.Sum(m => m.Amount)
                                  }).ToListAsync();

                return list;
            }
        }

        /// <summary>
        /// 按借还者统计借还信息
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <returns></returns>
        public async Task<List<SumListInfo<string>>> SumBorrowRepayTargetAsync(int familyID)
        {
            //收入型借还
            var lstIn = new List<int>();
            lstIn.Add((int)EnmBorrowRepayType.BorrowIn);
            lstIn.Add((int)EnmBorrowRepayType.RepayIn);

            using (IOSysContext db = new IOSysContext())
            {
                var list = await (from m in db.BorrowRepays
                                  group m by m.Target into g
                                  select new SumListInfo<string>()
                                  {
                                      name = g.Key,
                                      value = g.Sum(m => lstIn.Contains(m.BRType) ? m.Amount : -m.Amount)
                                  }).ToListAsync();
                return list;
            }
        }

        /// <summary>
        /// 拼接统计条件
        /// </summary>
        /// <param name="db">数据库上下文</param>
        /// <param name="familyID">家庭ID</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">截止日期</param>
        /// <param name="lstBRType">统计类型</param>
        /// <returns></returns>
        private IQueryable<BorrowRepay> GetSumBorrowRepayWhere(IOSysContext db, int familyID, DateTime? startDate, DateTime? endDate, List<int> lstBRType)
        {
            //家庭ID
            var q = db.BorrowRepays.AsNoTracking().Where(m => m.FamilyID == familyID);

            //开始日期
            if (startDate.HasValue)
            {
                q = q.Where(m => m.BRDate >= startDate.Value);
            }

            //截止日期
            if (endDate.HasValue)
            {
                q = q.Where(m => m.BRDate <= endDate.Value);
            }

            //借还类型
            if (lstBRType != null && lstBRType.Count > 0)
            {
                q = q.Where(m => lstBRType.Contains(m.BRType));
            }

            return q;
        }

        #endregion
    }
}
