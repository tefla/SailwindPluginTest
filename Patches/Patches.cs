using BepInEx;
using BepInEx.Logging;

using UnityEngine;


using HarmonyLib;


namespace MyFirstSailwindPlugin.Patches
{
    internal class GPButtonSteeringWheelPatches
    {
        static GPButtonSteeringWheel wheel;
        static GoPointer pointer;
        
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

        [HarmonyPatch(typeof(GPButtonSteeringWheel), "OnActivate")]
        internal class GPButtonSteeringWheelOnActivate
        {
            public static void Postfix(GPButtonSteeringWheel __instance, GoPointer activatingPointer)
            {
                wheel = __instance;
                pointer = activatingPointer;

                var log = BepInEx.Logging.Logger.CreateLogSource("GPButtonSteeringWheel");
                log.LogInfo($"{__instance.name} Awake");
                log.LogInfo($"{GetGameObjectPath(__instance.gameObject)}");
            }
        }
        
        [HarmonyPatch(typeof(GoPointer), "LateUpdate")]
        internal class GoPointerExtraLateUpdate
        {
            public static void Postfix()
            {
                var log = BepInEx.Logging.Logger.CreateLogSource("GPButtonSteeringWheel");
                log.LogInfo("Update");
                if(wheel == null)
                    return;
                if(pointer == null)
                    return;
                log.LogInfo($"{GetGameObjectPath(wheel.gameObject)}");
                if (Input.GetKey(KeyCode.Alpha0))
                {
                    log.LogInfo("Key");

                    wheel.OnActivate(pointer);
                }
            }
        }
        
        
    }

}
