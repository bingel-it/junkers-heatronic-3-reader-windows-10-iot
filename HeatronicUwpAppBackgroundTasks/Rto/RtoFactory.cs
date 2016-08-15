using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeatronicUwpLib;
using HeatronicUwpLib.Dto;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpApp.Tasks.Rto
{
    class RtoFactory
    {
        internal BaseMessageRto ConvertMessage(MessageType messageType, HeatronicDTO message)
        {
            switch (messageType)
            {
                case MessageType.Heater:
                    {
                        var msg = message as HeizgeraetDTO;
                        var restMsg = new HeizgeraetMessageRto();
                        restMsg.MessageType = MessageTypeEnum.Heater;
                        restMsg.Timestamp = DateTime.Now;
                        restMsg.BetriebsflammeAn = msg.BetriebsflammeAn;
                        restMsg.BrennerLeistung = msg.BrennerLeistung;
                        restMsg.HeizungsMode = msg.HeizungsMode;
                        restMsg.HeizungsPumpeAn = msg.HeizungsPumpeAn;
                        restMsg.ZirkulationsPumpeAn = msg.ZirkulationsPumpeAn;
                        return restMsg;
                    }
                case MessageType.Timestamp:
                    {
                        var msg = message as TimestampDTO;
                        var restMsg = new TimestampMessageRto();
                        restMsg.MessageType = MessageTypeEnum.Timestamp;
                        restMsg.Timestamp = DateTime.Now;
                        restMsg.SystemTimestamp = msg.SystemTimestamp;
                        return restMsg;
                    }
                case MessageType.HeaterCircuit:
                    {
                        var msg = message as HeizkreisDTO;
                        var restMsg = new HeizkreisMessageRto();
                        restMsg.MessageType = MessageTypeEnum.HeaterCircuit;
                        restMsg.Timestamp = DateTime.Now;
                        restMsg.Aussentemperatur = msg.Aussentemperatur;
                        return restMsg;
                    }
                case MessageType.Warmwater:
                    {
                        var msg = message as WarmWaterDTO;
                        var restMsg = new WarmwasserMessageRto();
                        restMsg.MessageType = MessageTypeEnum.Warmwater;
                        restMsg.Timestamp = DateTime.Now;
                        restMsg.IstTemperatur = msg.IstTemperatur;
                        restMsg.SollTemperatur = msg.SollTemperatur;
                        return restMsg;
                    }
                default:
                    throw new Exception("Unknown MessageType");
            }

        }
    }
}
