using UnityEngine;

namespace _01.Scripts.Player.Player_Battle
{
    public enum ImpactType
    {
        Normal = 0,
        Obstacle,
        Enemy,
        InteractionObject,
    }

    public class ImpactMemoryPool : MonoBehaviour
    {

        [SerializeField] private GameObject[] impactPrefab; //피격 이펙트
        private MemoryPool[] memoryPool; //피격 이펙트 메모리풀

        private void Awake()
        {
            //피격 이펙트가 여러 종류면 종류별로 memoryPool 생성
            memoryPool = new MemoryPool[impactPrefab.Length];
            for (int i = 0; i < impactPrefab.Length; ++i)
            {
                memoryPool[i] = new MemoryPool(impactPrefab[i]);
            }
        }

        public void SpawnImpact(RaycastHit hit)
        {
            //부딪친 오브젝트의 Tag에 따라 다르게 처리
            if (hit.transform.CompareTag("ImpactNormal"))
            {
                OnSpawnImpact(ImpactType.Normal, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.CompareTag("ImpactObstacle"))
            {
                OnSpawnImpact(ImpactType.Obstacle, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.CompareTag("Enemy"))
            {
                OnSpawnImpact(ImpactType.Enemy, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.CompareTag("ExplosiveBarrel"))
            {
                //오브젝트 색상에 따라 색상 변경
                Color color = hit.transform.GetComponentInChildren<MeshRenderer>().material.color;
                OnSpawnImpact(ImpactType.InteractionObject, hit.point, Quaternion.LookRotation(hit.normal), color);;
            }
        }

        public void SpawnImapct(Collider other, Transform knifeTransform)
        {
            //부딪친 오브젝트의 Tag에 따라 다르게 처리
            if (other.CompareTag("ImpactNormal"))
            {
                //Quaternion.Inverse(knifeTransform.rotation) rotation과 반대되는 회전 값을 반환
                OnSpawnImpact(ImpactType.Normal, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation));
            }
            else if (other.CompareTag("ImpactObstacle"))
            {
                OnSpawnImpact(ImpactType.Obstacle, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation));
            }
            else if (other.CompareTag("Enemy"))
            {
                OnSpawnImpact(ImpactType.Enemy, knifeTransform.position,Quaternion.Inverse(knifeTransform.rotation));
            }
            else if (other.CompareTag("ExplosiveBarrel"))
            {
                Color color = other.transform.GetComponentInChildren<MeshRenderer>().material.color;
                OnSpawnImpact(ImpactType.InteractionObject, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation), color);
            }
        }

        public void OnSpawnImpact(ImpactType type, Vector3 position, Quaternion rotation, Color color = new Color())
        {
            GameObject item = memoryPool[(int)type].ActivePoolItem();
            item.transform.position = position;
            item.transform.rotation = rotation;
            item.GetComponent<Impact>().Setup(memoryPool[(int)type]);

            if (type == ImpactType.InteractionObject)
            {
                // 파티클시스템의 메인프로퍼티는 바로 접근할 수 없기 때문에 변수를 생성한 후 접근해서 사용한다.
                ParticleSystem.MainModule main = item.GetComponent<ParticleSystem>().main;
                main.startColor = color;
            }
        }
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}