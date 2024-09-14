using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteUI : MonoBehaviour
{
    // �ʼǱ� UI �� GameObject
    public GameObject notebookUI;

    // ��ҽ�ɫ������
    private Character playerCharacter;

    // �ʼǱ� UI �ĳ�ʼ״̬
    private bool isNotebookOpen = false;

    void Start() {
        // ��ȡ��ҽ�ɫ������
        playerCharacter = Character.Instance;
    }

    void Update() {
        // ����Ƿ����� ESC ��
        if (Input.GetKeyDown(KeyCode.Alpha1) && isNotebookOpen) {
            // �л��ʼǱ� UI ��״̬
            ToggleNotebook();
        }
    }

    // �л��ʼǱ� UI ����ʾ״̬
    private void ToggleNotebook() {
        isNotebookOpen = !isNotebookOpen;
        notebookUI.SetActive(isNotebookOpen);

        if (isNotebookOpen) {
            // ��������ӽ�
            playerCharacter.LockCamera(playerCharacter.transform);
        }
        else {
            // ��������ӽ�
            playerCharacter.RestoreCamera();
        }
    }
}

