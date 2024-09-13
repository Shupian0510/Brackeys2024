using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueSpeaker))]
public class DialogueSpeakerCustomInspector : Editor
{
    SerializedProperty subtitlesText,
    useSubtitles,
    dialogues,
    enableScriptFinish,
    endScriptTimer,
    scriptToEnable;

    void OnEnable(){
        subtitlesText = serializedObject.FindProperty("subtitlesText");
        useSubtitles = serializedObject.FindProperty("useSubtitles");
        dialogues = serializedObject.FindProperty("dialogues");
        enableScriptFinish = serializedObject.FindProperty("enableScriptFinish");
        endScriptTimer = serializedObject.FindProperty("endScriptTimer");
        scriptToEnable = serializedObject.FindProperty("scriptToEnable");   
    }

    public override void OnInspectorGUI(){
        var button = GUILayout.Button(Resources.Load("ds_artwork") as Texture, GUILayout.Width(450), GUILayout.Height(255));
        EditorGUILayout.HelpBox("Please don't forget to leave a nice review if you like this package.", MessageType.Info);

        if (button) Application.OpenURL("http://u3d.as/22eA");

        DialogueSpeaker script = (DialogueSpeaker) target;
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Dialogue Options", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(subtitlesText, new GUIContent("Subtitles Text", "TextMeshPro object that will be your subtitle text."));
        EditorGUILayout.PropertyField(useSubtitles, new GUIContent("Use Subtitles", "Print subtitles or not."));
        EditorGUILayout.PropertyField(dialogues, new GUIContent("Dialogues", "Container of all your dialogue options"));
        
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Script Options", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(enableScriptFinish, new GUIContent("Enable Script Finish", "Should a passed script be enabled when the dialogue finishes?"));
        EditorGUI.BeginDisabledGroup (script.enableScriptFinish == false);
            EditorGUILayout.PropertyField(endScriptTimer, new GUIContent("End Script Timer", "The amount of seconds to pass before enabling the script on dialogue finish."));
            EditorGUILayout.PropertyField(scriptToEnable, new GUIContent("Script To Enable", "Pass a disabled script that will be enabled when the dialogue finishes."));
        EditorGUI.EndDisabledGroup ();

        serializedObject.ApplyModifiedProperties();
    }
}
