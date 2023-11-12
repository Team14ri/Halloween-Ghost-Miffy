using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.ScreenResolution
{
    public class ScreenResolution : MonoBehaviour
    {
        [SerializeField] private bool active = true;

        [Header("Resolution")]
        [SerializeField] private int width = 1920;
        [SerializeField] private int height = 1080;

        private Camera _camera;

        private void Awake()
        {
            if (active == false) return;

            Screen.SetResolution(width, height, true);
        }
    }
}