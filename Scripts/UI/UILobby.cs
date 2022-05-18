using Godot;

namespace GodotModules 
{
    public class UILobby : Node
    {
        [Export] public readonly NodePath NodePathLobbyName;
        [Export] public readonly NodePath NodePathLobbyPlayerCount;
        [Export] public readonly NodePath NodePathPlayerList;
        [Export] public readonly NodePath NodePathChatLogs;

        private Label _lobbyName;
        private Label _lobbyPlayerCount;
        private Control _playerList;
        private RichTextLabel _chatLogs;

        public override void _Ready()
        {
            _lobbyName = GetNode<Label>(NodePathLobbyName);
            _lobbyPlayerCount = GetNode<Label>(NodePathLobbyPlayerCount);
            _playerList = GetNode<Control>(NodePathPlayerList);
            _chatLogs = GetNode<RichTextLabel>(NodePathChatLogs);
        }
    }
}