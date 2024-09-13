using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueSpeaker : MonoBehaviour
{
    //Text Mesh Pro object of the subtitles text
    public TextMeshProUGUI subtitlesText;        

    //flag whether we should print subtitles or not
    public bool useSubtitles = true;       
    
    //class that holds the dialogue options
    public Dialogues[] dialogues;
    
    //current dialogue index
    [HideInInspector] public int index;
    
    //should a script be enabled when all dialogues have finished
    public bool enableScriptFinish = false;

    //enable the end script after (t) seconds
    public float endScriptTimer;
    
    //the ending script that will enable when all dialogues have finished
    public MonoBehaviour scriptToEnable;

    //flag if dialogue has been skipped
    bool instantSkip;

    //flag if dialogue has been started
    bool dialogueStarted;

    //flag that dialogue has been stopped
    bool stopped;

    //flag that subtitles need to be removed when a dialogue finishes
    bool tempRemoveSubtitles;


    void Awake(){
        //set dialogue index to 0
        index = 0;
    }

    //main public method that triggers the dialogues
    public void playDialogue(){
        if(!dialogueStarted){
            dialogueStarted = true;
            StartCoroutine(playDialogueIndex());
        }
    }

    //plays the current index dialogue and prints subtitles
    IEnumerator playDialogueIndex(){
        if (!instantSkip) yield return new WaitForSeconds(dialogues[index].time);
        else yield return new WaitForSeconds(0.15f);
    
        dialogues[index].audio.Play();
        if(dialogues[index].scriptToEnable != null) dialogues[index].scriptToEnable.enabled = true;
        tempRemoveSubtitles = false;

        if (useSubtitles) subtitlesText.transform.gameObject.SetActive(true);
        else subtitlesText.transform.gameObject.SetActive(false);

        subtitlesText.text = dialogues[index].subtitles;
        instantSkip = false;
        
        StartCoroutine(catchAudioEnds());
    }

    //get when audio finishes to play the next dialogue
    IEnumerator catchAudioEnds(){
        yield return new WaitForSeconds(dialogues[index].audio.clip.length - dialogues[index].audio.time);
        
        if(index + 1 < dialogues.Length){
            index++;
            tempRemoveSubtitles = true;
            subtitlesText.transform.gameObject.SetActive(false);
            StartCoroutine(playDialogueIndex());
        }else{
            reset();
            tempRemoveSubtitles = true;
            if (enableScriptFinish) StartCoroutine(endingMethod());
            subtitlesText.transform.gameObject.SetActive(false);
        }
    }

    //trigger the ending method
    IEnumerator endingMethod(){
        yield return new WaitForSeconds(endScriptTimer);
        if (scriptToEnable != null) scriptToEnable.enabled = true;
        else Debug.LogWarning("No end method in Dialogue Speaker to run");
    }

    //stop the current dialogue
    public void stop(){
        stopped = true;
        StopAllCoroutines();
        dialogues[index].audio.Pause();
    }

    //resume current dialogue
    public void resume(){
        if (!stopped) return;
        stopped = false;
        instantSkip = true;
        StartCoroutine(playDialogueIndex());
    }

    //reset the dialogue speaker
    void reset(){
        dialogueStarted = false;
        index = 0;
    }

    //skip to next dialogue
    public void skip(){
        if (!dialogueStarted || stopped) return;

        StopAllCoroutines();

        dialogues[index].audio.Stop();
        tempRemoveSubtitles = true;
        subtitlesText.transform.gameObject.SetActive(false);

        //if not last dialogue, increment and skip to next
        //if last, run the end method coroutine
        if((index + 1) < dialogues.Length){
            index++;
            instantSkip = true;
            StartCoroutine(playDialogueIndex());
        }else{
            reset();
            if (enableScriptFinish) StartCoroutine(endingMethod());
        }
    }

    void Update(){
        if (useSubtitles && !tempRemoveSubtitles) subtitlesText.transform.gameObject.SetActive(true);
        else subtitlesText.transform.gameObject.SetActive(false);
    }
}
