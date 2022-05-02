using Godot;
using GodotModules.Netcode;
using GodotModules.Netcode.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GodotModules
{
    public class SceneGameServers : Control
    {
        public static Dictionary<string, LobbyListing> LobbyListings { get; set; }
        public static SceneGameServers Instance { get; set; }
        public static UILobbyListing SelectedLobbyInstance { get; set; }
        public static bool ConnectingToLobby { get; set; }
        public static bool Disconnected { get; set; }

        [Export] public readonly NodePath NodePathServerList;
        [Export] public readonly NodePath NodePathServerCreationPopup;

        private VBoxContainer ServerList { get; set; }
        public UIPopupCreateLobby ServerCreationPopup { get; set; }

        public override void _Ready()
        {
            Instance = this;
            ServerList = GetNode<VBoxContainer>(NodePathServerList);
            ServerCreationPopup = GetNode<UIPopupCreateLobby>(NodePathServerCreationPopup);
            LobbyListings = new();

            if (Disconnected)
            {
                Disconnected = false;
                var message = "Disconnected";

                switch (NetworkManager.GameClient.DisconnectOpcode)
                {
                    case DisconnectOpcode.Timeout:
                        message = "Timed out from server";
                        break;

                    case DisconnectOpcode.Restarting:
                        message = "Server is restarting..";
                        break;

                    case DisconnectOpcode.Stopping:
                        message = "Server was stopped";
                        break;

                    case DisconnectOpcode.Kicked:
                        message = "You were kicked";
                        break;

                    case DisconnectOpcode.Banned:
                        message = "You were banned";
                        break;
                }

                GameManager.SpawnPopupMessage(message);
            }

            //await ListServers();
        }

        public override void _Input(InputEvent @event)
        {
            Utils.EscapeToScene("Menu", () =>
            {
                WebClient.Client.CancelPendingRequests();
                NetworkManager.CancelClientConnectingTokenSource();
            });
        }

        public static async Task JoinServer(string ip, ushort port)
        {
            if (SceneGameServers.ConnectingToLobby)
                return;

            SceneGameServers.ConnectingToLobby = true;

            GD.Print("Connecting to lobby...");
            NetworkManager.StartClient(ip, port);

            await NetworkManager.WaitForClientToConnect(3000, async () =>
            {
                await NetworkManager.GameClient.Send(ClientPacketOpcode.Lobby, new CPacketLobby
                {
                    LobbyOpcode = LobbyOpcode.LobbyJoin,
                    Username = GameManager.Options.OnlineUsername
                });
            });
        }

        public void AddServer(LobbyListing info)
        {
            var lobby = Prefabs.LobbyListing.Instance<UILobbyListing>();
            ServerList.AddChild(lobby);
            lobby.SetInfo(info);
        }

        public void ClearServers()
        {
            foreach (Control child in ServerList.GetChildren())
                child.QueueFree();
        }

        public async Task ListServers()
        {
            WebClient.TaskGetServers = WebClient.Get<LobbyListing[]>("servers/get");
            var res = await WebClient.TaskGetServers;

            if (res.Status == WebServerStatus.ERROR)
                return;

            LobbyListings.Clear();

            res.Content.ForEach(async server =>
            {
                PingServers.CancelTokenSource = new CancellationTokenSource();
                PingServers.CancelTokenSource.CancelAfter(1000);
                await Task.Run(() => PingServers.PingServer(), PingServers.CancelTokenSource.Token).ContinueWith((x) =>
                {
                    if (!PingServers.CancelTokenSource.IsCancellationRequested)
                    {
                        LobbyListings.Add(server.Ip, server);
                        AddServer(server);
                    }
                });
                PingServers.DummyClient.Stop();
            });
        }

        public async void PostServer(LobbyListing info)
        {
            var res = await WebClient.Post("servers/post", new Dictionary<string, string>
            {
                { "Name", info.Name },
                { "Ip", info.Ip },
                { "Port", "" + info.Port },
                { "Description", info.Description },
                { "MaxPlayerCount", "" + info.MaxPlayerCount }
            });

            if (res.Status == WebServerStatus.ERROR)
            {
                // TODO: Try to post server on master server 3 more times
            }
        }

        private void _on_Control_resized()
        {
            if (ServerCreationPopup != null)
                ServerCreationPopup.RectGlobalPosition = RectSize / 2 - ServerCreationPopup.RectSize / 2;
        }
    }
}