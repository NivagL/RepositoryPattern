using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Northpower.Files;
using Northpower.Logging;
using Northpower.Model.Meta;
using Northpower.Tools.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

//Mocks the async interface so...
#pragma warning disable CS1998
namespace Northpower.Repository.Mock
{
    public partial class FileRepository<TKey, TValue> : LoggingComponent<FileRepository<TKey, TValue>>,
        IModelRepositoryAsync<TKey, TValue>, IModelRepository<TKey, TValue>
    {
        public IFileReader<TKey, TValue> FileReader { get; protected set; }
        public IFileWriter<TKey, TValue> FileWriter { get; protected set; }
        public IFileDeleter<TKey, TValue> FileDeleter { get; protected set; }
        public IModel<TKey, TValue> Model { get; set; }
        public Func<TValue, TKey> KeyFunc { get { return Model.Key; } }

        public FileRepository(IConfiguration configuration, ILoggerFactory loggerFactory,
            IModel<TKey, TValue> model,
            //the data is in a set of files
            IFileReader<TKey, TValue> fileReader,
            IFileWriter<TKey, TValue> fileWriter,
            IFileDeleter<TKey, TValue> fileDeleter
            )
            : base(configuration, loggerFactory)
        {
            Model = model;
            FileReader = fileReader;
            FileWriter = fileWriter;
            FileDeleter = fileDeleter;
        }

        bool IModelRepository<TKey, TValue>.Any(Expression<Func<TValue, bool>> expression,
            LoadFlags loadFlags)
        {
            return FileReader.Read()
                .AsQueryable<Tuple<TKey, TValue>>()
                .Select(x => x.Item2)
                .Where(expression).Any();
        }

        bool IModelRepository<TKey, TValue>.Any()
        {
            return FileReader.Read().Any();
        }

        TValue IModelRepository<TKey, TValue>.Load(TKey key, LoadFlags loadFlags)
        {
            return FileReader.ReadOne(key).Item2;
        }

        IDictionary<TKey, TValue> IModelRepository<TKey, TValue>.LoadAll(LoadFlags loadFlags)
        {
            var read = FileReader.Read();
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var value in read)
            {
                dictionary.Add(value.Item1, value.Item2);
            }
            return dictionary;
        }

        IDictionary<TKey, TValue> IModelRepository<TKey, TValue>.GetQuery(
            Expression<Func<TValue, bool>> expression, 
            LoadFlags loadFlags)
        {
            var data = new Dictionary<TKey, TValue>();
            var values = FileReader.Read();
            foreach(var value in values)
            {
                if(expression.Compile().Invoke(value.Item2))
                {
                    data.Add(value.Item1, value.Item2);
                }
            }
            return data;
        }

        Tuple<TKey, Change> IModelRepository<TKey, TValue>.Save(TValue value, bool replace)
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
            return new Tuple<TKey, Change>(key, change);
        }

        IEnumerable<Tuple<TKey, Change>> IModelRepository<TKey, TValue>.SaveAll(IEnumerable<TValue> values)
        {
            var ret = new List<Tuple<TKey, Change>>();
            var repository = this as IModelRepository<TKey, TValue>;
            foreach (var item in values)
            {
                ret.Add(repository.Save(item));
            }
            return ret;
        }

        TValue IModelRepository<TKey, TValue>.Delete(TKey key)
        {
            return FileDeleter.Delete(key).Item2;
        }

        int IModelRepository<TKey, TValue>.DeleteAll(string tableName)
        {
            return FileDeleter.DeleteAll();
        }

        IDictionary<TKey, TValue> IModelRepository<TKey, TValue>.DeleteQuery(Expression<Func<TValue, bool>> expression)
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
            return data;
        }

        public bool IsServiceConnected()
        {
            return true;
        }

        IEnumerable<TResult> IModelRepository<TKey, TValue>.QueryDistinct<TResult>(Expression<Func<TValue, bool>> expression,
            IMapping<TValue, TResult> mapper)
        {
            var queryable = FileReader.Read().Select(x => x.Item2).AsQueryable();

            var distinct = queryable.Where(expression)
                .Select(x => mapper.MapOne(x)).Distinct();

            return distinct;
        }

        async Task<IEnumerable<TResult>> IModelRepositoryAsync<TKey, TValue>.GetDistinct<TResult>(Expression<Func<TValue, bool>> expression,
            Expression<Func<TValue, TResult>> mapper)
        {
            var queryable = FileReader.Read().Select(x => x.Item2).AsQueryable();

            var distinct = queryable.Where(expression)
                .Select(x => mapper.Compile().Invoke(x)).Distinct();

            return distinct;
        }
        public async Task<IDictionary<TKey, TValue>> FromSql(string sql)
        {
            return await LoadAll();
        }
    }
}
#pragma warning restore CS1998
