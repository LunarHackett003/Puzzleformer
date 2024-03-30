using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableThresholdEvent : MonoBehaviour
{

    [SerializeField] Interactable target;
    [SerializeField] float outputThreshold;
    [SerializeField] UnityEvent eventOnThresholdGreater, eventOnThresholdLess;
    private bool reachedThreshold, triggered;
    private void FixedUpdate()
    {
        reachedThreshold = target.currentOutput > outputThreshold;
        if (reachedThreshold && !triggered)
        {
            eventOnThresholdGreater?.Invoke();
            print($"{gameObject.name} activated!");
            triggered = true;
        }
        else if (triggered && !reachedThreshold)
        {
            eventOnThresholdLess?.Invoke();
            print($"{gameObject.name} deactivated!");
            triggered = false;
        }
    }
}
