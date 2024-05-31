using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public static class JsonHelper
    {
        /// <summary>
        /// Json To Object
        /// </summary>
        /// <param name="json">Json</param>
        /// <returns>Object</returns>
        public static object ToObject(this string json)
        {

            return json == null ? null : JsonConvert.DeserializeObject(json);
        }
        /// <summary>
        /// Object To Json
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Json</returns>
        public static string ToJson(this object obj)
        {

            var setting = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
            return obj==null?null:JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// Json To Object T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json)
        {
            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json);

        }
        public static T ToObject<T>(this string json,T obj)
        {
            return json == null ? default : JsonConvert.DeserializeAnonymousType(json,obj);
        }
        /// <summary>
        /// Json To List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string json)
        {
            return json == null ? null : JsonConvert.DeserializeObject<List<T>>(json);
        }
        /// <summary>
        /// Json To Table
        /// </summary>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static DataTable ToTable(this string json)
        {
            return json == null ? null : JsonConvert.DeserializeObject<DataTable>(json);
        }
    }
}
