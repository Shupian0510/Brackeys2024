using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteUI : MonoBehaviour
{
    // 笔记本 UI 的 GameObject
    public GameObject notebookUI;

    // 玩家角色的引用
    private Character playerCharacter;

    // 笔记本 UI 的初始状态
    private bool isNotebookOpen = false;

    void Start() {
        // 获取玩家角色的引用
        playerCharacter = Character.Instance;
    }

    void Update() {
        // 检查是否按下了 ESC 键
        if (Input.GetKeyDown(KeyCode.Alpha1) && isNotebookOpen) {
            // 切换笔记本 UI 的状态
            ToggleNotebook();
        }
    }

    // 切换笔记本 UI 的显示状态
    private void ToggleNotebook() {
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
}

