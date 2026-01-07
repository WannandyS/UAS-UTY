using System;
using UnityEngine;
using UnityEngine.Events;

public class MeshZoneTrigger : MonoBehaviour
{
    public string targetTag = "Player";

    public UnityEvent onZoneEnter;

    public UnityEvent onZoneExit;

    private void OnTriggerEnter(Collider other)
    {
        if (isValidObject(other))
        {
            Debug.Log("Enter");
            onZoneEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isValidObject(other))
        {
            Debug.Log("Exit");
            onZoneExit.Invoke();
        }
    }

    private bool isValidObject(Collider other)
    {
        if (string.IsNullOrEmpty(targetTag)) return true;
        return other.CompareTag(targetTag);
    }
}
