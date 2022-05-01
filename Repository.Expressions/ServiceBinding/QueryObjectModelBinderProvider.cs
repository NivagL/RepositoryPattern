//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using System.Collections.Generic;

//namespace Repository.Model
//{
//    public class QueryObjectModelBinderProvider : IModelBinderProvider
//    {
//        public IModelBinder GetBinder(ModelBinderProviderContext context)
//        {
//            if (context.Metadata.ModelType == typeof(IEnumerable<QueryObject>))
//                return new QueryObjectModelBinder();

//            if (context.Metadata.ModelType == typeof(List<QueryObject>))
//                return new QueryObjectModelBinder();

//            return null;
//        }
//    }
//}
