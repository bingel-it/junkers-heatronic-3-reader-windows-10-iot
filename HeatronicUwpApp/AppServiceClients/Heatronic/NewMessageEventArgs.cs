using BingelIT.MyHome.Heatronic.HeatronicUwpApp.AppServiceClients.Heatronic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.AppServiceClients.Heatronic
{
    class NewMessageEventArgs : EventArgs
    {
        public HeatronicDTO Message { get; set; }
        public MessageTypeEnum MessageType { get; set; }
    }
}
