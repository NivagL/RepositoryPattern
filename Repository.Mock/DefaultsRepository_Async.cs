using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Northpower.Repository.Mock
{
    public partial class DefaultsRepository<TKey, TValue> 
        : LoggingComponent<DefaultsRepository<TKey, TValue>>
        , IModelRepositoryAsync<TKey, TValue>
        , IModelRepository<TKey, TValue>
        where TValue : class
    {
        async Task<bool> IModelRepositoryAsync<TKey, TValue>.Any(Expression<Func<TValue, bool>> expression,
            LoadFlags loadFlags)
        {
            return await Task.FromResult(Defaults.AsQueryable<TValue>().Where(expression).Any());
        }

        async Task<bool> IModelRepositoryAsync<TKey, TValue>.Any()
        {
            return await Task.FromResult(Defaults.Any());
        }

        async Task<TValue> IModelRepositoryAsync<TKey, TValue>.Delete(TKey key)
        {
            foreach (var item in Defaults)
            {
                if (Model.Key(item).Equals(key))
                {
                    var data = item;
                    Defaults.Remove(item);
                    return await Task.FromResult(data);
                }
            }
            return null;
        }

        int IModelRepositoryAsync<TKey, TValue>.DeleteAll(string tableName)
        {
            var count = Defaults.Count();
            Defaults = new List<TValue>();
            return count;
        }

        async Task<IDictionary<TKey, TValue>> IModelRepositoryAsync<TKey, TValue>.DeleteQuery(Expression<Func<TValue, bool>> predicate)
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

            return await Task.FromResult(removed);
        }

        async Task<IDictionary<TKey, TValue>> IModelRepositoryAsync<TKey, TValue>.GetQuery(
            Expression<Func<TValue, bool>> expression,
            LoadFlags loadFlags)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in Defaults)
            {
                if (expression.Compile().Invoke(item))
                {
                    dictionary.Add(Model.Key(item), item);
                }
            }
            return await Task.FromResult(dictionary);
        }

        async Task<TValue> IModelRepositoryAsync<TKey, TValue>.Load(TKey key, LoadFlags loadFlags)
        {
            foreach (var item in Defaults)
            {
                if (Model.Key(item).Equals(key))
                {
                    return await Task.FromResult(item);
                }
            }
            throw new DbObjectNotFoundException("Could not find items in defaults");
        }

        async Task<IDictionary<TKey, TValue>> IModelRepositoryAsync<TKey, TValue>.LoadAll(LoadFlags loadFlags)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in Defaults)
            {
                dictionary.Add(Model.Key(item), item);
            }
            return await Task.FromResult(dictionary);
        }

        async Task<Tuple<TKey, Change>> IModelRepositoryAsync<TKey, TValue>.Save(TValue value, bool replace)
        {
            if (!Model.IsKeyValid(Model.Key(value)))
                Model.SetKey(value, Model.NewKey());

            //var valueKey = Model.Key(value);
            //Change change = Change.NoChange;
            //foreach (var item in Defaults)
            //{
            //    var itemKey = Model.Key(item);
            //    if (Model.Equal(itemKey, valueKey))
            //    {
            //        Model.Assign(item, value);
            //        change = Change.Updated;
            //    }
            //}
            //if (change == Change.NoChange)
            //{
            //    Defaults.Add(value);
            //    change = Change.Added;
            //}
            //return await Task.FromResult(new Tuple<TKey, Change>(valueKey, change));
            var valueKey = Model.Key(value);
            Change change = Change.NoChange;
            var found = false;
            foreach (var item in Defaults)
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
            //return new Tuple<TKey, Change>(valueKey, change);
            return await Task.FromResult(new Tuple<TKey, Change>(valueKey, change));
        }

        async Task<IEnumerable<Tuple<TKey, Change>>> IModelRepositoryAsync<TKey, TValue>.SaveAll(IEnumerable<TValue> values)
        {
            var data = new List<Tuple<TKey, Change>>();
            var repository = this as IModelRepositoryAsync<TKey, TValue>;
            foreach (var item in values)
            {

                var ret = await repository.Save(item);
                var key = Model.Key(item);
                data.Add(new Tuple<TKey, Change>(key, ret.Item2));
            }
            return data;
        }

        async Task<IEnumerable<TResult>> IModelRepositoryAsync<TKey, TValue>.QueryDistinct<TResult>(Expression<Func<TValue, bool>> expression,
            IMapping<TValue, TResult> mapper)
        {
            var queryable = Defaults.AsQueryable<TValue>();

            var distinct = queryable.Where(expression)
                .Select(x => mapper.MapOne(x)).Distinct();

            return await Task.FromResult(distinct);
        }
    }
}
