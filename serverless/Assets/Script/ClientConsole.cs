
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using System.Threading;
using Internet;
using UnityEngine.SceneManagement;

namespace Client
{
    [MoonSharpUserData]
    public class ClientConsole
    {
        public static ClientConsole main;
        
        private static Queue<string> stackComand = new Queue<string>();
        private static Object locker = new Object();
        private static Thread consoleThread;
        private static Script lua;
        
        public static Action<string> error;
        
        
        public static Dictionary<string,ILuaConsoleLib> lib = new Dictionary<string,ILuaConsoleLib>(){
            //{"ftp",new FTP()},
            {"udp",new UDP()},
            {"asset",new BundleLoader()}
        };
        
        public static void Main(){
            StartConsole();
            Console.ReadLine();
            
        }
        
        private static void RegisterAssembly(){
            UserData.RegisterAssembly();
            UDP.Callback = new sender(SetComand);
            UserData.RegisterType<UnityEngine.AssetBundle>();
            UserData.RegisterType<Scene>();
            //registry all type;
        }
        
        
        private static void StartConsole()
        {
            main = new ClientConsole();
            RegisterAssembly();
            lua = new Script();
            foreach(string lname in lib.Keys)
            lua.Globals[lname] = lib[lname];
            consoleThread = new Thread(main.RunConsole);
            consoleThread.IsBackground = true;
            consoleThread.Start();
        }
        
        public static void SetComand(string comand){
            lock(locker){
                stackComand.Enqueue(comand);
                Monitor.Pulse(locker);
            }
        }
        
        private void GetComand(){
            string s = "";
            lock(locker)
                if(stackComand.Count != 0)
                    s = stackComand.Dequeue();
            if(s != "")lua.DoString(s);
            lock(locker)
                while(stackComand.Count == 0)
                    Monitor.Wait(locker);
        }
        
        
        private void RunConsole(){
            try{
                while(true)GetComand();
            }catch(Exception e){
                error(e.Message);
                consoleThread = new Thread(main.RunConsole);
                consoleThread.IsBackground = true;
                consoleThread.Start();
            }
        }
        
        
    }
    
    public interface ILuaConsoleLib{}
}