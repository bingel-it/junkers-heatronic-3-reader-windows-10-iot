using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.Tasks.Helper
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
    
    }
}
