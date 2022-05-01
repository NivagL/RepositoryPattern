using Northpower.Logging;
using Northpower.Model.Meta;
using Northpower.Tools.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Northpower.Repository.Mock
{
    public partial class FileRepository<TKey, TValue> : LoggingComponent<FileRepository<TKey, TValue>>,
        IModelRepositoryAsync<TKey, TValue>, IModelRepository<TKey, TValue>
    {
        public async Task<bool> Any(Expression<Func<TValue, bool>> expression,
            LoadFlags loadFlags = LoadFlags.All)
        {
            var data = FileReader.Read()
                .AsQueryable<Tuple<TKey, TValue>>()
                .Select(x => x.Item2)
                .Where(expression).Any();

            return await Task.FromResult(data);
        }

        public async Task<bool> Any()
        {
            return await Task.FromResult(FileReader.Read().Any());
        }

        public async Task<TValue> Load(TKey key, LoadFlags loadFlags = LoadFlags.All)
        {
            return await Task.FromResult(FileReader.ReadOne(key).Item2);
        }

        public async Task<IDictionary<TKey, TValue>> LoadAll(LoadFlags loadFlags = LoadFlags.None)
        {
            var read = FileReader.Read();
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var value in read)
            {
                dictionary.Add(value.Item1, value.Item2);
            }
            return await Task.FromResult(dictionary);
        }

        public async Task<IDictionary<TKey, TValue>> GetQuery(
            Expression<Func<TValue, bool>> expression,
            LoadFlags loadFlags = LoadFlags.All)
        {
            var data = new Dictionary<TKey, TValue>();
            var values = FileReader.Read();
            foreach (var value in values)
            {
                if (expression.Compile().Invoke(value.Item2))
                {
                    data.Add(value.Item1, value.Item2);
                }
            }
            return await Task.FromResult(data);
        }

        public async Task<Tuple<TKey, Change>> Save(TValue value, bool replace = true)
        {
            var key = KeyFunc(value);
            var read = FileReader.ReadOne(key);

            var change = Change.Added;
            if (read != null)
            {
                change = Change.Updated;
            }

            if (read == null || (read != null && replace))
            {
                key = FileWriter.Write(value);
            }
            return await Task.FromResult(new Tuple<TKey, Change>(key, change));
        }

        public async Task<IEnumerable<Tuple<TKey, Change>>> SaveAll(IEnumerable<TValue> values)
        {
            var ret = new List<Tuple<TKey, Change>>();
            foreach (var item in values)
            {
                var save = await Save(item);
                ret.Add(save);
            }
            return ret;
        }

        public async Task<TValue> Delete(TKey key)
        {
            return await Task.FromResult(FileDeleter.Delete(key).Item2);
        }

        public int DeleteAll(string tableName)
        {
            return FileDeleter.DeleteAll();
        }

        public async Task<IDictionary<TKey, TValue>> DeleteQuery(Expression<Func<TValue, bool>> expression)
        {
            var data = new Dictionary<TKey, TValue>();
            var values = FileReader.Read();
            foreach (var value in values)
            {
                if (expression.Compile().Invoke(value.Item2))
                {
                    FileDeleter.Delete(value.Item1);
                }
            }
            return await Task.FromResult(data);
        }
        public async Task<IEnumerable<TResult>> QueryDistinct<TResult>(Expression<Func<TValue, bool>> expression,
            IMapping<TValue, TResult> mapper)
        {
            var queryable = FileReader.Read().Select(x => x.Item2).AsQueryable();

            var distinct = queryable.Where(expression)
                .Select(x => mapper.MapOne(x)).Distinct();

            return await Task.FromResult(distinct);
        }
    }
}
