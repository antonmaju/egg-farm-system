using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EggFarmSystem.Models;
using ServiceStack.OrmLite;

namespace EggFarmSystem.Services
{
    public interface IEggProductionService
    {
        SearchResult<EggProduction> Search(DateRangeSearchInfo searchInfo);

        EggProduction Get(Guid id);

        EggProduction GetByDate(DateTime date);

        void Save(EggProduction production);

        void Delete(Guid id);
    }

    public class EggProductionService : IEggProductionService
    {
        private readonly IDbConnectionFactory factory;

        public EggProductionService(IDbConnectionFactory factory)
        {
            this.factory = factory;
        }

        public SearchResult<EggProduction> Search(DateRangeSearchInfo searchInfo)
        {
            int start = (searchInfo.PageIndex - 1) * searchInfo.PageSize;

            var result = new SearchResult<EggProduction>();

            using (var conn = factory.OpenDbConnection())
            {
                var ev = OrmLiteConfig.DialectProvider.ExpressionVisitor<Models.Data.EggProduction>();

                if (searchInfo.Start.HasValue && searchInfo.End.HasValue)
                {
                    ev = ev.Where(e => e.Date >= searchInfo.Start.Value.Date && e.Date <= searchInfo.End.Value.Date);
                }

                ev.OrderByDescending(e => e.Date).Limit(start, searchInfo.PageSize);

                var productionList = conn.Select(ev);
                foreach (var productionData in productionList)
                {
                    var list = conn.Where<Models.Data.EggProductionDetail>(new { ProductionId = productionData.Id });

                    var production = MapDataToModel(productionData, list);
                    result.Items.Add(production);
                }

                if (searchInfo.Start.HasValue && searchInfo.End.HasValue)
                {
                    result.Total = (int)conn.Count<Models.Data.EggProduction>(
                        e => e.Date >= searchInfo.Start.Value.Date && e.Date <= searchInfo.End.Value.Date);
                }
                else
                {
                    result.Total = (int)conn.Count<Models.Data.EggProduction>();
                }
            }

            return result;
        }

        public EggProduction Get(Guid id)
        {
            using (var conn = factory.OpenDbConnection())
            {
                var data = conn.GetById<Models.Data.EggProduction>(id.ToString());

                if (data == null)
                    return null;

                var details = conn.Where<Models.Data.EggProductionDetail>(new { ProductionId = data.Id });

                return MapDataToModel(data, details);
            }
        }

        public EggProduction GetByDate(DateTime date)
        {
            using (var conn = factory.OpenDbConnection())
            {
                var productionData = conn.FirstOrDefault<Models.Data.EggProduction>(c => c.Date == date.Date);

                if (productionData == null)
                    return null;

                var details = conn.Where<Models.Data.EggProductionDetail>(new { ProductionId = productionData.Id });

                return MapDataToModel(productionData, details);
            }
        }

        public void Save(EggProduction production)
        {
            using (var db = factory.OpenDbConnection())
            {
                using (var tx = db.OpenTransaction())
                {
                    bool isNew = production.IsNew;

                    Models.Data.EggProduction productionData = null;

                    if (isNew)
                    {
                        productionData = db.FirstOrDefault<Models.Data.EggProduction>(u => u.Date == production.Date);
                    }
                    else
                    {
                        productionData = db.Query<Models.Data.EggProduction>("Date = @Date and Id <> @Id",
                                                                      new { Date = production.Date, Id = production.Id.ToString() })
                                     .FirstOrDefault();
                    }
                    if (productionData != null)
                    {
                        tx.Rollback();
                        throw new ServiceException("EggProduction_DuplicateDate");
                    }

                    if (isNew)
                        production.Id = Guid.NewGuid();

                    productionData = Mapper.Map<EggProduction, Models.Data.EggProduction>(production);

                    if (isNew) db.InsertParam(productionData); else db.UpdateParam(productionData);

                    if (!isNew)
                        db.Delete<Models.Data.EggProductionDetail>(where: "ProductionId = {0}".Params(productionData.Id.ToString()));

                    foreach (var detail in production.Details)
                    {
                        var detailData = Mapper.Map<EggProductionDetail, Models.Data.EggProductionDetail>(detail);
                        detailData.ProductionId = production.Id;
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
                    db.Delete<Models.Data.EggProductionDetail>(where: "ProductionId = {0}".Params(id.ToString()));
                    db.DeleteByIdParam<Models.Data.EggProduction>(id.ToString());
                    trans.Commit();
                }
            }
        }

        private EggProduction MapDataToModel(Models.Data.EggProduction eggData,
            IList<Models.Data.EggProductionDetail> detailsData)
        {
            var production = Mapper.Map<Models.Data.EggProduction, EggProduction>(eggData);

            if (production != null)
            {
                foreach (var detailData in detailsData)
                {
                    var detail = Mapper.Map<Models.Data.EggProductionDetail, EggProductionDetail>(detailData);
                    production.Details.Add(detail);
                }
            }

            return production;
        }
    }
}
