using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EggFarmSystem.Models;
using ServiceStack.OrmLite;


namespace EggFarmSystem.Services
{
    public interface IConsumableUsageService
    {
        SearchResult<ConsumableUsage> Search(ConsumableUsageSearchInfo searchInfo);

        ConsumableUsage Get(Guid id);

        ConsumableUsage GetByDate(DateTime date);

        bool Save(ConsumableUsage usage);

        bool Delete(Guid id);
    }

    public class ConsumableUsageService : IConsumableUsageService
    {
        private readonly IDbConnectionFactory factory;

        public ConsumableUsageService(IDbConnectionFactory factory)
        {
            this.factory = factory;
        }

        public SearchResult<ConsumableUsage> Search(ConsumableUsageSearchInfo searchInfo)
        {
            int start = (searchInfo.PageIndex - 1)*searchInfo.PageSize;

            var result = new SearchResult<ConsumableUsage>();

            using (var conn = factory.OpenDbConnection())
            {

                var ev = OrmLiteConfig.DialectProvider.ExpressionVisitor<Models.Data.ConsumableUsage>();
                
                if (searchInfo.Start.HasValue && searchInfo.End.HasValue)
                {
                    ev = ev.Where(e => e.Date >= searchInfo.Start.Value.Date && e.Date <= searchInfo.End.Value.Date);
                }

                ev.OrderByDescending(e => e.Date).Limit(start, searchInfo.PageSize);

                var usages = conn.Select(ev);
                foreach (var usage in usages)
                {
                    var list = conn.Where<Models.Data.ConsumableUsageDetail>(new { UsageId = usage.Id });

                    var usageModel = MapUsageToModel(usage, list);
                    result.Items.Add(usageModel);
                }

                if (searchInfo.Start.HasValue && searchInfo.End.HasValue)
                {
                    result.Total =conn.Count<Models.Data.ConsumableUsage>(
                        e => e.Date >= searchInfo.Start.Value.Date && e.Date <= searchInfo.End.Value.Date);
                }
                else
                {
                    result.Total =  conn.Count<Models.Data.ConsumableUsage>();
                }
            }

            return result;
        }

        public ConsumableUsage Get(Guid id)
        {
            using (var conn = factory.OpenDbConnection())
            {
                var usage =  conn.GetById<Models.Data.ConsumableUsage>(id);

                if (usage == null)
                    return null;

                var list = conn.Where<Models.Data.ConsumableUsageDetail>(new {UsageId = id});
                var model = MapUsageToModel(usage, list);
                return model;
            }
        }

        public ConsumableUsage GetByDate(DateTime date)
        {
            using (var conn = factory.OpenDbConnection())
            {
                var usage = conn.FirstOrDefault<Models.Data.ConsumableUsage>(u => u.Date == date.Date);

                if (usage == null)
                    return null;

                var list = conn.Where<Models.Data.ConsumableUsageDetail>(new { UsageId = usage.Id });
                var model = MapUsageToModel(usage, list);
                return model;
            }
        }

        public bool Save(ConsumableUsage model)
        {
            using (var db = factory.OpenDbConnection())
            {
                using (var trans = db.OpenTransaction())
                {
                     bool isNew = model.IsNew;
                
                    if (isNew)
                        model.Id = Guid.NewGuid();

                    var usage = Mapper.Map<ConsumableUsage, Models.Data.ConsumableUsage>(model);
                
                    if(isNew) db.InsertParam(usage); else db.UpdateParam(usage);
                    
                    if (! isNew)
                        db.Delete<Models.Data.ConsumableUsageDetail>(d => d.UsageId == usage.Id);
                    
                    foreach (var detailModel in model.Details)
                    {
                        var detail = Mapper.Map<ConsumableUsageDetail, Models.Data.ConsumableUsageDetail>(detailModel);
                        db.InsertParam(detail);
                    }

                    trans.Commit();
                    return true;
                }
            }

            return false;
        }

        public bool Delete(Guid id)
        {
            using (var db = factory.OpenDbConnection())
            {
                using (var trans = db.OpenTransaction())
                {
                    db.Delete<Models.Data.ConsumableUsageDetail>(d => d.UsageId == id);
                    db.DeleteById<Models.Data.ConsumableUsage>(id);

                    trans.Commit();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Maps the usage to model. Need to use automapper?
        /// </summary>
        /// <param name="usage">The usage.</param>
        /// <param name="details">The details.</param>
        /// <returns>EggFarmSystem.Models.ConsumableUsage.</returns>
        private ConsumableUsage MapUsageToModel(Models.Data.ConsumableUsage usage, List<Models.Data.ConsumableUsageDetail> details)
        {
            var model = new ConsumableUsage();
            model.Id = usage.Id;
            model.Date = usage.Date;
            model.Total = usage.Total;
            model.Details = new List<ConsumableUsageDetail>();
            
            foreach (var detail in details)
            {
                var detailModel = new ConsumableUsageDetail
                    {
                        ConsumableId = usage.Id,
                        Count = detail.Count,
                        HouseId = detail.HouseId,
                        SubTotal = detail.SubTotal,
                        UnitPrice = detail.UnitPrice
                    };
                model.Details.Add(detailModel);
            }

            return model;
        }
    }

   
}
