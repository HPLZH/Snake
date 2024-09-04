using Snake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Snake.Net
{
    public record struct GameCommand
    {
        public int OperationCode { get; set; }
        /*
        0 添加玩家
        1 退出
        2 重生
        3 转向
        4 更改速度
        */

        public int Id { get; set; }
        //添加玩家时为0

        public int CheckCode { get; set; }
        //添加玩家时为0

        public string Content { get; set; }
        //添加玩家时为Name
        //转向时为WSAD,大写单个字母

        public static GameCommand AddPlayer(string name) => new() { OperationCode = 0, Id = 0, CheckCode = 0, Content = name };
        public static GameCommand Exit(int id, int checkCode) => new() { OperationCode = 1, Id = id, CheckCode = checkCode, Content = "" };
        public static GameCommand Restart(int id, int checkCode) => new() { OperationCode = 2, Id = id, CheckCode = checkCode, Content = "" };
        public static GameCommand Turn(int id, int checkCode, Game.Direction direction) => new()
        {
            OperationCode = 3,
            Id = id,
            CheckCode = checkCode,
            Content = direction switch
            {
                Game.Direction.Up => "W",
                Game.Direction.Down => "S",
                Game.Direction.Left => "A",
                Game.Direction.Right => "D",
                _ => throw new ArgumentException("无效方向", nameof(direction)),
			}
		};
        public static GameCommand Speed(int id, int checkCode, int level) => new() { OperationCode = 4, Id = id, CheckCode = checkCode, Content = level.ToString() };

		public void Apply(GameController game)
        {
            switch (OperationCode)
            {
                case 1:
                    game.Exit(Id, CheckCode);
                    return;
                case 2:
                    game.Restart(Id, CheckCode);
                    return;
                case 3:
                    game.Input(Id, CheckCode, Content switch
                    {
                        "W" => Game.Direction.Up,
                        "S" => Game.Direction.Down,
                        "A" => Game.Direction.Left,
                        "D" => Game.Direction.Right,
                        _ => throw new Exception("无效方向代码"),
                    });
                    return;
                case 4:
                    int spdlv = 0;
                    if (!int.TryParse(Content, out spdlv)) spdlv = 0;
                    game.Input(Id, CheckCode, spdlv);
                    return;
                default:
                    throw new Exception("操作类型代码错误,添加玩家请使用具有输出的Apply函数");
            }
        }

        public void Apply(GameController game, out CheckInformation result)
        {
            switch (OperationCode)
            {
                case 0:
                    int id = game.NextFreeId();
                    int checkCode;
                    game.AddPlayer(id, Content, out checkCode);
                    result = new() { Id = id, CheckCode = checkCode };
                    return;
                default:
                    result = new();
                    Apply(game);
                    return;
            }
        }

        public string ToJsonString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }

        public static GameCommand FromJson(string json) => FromJson(System.Text.Encoding.UTF8.GetBytes(json));

        public byte[] ToJson()
        {
            return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(this);
        }

        public static GameCommand FromJson(byte[] json) => System.Text.Json.JsonSerializer.Deserialize<GameCommand>(json);
    }

    public record struct CheckInformation
    {

        public int Id { get; set; }
        public int CheckCode { get; set; }

        public string ToJsonString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }

        public static CheckInformation FromJson(string json) => FromJson(System.Text.Encoding.UTF8.GetBytes(json));

        public byte[] ToJson()
        {
            return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(this);
        }

        public static CheckInformation FromJson(byte[] json) => System.Text.Json.JsonSerializer.Deserialize<CheckInformation>(json);
    }
}
