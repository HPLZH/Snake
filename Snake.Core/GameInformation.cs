using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Snake.Core
{
    public class GameInformation : IGameInformation
    {
        public GameInformation() { }

        public List<(int x, int y)> food = new();
        public List<(int x, int y)> tails = new();
        public Dictionary<int, (int x, int y)> heads = new();
        public Dictionary<int, int> lengthes = new();

        public XYPair Size { get; set; }

        public List<XYPair> FoodList
        {
            get { return food.ConvertAll<XYPair>(x => x); }
            set { food = value.ConvertAll<(int x, int y)>(x => x); }
        }

        public List<XYPair> TailList
        {
            get { return tails.ConvertAll<XYPair>(x => x); }
            set { tails = value.ConvertAll<(int x, int y)>(x => x); }
        }

        public Dictionary<int, XYPair> HeadDict
        {
            get { return heads.ToList().ConvertAll(kvp => new KeyValuePair<int, XYPair>(kvp.Key, kvp.Value)).ToDictionary<KeyValuePair<int, XYPair>, int, XYPair>(kvp => kvp.Key, kvp => kvp.Value); }
            set { heads = value.ToList().ConvertAll(kvp => new KeyValuePair<int, (int x, int y)>(kvp.Key, kvp.Value)).ToDictionary<KeyValuePair<int, (int x, int y)>, int, (int x, int y)>(kvp => kvp.Key, kvp => kvp.Value); }
        }

        public Dictionary<int, int> LengthDict
        {
            get { return Lengthes(); }
            set { lengthes = new(value); }
        }

        public List<(int x, int y)> Food() => new(food);
        public List<(int x, int y)> Tails() => new(tails);
        public Dictionary<int, (int x, int y)> Heads() => new(heads);
        public Dictionary<int, int> Lengthes() => new(lengthes);

        public GameInformation(IGameInformation information)
        {
            food = information.Food();
            tails = information.Tails();
            heads = information.Heads();
            lengthes = information.Lengthes();
            Size = information.Size;
        }

        public virtual string ToJsonString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }

        public static GameInformation? FromJson(string json) => FromJson(System.Text.Encoding.UTF8.GetBytes(json));

        public virtual byte[] ToJson()
        {
            return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(this);
        }

        public static GameInformation? FromJson(byte[] json) => System.Text.Json.JsonSerializer.Deserialize<GameInformation>(json);
    }

    public class GameInformationPlus : GameInformation, IGameInformationPlus
    {
        public GameInformationPlus() { }

        public Dictionary<int, string> names = new();

        public Dictionary<int, string> NameDict
        {
            get { return Names(); }
            set { names = new(value); }
        }

        public Dictionary<int, string> Names() => new(names);
        
        public GameInformationPlus(IGameInformationPlus information) : base(information)
        {
            names = information.Names();
        }
        
        public override string ToJsonString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }

        public static new GameInformationPlus? FromJson(string json) => FromJson(System.Text.Encoding.UTF8.GetBytes(json));

        public override byte[] ToJson()
        {
            return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(this);
        }

        public static new GameInformationPlus? FromJson(byte[] json) => System.Text.Json.JsonSerializer.Deserialize<GameInformationPlus>(json);

    }

    public record struct XYPair
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static implicit operator (int x, int y)(XYPair pair) => (pair.X, pair.Y);
        public static implicit operator XYPair((int x, int y) value) => new() { X = value.x, Y = value.y };
    }
}
