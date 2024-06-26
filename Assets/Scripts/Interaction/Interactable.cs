using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    [SerializeField] bool canBeGrabbed;
    public bool CanBeGrabbed => canBeGrabbed;
    public float currentOutput;
    [SerializeField] protected TextMeshPro outputTMP;

    public virtual void Interact()
    {
        print($"Interacted with {gameObject.name} at postiion {transform.position}");
    }
    
}
