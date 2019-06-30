using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Serialization.Json;
using CymaticLabs.Unity3D.Amqp;
using System.Text;
using System.IO;


public class LuaAmpq : MonoBehaviour
{
    public delegate void handler();
    #region param
    public string LuaCode = "file";
    private Queue<string> queueMessage = new Queue<string>();


    public event handler UpdataHandler;
    private Script lua = new Script();
    #endregion param

    #region staticIni
    static LuaAmpq() {
        UserData.RegisterAssembly();
        UserData.RegisterType<LuaAmpq>();
        Script.DefaultOptions.DebugPrint = (x) => Debug.Log(x);
    }
    #endregion staticIni

    #region Ini
    void Start()
    {
        lua.Globals["this"] = this;
       lua.DoStream(new FileInfo(Application.streamingAssetsPath + "/" + LuaCode + ".lua").OpenRead());
        
    }
    #endregion Ini

    #region Handler
    public void HandlerMessageAMPQ(AmqpQueueSubscription u, IAmqpReceivedMessage message)
    {
        
        lua.DoString("message = " + Encoding.UTF8.GetString(message.Body));
        if (UpdataHandler != null) UpdataHandler();
    }
    #endregion Handler
}
