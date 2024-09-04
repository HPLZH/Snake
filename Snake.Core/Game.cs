using System.Diagnostics.CodeAnalysis;

namespace Snake.Core
{
    public class Game : IGameInformation
    {
        public readonly int width;
        public readonly int height;

		public readonly int MaxCd = 500;
		public readonly int MaxThr = 10000;

		public XYPair Size { get => new() { X = width, Y = height }; }

        public readonly Random random;

        public enum Direction
        {
            Left = -1,
            Right = 1,
            Up = -2,
            Down = 2,
            Static = 0,
        }

        public record Operation
        {
            public int id;
        }

        public record DirectionOperation : Operation
        {
            public Direction direction;
        }

        public record SpeedOperation : Operation
        {
            public int level;
        }

        private Direction RandomDirection()
        {
			return random.Next(4) switch
			{
				0 => Direction.Left,
				1 => Direction.Right,
				2 => Direction.Up,
				3 => Direction.Down,
				_ => Direction.Static,
			};
		}

        public class Point
        {
            private int x;
            public int X
            {
                get { return x; }
                set { x = value % parent.width; if (x < 0) x += parent.width; }
            }

            private int y;
            public int Y
            {
                get { return y; }
                set { y = value % parent.height; if (y < 0) y += parent.height; }
            }

            internal readonly Game parent;

            public Point(int x, int y, Game parent)
            {
                this.parent = parent;
                this.x = x % parent.width;
                this.y = y % parent.height;
            }

            public override string ToString() => $"X={x},Y={y}";
            public override int GetHashCode() => (x, y, parent).GetHashCode();
            public override bool Equals([NotNullWhen(true)] object? obj)
            {
                if (obj == null) return false;
                if (!obj.GetType().Equals(typeof(Point))) return false;
                Point p = (Point)obj;
                return x == p.x && y == p.y && parent == p.parent;
            }

            public static bool operator ==(Point point1, Point point2) => point1.x == point2.x && point1.y == point2.y && point1.parent == point2.parent;
            public static bool operator !=(Point point1, Point point2) => !(point1 == point2);

            public static Point operator +(Point point, (int x, int y) addValue) => new(point.x + addValue.x, point.y + addValue.y, point.parent);

            public Point Left(int addValue = 1) => new(this.x - addValue, this.y, this.parent);
            public Point Right(int addValue = 1) => new(this.x + addValue, this.y, this.parent);
            public Point Up(int addValue = 1) => new(this.x, this.y - addValue, this.parent);
            public Point Down(int addValue = 1) => new(this.x, this.y + addValue, this.parent);
            public Point GetPointByDirection(Direction direction, int addValue = 1)
            {
                return direction switch
                {
                    Direction.Up => Up(addValue),
                    Direction.Down => Down(addValue),
                    Direction.Left => Left(addValue),
                    Direction.Right => Right(addValue),
                    _ => Copy(),
                };
            }
            public Func<int,Point> GetPoint(Direction direction)=> direction switch
            {
                Direction.Up => Up,
                Direction.Down => Down,
                Direction.Left => Left,
                Direction.Right => Right,
                _ => Copy,
            };

            public void MoveLeft(int addValue = 1) => this.X -= addValue;
            public void MoveRight(int addValue = 1) => this.X += addValue;
            public void MoveUp(int addValue = 1) => this.Y -= addValue;
            public void MoveDown(int addValue = 1) => this.Y += addValue;
            public void Move(Direction direction, int addValue = 1)
            {
                switch (direction)
                {
                    case Direction.Up:
                        MoveUp(addValue);
                        break;
                    case Direction.Down:
                        MoveDown(addValue);
                        break;
                    case Direction.Left:
                        MoveLeft(addValue);
                        break;
                    case Direction.Right:
                        MoveRight(addValue);
                        break;
                    default:
                        break;
                }
            }
            public Action<int> GetMove(Direction direction) => direction switch
            {
                Direction.Up => MoveUp,
                Direction.Down => MoveDown,
                Direction.Left => MoveLeft,
                Direction.Right => MoveRight,
                _ => (int _) => { }
            };

