using UnityEngine;

public class EndMethod : MonoBehaviour
{
    public GameObject text;
    
    void OnEnable(){
        text.SetActive(true);
    }
}
