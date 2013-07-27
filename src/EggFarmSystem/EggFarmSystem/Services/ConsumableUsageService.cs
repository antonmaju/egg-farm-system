using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EggFarmSystem.Models;
using ServiceStack.OrmLite;


namespace EggFarmSystem.Services
{
    /// <summary>
    /// Contains collection of methods for managing consumable usage data
    /// </summary>
    public interface IConsumableUsageService
    {
        /// <summary>
        /// Searches the usage based on search criteria.
        /// </summary>
        /// <param name="searchInfo">The search criteria.</param>
        /// <returns>collection of usage data and total number.</returns>
        SearchResult<ConsumableUsage> Search(ConsumableUsageSearchInfo searchInfo);

        /// <summary>
        /// Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ConsumableUsage instance.</returns>
        ConsumableUsage Get(Guid id);

        /// <summary>
        /// Gets the usage by date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>ConsumableUsage instance.</returns>
        ConsumableUsage GetByDate(DateTime date);

        /// <summary>
        /// Saves the specified usage.
        /// </summary>
        /// <param name="usage">The usage.</param>
        /// <returns><c>true</c> if save process success, <c>false</c> otherwise</returns>
        void Save(ConsumableUsage usage);

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns><c>true</c> if delete success, <c>false</c> otherwise</returns>
        void Delete(Guid id);
    }

    /// <summary>
    /// Contains collection of methods for managing consumable usage data
    /// </summary>
    public class ConsumableUsageService : IConsumableUsageService
    {
        private readonly IDbConnectionFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumableUsageService"/> class.
        /// </summary>
        /// <param name="factory">The db connection factory.</param>
        public ConsumableUsageService(IDbConnectionFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Searches the usage based on search criteria.
        /// </summary>
        /// <param name="searchInfo">The search criteria.</param>
        /// <returns>collection of usage data and total number.</returns>
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
                    result.Total =(int) conn.Count<Models.Data.ConsumableUsage>(
                        e => e.Date >= searchInfo.Start.Value.Date && e.Date <= searchInfo.End.Value.Date);
                }
                else
                {
                    result.Total = (int)  conn.Count<Models.Data.ConsumableUsage>();
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ConsumableUsage instance.</returns>
        public ConsumableUsage Get(Guid id)
        {
            using (var conn = factory.OpenDbConnection())
            {
                var usage =  conn.GetById<Models.Data.ConsumableUsage>(id.ToString());

                if (usage == null)
                    return null;

                var list = conn.Where<Models.Data.ConsumableUsageDetail>(new {UsageId = id});
                var model = MapUsageToModel(usage, list);
                return model;
            }
        }

        /// <summary>
        /// Gets the usage by date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>ConsumableUsage instance.</returns>
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

        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if save success, <c>false</c> otherwise</returns>
        public void Save(ConsumableUsage model)
        {
            using (var db = factory.OpenDbConnection())
            {
                using (var trans = db.OpenTransaction())
                {
                     bool isNew = model.IsNew;

                    Models.Data.ConsumableUsage usage = null;

                    if (isNew)
                    {
                        usage = db.FirstOrDefault<Models.Data.ConsumableUsage>(u => u.Date == model.Date);
                    }
                    else
                    {
                        usage = db.Query<Models.Data.ConsumableUsage>("Date = @Date and Id <> @Id",
                                                                      new {Date = model.Date, Id = model.Id.ToString()})
                                  .FirstOrDefault();
                    }

                    if (usage != null)
                    {
                        trans.Rollback();
                        throw new ServiceException("Usage_DuplicateDate");
                    }
                                        

                    if (isNew)
                        model.Id = Guid.NewGuid();

                    usage = Mapper.Map<ConsumableUsage, Models.Data.ConsumableUsage>(model);
                
                    if(isNew) db.InsertParam(usage); else db.UpdateParam(usage);
                    
                    if (! isNew)
                        db.Delete<Models.Data.ConsumableUsageDetail>(where: "UsageId = {0}".Params(usage.Id.ToString()));
                    
                    foreach (var detailModel in model.Details)
                    {
                        var detail = Mapper.Map<ConsumableUsageDetail, Models.Data.ConsumableUsageDetail>(detailModel);
                        detail.UsageId = usage.Id;
                        db.InsertParam(detail);
                    }
                    try
                    {
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new ServiceException(ex.Message);
                    }
                    
                }
            }
        }

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns><c>true</c> if delete success, <c>false</c> otherwise</returns>
        public void Delete(Guid id)
        {
            using (var db = factory.OpenDbConnection())
            {
                using (var trans = db.OpenTransaction())
                {
                    db.Delete<Models.Data.ConsumableUsageDetail>(where: "UsageId = {0}".Params(id.ToString()));
                    db.DeleteByIdParam<Models.Data.ConsumableUsage>(id.ToString());
                    trans.Commit();
                }
            }
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
                        ConsumableId = detail.ConsumableId,
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
