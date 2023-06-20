using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Food : NetworkBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update
    //singleton = one only possible class instance
    private void OnTriggerEnter2D(Collider2D col){
        if(!col.CompareTag("Player")) return;
        if(!NetworkManager.Singleton.IsServer) return;
        if(col.TryGetComponent(out PlayerLength playerLength)){
            playerLength.AddLength();
        }else if(col.TryGetComponent(out Tail tail)){
            tail.networkedOwner.GetComponent<PlayerLength>().AddLength();
        }
        NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, prefab);
        if (NetworkObject.IsSpawned) NetworkObject.Despawn();
    }
}
