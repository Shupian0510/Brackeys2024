using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUI : MonoBehaviour
{
    Animator animator;
    public GameObject NextCam;
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        animator.Play(gameObject.name);
        Debug.Log(gameObject.name);
        yield return null;  // �ȴ�һ֡��ȷ������״̬����
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.Rebind();  // ȷ��Animator״̬����
        animator.Update(0); // ȷ����������������Ч
        gameObject.SetActive(false);
        NextCam.SetActive(true);
    }
}
