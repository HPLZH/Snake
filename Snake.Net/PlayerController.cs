using Snake.Core;
using Snake.Net.LAN;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Net
{
    public class PlayerController : IController
    {
        protected IGameClient client;

        public readonly int id;
        private readonly int checkCode;

        public PlayerController(IGameClient client, string name)
        {
            this.client = client;
            if (!client.AddPlayer(name, out id, out checkCode)) throw new Exception("添加玩家失败");
        }

        public void Turn(Game.Direction direction) => client.SendCommand(GameCommand.Turn(id, checkCode, direction));
        public void Speed(int level) => client.SendCommand(GameCommand.Speed(id, checkCode, level));
        public void Exit() => client.SendCommand(GameCommand.Exit(id, checkCode));
        public void Restart() => client.SendCommand(GameCommand.Restart(id, checkCode));
    }

    public interface IGameClient
    {
        public void SendCommand(GameCommand command);

        public bool AddPlayer(string name, out int id, out int checkCode);

        public event Action<GameInformationPlus?> Next;

    }

    public interface IController
    {
        public void Turn(Game.Direction direction);
        public void Speed(int level);
        public void Exit();
        public void Restart();
    }
}
