using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    //get the Dialogue Speaker component
    public DialogueSpeaker ds;

    // Update is called once per frame
    void Update()
    {   
        //start the dialogue on pressing SPACE
        if(Input.GetKeyDown(KeyCode.Space)){
            ds.playDialogue();
        }
        
        //skip current dialogue and move on to the next one on pressing E
        if(Input.GetKeyDown(KeyCode.E)){
            ds.skip();
        }

        //pause current dialogue on pressing D
        if(Input.GetKeyDown(KeyCode.D)){
            ds.stop();
        }
        
        //resume the stopped dialogue on pressing F
        if(Input.GetKeyDown(KeyCode.F)){
            ds.resume();
        }
    }
}
