using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatronicUwpLib.Dto
{
    public class HeatronicDTO
    {
        public HeatronicDTO()
        {
            this.Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; set; }
    }
}
