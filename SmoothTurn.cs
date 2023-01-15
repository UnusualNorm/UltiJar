using HarmonyLib;
using IllTaco.JarWars;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Valve.VR;

namespace UltraJars
{
    [HarmonyPatch(typeof(MainPlayer), "Awake")]
    static class SmoothTurnInjector
    {
        static void Postfix(MainPlayer __instance)
        {
            if (!__instance.gameObject.GetComponent<SmoothTurn>())
                __instance.gameObject.AddComponent<SmoothTurn>();
        }
    }

    class SmoothTurn : MonoBehaviour
    {
        float speed = 180;

        void Update()
        {
            var stick = SteamVR_Actions.farbridge_worlds_thumb_position.GetAxis(SteamVR_Input_Sources.RightHand);
            var degrees = stick.x * speed * Time.deltaTime;
            var offset = new Vector3(0, degrees, 0);
            var rotation = transform.rotation.eulerAngles + offset;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
