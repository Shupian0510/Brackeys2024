using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public GameObject sceneClean;
    public GameObject sceneWorn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) { 
            sceneClean.SetActive(!sceneClean.activeSelf);
            sceneWorn.SetActive(!sceneWorn.activeSelf);
        }
    }
}
