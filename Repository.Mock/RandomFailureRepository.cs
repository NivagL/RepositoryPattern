//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Northpower.Utils;
//using Northpower.Test.Utils;
//using Northpower.Model.Meta;
//using System.Linq;

//namespace Northpower.Repository.Mock
//{
//    public class RandomFailureRepository<TKey, TValue> : LoggingComponent<RandomFailureRepository<TKey, TValue>>,
//        IModelRepository<TKey, TValue> where TValue : class //, IAssignable<TValue>
//    {
//        public IModelRepository<TKey, TValue> Repository { get; private set; }
//        public IAvailabilityGenerator AvailabilityTimer { get; private set; }
//        public IRandomSuccess RandomSuccess { get; private set; }
//        public Exception Exception { get; protected set; }
//        public Func<TValue, TKey> KeyFunc { get { return Repository.KeyFunc; } }

//        public void OnAvailabilityChange(AvailabilityEnum availability)
//        {
//        }

//        public RandomFailureRepository(IConfiguration configuration, ILoggerFactory loggerFactory,
//            IModelRepository<TKey, TValue> repository, IAvailabilityGenerator availablilityTimer, 
//            IRandomSuccess randomSuccess, Exception exception)
//            : base(configuration, loggerFactory)
//        {
//            Repository = repository;
//            AvailabilityTimer = availablilityTimer;
//            RandomSuccess = randomSuccess;
//            Exception = exception;
//        }

//        public bool Any(Expression<Func<TValue, bool>> expression,
//            LoadFlags loadFlags = LoadFlags.All)
//        {
//            return Repository.Any(expression, loadFlags);
//        }

//        public bool Any()
//        {
//            if (RandomSuccess.Success(AvailabilityTimer.Availability))
//            {
//                return Repository.Any();
//            }
//            else
//            {
//                throw Exception;
//            }
//        }

//        public TValue Load(TKey key, LoadFlags loadFlags = LoadFlags.All)
//        {
//            if (RandomSuccess.Success(AvailabilityTimer.Availability))
//            {
//                return Repository.Load(key);
//            }
//            else
//            {
//                throw Exception;
//            }
//        }

//        public IDictionary<TKey, TValue> LoadAll(LoadFlags loadFlags = LoadFlags.All)
//        {
//            if (RandomSuccess.Success(AvailabilityTimer.Availability))
//            {
//                return Repository.LoadAll();
//            }
//            else
//            {
//                throw Exception;
//            }
//        }

//        public IDictionary<TKey, TValue> GetQuery(Expression<Func<TValue, bool>> expression,
//            LoadFlags loadFlags = LoadFlags.All)
//        {
//            if (RandomSuccess.Success(AvailabilityTimer.Availability))
//            {
//                return Repository.GetQuery(expression);
//            }
//            else
//            {
//                throw Exception;
//            }
//        }

//        public Tuple<TKey, Change> Save(TValue value, bool replace = true)
//        {
//            if (RandomSuccess.Success(AvailabilityTimer.Availability))
//            {
//                return Repository.Save(value, replace);
//            }
//            else
//            {
//                throw Exception;
//            }
//        }

//        public IEnumerable<Tuple<TKey, Change>> SaveAll(IEnumerable<TValue> values)
//        {
//            if (RandomSuccess.Success(AvailabilityTimer.Availability))
//            {
//                return Repository.SaveAll(values);
//            }
//            else
//            {
//                throw Exception;
//            }
//        }

//        public TValue Delete(TKey key)
//        {
//            if (RandomSuccess.Success(AvailabilityTimer.Availability))
//            {
//                return Repository.Delete(key);
//            }
//            else
//            {
//                throw Exception;
//            }
//        }

//        public int DeleteAll(string tableName)
//        {
//            if (RandomSuccess.Success(AvailabilityTimer.Availability))
//            {
//                return Repository.DeleteAll(tableName);
//            }
//            else
//            {
//                throw Exception;
//            }
//        }

//        public IDictionary<TKey, TValue> DeleteQuery(Expression<Func<TValue, bool>> predicate)
//        {
//            var data = GetQuery(predicate);
//            if (RandomSuccess.Success(AvailabilityTimer.Availability))
//            {
//                Repository.DeleteQuery(predicate);
//            }
//            else
//            {
//                throw Exception;
//            }
//            return data;
//        }

//        public bool IsServiceConnected()
//        {
//            //TODO **GL** make this random?
//            return true;
//        }

//        public IEnumerable<TResult> QueryDistinct<TResult>(Expression<Func<TValue, bool>> expression,
//            IMapping<TValue, TResult> mapper)
//        {
//            var queryable = Repository.LoadAll().Values.AsQueryable();

//            var distinct = queryable.Where(expression)
//                .Select(x => mapper.MapOne(x)).Distinct();

//            return distinct;
//        }
//    }
//}
