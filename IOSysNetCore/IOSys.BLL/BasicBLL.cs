using IOSys.DAL;
using IOSys.DTO;
using IOSys.DTO.Basic;
using IOSys.DTO.Enum;
using IOSys.Helper;
using IOSys.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IOSys.BLL
{
    /// <summary>
    /// 基础数据
    /// </summary>
    public class BasicBLL : BaseBLL
    {
        #region 账户

        /// <summary>
        /// 查询账户
        /// </summary>
        /// <returns></returns>
        public async Task<List<AmountAccountListInfo>> QueryAmountAccountAsync()
        {
            //查询数据库记录
            var lstModel = await BasicDAL.Inst.QueryAmountAccountAsync(this.LoginInfo.FamilyID);

            //结果
            var lstInfo = new List<AmountAccountListInfo>();

            foreach (var model in lstModel)
            {
                var info = new AmountAccountListInfo();
                lstInfo.Add(info);

                info.ID = model.ID;
                info.Name = model.Name;
                info.Amount = model.Amount;
                info.IsActive = model.IsActive;
            }

            return lstInfo;
        }

        /// <summary>
        /// 获取账户
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<AmountAccountInfo> GetAmountAccountAsync(int id)
        {
            //获取数据库信息
            var model = await BasicDAL.Inst.GetAmountAccountAsync(this.LoginInfo.FamilyID, id);
            if (model == null)
            {
                return null;
            }

            //结果
            var info = new AmountAccountInfo();
            info.ID = model.ID;
            info.Name = model.Name;
            info.Amount = model.Amount;
            info.IsActive = model.IsActive;
            info.Remark = model.Remark;

            return info;
        }

        /// <summary>
        /// 保存账户
        /// </summary>
        /// <param name="info">账户</param>
        /// <returns>主键ID</returns>
        public async Task<ResultInfo<int>> SaveAmountAccountAsync(AmountAccountInfo info)
        {
            //获取原有列表
            var list = await BasicDAL.Inst.QueryAmountAccountAsync(this.LoginInfo.FamilyID);

            //判断名称是否已被使用
            if (list.Exists(m => m.ID != info.ID && m.Name == info.Name))
            {
                return new ResultInfo<int>(false, this.Res.Bas.NameExisted, -1);
            }

            //尝试查找原数据
            var model = list.Find(m => m.ID == info.ID);

            //新建
            if (model == null)
            {
                model = new AmountAccount();
                model.FamilyID = this.LoginInfo.FamilyID;
                model.InitAmount = info.Amount;
                model.Amount = info.Amount;
            }

            model.Name = info.Name;
            model.IsActive = info.IsActive;
            model.Remark = info.Remark;

            //设置创建者/更新者字段值
            this.SetCreateUpdateFields(model);

            //保存到数据库
            await BasicDAL.Inst.SaveAmountAccountAsync(model);

            return new ResultInfo<int>(true, Res.Gen.OK, model.ID);
        }

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="id">主键ID</param>
        public async Task<ResultInfo<bool>> DeleteAmountAccountAsync(int id)
        {
            //判断是否已被使用
            if (await BasicDAL.Inst.IsAmountAccountUsedAsync(id))
            {
                return new ResultInfo<bool>(false, this.Res.Bas.BeenUsed, false);
            }

            //获取原数据
            var model = await BasicDAL.Inst.GetAmountAccountAsync(this.LoginInfo.FamilyID, id);
            if (model == null)
            {
                return new ResultInfo<bool>(false, this.Res.Gen.NoRight, false);
            }

            //从数据库删除
            await BasicDAL.Inst.DeleteAmountAccountAsync(model);

            return new ResultInfo<bool>(true, Res.Gen.OK, true);
        }

        #endregion

        #region 收入类型

        /// <summary>
        /// 查询收入类型列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<InTypeListInfo>> QueryInTypeAsync()
        {
            //查询数据库记录
            var lstModel = await BasicDAL.Inst.QueryInTypeAsync(this.LoginInfo.FamilyID);

            //结果
            var lstInfo = new List<InTypeListInfo>();

            foreach (var model in lstModel)
            {
                var info = new InTypeListInfo();
                lstInfo.Add(info);

                info.ID = model.ID;
                info.Name = model.Name;
                info.AmountAccountID = model.AmountAccountID;
                info.IsActive = model.IsActive;
            }

            return lstInfo;
        }

        /// <summary>
        /// 获取收入类型
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<InTypeInfo> GetInTypeAsync(int id)
        {
            //获取数据库信息
            var model = await BasicDAL.Inst.GetInTypeAsync(this.LoginInfo.FamilyID, id);
            if (model == null)
            {
                return null;
            }

            //结果
            var info = new InTypeInfo();
            info.ID = model.ID;
            info.Name = model.Name;
            info.AmountAccountID = model.AmountAccountID;
            info.IsActive = model.IsActive;
            info.Remark = model.Remark;

            return info;
        }

        /// <summary>
        /// 保存收入类型
        /// </summary>
        /// <param name="info">收入类型</param>
        /// <returns>主键ID</returns>
        public async Task<ResultInfo<int>> SaveInTypeAsync(InTypeInfo info)
        {
            //获取原有列表
            var list = await BasicDAL.Inst.QueryInTypeAsync(this.LoginInfo.FamilyID);

            //判断名称是否已被使用
            if (list.Exists(m => m.ID != info.ID && m.Name == info.Name))
            {
                return new ResultInfo<int>(false, this.Res.Bas.NameExisted, -1);
            }

            //判断默认账户是否存在
            var modelAmountAccount = await BasicDAL.Inst.GetAmountAccountAsync(this.LoginInfo.FamilyID, info.AmountAccountID);
            if (modelAmountAccount == null)
            {
                return new ResultInfo<int>(false, this.Res.Bas.AmountAccountUnexist, -1);
            }

            //尝试查找原数据
            var model = list.Find(m => m.ID == info.ID);

            //新建
            if (model == null)
            {
                model = new InType();
                model.FamilyID = this.LoginInfo.FamilyID;
            }

            model.Name = info.Name;
            model.AmountAccountID = info.AmountAccountID;
            model.IsActive = info.IsActive;
            model.Remark = info.Remark;

            //设置创建者/更新者字段值
            this.SetCreateUpdateFields(model);

            //保存到数据库
            await BasicDAL.Inst.SaveInTypeAsync(model);

            return new ResultInfo<int>(true, Res.Gen.OK, model.ID);
        }

        /// <summary>
        /// 删除收入类型
        /// </summary>
        /// <param name="id">主键ID</param>
        public async Task<ResultInfo<bool>> DeleteInTypeAsync(int id)
        {
            //判断是否已被使用
            if (await BasicDAL.Inst.IsInTypeUsedAsync(id))
            {
                return new ResultInfo<bool>(false, this.Res.Bas.BeenUsed, false);
            }

            //获取原数据
            var model = await BasicDAL.Inst.GetInTypeAsync(this.LoginInfo.FamilyID, id);
            if (model == null)
            {
                return new ResultInfo<bool>(false, this.Res.Gen.NoRight, false);
            }

            //从数据库删除
            await BasicDAL.Inst.DeleteInTypeAsync(model);

            return new ResultInfo<bool>(true, Res.Gen.OK, true);
        }

        #endregion

        #region 支出分类

        /// <summary>
        /// 查询支出分类列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<OutCategoryListInfo>> QueryOutCategoryAsync()
        {
            //查询数据库记录
            var lstModel = await BasicDAL.Inst.QueryOutCategoryAsync(this.LoginInfo.FamilyID);

            //结果
            var lstInfo = new List<OutCategoryListInfo>();

            foreach (var model in lstModel)
            {
                var info = new OutCategoryListInfo();
                lstInfo.Add(info);

                info.ID = model.ID;
                info.Name = model.Name;
                info.IsActive = model.IsActive;
            }

            return lstInfo;
        }

        /// <summary>
        /// 获取支出分类
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<OutCategoryInfo> GetOutCategoryAsync(int id)
        {
            //获取数据库信息
            var model = await BasicDAL.Inst.GetOutCategoryAsync(this.LoginInfo.FamilyID, id);
            if (model == null)
            {
                return null;
            }

            //结果
            var info = new OutCategoryInfo();
            info.ID = model.ID;
            info.Name = model.Name;
            info.IsActive = model.IsActive;
            info.Remark = model.Remark;

            return info;
        }

        /// <summary>
        /// 保存支出分类
        /// </summary>
        /// <param name="info">支出分类</param>
        /// <returns>主键ID</returns>
        public async Task<ResultInfo<int>> SaveOutCategoryAsync(OutCategoryInfo info)
        {
            //获取原有列表
            var list = await BasicDAL.Inst.QueryOutCategoryAsync(this.LoginInfo.FamilyID);

            //判断名称是否已被使用
            if (list.Exists(m => m.ID != info.ID && m.Name == info.Name))
            {
                return new ResultInfo<int>(false, this.Res.Bas.NameExisted, -1);
            }

            //尝试查找原数据
            var model = list.Find(m => m.ID == info.ID);

            //新建
            if (model == null)
            {
                model = new OutCategory();
                model.FamilyID = this.LoginInfo.FamilyID;
            }

            model.Name = info.Name;
            model.IsActive = info.IsActive;
            model.Remark = info.Remark;

            //设置创建者/更新者字段值
            this.SetCreateUpdateFields(model);

            //保存到数据库
            await BasicDAL.Inst.SaveOutCategoryAsync(model);

            return new ResultInfo<int>(true, Res.Gen.OK, model.ID);
        }

        /// <summary>
        /// 删除支出分类
        /// </summary>
        /// <param name="id">主键ID</param>
        public async Task<ResultInfo<bool>> DeleteOutCategoryAsync(int id)
        {
            //判断是否已被使用
            if (await BasicDAL.Inst.IsOutCategoryUsedAsync(id))
            {
                return new ResultInfo<bool>(false, this.Res.Bas.BeenUsed, false);
            }

            //获取原数据
            var model = await BasicDAL.Inst.GetOutCategoryAsync(this.LoginInfo.FamilyID, id);
            if (model == null)
            {
                return new ResultInfo<bool>(false, this.Res.Gen.NoRight, false);
            }

            //从数据库删除
            await BasicDAL.Inst.DeleteOutCategoryAsync(model);

            return new ResultInfo<bool>(true, Res.Gen.OK, true);
        }

        #endregion

        #region 支出类型

        /// <summary>
        /// 查询支出分类-类型列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<OutCategoryTypeListInfo>> QueryOutCategoryTypeAsync()
        {
            //查询数据库记录
            var taskOutCategory = BasicDAL.Inst.QueryOutCategoryAsync(this.LoginInfo.FamilyID);
            var taskOutType = BasicDAL.Inst.QueryOutTypeAsync(this.LoginInfo.FamilyID);

            List<OutCategory> lstModelOutCategory = await taskOutCategory;
            List<OutType> lstModelOutType = await taskOutType;

            //结果
            var lstInfo = new List<OutCategoryTypeListInfo>();

            foreach (var modelOutCategory in lstModelOutCategory)
            {
                var info = new OutCategoryTypeListInfo();
                lstInfo.Add(info);

                info.ID = modelOutCategory.ID;
                info.Name = modelOutCategory.Name;
                info.IsActive = modelOutCategory.IsActive;

                var lstSubModelOutType = lstModelOutType.FindAll(m => m.OutCategoryID == modelOutCategory.ID);
                foreach( var modelOutType in lstSubModelOutType)
                {
                    var infoOutType = new OutTypeListInfo();
                    info.LstOutType.Add(infoOutType);

                    infoOutType.ID = modelOutType.ID;
                    infoOutType.Name = modelOutType.Name;
                    infoOutType.AmountAccountID = modelOutType.AmountAccountID;
                    infoOutType.IsActive = modelOutType.IsActive;
                }
            }

            return lstInfo;
        }

        /// <summary>
        /// 获取支出类型
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<OutTypeInfo> GetOutTypeAsync(int id)
        {
            //获取数据库记录
            var model = await BasicDAL.Inst.GetOutTypeAsync(this.LoginInfo.FamilyID, id);
            if (model == null)
            {
                return null;
            }

            //结果
            var info = new OutTypeInfo();
            info.ID = model.ID;
            info.OutCategoryID = model.OutCategoryID;
            info.Name = model.Name;
            info.AmountAccountID = model.AmountAccountID;
            info.IsActive = model.IsActive;
            info.Remark = model.Remark;

            return info;
        }

        /// <summary>
        /// 保存支出类型
        /// </summary>
        /// <param name="info">支出类型</param>
        /// <returns>主键ID</returns>
        public async Task<ResultInfo<int>> SaveOutTypeAsync(OutTypeInfo info)
        {
            //获取原有列表
            var list = await BasicDAL.Inst.QueryOutTypeAsync(this.LoginInfo.FamilyID);

            //判断名称是否已被使用
            if (list.Exists(m => m.ID != info.ID && m.Name == info.Name))
            {
                return new ResultInfo<int>(false, this.Res.Bas.NameExisted, -1);
            }

            //判断默认账户是否存在
            var modelAmountAccount = await BasicDAL.Inst.GetAmountAccountAsync(this.LoginInfo.FamilyID, info.AmountAccountID);
            if (modelAmountAccount == null)
            {
                return new ResultInfo<int>(false, this.Res.Bas.AmountAccountUnexist, -1);
            }

            //尝试查找原数据
            var model = list.Find(m => m.ID == info.ID);

            //新建
            if (model == null)
            {
                model = new OutType();
                model.FamilyID = this.LoginInfo.FamilyID;
            }

            model.OutCategoryID = info.OutCategoryID;
            model.Name = info.Name;
            model.AmountAccountID = info.AmountAccountID;
            model.IsActive = info.IsActive;
            model.Remark = info.Remark;

            //设置创建者/更新者字段值
            this.SetCreateUpdateFields(model);

            //保存到数据库
            await BasicDAL.Inst.SaveOutTypeAsync(model);

            return new ResultInfo<int>(true, Res.Gen.OK, model.ID);
        }

        /// <summary>
        /// 删除支出类型
        /// </summary>
        /// <param name="id">主键ID</param>
        public async Task<ResultInfo<bool>> DeleteOutTypeAsync(int id)
        {
            //判断是否已被使用
            if (await BasicDAL.Inst.IsOutTypeUsedAsync(id))
            {
                return new ResultInfo<bool>(false, this.Res.Bas.BeenUsed, false);
            }

            //获取原数据
            var model = await BasicDAL.Inst.GetOutTypeAsync(this.LoginInfo.FamilyID, id);
            if (model == null)
            {
                return new ResultInfo<bool>(false, this.Res.Gen.NoRight, false);
            }

            //从数据库删除
            await BasicDAL.Inst.DeleteOutTypeAsync(model);

            return new ResultInfo<bool>(true, Res.Gen.OK, true);
        }

        #endregion

        #region 下拉框

        /// <summary>
        /// 获取借还类型
        /// </summary>
        /// <returns></returns>
        public List<IDNameInfo> QueryBorrowRepayType()
        {
            var list = new List<IDNameInfo>();

            list.Add(new IDNameInfo() { ID = (int)EnmBorrowRepayType.BorrowIn, Name = this.Res.Cst.BRT.BorrowIn });
            list.Add(new IDNameInfo() { ID = (int)EnmBorrowRepayType.RepayIn, Name = this.Res.Cst.BRT.RepayIn });
            list.Add(new IDNameInfo() { ID = (int)EnmBorrowRepayType.BorrowOut, Name = this.Res.Cst.BRT.BorrowOut });
            list.Add(new IDNameInfo() { ID = (int)EnmBorrowRepayType.RepayOut, Name = this.Res.Cst.BRT.RepayOut });

            return list;
        }

        /// <summary>
        /// 获取收入统计方式
        /// </summary>
        /// <returns></returns>
        public List<IDNameInfo> QueryInGroupType()
        {
            var list = new List<IDNameInfo>();

            list.Add(new IDNameInfo() { ID = (int)EnmInGroupType.InType, Name = this.Res.Cst.IGT.InType });
            list.Add(new IDNameInfo() { ID = (int)EnmInGroupType.AmountAccount, Name = this.Res.Cst.IGT.AmountAccount });
            list.Add(new IDNameInfo() { ID = (int)EnmInGroupType.Month, Name = this.Res.Cst.IGT.Month });
            list.Add(new IDNameInfo() { ID = (int)EnmInGroupType.Year, Name = this.Res.Cst.IGT.Year });

            return list;
        }

        /// <summary>
        /// 获取支出统计方式
        /// </summary>
        /// <returns></returns>
        public List<IDNameInfo> QueryOutGroupType()
        {
            var list = new List<IDNameInfo>();

            list.Add(new IDNameInfo() { ID = (int)EnmOutGroupType.OutCategory, Name = this.Res.Cst.OGT.OutCategory });
            list.Add(new IDNameInfo() { ID = (int)EnmOutGroupType.OutType, Name = this.Res.Cst.OGT.OutType });
            list.Add(new IDNameInfo() { ID = (int)EnmOutGroupType.AmountAccount, Name = this.Res.Cst.OGT.AmountAccount });
            list.Add(new IDNameInfo() { ID = (int)EnmOutGroupType.Month, Name = this.Res.Cst.OGT.Month });
            list.Add(new IDNameInfo() { ID = (int)EnmOutGroupType.Year, Name = this.Res.Cst.OGT.Year });

            return list;
        }

        #endregion
    }
}
