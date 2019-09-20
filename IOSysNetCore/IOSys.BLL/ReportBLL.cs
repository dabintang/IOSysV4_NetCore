using IOSys.DAL;
using IOSys.DTO;
using IOSys.DTO.Enum;
using IOSys.DTO.Report;
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
    /// 报表 逻辑
    /// </summary>
    public class ReportBLL : BaseBLL
    {
        #region 明细

        /// <summary>
        /// 查询收入明细列表
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<PageAmountList<InRecordListInfo>> QueryInRecordAsync(InRecordReq req)
        {
            //查询
            var taskModel = ReportDAL.Inst.QueryInRecordAsync(this.LoginInfo.FamilyID, req); //明细
            var taskInType = BasicDAL.Inst.QueryInTypeAsync(this.LoginInfo.FamilyID); //类型
            var taskAmountAccount = BasicDAL.Inst.QueryAmountAccountAsync(this.LoginInfo.FamilyID); //账户

            var lstModel = await taskModel; //明细
            var lstInType = await taskInType; //类型
            var lstAmountAccount = await taskAmountAccount; //账户

            var list = new List<InRecordListInfo>();
            var seq = req.GetSkip() + 1;
            foreach (var model in lstModel)
            {
                var item = new InRecordListInfo();
                list.Add(item);

                //类型
                var inType = lstInType.Find(m => m.ID == model.InTypeID);

                //账户
                var amountAccount = lstAmountAccount.Find(m => m.ID == model.AmountAccountID);

                item.Seq = seq++;
                item.ID = model.ID;
                item.InDate = model.InDate;
                item.InTypeName = inType != null ? inType.Name : "";
                item.AmountAccountName = amountAccount != null ? amountAccount.Name : "";
                item.Amount = model.Amount;
                item.Remark = model.Remark;
            }

            return new PageAmountList<InRecordListInfo>(list, lstModel.TotalRecord, lstModel.TotalAmount);
        }

        /// <summary>
        /// 查询支出明细列表
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<PageAmountList<OutRecordListInfo>> QueryOutRecordAsync(OutRecordReq req)
        {
            //查询
            var taskModel = ReportDAL.Inst.QueryOutRecordAsync(this.LoginInfo.FamilyID, req); //明细
            var taskOutCategory = BasicDAL.Inst.QueryOutCategoryAsync(this.LoginInfo.FamilyID); //分类
            var taskOutType = BasicDAL.Inst.QueryOutTypeAsync(this.LoginInfo.FamilyID); //类型
            var taskAmountAccount = BasicDAL.Inst.QueryAmountAccountAsync(this.LoginInfo.FamilyID); //账户

            var lstModel = await taskModel; //明细
            var lstOutCategory = await taskOutCategory; //分类
            var lstOutType = await taskOutType; //类型
            var lstAmountAccount = await taskAmountAccount; //账户

            var list = new List<OutRecordListInfo>();
            var seq = req.GetSkip() + 1;
            foreach (var model in lstModel)
            {
                var item = new OutRecordListInfo();
                list.Add(item);

                //类型
                var outType = lstOutType.Find(m => m.ID == model.OutTypeID);

                //分类
                OutCategory outCategory = null;
                if (outType != null)
                {
                    outCategory = lstOutCategory.Find(m => m.ID == outType.OutCategoryID);
                }

                //账户
                var amountAccount = lstAmountAccount.Find(m => m.ID == model.AmountAccountID);

                item.Seq = seq++;
                item.ID = model.ID;
                item.OutDate = model.OutDate;
                item.OutCategoryName = outCategory != null ? outCategory.Name : "";
                item.OutTypeName = outType != null ? outType.Name : "";
                item.AmountAccountName = amountAccount != null ? amountAccount.Name : "";
                item.Amount = model.Amount;
                item.Remark = model.Remark;
            }

            return new PageAmountList<OutRecordListInfo>(list, lstModel.TotalRecord, lstModel.TotalAmount);
        }

        /// <summary>
        /// 查询转账明细列表
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<PageList<TransferRecordListInfo>> QueryTransferRecordAsync(TransferRecordReq req)
        {
            //查询
            var taskModel = ReportDAL.Inst.QueryTransferRecordAsync(this.LoginInfo.FamilyID, req); //明细
            var taskAmountAccount = BasicDAL.Inst.QueryAmountAccountAsync(this.LoginInfo.FamilyID); //账户

            var lstModel = await taskModel; //明细
            var lstAmountAccount = await taskAmountAccount; //账户

            var list = new List<TransferRecordListInfo>();
            var seq = req.GetSkip() + 1;
            foreach (var model in lstModel)
            {
                var item = new TransferRecordListInfo();
                list.Add(item);

                //源账户
                var fromAmountAccount = lstAmountAccount.Find(m => m.ID == model.FromAmountAccountID);

                //目标账户
                var toAmountAccount = lstAmountAccount.Find(m => m.ID == model.ToAmountAccountID);

                item.Seq = seq++;
                item.ID = model.ID;
                item.TransferDate = model.TransferDate;
                item.FromAmountAccountName = fromAmountAccount != null ? fromAmountAccount.Name : "";
                item.ToAmountAccountName = toAmountAccount != null ? toAmountAccount.Name : "";
                item.Amount = model.Amount;
                item.Remark = model.Remark;
            }

            return new PageList<TransferRecordListInfo>(list, lstModel.TotalRecord);
        }

        /// <summary>
        /// 查询借还明细列表
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<PageList<BorrowRepayRecordListInfo>> QueryBorrowRepayRecordAsync(BorrowRepayRecordReq req)
        {
            //查询
            var taskModel = ReportDAL.Inst.QueryBorrowRepayRecordAsync(this.LoginInfo.FamilyID, req); //明细
            var taskAmountAccount = BasicDAL.Inst.QueryAmountAccountAsync(this.LoginInfo.FamilyID); //账户

            var lstModel = await taskModel; //明细
            var lstAmountAccount = await taskAmountAccount; //账户

            var list = new List<BorrowRepayRecordListInfo>();
            var seq = req.GetSkip() + 1;
            foreach (var model in lstModel)
            {
                var item = new BorrowRepayRecordListInfo();
                list.Add(item);

                //账户
                var amountAccount = lstAmountAccount.Find(m => m.ID == model.AmountAccountID);

                item.Seq = seq++;
                item.ID = model.ID;
                item.BRDate = model.BRDate;
                item.Target = model.Target;
                item.BRType = model.BRType;
                item.BRTypeName = this.GetBorrowRepayTypeName(model.BRType);
                item.AmountAccountName = amountAccount != null ? amountAccount.Name : "";
                item.Amount = model.Amount;
                item.Remark = model.Remark;
            }

            return new PageList<BorrowRepayRecordListInfo>(list, lstModel.TotalRecord);
        }

        /// <summary>
        /// 查询账户流水
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PageMonthAmountList<AccountTurnoverListInfo>> QueryAccountTurnoverAsync(AccountTurnoverListReq req)
        {
            //开始日期
            DateTime startDate = new DateTime(req.Month.Year, req.Month.Month, 1);
            //结束日期
            DateTime endDate = startDate.AddMonths(1).AddMilliseconds(-1);

            #region 查询

            //查询收入明细
            var reqIn = new InRecordReq()
            {
                StartDate = startDate,
                EndDate = endDate,
                LstAmountAccountID = req.LstAmountAccountID
            };
            var taskIn = this.QueryInRecordAsync(reqIn);

            //查询支出明细
            var reqOut = new OutRecordReq()
            {
                StartDate = startDate,
                EndDate = endDate,
                LstAmountAccountID = req.LstAmountAccountID
            };
            var taskOut = this.QueryOutRecordAsync(reqOut);

            //查询转账转出明细
            var reqTransferFrom = new TransferRecordReq()
            {
                StartDate = startDate,
                EndDate = endDate,
                LstFromAmountAccountID = req.LstAmountAccountID
            };
            var taskTransferFrom = this.QueryTransferRecordAsync(reqTransferFrom);

            //查询转账转入明细
            var reqTransferTo = new TransferRecordReq()
            {
                StartDate = startDate,
                EndDate = endDate,
                LstToAmountAccountID = req.LstAmountAccountID
            };
            var taskTransferTo = this.QueryTransferRecordAsync(reqTransferTo);

            //查询借还明细
            var reqBR = new BorrowRepayRecordReq()
            {
                StartDate = startDate,
                EndDate = endDate,
                LstAmountAccountID = req.LstAmountAccountID
            };
            var taskBR = this.QueryBorrowRepayRecordAsync(reqBR);

            #endregion

            #region 结果类型转换

            //返回结果
            var list = new List<AccountTurnoverListInfo>();

            //收入
            var lstIn = await taskIn;
            foreach (var record in lstIn)
            {
                var item = new AccountTurnoverListInfo();
                list.Add(item);

                item.DataType = EnmDataType.In;
                item.ID = record.ID;
                item.Date = record.InDate;
                item.AmountAccountName = record.AmountAccountName;
                item.TypeName = record.InTypeName;
                item.Amount = record.Amount;
            }

            //支出
            var lstOut = await taskOut;
            foreach (var record in lstOut)
            {
                var item = new AccountTurnoverListInfo();
                list.Add(item);

                item.DataType = EnmDataType.Out;
                item.ID = record.ID;
                item.Date = record.OutDate;
                item.AmountAccountName = record.AmountAccountName;
                item.TypeName = record.OutTypeName;
                item.Amount = -record.Amount;
            }

            //转账转出
            var lstTransferFrom = await taskTransferFrom;
            foreach (var record in lstTransferFrom)
            {
                var item = new AccountTurnoverListInfo();
                list.Add(item);

                item.DataType = EnmDataType.Transfer;
                item.ID = record.ID;
                item.Date = record.TransferDate;
                item.AmountAccountName = record.FromAmountAccountName;
                item.TypeName = this.Res.Cst.TranDir.TransferOut;
                item.Amount = -record.Amount;
            }

            //转账转入
            var lstTransferTo = await taskTransferTo;
            foreach (var record in lstTransferTo)
            {
                var item = new AccountTurnoverListInfo();
                list.Add(item);

                item.DataType = EnmDataType.Transfer;
                item.ID = record.ID;
                item.Date = record.TransferDate;
                item.AmountAccountName = record.ToAmountAccountName;
                item.TypeName = this.Res.Cst.TranDir.TransferIn;
                item.Amount = record.Amount;
            }

            //借还
            var lstBR = await taskBR;
            foreach (var record in lstBR)
            {
                var item = new AccountTurnoverListInfo();
                list.Add(item);

                item.DataType = EnmDataType.BorrowRepay;
                item.ID = record.ID;
                item.Date = record.BRDate;
                item.AmountAccountName = record.AmountAccountName;

                switch ((EnmBorrowRepayType)record.BRType)
                {
                    case EnmBorrowRepayType.BorrowIn:
                        item.TypeName = this.Res.Cst.BRT.BorrowIn;
                        item.Amount = record.Amount;
                        break;
                    case EnmBorrowRepayType.RepayIn:
                        item.TypeName = this.Res.Cst.BRT.RepayIn;
                        item.Amount = record.Amount;
                        break;
                    case EnmBorrowRepayType.BorrowOut:
                        item.TypeName = this.Res.Cst.BRT.BorrowOut;
                        item.Amount = -record.Amount;
                        break;
                    case EnmBorrowRepayType.RepayOut:
                        item.TypeName = this.Res.Cst.BRT.RepayOut;
                        item.Amount = -record.Amount;
                        break;
                }
            }

            //总金额
            var totalAmount = list.Sum(m => m.Amount);

            #endregion

            //排序
            list = list.OrderByDescending(m => m.Date).ThenByDescending(m => m.ID).ThenBy(m => m.Amount).ToList();

            //是否还有前一个月的数据
            var hasPreMonth = startDate > (await InOutDAL.Inst.GetMinTurnoverDate(this.LoginInfo.FamilyID));

            return new PageMonthAmountList<AccountTurnoverListInfo>(list, req.Month, totalAmount, hasPreMonth);
        }

        #endregion

        #region 统计

        /// <summary>
        /// 收入统计
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<SumListInfo<string>>> SumInComeAsync(InSumReq req)
        {
            //借还统计条件
            var reqSumBR = new BorrowRepaySumReq();
            reqSumBR.StartDate = req.StartDate;
            reqSumBR.EndDate = req.EndDate;
            reqSumBR.LstBRType.Add((int)EnmBorrowRepayType.BorrowIn);
            reqSumBR.LstBRType.Add((int)EnmBorrowRepayType.RepayIn);

            //统计类型
            switch (req.GroupType)
            {
                case EnmInGroupType.InType:
                    #region 类型
                    {
                        //类型
                        var taskInType = BasicDAL.Inst.QueryInTypeAsync(this.LoginInfo.FamilyID);
                        //统计
                        var taskSum = ReportDAL.Inst.SumInComeInTypeAsync(this.LoginInfo.FamilyID, req);
                        //统计借还
                        Task<List<BorrowRepaySumListInfo<int>>> taskSumBR = null;

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            reqSumBR.GroupType = EnmBorrowRepayGroupType.BRType;
                            taskSumBR = ReportDAL.Inst.SumBorrowRepayBRTypeAsync(this.LoginInfo.FamilyID, reqSumBR);
                        }

                        //等待查询完成
                        var lstInType = await taskInType;
                        var lstSum = await taskSum;

                        //返回值
                        var list = new List<SumListInfo<string>>();
                        foreach (var temp in lstSum)
                        {
                            var item = new SumListInfo<string>();
                            list.Add(item);

                            var inType = lstInType.Find(m => m.ID == temp.name);
                            item.name = inType != null ? inType.Name : string.Empty;
                            item.value = temp.value;
                        }

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            var lstSumBR = await taskSumBR;
                            foreach (var sumBR in lstSumBR)
                            {
                                var item = new SumListInfo<string>();
                                list.Add(item);

                                item.name = this.GetBorrowRepayTypeName(sumBR.BRType);
                                item.value = sumBR.value;
                            }
                        }

                        list = list.OrderByDescending(m => m.value).ToList();
                        return list;
                    }
                #endregion
                case EnmInGroupType.AmountAccount:
                    #region 账户
                    {
                        //账户
                        var taskAmountAccount = BasicDAL.Inst.QueryAmountAccountAsync(this.LoginInfo.FamilyID);
                        //统计
                        var taskSum = ReportDAL.Inst.SumInComeAmountAccountAsync(this.LoginInfo.FamilyID, req);
                        //统计借还
                        Task<List<BorrowRepaySumListInfo<int>>> taskSumBR = null;

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            reqSumBR.GroupType = EnmBorrowRepayGroupType.AmountAccount;
                            taskSumBR = ReportDAL.Inst.SumBorrowRepayAmountAccountAsync(this.LoginInfo.FamilyID, reqSumBR);
                        }

                        //等待查询完成
                        var lstAmountAccount = await taskAmountAccount;
                        var lstSum = await taskSum;

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            var lstSumBR = await taskSumBR;
                            lstSum.AddRange(lstSumBR);

                            lstSum = (from m in lstSum
                                      group m by m.name into g
                                      select new SumListInfo<int>()
                                      {
                                          name = g.Key,
                                          value = g.Sum(m => m.value)
                                      }).ToList();
                        }

                        //返回值
                        var list = new List<SumListInfo<string>>();
                        foreach (var temp in lstSum)
                        {
                            var item = new SumListInfo<string>();
                            list.Add(item);

                            var amountAccount = lstAmountAccount.Find(m => m.ID == temp.name);
                            item.name = amountAccount != null ? amountAccount.Name : string.Empty;
                            item.value = temp.value;
                        }

                        list = list.OrderByDescending(m => m.value).ToList();
                        return list;
                    }
                #endregion
                case EnmInGroupType.Month:
                    #region 月份
                    {
                        //统计
                        var taskSum = ReportDAL.Inst.SumInComeMonthAsync(this.LoginInfo.FamilyID, req);
                        //统计借还
                        Task<List<BorrowRepaySumListInfo<string>>> taskSumBR = null;

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            reqSumBR.GroupType = EnmBorrowRepayGroupType.Month;
                            taskSumBR = ReportDAL.Inst.SumBorrowRepayMonthAsync(this.LoginInfo.FamilyID, reqSumBR);
                        }

                        //等待查询完成
                        var lstSum = await taskSum;

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            var lstSumBR = await taskSumBR;
                            lstSum.AddRange(lstSumBR);

                            lstSum = (from m in lstSum
                                      group m by m.name into g
                                      select new SumListInfo<string>()
                                      {
                                          name = g.Key,
                                          value = g.Sum(m => m.value)
                                      }).ToList();
                        }

                        lstSum = lstSum.OrderByDescending(m => m.name).ToList();
                        return lstSum;
                    }
                #endregion
                case EnmInGroupType.Year:
                    #region 年度
                    {
                        //统计
                        var taskSum = ReportDAL.Inst.SumInComeYearAsync(this.LoginInfo.FamilyID, req);
                        //统计借还
                        Task<List<BorrowRepaySumListInfo<string>>> taskSumBR = null;

                        //等待查询完成
                        var lstSum = await taskSum;

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            reqSumBR.GroupType = EnmBorrowRepayGroupType.Year;
                            taskSumBR = ReportDAL.Inst.SumBorrowRepayYearAsync(this.LoginInfo.FamilyID, reqSumBR);
                        }

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            var lstSumBR = await taskSumBR;
                            lstSum.AddRange(lstSumBR);

                            lstSum = (from m in lstSum
                                      group m by m.name into g
                                      select new SumListInfo<string>()
                                      {
                                          name = g.Key,
                                          value = g.Sum(m => m.value)
                                      }).ToList();
                        }

                        lstSum = lstSum.OrderByDescending(m => m.name).ToList();
                        return lstSum;
                    }
                    #endregion
            }

            return new List<SumListInfo<string>>();
        }

        /// <summary>
        /// 支出统计
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<SumListInfo<string>>> SumOutPutAsync(OutSumReq req)
        {
            //借还统计条件
            var reqSumBR = new BorrowRepaySumReq();
            reqSumBR.StartDate = req.StartDate;
            reqSumBR.EndDate = req.EndDate;
            reqSumBR.LstBRType.Add((int)EnmBorrowRepayType.BorrowOut);
            reqSumBR.LstBRType.Add((int)EnmBorrowRepayType.RepayOut);

            //统计类型
            switch (req.GroupType)
            {
                case EnmOutGroupType.OutCategory:
                    #region 分类
                    {
                        //分类
                        var taskOutCategory = BasicDAL.Inst.QueryOutCategoryAsync(this.LoginInfo.FamilyID);
                        //统计
                        var taskSum = ReportDAL.Inst.SumOutPutOutCategoryAsync(this.LoginInfo.FamilyID, req);
                        //统计借还
                        Task<List<BorrowRepaySumListInfo<int>>> taskSumBR = null;

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            reqSumBR.GroupType = EnmBorrowRepayGroupType.BRType;
                            taskSumBR = ReportDAL.Inst.SumBorrowRepayBRTypeAsync(this.LoginInfo.FamilyID, reqSumBR);
                        }

                        //等待查询完成
                        var lstOutCategory = await taskOutCategory;
                        var lstSum = await taskSum;

                        //返回值
                        var list = new List<SumListInfo<string>>();
                        foreach (var temp in lstSum)
                        {
                            var item = new SumListInfo<string>();
                            list.Add(item);

                            var outCategory = lstOutCategory.Find(m => m.ID == temp.name);
                            item.name = outCategory != null ? outCategory.Name : string.Empty;
                            item.value = temp.value;
                        }

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            var lstSumBR = await taskSumBR;
                            foreach (var sumBR in lstSumBR)
                            {
                                var item = new SumListInfo<string>();
                                list.Add(item);

                                item.name = this.GetBorrowRepayTypeName(sumBR.BRType);
                                item.value = sumBR.value;
                            }
                        }

                        list = list.OrderByDescending(m => m.value).ToList();
                        return list;
                    }
                #endregion
                case EnmOutGroupType.OutType:
                    #region 类型
                    {
                        //类型
                        var taskOutType = BasicDAL.Inst.QueryOutTypeAsync(this.LoginInfo.FamilyID);
                        //统计
                        var taskSum = ReportDAL.Inst.SumOutPutOutTypeAsync(this.LoginInfo.FamilyID, req);
                        //统计借还
                        Task<List<BorrowRepaySumListInfo<int>>> taskSumBR = null;

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            reqSumBR.GroupType = EnmBorrowRepayGroupType.BRType;
                            taskSumBR = ReportDAL.Inst.SumBorrowRepayBRTypeAsync(this.LoginInfo.FamilyID, reqSumBR);
                        }

                        //等待查询完成
                        var lstOutType = await taskOutType;
                        var lstSum = await taskSum;

                        //返回值
                        var list = new List<SumListInfo<string>>();
                        foreach (var temp in lstSum)
                        {
                            var item = new SumListInfo<string>();
                            list.Add(item);

                            var outType = lstOutType.Find(m => m.ID == temp.name);
                            item.name = outType != null ? outType.Name : string.Empty;
                            item.value = temp.value;
                        }

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            var lstSumBR = await taskSumBR;
                            foreach (var sumBR in lstSumBR)
                            {
                                var item = new SumListInfo<string>();
                                list.Add(item);

                                item.name = this.GetBorrowRepayTypeName(sumBR.BRType);
                                item.value = sumBR.value;
                            }
                        }

                        list = list.OrderByDescending(m => m.value).ToList();
                        return list;
                    }
                #endregion
                case EnmOutGroupType.AmountAccount:
                    #region 账户
                    {
                        //账户
                        var taskAmountAccount = BasicDAL.Inst.QueryAmountAccountAsync(this.LoginInfo.FamilyID);
                        //统计
                        var taskSum = ReportDAL.Inst.SumOutPutAmountAccountAsync(this.LoginInfo.FamilyID, req);
                        //统计借还
                        Task<List<BorrowRepaySumListInfo<int>>> taskSumBR = null;

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            reqSumBR.GroupType = EnmBorrowRepayGroupType.AmountAccount;
                            taskSumBR = ReportDAL.Inst.SumBorrowRepayAmountAccountAsync(this.LoginInfo.FamilyID, reqSumBR);
                        }

                        //等待查询完成
                        var lstAmountAccount = await taskAmountAccount;
                        var lstSum = await taskSum;

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            var lstSumBR = await taskSumBR;
                            lstSum.AddRange(lstSumBR);

                            lstSum = (from m in lstSum
                                      group m by m.name into g
                                      select new SumListInfo<int>()
                                      {
                                          name = g.Key,
                                          value = g.Sum(m => m.value)
                                      }).ToList();
                        }

                        //返回值
                        var list = new List<SumListInfo<string>>();
                        foreach (var temp in lstSum)
                        {
                            var item = new SumListInfo<string>();
                            list.Add(item);

                            var amountAccount = lstAmountAccount.Find(m => m.ID == temp.name);
                            item.name = amountAccount != null ? amountAccount.Name : string.Empty;
                            item.value = temp.value;
                        }

                        list = list.OrderByDescending(m => m.value).ToList();
                        return list;
                    }
                #endregion
                case EnmOutGroupType.Month:
                    #region 月份
                    {
                        //统计
                        var taskSum = ReportDAL.Inst.SumOutPutMonthAsync(this.LoginInfo.FamilyID, req);
                        //统计借还
                        Task<List<BorrowRepaySumListInfo<string>>> taskSumBR = null;

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            reqSumBR.GroupType = EnmBorrowRepayGroupType.Month;
                            taskSumBR = ReportDAL.Inst.SumBorrowRepayMonthAsync(this.LoginInfo.FamilyID, reqSumBR);
                        }

                        //等待查询完成
                        var lstSum = await taskSum;

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            var lstSumBR = await taskSumBR;
                            lstSum.AddRange(lstSumBR);

                            lstSum = (from m in lstSum
                                      group m by m.name into g
                                      select new SumListInfo<string>()
                                      {
                                          name = g.Key,
                                          value = g.Sum(m => m.value)
                                      }).ToList();
                        }

                        lstSum = lstSum.OrderByDescending(m => m.name).ToList();
                        return lstSum;
                    }
                #endregion
                case EnmOutGroupType.Year:
                    #region 年度
                    {
                        //统计
                        var taskSum = ReportDAL.Inst.SumOutPutYearAsync(this.LoginInfo.FamilyID, req);
                        //统计借还
                        Task<List<BorrowRepaySumListInfo<string>>> taskSumBR = null;

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            reqSumBR.GroupType = EnmBorrowRepayGroupType.Year;
                            taskSumBR = ReportDAL.Inst.SumBorrowRepayYearAsync(this.LoginInfo.FamilyID, reqSumBR);
                        }

                        //等待查询完成
                        var lstSum = await taskSum;

                        //是否包含借还
                        if (req.IsContainBorrowRepay)
                        {
                            var lstSumBR = await taskSumBR;
                            lstSum.AddRange(lstSumBR);

                            lstSum = (from m in lstSum
                                      group m by m.name into g
                                      select new SumListInfo<string>()
                                      {
                                          name = g.Key,
                                          value = g.Sum(m => m.value)
                                      }).ToList();
                        }

                        lstSum = lstSum.OrderByDescending(m => m.name).ToList();
                        return lstSum;
                    }
                    #endregion
            }

            return new List<SumListInfo<string>>();
        }

        /// <summary>
        /// 按借还者统计借还信息
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<SumListInfo<string>>> SumBorrowRepayTargetAsync(BorrowRepaySumReq req)
        {
            //统计
            var list = await ReportDAL.Inst.SumBorrowRepayTargetAsync(this.LoginInfo.FamilyID);

            //是否显示金额为0的记录
            if (req.IsShowZero == false)
            {
                list = list.Where(m => m.value != 0).ToList();
            }

            //按金额绝对值降序排序
            list = list.OrderByDescending(m => Math.Abs(m.value)).ToList();
            return list;
        }

        /// <summary>
        /// 用户收支统计
        /// </summary>
        /// <returns></returns>
        public async Task<UserTotalInfo> SumUserTotal()
        {
            //用户信息
            var taskUser = AccountDAL.Inst.GetUserAsync(this.LoginInfo.FamilyID, this.LoginInfo.UserID);

            // 获取家庭信息
            var taskFamily = BasicDAL.Inst.GetFamilyAsync(this.LoginInfo.FamilyID);

            //账户信息
            var taskLstAccount = BasicDAL.Inst.QueryAmountAccountAsync(this.LoginInfo.FamilyID);

            //当前年开始
            DateTime curYearStart = new DateTime(DateTime.Today.Year, 1, 1);
            //当前年结束
            DateTime curYearEnd = curYearStart.AddYears(1).AddMilliseconds(-1);
            //当前月开始
            DateTime curMonthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            //当前月结束
            DateTime curMonthEnd = curMonthStart.AddMonths(1).AddMilliseconds(-1);

            //统计年收入
            var taskSumInYear = ReportDAL.Inst.SumInComeAsync(this.LoginInfo.FamilyID, curYearStart, curYearEnd);
            //统计月收入
            var taskSumInMonth = ReportDAL.Inst.SumInComeAsync(this.LoginInfo.FamilyID, curMonthStart, curMonthEnd);

            //统计年支出
            var taskSumOutYear = ReportDAL.Inst.SumOutPutAsync(this.LoginInfo.FamilyID, curYearStart, curYearEnd);
            //统计月支出
            var taskSumOutMonth = ReportDAL.Inst.SumOutPutAsync(this.LoginInfo.FamilyID, curMonthStart, curMonthEnd);

            //统计年借还
            var reqBRYear = new BorrowRepaySumReq();
            reqBRYear.StartDate = curYearStart;
            reqBRYear.EndDate = curYearEnd;
            var taskSumBRYear = ReportDAL.Inst.SumBorrowRepayBRTypeAsync(this.LoginInfo.FamilyID, reqBRYear);

            //统计月借还
            var reqBRMonth = new BorrowRepaySumReq();
            reqBRMonth.StartDate = curMonthStart;
            reqBRMonth.EndDate = curMonthEnd;
            var taskSumBRMonth = ReportDAL.Inst.SumBorrowRepayBRTypeAsync(this.LoginInfo.FamilyID, reqBRMonth);

            var info = new UserTotalInfo();
            info.NickName = (await taskUser).NickName;
            info.FamilyName = (await taskFamily).Name;
            info.TotalAmount = (await taskLstAccount).Sum(m => m.Amount);
            info.Year = curYearStart.Year;
            info.TotalInCurYear = await taskSumInYear;
            info.TotalOutCurYear = await taskSumOutYear;
            info.Month = curMonthStart.Month;
            info.TotalInCurMonth = await taskSumInMonth;
            info.TotalOutCurMonth = await taskSumOutMonth;

            var lstSumBRYear = await taskSumBRYear;
            info.TotalBRInCurYear = lstSumBRYear
               .Where(m => m.BRType == (int)EnmBorrowRepayType.BorrowIn || m.BRType == (int)EnmBorrowRepayType.RepayIn).Sum(m => m.value);
            info.TotalBROutCurYear = lstSumBRYear
                .Where(m => m.BRType == (int)EnmBorrowRepayType.BorrowOut || m.BRType == (int)EnmBorrowRepayType.RepayOut).Sum(m => m.value);

            var lstSumBTMonth = await taskSumBRMonth;
            info.TotalBRInCurMonth = lstSumBTMonth
                .Where(m => m.BRType == (int)EnmBorrowRepayType.BorrowIn || m.BRType == (int)EnmBorrowRepayType.RepayIn).Sum(m => m.value);
            info.TotalBROutCurMonth = lstSumBTMonth
                .Where(m => m.BRType == (int)EnmBorrowRepayType.BorrowOut || m.BRType == (int)EnmBorrowRepayType.RepayOut).Sum(m => m.value);

            return info;
        }

        /// <summary>
        /// 月份统计
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<List<MonthSumListInfo<string>>> SumMonthAsync(MonthSumReq req)
        {
            //收入
            var reqIn = new InSumReq();
            reqIn.GroupType = EnmInGroupType.Month;
            reqIn.IsContainBorrowRepay = req.IsContainBorrowRepay;
            var taskIn = this.SumInComeAsync(reqIn);

            //支出
            var reqOut = new OutSumReq();
            reqOut.GroupType = EnmOutGroupType.Month;
            reqOut.IsContainBorrowRepay = req.IsContainBorrowRepay;
            var taskOut = this.SumOutPutAsync(reqOut);

            //结果
            var list = new List<MonthSumListInfo<string>>();

            //收入
            var lstIn = await taskIn;
            foreach (var income in lstIn)
            {
                list.Add(new MonthSumListInfo<string>()
                {
                    name = income.name,
                    value = income.value,
                    DataType = EnmDataType.In,
                    DataTypeName = this.Res.Cst.DT.In
                });
            }

            //支出
            var lstOut = await taskOut;
            foreach (var output in lstOut)
            {
                list.Add(new MonthSumListInfo<string>()
                {
                    name = output.name,
                    value = -output.value,
                    DataType = EnmDataType.Out,
                    DataTypeName = this.Res.Cst.DT.Out
                });
            }

            //排序
            list = list.OrderByDescending(m => m.name).ThenByDescending(m => m.value).ToList();

            return list;
        }

        /// <summary>
        /// 月份支出统计
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns></returns>
        public async Task<List<MonthOutCategorySumListInfo>> SumMonthOutAsync(MonthOutSumReq req)
        {
            //开始日期
            var startDate = new DateTime(req.Month.Year, req.Month.Month, 1);
            //截止日期
            var endDate = startDate.AddMonths(1).AddMilliseconds(-1);

            //类型
            var taskOutType = BasicDAL.Inst.QueryOutTypeAsync(this.LoginInfo.FamilyID);
            //分类
            var taskOutCategory = BasicDAL.Inst.QueryOutCategoryAsync(this.LoginInfo.FamilyID);
            //统计借还
            Task<List<BorrowRepaySumListInfo<int>>> taskSumBR = null;

            //是否包含借还
            if (req.IsContainBorrowRepay)
            {
                //借还统计条件
                var reqBR = new BorrowRepaySumReq();
                reqBR.StartDate = startDate;
                reqBR.EndDate = endDate;
                reqBR.LstBRType.Add((int)EnmBorrowRepayType.BorrowOut);
                reqBR.LstBRType.Add((int)EnmBorrowRepayType.RepayOut);
                reqBR.GroupType = EnmBorrowRepayGroupType.BRType;

                //借还统计
                taskSumBR = ReportDAL.Inst.SumBorrowRepayBRTypeAsync(this.LoginInfo.FamilyID, reqBR);
            }

            //支出统计条件
            var reqOut = new OutSumReq();
            reqOut.StartDate = startDate;
            reqOut.EndDate = endDate;
            reqOut.GroupType = EnmOutGroupType.OutType;
            reqOut.IsContainBorrowRepay = req.IsContainBorrowRepay;

            //统计支出
            var taskSumOut = ReportDAL.Inst.SumOutPutOutTypeAsync(this.LoginInfo.FamilyID, reqOut);

            //等待查询完成
            var lstOutCategory = await taskOutCategory;
            var lstOutType = await taskOutType;
            var lstOutPut = await taskSumOut;

            var list = new List<MonthOutCategorySumListInfo>();
            foreach (var output in lstOutPut)
            {
                //支出类型
                var outType = lstOutType.Find(m => m.ID == output.name);
                //支出分类
                var outCategory = lstOutCategory.Find(m => m.ID == outType.OutCategoryID);

                //列表中如果没有此支出分类
                if (list.Exists(m => m.ID == outCategory.ID) == false)
                {
                    var itemOC = new MonthOutCategorySumListInfo();
                    list.Add(itemOC);

                    itemOC.ID = outCategory.ID;
                    itemOC.Name = outCategory.Name;
                    itemOC.DataType = EnmDataType.Out;
                    itemOC.LstSumOutType = new List<MonthOutTypeSumListInfo>();
                }

                //支出分类统计
                var ocSum = list.Find(m => m.ID == outCategory.ID);

                //支出类型统计
                var otSum = new MonthOutTypeSumListInfo();
                ocSum.LstSumOutType.Add(otSum);

                otSum.ID = outType.ID;
                otSum.Name = outType.Name;
                otSum.Amount = output.value;
            }

            //借还
            if (req.IsContainBorrowRepay)
            {
                var itemBR = new MonthOutCategorySumListInfo();
                list.Add(itemBR);

                itemBR.ID = 0;
                itemBR.Name = this.Res.Cst.DT.BorrowRepay;
                itemBR.DataType = EnmDataType.BorrowRepay;
                itemBR.LstSumOutType = new List<MonthOutTypeSumListInfo>();

                var lstSumBR = await taskSumBR;
                foreach (var sumBR in lstSumBR)
                {
                    //借还统计
                    var brSum = new MonthOutTypeSumListInfo();
                    itemBR.LstSumOutType.Add(brSum);

                    brSum.ID = sumBR.name;
                    brSum.Name = this.GetBorrowRepayTypeName(sumBR.BRType);
                    brSum.Amount = sumBR.value;
                }
            }

            //排序
            list = list.OrderByDescending(m => m.Amount).ToList();

            return list;
        }

        #endregion
    }
}
