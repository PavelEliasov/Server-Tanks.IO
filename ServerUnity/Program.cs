using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ServerUnity {
    class Program {
        static Thread serverThread;
        static void Main(string[] args) {
            serverThread = new Thread(new ThreadStart(new ServerObject().Listen));
            serverThread.Start();

        }
    }
}
