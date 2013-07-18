using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EggFarmSystem.Models;
using ServiceStack.OrmLite;

namespace EggFarmSystem.Services
{
    public interface IEmployeeCostService
    {
        SearchResult<EmployeeCost> Search(DateRangeSearchInfo searchInfo);

        EmployeeCost Get(Guid id);

        EmployeeCost GetByDate(DateTime date);

        void Save(EmployeeCost cost);

        void Delete(Guid id);
    }

    public class EmployeeCostService : IEmployeeCostService
    {
        private readonly IDbConnectionFactory factory;

        public EmployeeCostService(IDbConnectionFactory factory)
        {
            this.factory = factory;
        }

        public SearchResult<EmployeeCost> Search(DateRangeSearchInfo searchInfo)
        {
            int start = (searchInfo.PageIndex - 1) * searchInfo.PageSize;

            var result = new SearchResult<EmployeeCost>();

            using (var conn = factory.OpenDbConnection())
            {
                var ev = OrmLiteConfig.DialectProvider.ExpressionVisitor<Models.Data.EmployeeCost>();

                if (searchInfo.Start.HasValue && searchInfo.End.HasValue)
                {
                    ev = ev.Where(e => e.Date >= searchInfo.Start.Value.Date && e.Date <= searchInfo.End.Value.Date);
                }

                ev.OrderByDescending(e => e.Date).Limit(start, searchInfo.PageSize);

                var costList = conn.Select(ev);
                foreach (var costData in costList)
                {
                    var list = conn.Where<Models.Data.EmployeeCostDetail>(new { CostId = costData.Id });

                    var cost = MapDataToModel(costData, list);
                    result.Items.Add(cost);
                }

                if (searchInfo.Start.HasValue && searchInfo.End.HasValue)
                {
                    result.Total = (int)conn.Count<Models.Data.EmployeeCost>(
                        e => e.Date >= searchInfo.Start.Value.Date && e.Date <= searchInfo.End.Value.Date);
                }
                else
                {
                    result.Total = (int)conn.Count<Models.Data.EmployeeCost>();
                }
            }

            return result;
        }

        public EmployeeCost Get(Guid id)
        {
            using (var conn = factory.OpenDbConnection())
            {
                var costData = conn.GetById<Models.Data.EmployeeCost>(id.ToString());

                if (costData == null)
                    return null;

                var details = conn.Where<Models.Data.EmployeeCostDetail>(new {CostId = costData.Id});

                return MapDataToModel(costData, details);
             }
        }

        public EmployeeCost GetByDate(DateTime date)
        {
            using (var conn = factory.OpenDbConnection())
            {
                var costData = conn.FirstOrDefault<Models.Data.EmployeeCost>(c => c.Date == date.Date);

                if (costData == null)
                    return null;

                var details = conn.Where<Models.Data.EmployeeCostDetail>(new { CostId = costData.Id });

                return MapDataToModel(costData, details);
            }
        }

        public void Save(EmployeeCost cost)
        {
            using (var db = factory.OpenDbConnection())
            {
                using (var tx = db.OpenTransaction())
                {
                    bool isNew = cost.IsNew;

                    Models.Data.EmployeeCost costData = null;

                    if (isNew)
                    {
                        costData = db.FirstOrDefault<Models.Data.EmployeeCost>(u => u.Date == cost.Date);

                        if (costData != null)
                        {
                            tx.Rollback();
                            throw new ServiceException("EmployeeCost_DuplicateDate");
                        }
                    }

                    if (isNew)
                        cost.Id = Guid.NewGuid();

                    costData = Mapper.Map<EmployeeCost, Models.Data.EmployeeCost>(cost);

                    if (isNew) db.InsertParam(costData); else db.UpdateParam(costData);

                    if (!isNew)
                        db.Delete<Models.Data.EmployeeCostDetail>(where: "CostId = {0}".Params(costData.Id.ToString()));

                    foreach (var detail in cost.Details)
                    {
                        var detailData = Mapper.Map<EmployeeCostDetail, Models.Data.EmployeeCostDetail>(detail);
                        detailData.CostId = cost.Id;
                        db.InsertParam(detailData);
                    }
                    try
                    {
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw new ServiceException(ex.Message);
                    }
                }
            }
        }

        public void Delete(Guid id)
        {
            using (var db = factory.OpenDbConnection())
            {
                using (var trans = db.OpenTransaction())
                {
                    db.Delete<Models.Data.EmployeeCostDetail>(where: "CostId = {0}".Params(id.ToString()));
                    db.DeleteByIdParam<Models.Data.EmployeeCost>(id.ToString());
                    trans.Commit();
                }
            }
        }

        private EmployeeCost MapDataToModel(Models.Data.EmployeeCost costData,
                                            IList<Models.Data.EmployeeCostDetail> detailsData)
        {
            var cost = Mapper.Map<Models.Data.EmployeeCost, EmployeeCost>(costData);

            if (detailsData != null)
            {
                foreach (var detailData in detailsData)
                {
                    var detail = Mapper.Map<Models.Data.EmployeeCostDetail, EmployeeCostDetail>(detailData);
                    cost.Details.Add(detail);
                }
            }

            return cost;
        }
    }
}