            public static Point RandomPoint(Game parent) => new(parent.random.Next(parent.width), parent.random.Next(parent.height), parent);
            public void SetRandom()
            {
                this.x = parent.random.Next(parent.width);
                this.y = parent.random.Next(parent.height);
            }

            public Point Copy() => new(this.x, this.y, this.parent);
            public Point Copy(int _) => Copy();
        }

        public class Snake
        {
            internal Point head;
            internal List<Point> tail = new();
            private Direction direction = Direction.Static;
            private Direction nextDirection = Direction.Static;

            internal int MaxCd = 5000;
            internal int MaxThr = 5000;
            internal int cd = 0;
            internal int thr = 0;
            internal int spd = 100;
            internal const int defspd = 100;

            public Snake(Point head) => this.head = head;
            public Snake(Game parent) => this.head = Point.RandomPoint(parent);

            public bool Turn(Direction direction)
            {
                if (direction == Direction.Static) return false;
                if ((int)this.direction + (int)direction == 0) return false;
                nextDirection = direction;
                return true;
            }

            public void Next(Func<Snake, bool> checkEat)
            {
                if (spd > defspd)
                {
                    thr += spd - defspd;
                    if (thr >= MaxThr && tail.Count > 0)
                    {
                        tail.RemoveAt(tail.Count - 1);
                        thr = 0;
					}
                }
                if (cd < MaxCd)
                {
                    cd += tail.Count > 0 ? spd : defspd;
                    return;
                }
                else
                {
                    cd = 0;
                }
                this.direction = nextDirection;
                if (direction == Direction.Static) return;
                tail.Insert(0, head.Copy());
                switch (direction)
                {
                    case Direction.Left:
                        head.MoveLeft();
                        break;
                    case Direction.Right:
                        head.MoveRight();
                        break;
                    case Direction.Up:
                        head.MoveUp();
                        break;
                    case Direction.Down:
                        head.MoveDown();
                        break;
                    default:
                        return;
                }
                if (!checkEat(this) && tail.Count > 0) tail.RemoveAt(tail.Count - 1);
            }

            public bool Eat(Point food) => food == head;
            public bool HitBy(Point head, bool tailOnly = false)
            {
                if (!tailOnly && head == this.head) return true;
                foreach (Point tailPart in tail) if (tailPart == head) return true;
                return false;
            }
            public bool HitOn(Snake snake, bool tailOnly = false) => snake.HitBy(this.head, tailOnly);
        }

        public Game(int width, int height)
        {
            if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException("width,height", "高度和宽度必须大于0");
            this.width = width;
            this.height = height;
            this.random = new Random();
        }

        private List<Point> food = new();
        private Dictionary<int, Snake> snakes = new();

        private bool HitSnake(Point foodPoint, bool tailOnly = true)
        {
            foreach (Snake snake in snakes.Values) if (snake.HitBy(foodPoint, tailOnly)) return true;
            return false;
        }

        private bool HitFood(Point foodPoint)
        {
            foreach(Point foodPart in food)if(foodPart == foodPoint) return true;
            return false;
        }

        private void MoveFood()
        {
            foreach(Point foodPart in food)
            {
                if (random.Next(10 * MaxCd / Snake.defspd) != 0) continue;
                Direction direction = RandomDirection();
                if (!HitSnake(foodPart.GetPointByDirection(direction)) && !HitFood(foodPart.GetPointByDirection(direction))) foodPart.Move(direction);
            }
        }

        public void AddFood()
        {
            Point foodPoint = Point.RandomPoint(this);
            while (HitFood(foodPoint) || HitSnake(foodPoint, false)) foodPoint.SetRandom();
            food.Add(foodPoint);
        }

        public void AddFood(Point foodPoint)
        {
            Point foodPoint1 = foodPoint.Copy();
            while (HitFood(foodPoint1) || HitSnake(foodPoint1)) foodPoint1.SetRandom();
            food.Add(foodPoint1);
        }

        private void FillFood()
        {
            while (food.Count < snakes.Count) AddFood();
        }

