using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NetworkClasses;

namespace ServerUnity {
    class ServerObject {

        protected internal List<ClientObject> _clients = new List<ClientObject>();
        List<TankData> _tanks = new List<TankData>();
        string _ip = "134.249.78.11"; 
        int _port = 8888;
        TcpListener _server;

        public void Listen() {
            Console.WriteLine("Start Listening");
           // Console.ReadLine();
            _server = new TcpListener(IPAddress.Parse(_ip),_port);
            
            try {
                _server.Start();
                while (true) {
                   TcpClient tcpClient = _server.AcceptTcpClient();
                   DiagnosticClass._timer.Start();
                   Console.WriteLine("StartTimer value="+DiagnosticClass._timer.ElapsedMilliseconds);
                    Console.WriteLine("ClientConnect");
                    ClientObject client = new ClientObject(tcpClient,this);
                    _clients.Add(client);
                    Thread clientThread = new Thread(new ThreadStart(client.Process));
                    clientThread.Start();
                }

            }
            catch(Exception e) {
                Console.WriteLine(e);
            }
        }

        public void BroadCastAutorisation( Message msg, string id,string playerName) {
            Console.WriteLine("Broadcast Autorisation");
            foreach (ClientObject client in _clients) {
                if (client._id == id) {
                   // msg._messageType = MessageType.System;
                    msg._methodName = "StartGame";
                    msg._methodParameters = new Parameters(playerName,id);
                    client.SendData(msg);
                   // client.SendData(msg);
                }
                else {

                    if (client.IsAvailable) {
                        msg._methodName = "ConnectNewPlayer";
                        client.SendData(msg);
                    }
                 
                }
                
            }

        }

        public void BroadCastPosition(Message msg,string id) {

            foreach (ClientObject client in _clients) {
                if (client.IsAvailable) {
                    if (client._id == id) {
                        msg._methodName = "Move";
                    }
                    else {
                        msg._methodName = "EnemyMove";
                    }
                    client.SendData(msg);
                }

            }

        }


    }

    
}
