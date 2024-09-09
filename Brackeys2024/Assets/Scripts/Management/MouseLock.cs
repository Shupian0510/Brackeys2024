using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for testing only
/// </summary>
public class MouseLock : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
