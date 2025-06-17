using UnityEngine;

namespace _01.Scripts.Component
{
    public class PlayerManager
    {
        public static PlayerManager Instance {get; private set;}
        public MainCamera MainCamera { get; private set; }
        public BasePlayerController Player { get; private set; }

        static PlayerManager()
        {
            Instance = new PlayerManager();
        }
    
        private PlayerManager(){}

        public void SetCamera(MainCamera camera)
        {
            if (MainCamera == camera) return;
            MainCamera = camera;
        }
        public void SetPlayer(BasePlayerController player)
        {
            if (Player == player) return;
            Player = player;
        }
    }
}
