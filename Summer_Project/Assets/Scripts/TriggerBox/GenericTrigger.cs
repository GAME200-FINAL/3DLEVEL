using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a generic trigger. 
/// When player enters the attached trigger box, it will send message to its receiver.
/// </summary>

// This component must attach to a gameobject with any type of collider.
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(TriggerReceiver))]
public class GenericTrigger : MonoBehaviour
{
    [Header("General Settings")]
    public GameObject targetObject;
    public bool triggerOnce = true;
    public bool triggeredByPlayer = false;
    public bool triggeredByEnemy = false;
    public bool triggeredByObj = false;

    // Use this for initialization
    private void Start()
    {
        // Double check the object is not static.
        if (targetObject)
        {
            if (targetObject.isStatic)
                targetObject.isStatic = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggeredByPlayer)
        {
            if (other.CompareTag("Player"))
            {
                GetComponent<TriggerReceiver>().ToggleTrigger();

                if (triggerOnce)
                    transform.GetComponent<BoxCollider>().enabled = false;
            }
        }

        if (triggeredByEnemy)
        {
            if (other.CompareTag("Enemy"))
            {
                GetComponent<TriggerReceiver>().ToggleTrigger();

                if (triggerOnce)
                    transform.GetComponent<BoxCollider>().enabled = false;
            }
        }

        if (triggeredByObj)
        {
            if (other.CompareTag("ObjectTrigger"))
            {
                GetComponent<TriggerReceiver>().ToggleTrigger();
                if (triggerOnce)
                    transform.GetComponent<BoxCollider>().enabled = false;
            }

            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (triggeredByPlayer)
        {
            if (other.CompareTag("Player"))
                GetComponent<TriggerReceiver>().ToggleTrigger();
        }

        if (triggeredByEnemy)
        {
            if (other.CompareTag("Enemy"))
                GetComponent<TriggerReceiver>().ToggleTrigger();
        }

        if (triggeredByObj)
        {
            if (other.CompareTag("ObjectTrigger"))
                GetComponent<TriggerReceiver>().ToggleTrigger();
        }
    }
}
