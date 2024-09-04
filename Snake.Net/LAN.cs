using Snake.Core;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Intrinsics.Arm;
using System.Timers;

namespace Snake.Net
{
	namespace LAN
	{
		public class Server : IGameInformationPlus
		{
			private TcpListener tcpListener;
			private TcpListener tcpListener1;
			private GameController controller;
			private Thread listenThread;
			private Thread listenThread1;
			private System.Timers.Timer timer;
			private List<(TcpClient client, Thread rec)> clients = new();
			private List<TcpClient> clients1 = new();
			public readonly int serverPort;
			public readonly int dataPort;

			private GameInformationPlus info = new();

			public Server(int width, int height, int port, int dataPort, GameController.Gamemode gamemode = GameController.Gamemode.Normal, double gameInterval = 10, double dispInterval = 30)
			{
				serverPort = port;
				this.dataPort = dataPort;
				tcpListener = new(IPAddress.Any, port);
				tcpListener1 = new(IPAddress.Any, dataPort);

				controller = new(width, height, gamemode, gameInterval)
				{
					Next = (g) => { info = new GameInformationPlus(g); }
				};

				timer = new(dispInterval)
				{
					AutoReset = true,
					Enabled = false
				};
				timer.Elapsed += (_, _) => { SendData(controller); };

				listenThread = new(Listen)
				{
					IsBackground = true
				};

				listenThread1 = new(Listen1)
				{
					IsBackground = true
				};

				controller.Start();
				listenThread.Start();
				listenThread1.Start();
				timer.Start();
			}

			public void Close()
			{
				timer.Stop();
				listenThread.Interrupt();
				listenThread1.Interrupt();
				controller.Close();
				timer.Close();
			}

			private void SendData(GameController game)
			{
				lock (clients1)
				{
					List<TcpClient> toRem = new();
					foreach (var client in clients1)
					{
						try
						{
							if(!client.Connected) toRem.Add(client);
							client.GetStream().Write(info.ToJson());
						}
						catch (Exception)
						{
							//client.Close();
							//toRem.Add(client);
							//Log(e.Message);
						}
					}
					foreach (var client in toRem)
					{
						clients1.Remove(client);
					}
				}
				//udp.Send(new GameInformationPlus(game).ToJson(), new IPEndPoint(IPAddress.Broadcast, clientPort));
			}

			private void Listen()
			{
				try
				{
					tcpListener.Start();
					while (true)
					{
						try
						{
							var tcp = tcpListener.AcceptTcpClient();
							var rec = new Thread(() => RecieveCommand(tcp));

							rec.Start();
							clients.Add((tcp, rec));
						}
						catch (Exception e)
						{
							Log(e.Message);
						}
					}
				}
				catch (ThreadInterruptedException)
				{
					tcpListener.Stop();
					return;
				}
				catch (Exception e)
				{
					Log(e.Message);
				}

			}

			private void Listen1()
			{
				try
				{
					tcpListener1.Start();
					while (true)
					{
						try
						{
							var tcp = tcpListener1.AcceptTcpClient();
							lock (clients1)
							{
								clients1.Add(tcp);
							}
						}
						catch (Exception e)
						{
							Log(e.Message);
						}
					}
				}
				catch (ThreadInterruptedException)
				{
					tcpListener1.Stop();
					return;
				}
				catch (Exception e)
				{
					Log(e.Message);
				}

			}

			private void RecieveCommand(TcpClient client)
			{
				try
				{
					NetworkStream networkStream = client.GetStream();
					byte[] buf = new byte[1024];

					while (true)
					{
						try
						{
							if(!client.Connected) clients.RemoveAll(cp => cp.client == client);
							var size = networkStream.Read(buf, 0, buf.Length);
							if (size == 0) continue;
							GameCommand command = GameCommand.FromJson(buf[0..size]);
							if (command.OperationCode == 0)
							{
								command.Apply(controller, out CheckInformation check);
								try
								{
									networkStream.Write(check.ToJson());
								}
								catch (Exception e)
								{
									Log(e.Message);
								}
							}
							else
							{
								command.Apply(controller);
							}
							/*
                            IPEndPoint endPoint = new(IPAddress.Any, 0);
                            byte[] data = udp.Receive(ref endPoint);
                            GameCommand command = GameCommand.FromJson(data);
                            if (command.OperationCode == 0)
                            {
                                command.Apply(controller, out CheckInformation check);
                                udp.Send(check.ToJson(), endPoint);
                            }
                            else
                            {
                                command.Apply(controller);
                            }*/
						}
						catch (ThreadInterruptedException)
						{
							return;
						}
						catch (Exception)
						{
							//client.Close();
							//clients.RemoveAll(cp => cp.client == client);
							//Log(e.Message);
							//return;
						}
					}
				}
				catch (Exception e)
				{
					Log(e.Message);
				}
			}

