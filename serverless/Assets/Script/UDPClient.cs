
using System;
using System.Net;
using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using MoonSharp.Interpreter;
using Client;

namespace Internet
{
    public delegate void sender(string messag);
    [MoonSharpUserData]
    public class UDP :ILuaConsoleLib
    {
        private static UDP main;
        private UdpClient udp;
        private UdpClient udpSender;
        public static sender Callback;
       // public static sender Logs;
        private Object locker = new Object();
        private IPAddress address;
        private int InPort = 41290;
        private int OutPort = 41290;
    
        public void connect(string ip,int port){
            lock(locker){
                OutPort = port;
                address = IPAddress.Parse(ip);
                IPEndPoint ep = new IPEndPoint(IPAddress.Any,InPort);
                main = this;
                udpSender = new UdpClient();
                udpSender.ExclusiveAddressUse = false;
                udpSender.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpSender.Client.Bind(ep);
                udp = new UdpClient();
                udp.ExclusiveAddressUse = false;
                udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udp.Client.Bind(ep);
                new Thread(runReceive).Start();
            }
        }
        
        private void runReceive(){
            IPEndPoint endPoint = null;
            Byte[] message;
            while(true){
                message = udp.Receive(ref endPoint);
                if(Callback != null)
                Callback(Encoding.UTF8.GetString(message));
            }
        }
        
        public static void Send(string m){
            main.send(m);
        }
        
        public string combine(string nameRequest,DynValue[] param){
            string p = " require'" + nameRequest + "'";
            if(param.Length == 0)
            return p + "; ";
            p+= "(";
            foreach(DynValue d in param){
                switch(d.Type){
                    case DataType.Nil:
                    p+= "_,";
                    break;
                    case DataType.Number:
                    p+=d.Number.ToString() +",";
                    break;
                    case DataType.String:
                    p+="'" + d.ToString() + "',";
                    break;
                    default:
                    throw new Exception("combine string for lua get don't registed type");
                    break;
                }
            }
            p.Remove(p.Length-1,1);
            return p + "); ";
        }
        
        public void send(string m){
            ThreadPool.QueueUserWorkItem((o) => {
                lock(locker)
                udpSender.Send(
                    Encoding.UTF8.GetBytes(m),
                    m.Length,
                    address.ToString(),
                    OutPort
                );
            },null);
        }
    }
}