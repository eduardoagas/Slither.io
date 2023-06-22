using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using JetBrains.Annotations;

public class LoginSystem : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI inputButton;
    [SerializeField] private TMP_InputField inputArea;
    [SerializeField] private TextMeshProUGUI fetchButton;
    [SerializeField] private TMP_Text returnText;

    public void Submit(){
        askForUpdateMessageServerRpc(inputArea.text);
        //StartCoroutine(SubmitCR());
    }
    public void Fetch(){
        askForUpdateUIServerRPC();
    }
    /*public IEnumerator SubmitCR(){
        Debug.Log("input = " + inputArea.text);
        GameDataManager.i.GameData.stuff = inputArea.text;
        GameDataManager.i.writeFile();
        yield return new WaitForSeconds(2f);
        GameDataManager.i.readFile();
        yield return new WaitForSeconds(2f);
        Debug.Log("ta no livro = " + GameDataManager.i.GameData.stuff);
    }*/

    [ServerRpc(RequireOwnership = false)]
    private void askForUpdateMessageServerRpc(string str){
        UpdateMessage(str); 
    }
    void UpdateMessage(string str){
        if (!IsServer) return;
        ServerDatabase.i.Message = str;
        Debug.Log("mensagem agora é = " + ServerDatabase.i.Message);
        //YouMustUpdateClientRpc(ServerDatabase.i.Message); //update right in time?
    }

    [ClientRpc]
    void YouMayUpdateUIClientRpc(string str){
        UpdateUI(str);
        Debug.Log(str);
    }
    void UpdateUI(string str){
         returnText.text = str;
         Debug.Log("UI agora deveria estar =" + str);
    }

    [ServerRpc(RequireOwnership = false)]
    private void askForUpdateUIServerRPC(){
         YouMayUpdateUIClientRpc(ServerDatabase.i.Message);
    }
  
    

}