			public Dictionary<int, string> Names()
			{
				return ((IGameInformationPlus)controller).Names();
			}

			public List<(int x, int y)> Tails()
			{
				return ((IGameInformation)controller).Tails();
			}

			public List<(int x, int y)> Food()
			{
				return ((IGameInformation)controller).Food();
			}

			public Dictionary<int, (int x, int y)> Heads()
			{
				return ((IGameInformation)controller).Heads();
			}

			public Dictionary<int, int> Lengthes()
			{
				return ((IGameInformation)controller).Lengthes();
			}

			public Action<string> Log = _ => { };

			public XYPair Size => ((IGameInformation)controller).Size;
		}

		public class Client : IGameClient
		{
			private TcpClient tcp;
			private TcpClient tcp1;
			private Thread thread;

			private readonly string host;
			private readonly int serverPort;
			private readonly int dataPort;

			public Client(string host, int serverPort, int dataPort)
			{
				this.host = host;
				this.serverPort = serverPort;
				this.dataPort = dataPort;

				Next += (_) => { };

				tcp = new(host, serverPort);
				tcp1 = new(host, dataPort);

				thread = new(RecieveData)
				{
					IsBackground = true
				};

				thread.Start();
			}

			public void Close()
			{
				thread.Interrupt();
				tcp.Close();
			}

			public event Action<GameInformationPlus?> Next;
			public Action<string> Log = _ => { };

			private CheckInformation checkInformation;
			private bool isChecked = false;
			private void RecieveData()
			{
				NetworkStream stream = tcp1.GetStream();
				try
				{
					byte[] buf = new byte[4096];
					while (true)
					{
						try
						{
							var size = stream.Read(buf);
							if (size == 0) continue;
							Next(GameInformationPlus.FromJson(buf[0..size]));
						}
						catch (ThreadInterruptedException)
						{
							return;
						}
						catch (Exception e)
						{
							Log(e.Message);
						}
					}
				}
				catch (Exception e)
				{
					Log(e.Message);
					return;
				}
			}

			public void SendCommand(GameCommand command)
			{
				tcp.GetStream().Write(command.ToJson());
			}

			public bool AddPlayer(string name, out int id, out int checkCode)
			{
				isChecked = false;
				id = 0;
				checkCode = 0;
				SendCommand(GameCommand.AddPlayer(name));
				byte[] buf = new byte[1024];
				int size;
				while ((size = tcp.GetStream().Read(buf)) == 0) ;
				checkInformation = CheckInformation.FromJson(buf[0..size]);
				isChecked = true;
				if (!isChecked) return false;
				else
				{
					id = checkInformation.Id;
					checkCode = checkInformation.CheckCode;
					return true;
				}

			}
		}

		public class UdpPortForwardingServer
		{
			UdpClient udp;
			Thread thread;
			readonly IPAddress ip;
			readonly int rPort;

			public readonly List<int> ports = new();
			public Action<string> Log = _ => { };

			public UdpPortForwardingServer(int port, IPAddress hostIP, int hostPort)
			{
				ip = hostIP;
				rPort = hostPort;

				this.udp = new UdpClient(port)
				{
					EnableBroadcast = true
				};

				this.thread = new Thread(Forwarding)
				{
					IsBackground = true
				};

				this.thread.Start();
			}

			private void Forwarding()
			{
				while (true)
				{
					try
					{
						IPEndPoint endPoint = new(ip, rPort);
						byte[] data = udp.Receive(ref endPoint);
						foreach (int p in ports)
						{
							udp.Send(data, new IPEndPoint(IPAddress.Loopback, p));
						}
					}
					catch (Exception e)
					{
						Log(e.Message);
					}
				}
			}
		}
	}
}