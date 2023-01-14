using HarmonyLib;
using IllTaco.JarWars;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valve.VR;

namespace UltraJars
{
    [HarmonyPatch(typeof(MainPlayer), "Awake")]
    static class SmoothControllerInjector
    {
        static void Postfix(MainPlayer __instance)
        {
            if (!__instance.gameObject.GetComponent<SmoothController>())
                __instance.gameObject.AddComponent<SmoothController>();
        }
    }

    class SmoothController : MonoBehaviour
    {
        float speed = 6;
        float airSpeed = 2.5f;
        CharacterController character;
        MainPlayer player;

        void Start()
        {
            player = GetComponent<MainPlayer>();
            character = gameObject.AddComponent<CharacterController>();
            character.height = .1f;
            character.radius = .0000001f;
            character.center = new Vector3(0, .1f, 0);

            /*var cam = new GameObject("UJSmoothCam");
            cam.AddComponent<Camera>();
            var smoothCam = cam.AddComponent<SmoothCam>();
            smoothCam.target = player.Head.transform;*/
        }

        void Update()
        {
            var stick = SteamVR_Actions.farbridge_worlds_thumb_position.GetAxis(SteamVR_Input_Sources.LeftHand);
            //Melon<UltraJarsMelon>.Logger.Msg($"{stick.x} {stick.y}");
            Quaternion headYaw = Quaternion.Euler(0, player.Head.transform.rotation.eulerAngles.y, 0);
            Vector3 moveDirection = headYaw * new Vector3(stick.x, 0, stick.y);
            if (character.isGrounded)
            {
                moveDirection.x *= speed;
                moveDirection.z *= speed;
            }
            else
            {
                moveDirection.x *= airSpeed;
                moveDirection.z *= airSpeed;

                moveDirection.y += Physics.gravity.y * Time.deltaTime * 9.8f;
            }

            character.Move(moveDirection * Time.deltaTime);
        }
    }
}
