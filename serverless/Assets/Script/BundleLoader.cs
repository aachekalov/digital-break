//+ref=/storage/emulated/0/unity/UnityEngine.dll
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoonSharp.Interpreter;
using Client;

    [MoonSharpUserData]
    public class BundleLoader :ILuaConsoleLib
    {
        //AssetBundle,BundleLoader,Scene
        public static string path;
        public static List<AssetBundle> bundles = new List<AssetBundle>();
        
        public static void unloadBundle(string name,bool destroy){
#if UNITY_EDITOR
            return;
#endif
            AssetBundle b = getBundle(name);
            if(b == null) return;
           // throw new Exception("you try unload Bundle but don't loaded him");
            b.Unload(destroy);
            bundles.Remove(b);
        }
        
        public static void loadBundle(string name){
            if(getBundle(name) != null) return;
            bundles.Add(AssetBundle.LoadFromFile(path + name));
        }
        
        public static AssetBundle getBundle(string bundleName){
            foreach(AssetBundle b in bundles)
            if(b.name == bundleName)
            return b;
            return null;
        }
        
        public static void loadScene(string name){
            SceneManager.LoadScene(name);
        }
        
        public static Scene getScene(string name){
           return  SceneManager.GetSceneByName(name);
        }
        
    }