using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AutoMapper;
using EggFarmSystem.Models;
using MySql.Data.MySqlClient;
using ServiceStack.OrmLite;

namespace EggFarmSystem.Services
{
    public interface IHenDepreciationService
    {
        SearchResult<HenDepreciation> Search(DateRangeSearchInfo searchInfo);

        HenDepreciation Get(Guid id);

        HenDepreciation GetByDate(DateTime date);

        void Save(HenDepreciation depreciation);

        void Delete(Guid id);

        HenDepreciation GetInitialValues(DateTime date);
    }

    public class HenDepreciationService : IHenDepreciationService
    {
        private readonly IDbConnectionFactory factory; 

        public HenDepreciationService(IDbConnectionFactory factory)
        {
            this.factory = factory;
        }

        public SearchResult<HenDepreciation> Search(DateRangeSearchInfo searchInfo)
        {
            int start = (searchInfo.PageIndex - 1) * searchInfo.PageSize;

            var result = new SearchResult<HenDepreciation>();

            using (var conn = factory.OpenDbConnection())
            {
                var ev = OrmLiteConfig.DialectProvider.ExpressionVisitor<Models.Data.HenDepreciation>();

                if (searchInfo.Start.HasValue && searchInfo.End.HasValue)
                {
                    ev = ev.Where(e => e.Date >= searchInfo.Start.Value.Date && e.Date <= searchInfo.End.Value.Date);
                }

                ev.OrderByDescending(e => e.Date).Limit(start, searchInfo.PageSize);

                var depreciationList = conn.Select(ev);
                foreach (var depreciationData in depreciationList)
                {
                    var list = conn.Where<Models.Data.HenDepreciationDetail>(new { DepreciationId = depreciationData.Id });

                    var depreciation = MapDataToModel(depreciationData, list);
                    result.Items.Add(depreciation);
                }

                if (searchInfo.Start.HasValue && searchInfo.End.HasValue)
                {
                    result.Total = (int)conn.Count<Models.Data.HenDepreciation>(
                        e => e.Date >= searchInfo.Start.Value.Date && e.Date <= searchInfo.End.Value.Date);
                }
                else
                {
                    result.Total = (int)conn.Count<Models.Data.HenDepreciation>();
                }
            }

            return result;
        }

        public HenDepreciation Get(Guid id)
        {
            using (var conn = factory.OpenDbConnection())
            {
                var data = conn.GetById<Models.Data.HenDepreciation>(id.ToString());

                if (data == null)
                    return null;

                var details = conn.Where<Models.Data.HenDepreciationDetail>(new { DepreciationId = data.Id });

                return MapDataToModel(data, details);
            }
        }

        public HenDepreciation GetByDate(DateTime date)
        {
            using (var conn = factory.OpenDbConnection())
            {
                var depreciationData = conn.FirstOrDefault<Models.Data.HenDepreciation>(c => c.Date == date.Date);

                if (depreciationData == null)
                    return null;

                var details = conn.Where<Models.Data.HenDepreciationDetail>(new { DepreciationId = depreciationData.Id });

                return MapDataToModel(depreciationData, details);
            }
        }

