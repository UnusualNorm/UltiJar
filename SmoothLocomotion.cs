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
    static class SmoothControllerInjector
    {
        static void Postfix(MainPlayer __instance)
        {
            if (!__instance.gameObject.GetComponent<SmoothLocomotion>())
                __instance.gameObject.AddComponent<SmoothLocomotion>();
        }
    }

    class SmoothLocomotion : MonoBehaviour
    {
        float speed = 4;
        float airSpeed = 2.5f;
        CharacterController character;
        MainPlayer player;

        void Start()
        {
            player = GetComponent<MainPlayer>();
            character = gameObject.AddComponent<CharacterController>();
            character.height = .1f;
            character.radius = .01f;
            character.center = new Vector3(0, .1f, 0);

            /*var cam = new GameObject("UJSmoothCam");
            cam.AddComponent<Camera>();
            var smoothCam = cam.AddComponent<SmoothCam>();
            smoothCam.target = player.Head.transform;*/
        }

        float onMeshThreshold = 1;
        void Update()
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(gameObject.transform.position, out hit, 2, NavMesh.AllAreas))
            {
                var onMesh = hit.position.y < gameObject.transform.position.y &&
                    Vector3.Distance(hit.position, gameObject.transform.position) <= onMeshThreshold;

                if (!onMesh) transform.position = hit.position;
            }

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
