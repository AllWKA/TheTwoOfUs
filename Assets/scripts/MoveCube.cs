using System;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    public GameObject foward, back, left, right;
    public Boolean isPlayerIn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.isPlayerIn = true;
            SendMessageUpwards("EnterMoveCube", this.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.isPlayerIn && other.gameObject.CompareTag("Player"))
        {
            this.isPlayerIn = false;
            SendMessageUpwards("ExitMoveCube", this.gameObject.name);
        }
    }
}
