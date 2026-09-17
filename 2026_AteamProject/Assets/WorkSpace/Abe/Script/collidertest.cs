using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class collidertest : MonoBehaviour
{
    [SerializeField] private GameObject sphere = null;
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Instantiate(sphere, new Vector3(0.0f, 1.0f, 5.0f), Quaternion.identity);
        }
    }
}
