//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Repository.Model
//{
//    public class QueryObjectModelBinder : IModelBinder
//    {
//        public Task BindModelAsync(ModelBindingContext bindingContext)
//        {
//            var serialiser = new JsonSerialiser<QueryObject>();

//            //var jsonString = bindingContext.ActionContext.HttpContext.Request.Query["query"];
//            var query = bindingContext.ActionContext.HttpContext.Request.Query.First();
//            var values = query.Value;
//            var jsonString = string.Format("[{0}]", string.Join(",", values));

//            if (String.IsNullOrEmpty(jsonString))
//            {
//                ModelBindingResult.Success(null);
//            }
//            else
//            {
//                var queryObjects = serialiser.CreateObjects(jsonString);

//                // need to convert the name used on client side from camel to pascal case
//                foreach (var item in queryObjects)
//                {
//                    item.PropertyName = ConvertPropertyNameToPascal(item.PropertyName);
//                }

//                bindingContext.Result = ModelBindingResult.Success(queryObjects);
//            }

//            return Task.CompletedTask;
//        }

//        private static string ConvertPropertyNameToPascal(string propertyName)
//        {
//            if (string.IsNullOrEmpty(propertyName))
//            {
//                return string.Empty;
//            }

//            return char.ToUpper(propertyName[0]) + propertyName.Substring(1);
//        }
//    }
//}
