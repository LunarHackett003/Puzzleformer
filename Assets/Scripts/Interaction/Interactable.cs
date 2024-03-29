using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    [SerializeField] bool canBeGrabbed;
    public bool CanBeGrabbed => canBeGrabbed;
    public float currentOutput;

    public virtual void Interact()
    {
        print($"Interacted with {gameObject.name} at postiion {transform.position}");
    }
    
}
