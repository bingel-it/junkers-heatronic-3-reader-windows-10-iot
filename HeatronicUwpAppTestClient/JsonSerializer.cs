using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpAppTestClient
{
    public static class JSONSerializer
    {
        /// <summary>
        /// Serializes an object to JSON
        /// </summary>
        public static string SerializeObject(Object instance)
        {
            var serializer = new DataContractJsonSerializer(instance.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, instance);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// DeSerializes an object from JSON
        /// </summary>
        public static T DeSerialize<T>(string json) where T : class
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return serializer.ReadObject(stream) as T;
            }
        }
    }
}
