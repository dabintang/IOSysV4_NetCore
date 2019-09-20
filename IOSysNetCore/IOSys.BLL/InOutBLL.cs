using IOSys.DAL;
using IOSys.DTO;
using IOSys.DTO.Enum;
using IOSys.DTO.InOut;
using IOSys.Helper;
using IOSys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOSys.BLL
{
    /// <summary>
    /// 收支操作 逻辑
    /// </summary>
    public class InOutBLL : BaseBLL
    {
        #region 收入

        /// <summary>
        /// 获取指定日期的收入信息
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public async Task<List<InComeListInfo>> QueryInComeAsync(DateTime date)
        {
            //查询数据库记录
            var taskModel = InOutDAL.Inst.QueryInComeAsync(this.LoginInfo.FamilyID, date);
            var taskInType = BasicDAL.Inst.QueryInTypeAsync(this.LoginInfo.FamilyID);
            var taskAmountAccount = BasicDAL.Inst.QueryAmountAccountAsync(this.LoginInfo.FamilyID);

            var lstModel = await taskModel;
            var lstInType = await taskInType;
            var lstAmountAccount = await taskAmountAccount;

            //结果
            var lstInfo = new List<InComeListInfo>();

            foreach (var model in lstModel)
            {
                var info = new InComeListInfo();
                lstInfo.Add(info);

                //收入类型
                var inType = lstInType.Find(m => m.ID == model.InTypeID);
                //账户信息
                var amountAccount = lstAmountAccount.Find(m => m.ID == model.AmountAccountID);

                info.ID = model.ID;
                info.InTypeName = inType != null ? inType.Name : string.Empty;
                info.AmountAccountName = amountAccount != null ? amountAccount.Name : string.Empty;
                info.Amount = model.Amount;
            }

            return lstInfo;
        }

        /// <summary>
        /// 获取收入信息
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<InComeInfo> GetInComeAsync(int id)
        {
            //获取数据库信息
            var model = await InOutDAL.Inst.GetInComeAsync(this.LoginInfo.FamilyID, id);
            if (model == null)
            {
                return null;
            }

            //结果
            var info = new InComeInfo();
            info.ID = model.ID;
            info.InDate = model.InDate;
            info.InTypeID = model.InTypeID;
            info.AmountAccountID = model.AmountAccountID;
            info.Amount = model.Amount;
            info.Remark = model.Remark;

            return info;
        }

        /// <summary>
        /// 保存收入信息
        /// </summary>
        /// <param name="info">收入信息</param>
        /// <returns>主键ID</returns>
        public async Task<ResultInfo<int>> SaveInComeAsync(InComeInfo info)
        {
            //判断收入类型是否存在
            var modelInType = await BasicDAL.Inst.GetInTypeAsync(this.LoginInfo.FamilyID, info.InTypeID);
            if (modelInType == null)
            {
                return new ResultInfo<int>(false, this.Res.Bas.InTypeUnexist, -1);
            }

            //判断账户是否存在
            var modelAmountAccount = await BasicDAL.Inst.GetAmountAccountAsync(this.LoginInfo.FamilyID, info.AmountAccountID);
            if (modelAmountAccount == null)
            {
                return new ResultInfo<int>(false, this.Res.Bas.AmountAccountUnexist, -1);
            }

            //转成model
            var model = new InCome();
            model.ID = info.ID;
            model.FamilyID = this.LoginInfo.FamilyID;
            model.InDate = info.InDate;
            model.InTypeID = info.InTypeID;
            model.AmountAccountID = info.AmountAccountID;
            model.Amount = info.Amount;
            model.Remark = info.Remark;

            //设置创建者/更新者字段值
            this.SetCreateUpdateFields(model);

            //保存到数据库
            var ret = await CacheLock.DoByLockFamilyAsync(this.LoginInfo.FamilyID, () => InOutDAL.Inst.SaveInComeAsync(model));
            if (!ret.IsDone)
            {
                return new ResultInfo<int>(false, Res.Gen.WaiteTimeOut, model.ID);
            }

            return new ResultInfo<int>(true, Res.Gen.OK, model.ID);
        }

        /// <summary>
        /// 删除收入信息
        /// </summary>
        /// <param name="id">主键ID</param>
        public async Task<ResultInfo<bool>> DeleteInComeAsync(int id)
        {
            //从数据库删除
            var ret = await CacheLock.DoByLockFamilyAsync(this.LoginInfo.FamilyID, () => InOutDAL.Inst.DeleteInComeAsync(this.LoginInfo.FamilyID, id));
            if (!ret.IsDone)
            {
                return new ResultInfo<bool>(false, Res.Gen.WaiteTimeOut, false);
            }

            if (ret.Result)
            {
                return new ResultInfo<bool>(true, Res.Gen.OK, true);
            }
            else
            {
                return new ResultInfo<bool>(false, this.Res.Gen.NoRight, false);
            }
        }

        #endregion

        #region 支出

        /// <summary>
        /// 获取指定日期的支出信息
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public async Task<List<OutPutListInfo>> QueryOutPutAsync(DateTime date)
        {
            //查询数据库记录
            var taskModel = InOutDAL.Inst.QueryOutPutAsync(this.LoginInfo.FamilyID, date);
            var taskOutCategory = BasicDAL.Inst.QueryOutCategoryAsync(this.LoginInfo.FamilyID);
            var taskOutType = BasicDAL.Inst.QueryOutTypeAsync(this.LoginInfo.FamilyID);
            var taskAmountAccount = BasicDAL.Inst.QueryAmountAccountAsync(this.LoginInfo.FamilyID);

            var lstModel = await taskModel;
            var lstOutCategory = await taskOutCategory;
            var lstOutType = await taskOutType;
            var lstAmountAccount = await taskAmountAccount;

            //结果
            var lstInfo = new List<OutPutListInfo>();

            foreach (var model in lstModel)
            {
                var info = new OutPutListInfo();
                lstInfo.Add(info);

                //支出类型
                var outType = lstOutType.Find(m => m.ID == model.OutTypeID);
                //支出分类
                OutCategory outCategory = outType != null ? lstOutCategory.Find(m => m.ID == outType.OutCategoryID) : null;
                //账户信息
                var amountAccount = lstAmountAccount.Find(m => m.ID == model.AmountAccountID);

                info.ID = model.ID;
                info.OutCategoryName = outCategory != null ? outCategory.Name : string.Empty;
                info.OutTypeName = outType != null ? outType.Name : string.Empty;
                info.AmountAccountName = amountAccount != null ? amountAccount.Name : string.Empty;
                info.Amount = model.Amount;
            }

            return lstInfo;
        }

        /// <summary>
        /// 获取支出信息
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<OutPutInfo> GetOutPutAsync(int id)
        {
            //获取数据库信息
            var model = await InOutDAL.Inst.GetOutPutAsync(this.LoginInfo.FamilyID, id);
            if (model == null)
            {
                return null;
            }

            //结果
            var info = new OutPutInfo();
            info.ID = model.ID;
            info.OutDate = model.OutDate;
            info.OutTypeID = model.OutTypeID;
            info.AmountAccountID = model.AmountAccountID;
            info.Amount = model.Amount;
            info.Remark = model.Remark;

            return info;
        }

        /// <summary>
        /// 保存支出信息
        /// </summary>
        /// <param name="info">支出信息</param>
        /// <returns>主键ID</returns>
        public async Task<ResultInfo<int>> SaveOutPutAsync(OutPutInfo info)
        {
            //判断支出类型是否存在
            var modelOutType = await BasicDAL.Inst.GetOutTypeAsync(this.LoginInfo.FamilyID, info.OutTypeID);
            if (modelOutType == null)
            {
                return new ResultInfo<int>(false, this.Res.Bas.OutTypeUnexist, -1);
            }

            //判断账户是否存在
            var modelAmountAccount = await BasicDAL.Inst.GetAmountAccountAsync(this.LoginInfo.FamilyID, info.AmountAccountID);
            if (modelAmountAccount == null)
            {
                return new ResultInfo<int>(false, this.Res.Bas.AmountAccountUnexist, -1);
            }

            //转成model
            var model = new OutPut();
            model.ID = info.ID;
            model.FamilyID = this.LoginInfo.FamilyID;
            model.OutDate = info.OutDate;
            model.OutTypeID = info.OutTypeID;
            model.AmountAccountID = info.AmountAccountID;
            model.Amount = info.Amount;
            model.Remark = info.Remark;

            //设置创建者/更新者字段值
            this.SetCreateUpdateFields(model);

            //保存到数据库
            var ret = await CacheLock.DoByLockFamilyAsync(this.LoginInfo.FamilyID, () => InOutDAL.Inst.SaveOutPutAsync(model));
            if (!ret.IsDone)
            {
                return new ResultInfo<int>(false, Res.Gen.WaiteTimeOut, model.ID);
            }

            return new ResultInfo<int>(true, Res.Gen.OK, model.ID);
        }

        /// <summary>
        /// 删除支出信息
        /// </summary>
        /// <param name="id">主键ID</param>
        public async Task<ResultInfo<bool>> DeleteOutPutAsync(int id)
        {
            //从数据库删除
            var ret = await CacheLock.DoByLockFamilyAsync(this.LoginInfo.FamilyID, () => InOutDAL.Inst.DeleteOutPutAsync(this.LoginInfo.FamilyID, id));
            if (!ret.IsDone)
            {
                return new ResultInfo<bool>(false, Res.Gen.WaiteTimeOut, false);
            }

            if (ret.Result)
            {
                return new ResultInfo<bool>(true, Res.Gen.OK, true);
            }
            else
            {
                return new ResultInfo<bool>(false, this.Res.Gen.NoRight, false);
            }
        }

        #endregion

        #region 转账

        /// <summary>
        /// 获取指定日期的转账信息
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public async Task<List<TransferListInfo>> QueryTransferAsync(DateTime date)
        {
            //查询数据库记录
            var taskModel = InOutDAL.Inst.QueryTransferAsync(this.LoginInfo.FamilyID, date);
            var taskAmountAccount = BasicDAL.Inst.QueryAmountAccountAsync(this.LoginInfo.FamilyID);

            var lstModel = await taskModel;
            var lstAmountAccount = await taskAmountAccount;

            //结果
            var lstInfo = new List<TransferListInfo>();

            foreach (var model in lstModel)
            {
                var info = new TransferListInfo();
                lstInfo.Add(info);

                //账户信息
                var fromAmountAccount = lstAmountAccount.Find(m => m.ID == model.FromAmountAccountID);
                var toAmountAccount = lstAmountAccount.Find(m => m.ID == model.ToAmountAccountID);

                info.ID = model.ID;
                info.FromAmountAccountName = fromAmountAccount != null ? fromAmountAccount.Name : string.Empty;
                info.ToAmountAccountName = toAmountAccount != null ? toAmountAccount.Name : string.Empty;
                info.Amount = model.Amount;
            }

            return lstInfo;
        }

        /// <summary>
        /// 获取转账信息
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<TransferInfo> GetTransferAsync(int id)
        {
            //获取数据库信息
            var model = await InOutDAL.Inst.GetTransferAsync(this.LoginInfo.FamilyID, id);
            if (model == null)
            {
                return null;
            }

            //结果
            var info = new TransferInfo();
            info.ID = model.ID;
            info.TransferDate = model.TransferDate;
            info.FromAmountAccountID = model.FromAmountAccountID;
            info.ToAmountAccountID = model.ToAmountAccountID;
            info.Amount = model.Amount;
            info.Remark = model.Remark;

            return info;
        }

        /// <summary>
        /// 保存转账信息
        /// </summary>
        /// <param name="info">转账信息</param>
        /// <returns>主键ID</returns>
        public async Task<ResultInfo<int>> SaveTransferAsync(TransferInfo info)
        {
            //判断转出账户是否存在
            var modelFromAmountAccount = await BasicDAL.Inst.GetAmountAccountAsync(this.LoginInfo.FamilyID, info.FromAmountAccountID);
            if (modelFromAmountAccount == null)
            {
                return new ResultInfo<int>(false, this.Res.Bas.AmountAccountUnexist, -1);
            }

            //判断转入账户是否存在
            var modelToAmountAccount = await BasicDAL.Inst.GetAmountAccountAsync(this.LoginInfo.FamilyID, info.ToAmountAccountID);
            if (modelToAmountAccount == null)
            {
                return new ResultInfo<int>(false, this.Res.Bas.AmountAccountUnexist, -1);
            }

            //转成model
            var model = new AmountAccountTransfer();
            model.ID = info.ID;
            model.FamilyID = this.LoginInfo.FamilyID;
            model.TransferDate = info.TransferDate;
            model.FromAmountAccountID = info.FromAmountAccountID;
            model.ToAmountAccountID = info.ToAmountAccountID;
            model.Amount = info.Amount;
            model.Remark = info.Remark;

            //设置创建者/更新者字段值
            this.SetCreateUpdateFields(model);

            //保存到数据库
            var ret = await CacheLock.DoByLockFamilyAsync(this.LoginInfo.FamilyID, () => InOutDAL.Inst.SaveTransferAsync(model));
            if (!ret.IsDone)
            {
                return new ResultInfo<int>(false, Res.Gen.WaiteTimeOut, model.ID);
            }

            return new ResultInfo<int>(true, Res.Gen.OK, model.ID);
        }

        /// <summary>
        /// 删除账户信息
        /// </summary>
        /// <param name="id">主键ID</param>
        public async Task<ResultInfo<bool>> DeleteTransferAsync(int id)
        {
            //从数据库删除
            var ret = await CacheLock.DoByLockFamilyAsync(this.LoginInfo.FamilyID, () => InOutDAL.Inst.DeleteTransferAsync(this.LoginInfo.FamilyID, id));
            if (!ret.IsDone)
            {
                return new ResultInfo<bool>(false, Res.Gen.WaiteTimeOut, false);
            }

            if (ret.Result)
            {
                return new ResultInfo<bool>(true, Res.Gen.OK, true);
            }
            else
            {
                return new ResultInfo<bool>(false, this.Res.Gen.NoRight, false);
            }
        }

        #endregion

        #region 借还

        /// <summary>
        /// 获取指定日期的借还信息
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public async Task<List<BorrowRepayListInfo>> QueryBorrowRepayAsync(DateTime date)
        {
            //查询数据库记录
            var taskModel = InOutDAL.Inst.QueryBorrowRepayAsync(this.LoginInfo.FamilyID, date);
            var taskAmountAccount = BasicDAL.Inst.QueryAmountAccountAsync(this.LoginInfo.FamilyID);

            var lstModel = await taskModel;
            var lstAmountAccount = await taskAmountAccount;

            //结果
            var lstInfo = new List<BorrowRepayListInfo>();

            foreach (var model in lstModel)
            {
                var info = new BorrowRepayListInfo();
                lstInfo.Add(info);

                //账户信息
                var amountAccount = lstAmountAccount.Find(m => m.ID == model.AmountAccountID);

                info.ID = model.ID;
                info.Target = model.Target;
                info.BRTypeName = this.GetBorrowRepayTypeName(model.BRType);
                info.AmountAccountName = amountAccount != null ? amountAccount.Name : string.Empty;
                info.Amount = model.Amount;
            }

            return lstInfo;
        }

        /// <summary>
        /// 获取借还信息
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<BorrowRepayInfo> GetBorrowRepayAsync(int id)
        {
            //获取数据库信息
            var model = await InOutDAL.Inst.GetBorrowRepayAsync(this.LoginInfo.FamilyID, id);
            if (model == null)
            {
                return null;
            }

            //结果
            var info = new BorrowRepayInfo();
            info.ID = model.ID;
            info.BRDate = model.BRDate;
            info.Target = model.Target;
            info.BRType = model.BRType;
            info.AmountAccountID = model.AmountAccountID;
            info.Amount = model.Amount;
            info.Remark = model.Remark;

            return info;
        }

        /// <summary>
        /// 保存借还信息
        /// </summary>
        /// <param name="info">借还信息</param>
        /// <returns>主键ID</returns>
        public async Task<ResultInfo<int>> SaveBorrowRepayAsync(BorrowRepayInfo info)
        {
            //对方名称不能为空
            if (string.IsNullOrWhiteSpace(info.Target))
            {
                return new ResultInfo<int>(false, this.Res.IO.BRTargetEmpty, -1);
            }

            //判断借还类型是否存在
            if (Enum.IsDefined(typeof(EnmBorrowRepayType), info.BRType) == false)
            {
                return new ResultInfo<int>(false, this.Res.Bas.BorrowRepayTypeUnexist, -1);
            }

            //判断账户是否存在
            var modelAmountAccount = await BasicDAL.Inst.GetAmountAccountAsync(this.LoginInfo.FamilyID, info.AmountAccountID);
            if (modelAmountAccount == null)
            {
                return new ResultInfo<int>(false, this.Res.Bas.AmountAccountUnexist, -1);
            }

            //转成model
            var model = new BorrowRepay();
            model.ID = info.ID;
            model.FamilyID = this.LoginInfo.FamilyID;
            model.BRDate = info.BRDate;
            model.Target = info.Target;
            model.BRType = info.BRType;
            model.AmountAccountID = info.AmountAccountID;
            model.Amount = info.Amount;
            model.Remark = info.Remark;

            //设置创建者/更新者字段值
            this.SetCreateUpdateFields(model);

            //保存到数据库
            var ret = await CacheLock.DoByLockFamilyAsync(this.LoginInfo.FamilyID, () => InOutDAL.Inst.SaveBorrowRepayAsync(model));
            if (!ret.IsDone)
            {
                return new ResultInfo<int>(false, Res.Gen.WaiteTimeOut, model.ID);
            }

            return new ResultInfo<int>(true, Res.Gen.OK, model.ID);
        }

        /// <summary>
        /// 删除借还信息
        /// </summary>
        /// <param name="id">主键ID</param>
        public async Task<ResultInfo<bool>> DeleteBorrowRepayAsync(int id)
        {
            //从数据库删除
            var ret = await CacheLock.DoByLockFamilyAsync(this.LoginInfo.FamilyID, () => InOutDAL.Inst.DeleteBorrowRepayAsync(this.LoginInfo.FamilyID, id));
            if (!ret.IsDone)
            {
                return new ResultInfo<bool>(false, Res.Gen.WaiteTimeOut, false);
            }

            if (ret.Result)
            {
                return new ResultInfo<bool>(true, Res.Gen.OK, true);
            }
            else
            {
                return new ResultInfo<bool>(false, this.Res.Gen.NoRight, false);
            }
        }

        #endregion

        #region 其他

        /// <summary>
        /// 更新账户的排序权重字段
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateAmountAccountSortWeightAsync()
        {
            //30天内
            var cur30 = DateTime.Today.AddDays(-30);
            //30天前
            var pre30 = cur30.AddMilliseconds(-1);

            //统计30天前账户在收入表中的使用次数
            var taskPreInCome = InOutDAL.Inst.SumAmountAccountTimesInInComeAsync(null, pre30);
            //统计30天内账户在收入表中的使用次数
            var taskCurInCome = InOutDAL.Inst.SumAmountAccountTimesInInComeAsync(cur30, null);

            //统计30天前账户在支出表中的使用次数
            var taskPreOutPut = InOutDAL.Inst.SumAmountAccountTimesInOutPutAsync(null, pre30);
            //统计30天内账户在支出表中的使用次数
            var taskCurOutPut = InOutDAL.Inst.SumAmountAccountTimesInOutPutAsync(cur30, null);

            //统计30天前账户在转账表中的使用次数
            var taskPreTransfer = InOutDAL.Inst.SumAmountAccountTimesInTransferAsync(null, pre30);
            //统计30天内账户在转账表中的使用次数
            var taskCurTransfer = InOutDAL.Inst.SumAmountAccountTimesInTransferAsync(cur30, null);

            //统计30天前账户在借还表中的使用次数
            var taskPreBorrowRepay = InOutDAL.Inst.SumAmountAccountTimesInBorrowRepayAsync(null, pre30);
            //统计30天内账户在借还表中的使用次数
            var taskCurBorrowRepay = InOutDAL.Inst.SumAmountAccountTimesInBorrowRepayAsync(cur30, null);

            //等待统计完成
            //统计30天前账户在收入表中的使用次数
            var lstPreInCome = await taskPreInCome;
            //统计30天内账户在收入表中的使用次数
            var lstCurInCome = await taskCurInCome;

            //统计30天前账户在支出表中的使用次数
            var lstPreOutPut = await taskPreOutPut;
            //统计30天内账户在支出表中的使用次数
            var lstCurOutPut = await taskCurOutPut;

            //统计30天前账户在转账表中的使用次数
            var lstPreTransfer = await taskPreTransfer;
            //统计30天内账户在转账表中的使用次数
            var lstCurTransfer = await taskCurTransfer;

            //统计30天前账户在借还表中的使用次数
            var lstPreBorrowRepay = await taskPreBorrowRepay;
            //统计30天内账户在借还表中的使用次数
            var lstCurBorrowRepay = await taskCurBorrowRepay;

            //30天前使用次数合计
            var lstPre = (from m in lstPreInCome.Union(lstPreOutPut).Union(lstPreTransfer).Union(lstPreBorrowRepay)
                          group m by m.ID into g
                          select new UseTimesTemp()
                          {
                              ID = g.Key,
                              Times = g.Sum(m => m.Times)
                          }).ToList();

            //30天内使用次数合计
            var lstCur = (from m in lstCurInCome.Union(lstCurOutPut).Union(lstCurTransfer).Union(lstCurBorrowRepay)
                          group m by m.ID into g
                          select new UseTimesTemp()
                          {
                              ID = g.Key,
                              Times = g.Sum(m => m.Times)
                          }).ToList();

            //账户信息
            var list = await BasicDAL.Inst.QueryAllAmountAccountAsync();
            foreach (var item in list)
            {
                var preItem = lstPre.Find(m => m.ID == item.ID);
                var curItem = lstCur.Find(m => m.ID == item.ID);

                item.SortWeight = this.SumSortWeight(preItem, curItem);
            }

            //保存
            await BasicDAL.Inst.UpdateAmountAccountSortWeightAsync(list);

            return true;
        }

        /// <summary>
        /// 更新收入类型的排序权重字段
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateInTypeSortWeightAsync()
        {
            //365天内
            var cur365 = DateTime.Today.AddDays(-365);
            //365天前
            var pre365 = cur365.AddMilliseconds(-1);

            //统计365天前收入类型的使用次数
            var taskPre = InOutDAL.Inst.SumInTypeTimesAsync(null, pre365);
            //统计365天内收入类型的使用次数
            var taskCur = InOutDAL.Inst.SumInTypeTimesAsync(cur365, null);

            //等待统计完成
            //统计365天前收入类型的使用次数
            var lstPre = await taskPre;
            //统计365天内收入类型的使用次数
            var lstCur = await taskCur;

            //收入类型信息
            var list = await BasicDAL.Inst.QueryAllInTypeAsync();
            foreach (var item in list)
            {
                var preItem = lstPre.Find(m => m.ID == item.ID);
                var curItem = lstCur.Find(m => m.ID == item.ID);

                item.SortWeight = this.SumSortWeight(preItem, curItem);
            }

            //保存
            await BasicDAL.Inst.SaveInTypesAsync(list);

            return true;
        }

        /// <summary>
        /// 更新支出类型的排序权重字段
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateOutTypeSortWeightAsync()
        {
            //30天内
            var cur30 = DateTime.Today.AddDays(-30);
            //30天前
            var pre30 = cur30.AddMilliseconds(-1);

            //统计30天前支出类型的使用次数
            var taskPre = InOutDAL.Inst.SumOutTypeTimesAsync(null, pre30);
            //统计30天内支出类型的使用次数
            var taskCur = InOutDAL.Inst.SumOutTypeTimesAsync(cur30, null);

            //等待统计完成
            //统计30天前支出类型的使用次数
            var lstPre = await taskPre;
            //统计30天内支出类型的使用次数
            var lstCur = await taskCur;

            //收入类型信息
            var lstOutType = await BasicDAL.Inst.QueryAllOutTypeAsync();
            foreach (var outType in lstOutType)
            {
                var preItem = lstPre.Find(m => m.ID == outType.ID);
                var curItem = lstCur.Find(m => m.ID == outType.ID);

                outType.SortWeight = this.SumSortWeight(preItem, curItem);
            }

            //收入分类
            var lstOutCategory = await BasicDAL.Inst.QueryAllOutCategoryAsync();
            foreach (var outCategory in lstOutCategory)
            {
                outCategory.SortWeight = lstOutType.Where(m => m.OutCategoryID == outCategory.ID).Sum(m => m.SortWeight);
            }

            //保存
            await BasicDAL.Inst.SaveOutTypesAsync(lstOutType);
            await BasicDAL.Inst.SaveOutCategorysAsync(lstOutCategory);

            return true;
        }

        /// <summary>
        /// 统计排序权重值
        /// </summary>
        /// <param name="preTimes">之前的使用次数</param>
        /// <param name="curTimes">当前的使用次数</param>
        /// <returns></returns>
        private int SumSortWeight(UseTimesTemp preTimes, UseTimesTemp curTimes)
        {
            int sortWeight = 0;

            if (preTimes != null)
            {
                //之前的使用次数只要百分之一，但最多只要10
                sortWeight += Math.Min(preTimes.Times / 100, 10);
            }

            if (curTimes != null)
            {
                //当前的使用次数
                sortWeight += curTimes.Times;
            }

            return sortWeight;
        }

        #endregion
    }
}
