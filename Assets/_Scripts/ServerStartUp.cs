using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class ServerStartUp : MonoBehaviour
{
    private const string InternalServerIP = "0.0.0.0";
    private ushort _serverPort = 7777;
    void Start(){
        bool server = false;
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if(args[i] == "-dedicatedServer"){
                server = true;
            }
            if(args[i] == "-port" && (i + 1 < args.Length)){
                _serverPort = (ushort)int.Parse(args[i + 1]);
            }
        }
        //#if SERVER //"SERVER" symbol defined in player settings' script compilation
        if(server){
            StartServer();
        }
        //#endif 
    }

    private void StartServer()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(InternalServerIP, _serverPort);
        NetworkManager.Singleton.StartServer();
    }
}
