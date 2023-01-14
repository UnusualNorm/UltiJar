using HarmonyLib;
using System;
using UnityEngine;
using IllTaco.JarWars;
using System.Collections;
using FarBridge.Worlds;
using static Valve.VR.SteamVR_ExternalCamera;

namespace UltraJars
{
    class SmoothCam : MonoBehaviour
    {
        public Transform target;
        public float smoothness = 1f;
        public Camera camera;

        void Start()
        {
            camera = GetComponent<Camera>();
            camera.stereoTargetEye = StereoTargetEyeMask.None;
            camera.targetDisplay = 1;
            camera.depth = 1;
            camera.tag = "MainCamera";
            camera.fieldOfView = 90;
        }

        void Update()
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * smoothness);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * smoothness);
        }
    }
}
