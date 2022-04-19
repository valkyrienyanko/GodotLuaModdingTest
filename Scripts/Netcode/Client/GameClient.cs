using Common.Netcode;
using ENet;
using GodotModules.Settings;
using System.Collections.Generic;

namespace GodotModules.Netcode.Client
{
    public class GameClient : ENetClient
    {
        public Dictionary<uint, string> Players = new Dictionary<uint, string>();

        protected override void Connect(Event netEvent)
        {
            Log("Client connected to server");
        }

        protected override void Timeout(Event netEvent)
        {
            Log("Client connection timeout");
        }

        protected override void Disconnect(Event netEvent)
        {
            Log("Client disconnected from server");
        }
    }
}