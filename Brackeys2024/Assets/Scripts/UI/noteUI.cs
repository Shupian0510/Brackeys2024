using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour, IInteractive {
    // �ʼǱ� UI �� GameObject
    public GameObject notebookUI, phone;

    // ��ҽ�ɫ������
    private Character playerCharacter;

    // �ʼǱ� UI �ĳ�ʼ״̬
    public bool isNotebookOpen = false;

    private AudioSource audio;
    private bool active = false;

    // ��������״̬�Զ�����Ļ�������
    public string GetInteractText() => active ? "Journal" : "";

    private void Start() {
        // ��ȡ��ҽ�ɫ������
        playerCharacter = Character.Instance;

        Character.OnPlayerInteract += (_player, trans) => {
            if (trans == transform && active) {
                OnInteract();
            }
        };
    }

    private void Update() {
        // ����Ƿ����� ESC ��
        if (Input.GetKeyDown(KeyCode.Space) && isNotebookOpen) {
            // �л��ʼǱ� UI ��״̬
            ToggleNotebook();
            if(StoryFlowControl.state == -1) phone.GetComponent<Phone>().ActiveOn();
        }
    }

    // �л��ʼǱ� UI ����ʾ״̬
    public void ToggleNotebook() {
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

    private void OnInteract() {
        // Journal Panel On
        // ActiveOff();
        StorySystem.PlayStoryAudio("int_journal");
        if (StoryFlowControl.state == 1) StoryFlowControl.state = 2;
    }

    // TODO: ��Դ��������������
    public void ActiveOn() {
        active = true;
        // audio.Play();
    }

    public void ActiveOff() {
        active = false;
        // audio.Stop();
    }
}
