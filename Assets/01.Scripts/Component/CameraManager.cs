using UnityEngine;

namespace _01.Scripts.Component
{
    public class CameraManager
    {
        public static CameraManager Instance {get; private set;}
        public Camera MainCamera { get; private set; }

        static CameraManager()
        {
            Instance = new CameraManager();
        }
    
        private CameraManager(){}

        public void SetCamera(Camera camera)
        {
            if (MainCamera == camera) return;
            MainCamera = camera;
        }
    }
}