        public void Save(HenDepreciation depreciation)
        {
            using (var db = factory.OpenDbConnection())
            {
                using (var tx = db.OpenTransaction())
                {
                    bool isNew = depreciation.IsNew;

                    Models.Data.HenDepreciation depreciationData = null;

                    if (isNew)
                    {
                        depreciationData = db.FirstOrDefault<Models.Data.HenDepreciation>(u => u.Date == depreciation.Date);
                    }
                    else
                    {
                        depreciationData = db.Query<Models.Data.HenDepreciation>("Date = @Date and Id <> @Id",
                                                                      new { Date = depreciation.Date, Id = depreciation.Id.ToString() })
                                     .FirstOrDefault();
                    }
                    if (depreciationData != null)
                    {
                        tx.Rollback();
                        throw new ServiceException("HenDepreciation_DuplicateDate");
                    }

                    if (isNew)
                        depreciation.Id = Guid.NewGuid();

                    depreciationData = Mapper.Map<HenDepreciation, Models.Data.HenDepreciation>(depreciation);

                    if (isNew) db.InsertParam(depreciationData); else db.UpdateParam(depreciationData);

                    if (!isNew)
                        db.Delete<Models.Data.HenDepreciationDetail>(where: "DepreciationId = {0}".Params(depreciationData.Id.ToString()));

                    foreach (var detail in depreciation.Details)
                    {
                        var detailData = Mapper.Map<HenDepreciationDetail, Models.Data.HenDepreciationDetail>(detail);
                        detailData.DepreciationId = depreciation.Id;
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
                    db.Delete<Models.Data.HenDepreciationDetail>(where: "DepreciationId = {0}".Params(id.ToString()));
                    db.DeleteByIdParam<Models.Data.HenDepreciation>(id.ToString());
                    trans.Commit();
                }
            }
        }

        private HenDepreciation MapDataToModel(Models.Data.HenDepreciation depreciationData,
           IList<Models.Data.HenDepreciationDetail> detailsData)
        {
            var depreciation = Mapper.Map<Models.Data.HenDepreciation, HenDepreciation>(depreciationData);

            if (depreciation != null)
            {
                foreach (var detailData in detailsData)
                {
                    var detail = Mapper.Map<Models.Data.HenDepreciationDetail, HenDepreciationDetail>(detailData);
                    depreciation.Details.Add(detail);
                }
            }

            return depreciation;
        }


        public HenDepreciation GetInitialValues(DateTime date)
        {
            var depreciation = new HenDepreciation{Date = date.Date};

            string totalFeedQuery = @"SELECT SUM(ConsumableUsageDetail.Count) 
FROM ConsumableUsageDetail JOIN ConsumableUsage ON  ConsumableUsageDetail.UsageId = ConsumableUsage.Id
JOIN Consumable ON ConsumableUsagedetail.ConsumableId = Consumable.Id WHERE  ConsumableUsageDetail.HouseId=@houseId AND ConsumableUsage.Date=@date
AND Consumable.Type=0";

            string retailQuantityQuery =
                @"SELECT RetailQuantity FROM EggProductionDetail JOIN EggProduction ON EggProductionDetail.ProductionId = EggProduction.Id 
WHERE EggProductionDetail.HouseId=@houseId AND EggProduction.Date=@date";

            using (var db = factory.OpenDbConnection())
            {
                var houses = db.Select<HenHouse>(h => h.Active).OrderBy(h => h.Name).ToList();
                
                foreach (var house in houses)
                {
                    var detail = new HenDepreciationDetail();
                    detail.HouseId = house.Id;

                    var hens = db.Where<Hen>(new{HouseId = house.Id, Active=true}).ToList();
                    var cost = hens.Sum(h => h.Count * h.Cost);

                    detail.InitialPrice = cost;

                    if (house.ProductiveAge > 0)
                    {
                         int totalFeed = 0;

                        var command = db.CreateCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = totalFeedQuery;
                        command.Parameters.Add(new MySqlParameter("@houseId", MySqlDbType.Guid) { Value = house.Id });
                        command.Parameters.Add(new MySqlParameter("@date", MySqlDbType.DateTime) { Value = date });

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                            totalFeed = Convert.ToInt32(result);

                        if (totalFeed > 0 )
                        {
                            command = db.CreateCommand();
                            command.CommandType = CommandType.Text;
                            command.CommandText = retailQuantityQuery;
                            command.Parameters.Add(new MySqlParameter("@houseId", MySqlDbType.Guid) { Value = house.Id });
                            command.Parameters.Add(new MySqlParameter("@date", MySqlDbType.DateTime) { Value = date });

                            result = command.ExecuteScalar();

                            decimal retailQuantity = 0;

                            if (result != null && result != DBNull.Value)
                                retailQuantity = Convert.ToDecimal(result);

                            if (retailQuantity > 0)
                            {
                               
                                detail.Depreciation = totalFeed/retailQuantity/house.ProductiveAge;
                            }
                        }
                    }
                    else
                    {
                        detail.SellingPrice = 0;
                    }

                    depreciation.Details.Add(detail);
                }
            }

            return depreciation;
        }
    }
}
