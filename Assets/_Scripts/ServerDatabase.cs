using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerDatabase : MonoBehaviour
{
    string message;
    public string Message {get => message; set => message = value;}
    public static ServerDatabase i; 
     void Awake()
    {
        i = this;
        i.Message = "default message";
        Debug.Log("MESSAGE AWAKEN = " + message);
    }
}