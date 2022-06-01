namespace GodotModules 
{
    public class SceneGame : AScene
    {
        [Export] protected readonly NodePath NodePathPositionPlayerSpawn;
        [Export] protected readonly NodePath NodePathPositionEnemySpawn;
        [Export] protected readonly NodePath NodePathNavigation2D;
        [Export] protected readonly NodePath NodePathCoinList;
        [Export] protected readonly NodePath NodePathChestList;

        public Navigation2D Navigation2D { get; set; }
        public Position2D PositionPlayerSpawn { get; set; }
        public Position2D PositionEnemySpawn { get; set; }
        public Node CoinList { get; set; }
        public Node ChestList { get; set; }

        public List<OtherPlayer> Players { get; set; }
        public Player Player { get; set; }
        public List<Enemy> Enemies { get; set; }

        private Managers _managers;

        public override void PreInitManagers(Managers managers)
        {
            _managers = managers;
        }

        public override void _Ready()
        {
            Navigation2D = GetNode<Navigation2D>(NodePathNavigation2D);
            PositionPlayerSpawn = GetNode<Position2D>(NodePathPositionPlayerSpawn);
            PositionEnemySpawn = GetNode<Position2D>(NodePathPositionEnemySpawn);
            CoinList = GetNode<Node>(NodePathCoinList);
            ChestList = GetNode<Node>(NodePathChestList);

            foreach (Chest chest in ChestList.GetChildren())
                chest.PreInit(this);

            Enemies = new List<Enemy>();
            Players = new List<OtherPlayer>();

            CreateMainPlayer(PositionPlayerSpawn.Position);

            Notifications.AddListener(this, Event.OnKeyboardInput, (sender, args) => {
                _managers.ManagerScene.HandleEscape(async () => {
                    _managers.Tokens.Cancel("client_running");
                    _managers.Tokens.Cancel("server_running");
                    await _managers.ManagerScene.ChangeScene(GameScene.Menu);
                    _managers.MenuParticles.Emitting = true;
                    _managers.MenuParticles.Visible = true;
                });
            });

            ModLoader.Hook("Game", nameof(CreateMainPlayer),  (Action<Vector2>)CreateMainPlayer);
            ModLoader.Hook("Game", nameof(CreateOtherPlayer), (Action<Vector2>)CreateOtherPlayer);
            ModLoader.Hook("Game", nameof(CreateEnemy),       (Action<Vector2>)CreateEnemy);
            ModLoader.Call("OnGameInit");

            //for (int i = 0; i < 5; i++)
                //CreateEnemy(PositionEnemySpawn.Position);
        }

        public override void _PhysicsProcess(float delta)
        {
            ModLoader.Call("OnGameUpdate", delta);
        }

        public void CreateMainPlayer(Vector2 pos = default)
        {
            var player = Prefabs.Player.Instance<Player>();
            player.Position = pos;
            AddChild(player);
            Player = player;
            Players.Add(player);
        }

        public void CreateOtherPlayer(Vector2 pos = default)
        {
            var otherPlayer = Prefabs.OtherPlayer.Instance<OtherPlayer>();
            otherPlayer.Position = pos;
            AddChild(otherPlayer);
            Players.Add(otherPlayer);
        }

        public void CreateEnemy(Vector2 pos = default)
        {
            var enemy = Prefabs.Enemy.Instance<Enemy>();
            enemy.Init(Players, Navigation2D);
            enemy.Position = pos + Utils.RandomDir() * 10;
            AddChild(enemy);
            Enemies.Add(enemy);
        }
    }
}
