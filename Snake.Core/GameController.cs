using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core
{
    public class GameController : IGameInformationPlus
    {
        private readonly Game game;
        private readonly System.Timers.Timer timer;
        public readonly double interval;

        public XYPair Size { get => game.Size; }

        public Action<GameController> Next;

        private readonly Dictionary<int, (int checkCode, string name)> players;
        private readonly Gamemode gamemode;
        public enum Gamemode
        {
            Normal,
            AutoRestart,
            AutoExit,
        }


        public GameController(int width,int height, Gamemode gamemode = Gamemode.Normal, double interval = 50)
        {
            game = new(width, height);
            players = new();
            Next = (GameController _) => { };
            this.interval = interval;
            timer = new()
            {
                Interval = this.interval,
                AutoReset = true
            };
            timer.Elapsed += Tick;
            this.gamemode = gamemode;
        }

        public void Start() => timer.Start();
        public void Stop() => timer.Stop();

        public void Close()
        {
            timer.Stop();
            timer.Close();
        }

        public bool AddPlayer(int id, string name, out int checkCode)
        {
            checkCode = 0;
            if (Contains(id)) return false;
            checkCode = new Random().Next();
            players.Add(id, (checkCode, name));
            game.AddSnake(id);
            return true;
        }

        public void Restart(int id, int checkCode)
        {
            if (!players.ContainsKey(id)) return;
            if (checkCode != players[id].checkCode) return;
            game.AddSnake(id);
        }

        public void Exit(int id, int checkCode)
        {
            if (!players.ContainsKey(id)) return;
            if (checkCode != players[id].checkCode) return;
            players.Remove(id);
            game.KillSnake(id);
        }

        public int NextFreeId()
        {
            int result = game.NextFreeId();
            while (Contains(result)) result = game.NextFreeId();
            return result;
        }

        public bool Contains(int id) => players.ContainsKey(id);

        public void Input(int id, int checkCode, Game.Direction direction)
        {
            if (!players.ContainsKey(id)) return;
            if (checkCode != players[id].checkCode) return;
            game.Input(id, direction);
        }

		public void Input(int id, int checkCode, int speedLevel)
		{
			if (!players.ContainsKey(id)) return;
			if (checkCode != players[id].checkCode) return;
			game.Input(id, speedLevel);
		}

		private void Tick(object? s, System.Timers.ElapsedEventArgs e)
        {
            game.Next();
            switch (gamemode)
            {
                case Gamemode.AutoRestart:
                    foreach (var player in players)
                    {
                        if (!game.Contains(player.Key)) game.AddSnake(player.Key);
                    }
                    break;
                case Gamemode.AutoExit:
                    foreach (var player in players)
                    {
                        if (!game.Contains(player.Key)) players.Remove(player.Key);
                    }
                    break;
                default:
                    break;
            }
            Next(this);
        }

        public string Name(int id)
        {
            if (!Contains(id)) return "";
            return players[id].name;
        }

        public Dictionary<int,string> Names()
        {
            var result = new Dictionary<int,string>();
            foreach (var kvp in players) result.Add(kvp.Key, kvp.Value.name);
            return result;
        }

        public List<(int x, int y)> Tails() => game.Tails();
        public Dictionary<int, (int x, int y)> Heads() => game.Heads();
        public List<(int x, int y)> Food() => game.Food();
        public Dictionary<int, int> Lengthes() => game.Lengthes();
    }
}
