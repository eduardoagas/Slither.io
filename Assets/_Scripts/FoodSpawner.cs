using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FoodSpawner : NetworkBehaviour{
    // Start is called before the first frame update
    [SerializeField] private GameObject prefab;
    private const int MaxPrefabCount = 50;
    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += SpawnFoodStart;
    }

    private void SpawnFoodStart(){
        NetworkManager.Singleton.OnServerStarted -= SpawnFoodStart;
        NetworkObjectPool.Singleton.InitializePool();
        for (int i = 0; i < 30; i++){
            SpawnFood();
        }
        StartCoroutine(SpawnOverTime());
    }

    private void SpawnFood(){
        NetworkObject obj = NetworkObjectPool.Singleton.GetNetworkObject(prefab, GetRandomPositionOnMap(), Quaternion.identity);
        obj.GetComponent<Food>().prefab = prefab;
        if(!obj.IsSpawned) obj.Spawn(true);
    }

    private Vector3 GetRandomPositionOnMap(){
        return new Vector3(Random.Range(-9f, 9f), Random.Range(-5f,5f),0f);
    }

    private IEnumerator SpawnOverTime(){
        while (NetworkManager.Singleton.ConnectedClients.Count > 0){
            yield return new WaitForSeconds(2f);
            if(NetworkObjectPool.Singleton.GetCurrentPrefabCount(prefab) < MaxPrefabCount)
                SpawnFood();            
        }
    }
}
