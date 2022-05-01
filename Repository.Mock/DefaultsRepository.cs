using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

//Mocks the async interface so...
#pragma warning disable CS1998
namespace Northpower.Repository.Mock
{
    public partial class Repository<TKey, TValue> 
        : LoggingComponent<DefaultsRepository<TKey, TValue>>
        , IModelRepositoryAsync<TKey, TValue>
        , IModelRepository<TKey, TValue>
        , IRepositoryPagedQuery<TKey, TValue>
        where TValue : class
    {
        IModel<TKey, TValue> Model { get; set; }
        public ICollection<TValue> Defaults { get; set; }
        public Func<TValue, TKey> KeyFunc { get { return Model.Key; } }

        public DefaultsRepository(IConfiguration configuration, ILoggerFactory loggerFactory,
            IModel<TKey, TValue> model, ICollection<TValue> defaults //the data is just an in memory collection
            )
            : base(configuration, loggerFactory)
        {
            Defaults = defaults;
            Model = model;
        }

        public bool Any(Expression<Func<TValue, bool>> expression,
            LoadFlags loadFlags = LoadFlags.All)
        {
            return Defaults.AsQueryable<TValue>().Where(expression).Any();
        }

        public bool Any()
        {
            return Defaults.Any();
        }

        public bool Any(TKey key)
        {
            foreach (var item in Defaults)
            {
                if (Model.Key(item).Equals(key))
                    return true;
            }
            return false;
        }

        public TValue Delete(TKey key)
        {
            foreach (var item in Defaults)
            {
                if(Model.Key(item).Equals(key))
                {
                    var data = item;
                    Defaults.Remove(item);
                    return data;
                }
            }
            return null;
        }

        public int DeleteAll(string tableName)
        {
            var count = Defaults.Count();
            Defaults = new List<TValue>();
            return count;
        }

        public IDictionary<TKey, TValue> DeleteQuery(Expression<Func<TValue, bool>> predicate)
        {
            var removed = new Dictionary<TKey, TValue>();
            foreach (var item in Defaults)
            {
                if (predicate.Compile().Invoke(item))
                {
                    removed.Add(Model.Key(item), item);
                    
                }
            }

            foreach (var i in removed)
                Defaults.Remove(i.Value);

            return removed;
        }

        public IDictionary<TKey, TValue> GetQuery(
            Expression<Func<TValue, bool>> expression, 
            LoadFlags loadFlags = LoadFlags.All)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in Defaults)
            {
                if (expression.Compile().Invoke(item))
                {
                    dictionary.Add(Model.Key(item), item);
                }
            }
            return dictionary;
        }

        public TValue Load(TKey key, LoadFlags loadFlags = LoadFlags.None)
        {
            foreach (var item in Defaults)
            {
                if (Model.Key(item).Equals(key))
                {
                    return item;
                }
            }
            throw new DbObjectNotFoundException("Could not find items in defaults");
        }

        public IDictionary<TKey, TValue> LoadAll(LoadFlags loadFlags = LoadFlags.All)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in Defaults)
            {
                dictionary.Add(Model.Key(item), item);
            }
            return dictionary;
        }

        public Tuple<TKey, Change> Save(TValue value, bool replace = true)
        {
            if(!Model.IsKeyValid(Model.Key(value)))
                Model.SetKey(value, Model.NewKey());

            var valueKey = Model.Key(value);
            Change change = Change.NoChange;
            var found = false;
            foreach(var item in Defaults)
            {
                var itemKey = Model.Key(item);
                if (Model.KeysEqual(itemKey, valueKey))
                {
                    found = true;
                    if (Model.Differ(item, value))
                    {
                        change = Change.Updated;
                        Model.Assign(item, value);
                    }
                    break;
                }
            }
            if (!found)
            {
                Defaults.Add(value);
                change = Change.Added;
            }
            return new Tuple<TKey, Change>(valueKey, change);
        }

        public IEnumerable<Tuple<TKey, Change>> SaveAll(IEnumerable<TValue> values)
        {
            var data = new List<Tuple<TKey, Change>>();
            var repository = this as IModelRepository<TKey, TValue>;
            foreach (var item in values)
            {
                var ret = repository.Save(item);
                var key = Model.Key(item);
                data.Add(new Tuple<TKey, Change>(key, ret.Item2));
            }
            return data;
        }

        public bool IsServiceConnected()
        {
            return true;
        }

        public IEnumerable<TResult> QueryDistinct<TResult>(Expression<Func<TValue, bool>> expression,
            IMapping<TValue, TResult> mapper)
        {
            var queryable = Defaults.AsQueryable<TValue>();

            var distinct = queryable.Where(expression)
                .Select(x => mapper.MapOne(x)).Distinct();

            return distinct;
        }

        public async Task<IEnumerable<TResult>> GetDistinct<TResult>(Expression<Func<TValue, bool>> expression,
            Expression<Func<TValue, TResult>> mapper)
        {
            var queryable = Defaults.AsQueryable<TValue>();

            var distinct = queryable.Where(expression)
                .Select(x => mapper.Compile().Invoke(x)).Distinct();

            return distinct;
        }

        public async Task<IDictionary<TKey, TValue>> FromSql(string sql)
        {
            return LoadAll();
        }

        public Task<Tuple<int, Dictionary<TKey, TValue>>> LoadAll(Expression<Func<TValue, object>> orderExpression, SortOrderEnum sortOrder, PageSelection pageSelection, LoadFlags loadFlags = LoadFlags.All)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<int, Dictionary<TKey, TValue>>> GetQuery(Expression<Func<TValue, bool>> where, Expression<Func<TValue, object>> orderExpression, SortOrderEnum sortOrder, PageSelection pageSelection, LoadFlags loadFlags = LoadFlags.All)
        {
            throw new NotImplementedException();
        }
    }
}
#pragma warning restore CS1998
