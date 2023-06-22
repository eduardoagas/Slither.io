using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FoodSpawner : NetworkBehaviour{
    // Start is called before the first frame update
    [SerializeField] private GameObject prefab;
    private const int MaxPrefabCount = 50;
    private bool spawning = false;
    private bool awoke = false;
    private void Awake(){
        if(!IsServer) return;
        NetworkManager.Singleton.OnServerStarted += SpawnFoodStart;
        awoke = true;
    }
    private void Start()
    {
        if(!awoke) NetworkManager.Singleton.OnServerStarted += SpawnFoodStart;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnDisable() {
        if(!NetworkManager.Singleton) return;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnect;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
    }

    private void OnClientDisconnect(ulong clientId)
    {
        if(!NetworkManager.Singleton.IsServer) return;
        if(spawning && NetworkManager.Singleton.ConnectedClients.Count == 0){
            spawning = false;
        }
    }

    private void OnClientConnect(ulong clientId)
    {
        Debug.Log("On Client Connect " + NetworkManager.Singleton.IsServer);
        if(!NetworkManager.Singleton.IsServer) return;
        if(spawning) return;
        StartCoroutine(SpawnOverTime());
    }

    private void SpawnFoodStart(){
        NetworkManager.Singleton.OnServerStarted -= SpawnFoodStart;
        NetworkObjectPool.Singleton.InitializePool();
        for (int i = 0; i < 30; i++){
            SpawnFood();
        }
        //StartCoroutine(SpawnOverTime());
    }

    private void SpawnFood(){
        NetworkObject obj = NetworkObjectPool.Singleton.GetNetworkObject(prefab, GetRandomPositionOnMap(), Quaternion.identity);
        if(!obj.IsSpawned) obj.Spawn(true);
    }

    private Vector3 GetRandomPositionOnMap(){
        return new Vector3(Random.Range(-9f, 9f), Random.Range(-5f,5f),0f);
    }

    private IEnumerator SpawnOverTime(){
        spawning = true;
        while (spawning && NetworkManager.Singleton.ConnectedClients.Count > 0){
            yield return new WaitForSeconds(2f);
            if(NetworkObjectPool.Singleton.GetCurrentPrefabCount(prefab) < MaxPrefabCount)
                SpawnFood();            
        }
        spawning = false;
    }
}
