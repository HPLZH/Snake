using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Snake.Core.Game;

namespace Snake.Core
{
    public interface IGameInformation
    {
        public XYPair Size { get; }

        public bool Contains(int id) => Heads().ContainsKey(id);

        public List<(int x, int y)> Tails();

        public List<(int x, int y)> Food();

        public Dictionary<int, (int x, int y)> Heads();

        public Dictionary<int, int> Lengthes();

        public List<(int id, int length)> LengthList()
        {
            List<(int id, int length)> result = new();
            var lengthes = Lengthes();
            foreach(var kvp in lengthes)
            {
                bool inserted = false;
                for (int i = 0; i<result.Count; i++)
                {
                    if (kvp.Value > result[i].length)
                    {
                        result.Insert(i, (kvp.Key, kvp.Value));
                        inserted = true;
                        break;
                    }
                }
                if (!inserted) result.Add((kvp.Key, kvp.Value));
            }
            return result;
        }
    }

    public interface IGameInformationPlus : IGameInformation
    {
        public Dictionary<int, string> Names();

        public string Name(int id) => Names().ContainsKey(id) ? Names()[id] : "";
    }
}
