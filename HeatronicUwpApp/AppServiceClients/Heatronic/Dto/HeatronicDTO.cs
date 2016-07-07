using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.AppServiceClients.Heatronic.Dto
{
    [DataContract]
    public class HeatronicDTO
    {
        public HeatronicDTO()
        {
            this.Timestamp = DateTime.Now;
        }

        [DataMember]
        public DateTime Timestamp { get; set; }
    }
}
