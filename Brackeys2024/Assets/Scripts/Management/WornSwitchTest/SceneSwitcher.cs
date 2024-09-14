using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance;


    public GameObject sceneClean;
    public GameObject sceneWorn;


    public Animator transition;
    public float transitionTime = 1f;


    private void Awake() => Instance = this;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //SwitchScene();
    }
    public void SwitchScene() {

        Debug.Log("switch");
            sceneClean.SetActive(!sceneClean.activeSelf);
            sceneWorn.SetActive(!sceneWorn.activeSelf);
            LoadLevel();
    }

    IEnumerator LoadLevel()
    {
        //transition.SetTrigger("End");
        //transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
    }
}
