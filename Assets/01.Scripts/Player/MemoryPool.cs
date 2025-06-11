using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{
    private class PoolItem
    {
        public bool isActive;           //게임오브젝트의 활성화/비활성화 정보
        public GameObject gameObject;   //화면에 보이는 실제 게임 오브젝트
    }

    private int increaseCount = 5;      //오브젝트가 부족할 때 Instantiate()로 추가 생성되는 오브젝트 개수
    private int maxCount;               //현재 리스트에 등록되어 있는 오브젝트 개수
    private int activeCount;            //현재 게임에 사용되고 있는 (활성화) 오브젝트 개수
    private GameObject poolObject;
    private List<PoolItem> poolItemList;  //오브젝트 풀링에서 관리하는 게임 오브젝트 프리펩
    
    public int MaxCount => maxCount;       //외부에서 현재 리스트에 등록되어 있는 오브젝트 개수 확인을 위한 프로퍼티
    public int ActiveCount => activeCount; //외부에서 현재 활성화 되어있는 오브젝트 개수 확인을 위한 프로퍼티 

    public MemoryPool(GameObject poolObject)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;
        
        poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }

    private void InstantiateObjects()
    {
        maxCount += increaseCount;

        for (int i = 0; i < increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();
            
            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            poolItem.gameObject.SetActive(false);
            
            poolItemList.Add(poolItem);
        }
    }
}