        public bool CheckEat(Snake snake)
        {
            foreach(Point foodPart in food)
            {
                if (snake.Eat(foodPart))
                {
                    food.Remove(foodPart);
                    return true;
                }
            }
            return false;
        }

        public void Next()
        {
            MoveFood();
            HandleInput();
            foreach (Snake snake in snakes.Values) snake.Next(CheckEat);
            List<int> dieSnakes = new List<int>();
            foreach(KeyValuePair<int,Snake> kvp in snakes)
            {
                foreach(Snake snake in snakes.Values)
                {
                    if (kvp.Value.HitOn(snake, snake == kvp.Value))
                    {
                        dieSnakes.Add(kvp.Key);
                        break;
                    }
                }
            }
            foreach (int key in dieSnakes) KillSnake(key);
            FillFood();
        }

        public bool AddSnake(int id)
        {
            if (Contains(id)) return false;
            Point head = Point.RandomPoint(this);
            while (HitFood(head) || HitSnake(head, false)) head.SetRandom();
            snakes.Add(id, new(head));
            snakes[id].MaxCd = MaxCd;
            snakes[id].MaxThr = MaxThr;
            return true;
        }

        public void KillSnake(int id)
        {
            Snake dieSnake = snakes[id];
            snakes.Remove(id);
            AddFood(dieSnake.head.Copy());
            foreach(Point tailPoint in dieSnake.tail)
            {
                if (random.Next(2) == 0) AddFood(tailPoint.Copy());
            }
        }

        public int NextFreeId()
        {
            int id = random.Next();
            while(Contains(id)) id = random.Next();
            return id;
        }

        public bool Contains(int id) => snakes.ContainsKey(id);

        List<Operation> operations = new();

        public void Input(int id, Direction direction)
        {
            operations.Add(new DirectionOperation
            {
                id = id,
                direction = direction
            });
        }

        public void Input(int id, int speedLevel)
        {
            operations.Add(new SpeedOperation
            {
                id = id,
                level = speedLevel
            });
        }

        private void HandleInput()
        {
            foreach(var op in operations)
            {
                if (snakes.ContainsKey(op.id))
                {
                    if(op is DirectionOperation op1)
                    {
                        snakes[op.id].Turn(op1.direction);
                    } 
                    if(op is SpeedOperation op2)
                    {
                        snakes[op.id].spd = op2.level > 0 ? 2 * Snake.defspd : Snake.defspd;
                    }
                }
            }
            operations.Clear();
        }

        public List<(int x, int y)> Tails()
        {
            List<(int x, int y)> result = new();
            foreach(Snake snake in snakes.Values)
            {
                foreach(Point tailPoint in snake.tail)
                {
                    result.Add((tailPoint.X, tailPoint.Y));
                }
            }
            return result;
        }

        public List<(int x, int y)> Food()
        {
            List<(int x, int y)> result = new();
            foreach(Point foodPoint in food)
            {
                result.Add((foodPoint.X, foodPoint.Y));
            }
            return result;
        }

        public Dictionary<int, (int x, int y)> Heads()
        {
            Dictionary<int, (int x, int y)> result = new();
            foreach(var kvp in snakes)
            {
                result.Add(kvp.Key, (kvp.Value.head.X, kvp.Value.head.Y));
            }
            return result;
        }

        public Dictionary<int, int> Lengthes()
        {
            Dictionary<int, int> result = new();
            foreach (var kvp in snakes)
            {
                result.Add(kvp.Key, kvp.Value.tail.Count + 1);
            }
            return result;
        }

        /*
        public List<(int id, int length)> LengthList()
        {
            List<(int id, int length)> result = new();
            var dict = Lengthes();
            foreach (var (id, length) in dict) result.Add((id, length));
            result.Sort(((int, int) v1, (int, int) v2) =>
            {
                if (v1.Item2 == v2.Item2) return 0;
                if (v1.Item2 > v2.Item2) return 1;
                if (v1.Item1 < v2.Item1) return -1;
                return 0;
            });
            return result;
        }
        */
    }
}