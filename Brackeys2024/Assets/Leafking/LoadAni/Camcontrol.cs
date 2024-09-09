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
        yield return null;  // 等待一帧，确保动画状态更新
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.Rebind();  // 确保Animator状态重置
        animator.Update(0); // 确保动画更新立即生效
        gameObject.SetActive(false);
        NextCam.SetActive(true);
    }
}
