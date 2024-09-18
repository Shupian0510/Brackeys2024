using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour, IInteractive {
    // 笔记本 UI 的 GameObject
    public GameObject notebookUI, phone;

    // 玩家角色的引用
    private Character playerCharacter;

    // 笔记本 UI 的初始状态
    public bool isNotebookOpen = false;

    private AudioSource audio;
    private bool active = false;

    // 根据响铃状态自动变更的互动文字
    public string GetInteractText() => active ? "Journal" : "";

    private void Start() {
        // 获取玩家角色的引用
        playerCharacter = Character.Instance;

        Character.OnPlayerInteract += (_player, trans) => {
            if (trans == transform && active) {
                OnInteract();
            }
        };
    }

    private void Update() {
        // 检查是否按下了 ESC 键
        if (Input.GetKeyDown(KeyCode.Space) && isNotebookOpen) {
            // 切换笔记本 UI 的状态
            ToggleNotebook();
            if(StoryFlowControl.state == -1) phone.GetComponent<Phone>().ActiveOn();
        }
    }

    // 切换笔记本 UI 的显示状态
    public void ToggleNotebook() {
        isNotebookOpen = !isNotebookOpen;
        notebookUI.SetActive(isNotebookOpen);

        if (isNotebookOpen) {
            // 锁定玩家视角
            playerCharacter.LockCamera(playerCharacter.transform);
        }
        else {
            // 解锁玩家视角
            playerCharacter.RestoreCamera();
        }
    }

    private void OnInteract() {
        // Journal Panel On
        // ActiveOff();
        StorySystem.PlayStoryAudio("int_journal");
        if (StoryFlowControl.state == 1) StoryFlowControl.state = 2;
    }

    // TODO: 音源操作代码需完善
    public void ActiveOn() {
        active = true;
        // audio.Play();
    }

    public void ActiveOff() {
        active = false;
        // audio.Stop();
    }
}
