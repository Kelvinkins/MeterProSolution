using MeterPro.DATA.DataContexts;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MeterPro.DATA.DAL
{

    public class MeterProRepository<TEntity> where TEntity : class
    {
        protected static MeterProDataContext _context;
        protected static IMongoCollection<TEntity> DbSet;
        //protected static  IMongoCollection<BsonDocument> DbSetRaw;

        public MeterProRepository(MeterProDataContext context)
        {
            _context = context;
          
                DbSet = _context.GetCollection<TEntity>($"{typeof(TEntity).Name}s");
            
            //DbSetRaw = _context.GetCollection<BsonDocument>("AuthNumModels");

        }

        public virtual Task Add(TEntity obj)
        {
            return _context.AddCommand(async () => await DbSet.InsertOneAsync(obj));
        }

        public virtual Task AddBulk(IEnumerable<TEntity> obj)
        {
            return _context.AddCommand(async () => await DbSet.InsertManyAsync(obj));
        }



        public virtual async Task<TEntity> GetById(ObjectId id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq(" _id ", id));
            return data.FirstOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll(FilterDefinition<TEntity> filter)
        {

            var all = await DbSet.FindAsync(filter);
            return all.ToList();
        }




        //public virtual async Task<IEnumerable<BsonDocument>> GetAllRaw(FilterDefinition<BsonDocument> filter)
        //{

        //    var all = await DbSetRaw.FindAsync(filter);
        //    return all.ToList();
        //}

        public virtual async Task<IEnumerable<TEntity>> GetAllPaged(FilterDefinition<TEntity> filter, int limit = 50, int offset = 1)
        {

            var data = await GetPagerAsync(offset, limit, DbSet, filter);
            return data;
        }


        public virtual async Task<Pager<TEntity>> GetPaged(FilterDefinition<TEntity> filter, int page = 1, int pageSize = 50)
        {
            var data = await GetPagerResultAsync(page, pageSize, DbSet, filter);
            return data;
        }


        public virtual async Task<long> GetCount(FilterDefinition<TEntity> filter)
        {
            var data = await GetCountResult(DbSet, filter);
            return data;
        }

        private static async Task<Pager<TEntity>> GetPagerResultAsync(int page, int pageSize, IMongoCollection<TEntity> collection, FilterDefinition<TEntity> filter)
        {
            // count facet, aggregation stage of count
            var countFacet = AggregateFacet.Create("countFacet",
                PipelineDefinition<TEntity, AggregateCountResult>.Create(new[]
                {
                PipelineStageDefinitionBuilder.Count<TEntity>()
                }));

            var dataFacet = AggregateFacet.Create("dataFacet",
                PipelineDefinition<TEntity, TEntity>.Create(new[]
                {
                PipelineStageDefinitionBuilder.Skip<TEntity>((page - 1) * pageSize),
                PipelineStageDefinitionBuilder.Limit<TEntity>(pageSize),
                }));

            var aggregation = await collection.Aggregate()
                .Match(filter)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "countFacet")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            var data = aggregation.First()
                .Facets.First(x => x.Name == "dataFacet")
                .Output<TEntity>();

            return new Pager<TEntity>()
            {
                Count = (int)count / pageSize,
                Size = pageSize,
                Page = page,
                Items = data
            };
        }


        private static async Task<IEnumerable<TEntity>> GetPagerAsync(int page, int pageSize, IMongoCollection<TEntity> collection, FilterDefinition<TEntity> filter)
        {


            var dataFacet = AggregateFacet.Create("dataFacet",
                PipelineDefinition<TEntity, TEntity>.Create(new[]
                {
                PipelineStageDefinitionBuilder.Skip<TEntity>((page - 1) * pageSize),
                PipelineStageDefinitionBuilder.Limit<TEntity>(pageSize),
                }));

            var aggregation = await collection.Aggregate()
                .Match(filter)
                .Facet(dataFacet)
                .ToListAsync();

            var data = aggregation.First()
                .Facets.First(x => x.Name == "dataFacet")
                .Output<TEntity>();
            return data;


        }





        private static async Task<long> GetCountResult(IMongoCollection<TEntity> collection, FilterDefinition<TEntity> filter)
        {
            // count facet, aggregation stage of count
            var countFacet = AggregateFacet.Create("countFacet",
                PipelineDefinition<TEntity, AggregateCountResult>.Create(new[]
                {
                PipelineStageDefinitionBuilder.Count<TEntity>()
                }));


            //var filter = Builders<TEntity>.Filter.Empty;
            var aggregation = await collection.Aggregate()
                .Match(filter)
                .Facet(countFacet)
                .ToListAsync();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "countFacet")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            return count;
        }




        //public virtual Task Update(UpdateDefinition<TEntity> obj, string rrn)
        //{

        //    return _context.AddCommand(async () =>
        //    {
        //        await DbSet.UpdateManyAsync(Builders<TEntity>.Filter.Eq("RRN", rrn), obj);

        //    });
        //}

        //public virtual Task ServiceUpdate(UpdateDefinition<TEntity> obj, FilterDefinition<TEntity> filter)
        //{
        //    var DDd = obj.GetId();

        //    return _context.AddCommand(async () =>
        //    {
        //        await DbSet.UpdateManyAsync(filter, obj);

        //    });
        //}

        //public virtual Task Remove(TEntity data) => _context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq(" _id ", data.GetId())));

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }


    public class Pager<TEntity> where TEntity : class
    {
        public int Count { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }

        public IEnumerable<TEntity> Items { get; set; }
    }
}
