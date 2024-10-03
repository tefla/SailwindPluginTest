using BepInEx;
using BepInEx.Logging;

using UnityEngine;


using HarmonyLib;


namespace MyFirstSailwindPlugin.Patches
{
    internal class GpButtonSteeringWheelPatches
    {
        //static GPButtonSteeringWheel wheel;
        static GoPointer pointer;
        static Dictionary<KeyCode, GoPointerButton> openWith =
            new Dictionary<KeyCode, GoPointerButton>();
    
        public static string GetGameObjectPath(GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }
    
        public static KeyCode ChecKey()
        {
            if(Input.GetKey(KeyCode.Alpha0)) return KeyCode.Alpha0;
            if(Input.GetKey(KeyCode.Alpha1)) return KeyCode.Alpha1;
            if(Input.GetKey(KeyCode.Alpha2)) return KeyCode.Alpha2;
            if(Input.GetKey(KeyCode.Alpha3)) return KeyCode.Alpha3;
            if(Input.GetKey(KeyCode.Alpha4)) return KeyCode.Alpha4;
            if(Input.GetKey(KeyCode.Alpha5)) return KeyCode.Alpha5;
            if(Input.GetKey(KeyCode.Alpha6)) return KeyCode.Alpha6;
            if(Input.GetKey(KeyCode.Alpha7)) return KeyCode.Alpha7;
            if(Input.GetKey(KeyCode.Alpha8)) return KeyCode.Alpha8;
            if(Input.GetKey(KeyCode.Alpha9)) return KeyCode.Alpha9;
            return KeyCode.None;
        }
    
        [HarmonyPatch(typeof(GPButtonRopeWinch), "OnActivate")]
        internal class GPButtonRopeWinchOnActivate
        {
            public static void Postfix(GPButtonRopeWinch __instance, GoPointer activatingPointer)
            {
                var Log = BepInEx.Logging.Logger.CreateLogSource("GPButtonRopeWinch");
                Log.LogInfo("OnActivate");
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    Log.LogInfo("Shift pressed");
                    var key = ChecKey();
                    if(key == KeyCode.None)
                        return;
                    Log.LogInfo("Key pressed: " + key);
                    openWith[key] = __instance;
                    if (pointer == null)
                    {
                        pointer = activatingPointer;
                    }
                }
            }
        }
        
        [HarmonyPatch(typeof(GPButtonSteeringWheel), "OnActivate")]
        internal class GPButtonSteeringWheelOnActivate
        {
            public static void Postfix(GPButtonSteeringWheel __instance, GoPointer activatingPointer)
            {
                var Log = BepInEx.Logging.Logger.CreateLogSource("GPButtonSteeringWheel");
                Log.LogInfo("OnActivate");
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    Log.LogInfo("Shift pressed");
                    var key = ChecKey();
                    if(key == KeyCode.None)
                        return;
                    Log.LogInfo("Key pressed: " + key);
                    openWith[key] = __instance;
                    if (pointer == null)
                    {
                        pointer = activatingPointer;
                    }
                }
            }
        }
        
        
        [HarmonyPatch(typeof(GoPointer), "LateUpdate")]
        internal class GoPointerExtraLateUpdate
        {
            public static void Postfix()
            {
    
                if(pointer == null)
                    return;
                var key = ChecKey();
                if(key == KeyCode.None)
                    return;
                
                GoPointerButton gpButton = openWith[key];
                
                if(gpButton  != null)
                {
                    pointer.UnStickyClick();
                    gpButton.OnActivate(pointer);
                }
            }
        }
        
        
    }

}
