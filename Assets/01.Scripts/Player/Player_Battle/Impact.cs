using UnityEngine;

namespace _01.Scripts.Player.Player_Battle
{
    public class Impact : MonoBehaviour
    {
        private ParticleSystem particle;

        private MemoryPool memoryPool;

        private void Awake()
        {
            particle = GetComponent<ParticleSystem>();
        }

        //타격 이펙트는 삭제하지 않고 메모리풀로 관리하기 때문에
        //Setup메서드에서 메모리풀 매개변수를 받아와 멤버변수 메모리풀에 저장
        public void Setup(MemoryPool pool)
        {
            memoryPool = pool;
        }

        void Start()
        {
        
        }

        void Update()
        {
            //파티클이 재생중이 아니면 삭제
            if (particle.isPlaying == false)
            {
                if (memoryPool != null)
                {
                    memoryPool.DeactivatePoolItem(gameObject);
                }
            }
        }
    }
}
