﻿using Common.Netcode;

namespace Valk.Modules.Netcode.Server
{
    public class RPacketLogin : IReadable
    {
        public string Username { get; set; }

        public void Read(PacketReader reader)
        {
            Username = reader.ReadString();
        }
    }
}