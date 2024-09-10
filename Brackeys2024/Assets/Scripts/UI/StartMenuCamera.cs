using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StartMenuCamera : MonoBehaviour
{
    public GameObject NextCamera;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (gameObject.activeInHierarchy)
            PlayAnimation();
    }

    public void PlayAnimation()
    {
        animator.Play(gameObject.name);
        var clipLength = animator.runtimeAnimatorController.animationClips.Sum(x => x.length);
        print(clipLength);
        Invoke(nameof(JumpToNextCamera), clipLength);
    }

    private void JumpToNextCamera()
    {
        print("get");
        animator.Rebind();
        animator.Update(0);
        gameObject.SetActive(false);
        NextCamera.SetActive(true);
        if (NextCamera.TryGetComponent<StartMenuCamera>(out var component))
            component.PlayAnimation();
    }
}
