// See https://aka.ms/new-console-template for more information
using Snake.Core;

Snake.Net.LAN.Server server = new(80, 60, 22000, 22001, Snake.Core.GameController.Gamemode.AutoRestart, 1, 10)
{
    Log = Console.WriteLine
};
while (true)
{
    Thread.Sleep(1000);
    var list = ((IGameInformation)server).LengthList();
    var names = server.Names();
    Console.Clear();
    for(int i = 0; i < list.Count; i++)
    {
        Console.WriteLine($"{i + 1,3} {list[i].length,4} {names[list[i].id]}") ;
    }
}