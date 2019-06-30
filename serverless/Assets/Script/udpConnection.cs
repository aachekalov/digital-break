using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace Server
{

    public static class Connect
    {
        private static event Action<string,string> LoginPlayer;
        
        
        public static int port = 41290;
        
        private static Socket socket;
        public static Dictionary<string,Queue<string>> stack = new Dictionary<string, Queue<string>>();
        private static Object locker = new Object();
        
        public static void Start() 
        {
           Console.WriteLine("resive start");
           ThreadPool.QueueUserWorkItem((a) => resive(),null);
        }
        
        private static void resive(){
            socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp
            );
            socket.Bind((EndPoint)new IPEndPoint(IPAddress.Any,port));
            
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any,0);
            byte[] buffer = new byte[1280];
            int i = 0;
            string connectClient;
            string messag;
            EndPoint ep = (EndPoint)ipEndPoint;
            while(true){
               i = socket.ReceiveFrom(buffer,ref ep);
               ipEndPoint = ep as IPEndPoint;
               connectClient = (ipEndPoint.Address.ToString() + 
                   ":" + ipEndPoint.Port.ToString());
                messag = Encoding.UTF8.GetString(buffer,0,i);
                Console.WriteLine(connectClient + " => " + messag);
                //ClientInterface.SetMessag(ipEndPoint.Address,ipEndPoint.Port,messag);
            }
        }
        /*
        //??
        public static bool messagBroken(ref string messag){
            if(messag.ToCharArray()[0] == Char.Parse("#")){
                messag.Remove(0,1);
                return true;
            }
            Console.WriteLine(messag);
            return false;
        }
        //??
        public static void messageReqest(ref string messag){
            if(messag.ToCharArray()[0] == Char.Parse("?")){
                messag.Remove(0,1);
                Console.WriteLine(messag);
            }
        }*/
        
        public static void send(IPEndPoint uri,string message){
            byte[] m = Encoding.UTF8.GetBytes(message);
            ThreadPool.QueueUserWorkItem((a) =>{
             lock(locker)socket.SendTo(m,uri);
            },null);
        }
    }
}