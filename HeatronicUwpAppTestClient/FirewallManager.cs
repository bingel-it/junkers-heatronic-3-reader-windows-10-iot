using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NATUPNPLib;
using NetFwTypeLib;
using NETCONLib;

namespace BingelIT.MyHome.Heatronic.HeatronicUwpAppTestClient
{
    class FirewallManager
    {

        /// <summary>
        /// The protocols.
        /// </summary>
        public enum Protocol
        {
            /// <summary>
            /// The TCP protocol.
            /// </summary>
            Tcp,

            /// <summary>
            /// The UDP protocol
            /// </summary>
            Udp
        }


        private static string CLSID_FIREWALL_MANAGER = "{304CE942-6E39-40D8-943A-B913C40C9CD4}";
        private static string PROGID_AUTHORIZED_APPLICATION = "HNetCfg.FwAuthorizedApplication";
        private static string PROGID_OPEN_PORT = "HNetCfg.FWOpenPort";
        private static string PROGID_FW_RULE = "HNetCfg.FWRule";
        private static string PROGID_FW_POLICY2 = "HNetCfg.FwPolicy2";

        private static INetFwMgr GetFirewallManager()
        {
            Type objectType = Type.GetTypeFromCLSID(new Guid(CLSID_FIREWALL_MANAGER));
            return Activator.CreateInstance(objectType) as NetFwTypeLib.INetFwMgr;
        }

        /// <summary>
        /// Public Firewall is Enabled
        /// </summary>
        public static bool FirewallEnabled
        {
            get
            {
                INetFwMgr manager = GetFirewallManager();
                return manager.LocalPolicy.CurrentProfile.FirewallEnabled;
            }
            set {
                if (FirewallEnabled == false)
                {
                    INetFwMgr manager = GetFirewallManager();
                    manager.LocalPolicy.CurrentProfile.FirewallEnabled = true;
                }
            }
        }

        public void AuthorizeApplication(string title, string applicationPath, 
            NET_FW_SCOPE_ scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL, 
            NET_FW_IP_VERSION_ ipVersion = NET_FW_IP_VERSION_.NET_FW_IP_VERSION_ANY)
        {
            // Create the type from prog id
            Type type = Type.GetTypeFromProgID(PROGID_AUTHORIZED_APPLICATION);
            INetFwAuthorizedApplication auth = Activator.CreateInstance(type) as INetFwAuthorizedApplication;
            auth.Name = title;
            auth.ProcessImageFileName = applicationPath;
            auth.Scope = scope;
            auth.IpVersion = ipVersion;
            auth.Enabled = true;
            

            INetFwMgr manager = GetFirewallManager();
            try
            {
                manager.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(auth);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Allows a port through the firewall.
        /// </summary>
        /// <param name="name">The rule name.</param>
        /// <param name="port">The port.</param>
        /// <param name="protocol">The protocol.</param>
        public static void OpenPort(string name, int port, Protocol protocol)
        {
            if (IsPortOpened(port, protocol))
            {
                return;
            }

            var manager = GetFirewallManager();
            var rule = (INetFwOpenPort)Activator.CreateInstance(Type.GetTypeFromProgID(PROGID_OPEN_PORT));
            rule.Name = name;
            rule.Port = port;
            rule.Protocol = GetProtocol(protocol);
            rule.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
            rule.Enabled = true;
            manager.LocalPolicy.CurrentProfile.GloballyOpenPorts.Add(rule);
            if (!IsPortOpened(port, protocol))
            {
                throw new Exception("Could not open Port. Do you have administration rights?");
            }
        }

        /// <summary>
        /// Disallows traffic to a specified port and protocol.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="protocol">The protocol.</param>
        public static void ClosePort(int port, Protocol protocol)
        {
            var manager = GetFirewallManager();
            manager.LocalPolicy.CurrentProfile.GloballyOpenPorts.Remove(port, GetProtocol(protocol));
        }

        /// <summary>
        /// Returns true if the specified port is authorized or false otherwise.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="protocol">The protocol.</param>
        /// <returns>True if the specified port is authorized or false otherwise.</returns>
        public static bool IsPortOpened(int port, Protocol protocol)
        {
            var manager = GetFirewallManager();
            return
                manager.LocalPolicy.CurrentProfile.GloballyOpenPorts.Cast<INetFwOpenPort>()
                    .Any(_ => _.Port == port && _.Protocol == GetProtocol(protocol));
        }



        private static void AddRule(String name, String description, NET_FW_ACTION_ action, NET_FW_RULE_DIRECTION_ direction)
        {
            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID(PROGID_FW_RULE));
            firewallRule.Action = action;
            firewallRule.Description = description;
            firewallRule.Direction = direction;
            firewallRule.Enabled = true;
            firewallRule.InterfaceTypes = "All";
            firewallRule.Name = name;

            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID(PROGID_FW_POLICY2));
            firewallPolicy.Rules.Add(firewallRule);
        }

        public static void RemoveRule(String name) {
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID(PROGID_FW_POLICY2));
            firewallPolicy.Rules.Remove(name);

            foreach (INetFwRule rule in firewallPolicy.Rules)
            {
                Console.WriteLine("Firewall Rule: " + rule.Name + " - " + rule.ApplicationName);
            }

        }



        public void GloballyOpenPort(string title, int portNo,
            NET_FW_SCOPE_ scope, NET_FW_IP_PROTOCOL_ protocol,
            NET_FW_IP_VERSION_ ipVersion)
        {
            Type type = Type.GetTypeFromProgID(PROGID_OPEN_PORT);
            INetFwOpenPort port = Activator.CreateInstance(type)
                as INetFwOpenPort;
            port.Name = title;
            port.Port = portNo;
            port.Scope = scope;
            port.Protocol = protocol;
            port.IpVersion = ipVersion;
            INetFwMgr manager = GetFirewallManager();
            try
            {
                manager.LocalPolicy.CurrentProfile.GloballyOpenPorts.Add(port);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static NET_FW_IP_PROTOCOL_ GetProtocol(Protocol protocol)
        {
            return protocol == Protocol.Tcp
                       ? NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP
                       : NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP;
        }


    }
}
