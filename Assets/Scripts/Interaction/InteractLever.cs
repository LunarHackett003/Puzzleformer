using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractLever : Interactable
{
    [SerializeField] Vector2 minMaxAngle;
    [SerializeField] float offsetAngle;
    [SerializeField] Vector2 minMaxOutput;
    public override void Interact()
    {
        base.Interact();
    }
    private void Start()
    {
        transform.localRotation = Quaternion.Euler(minMaxAngle.x, 0, 0);
    }
    private void FixedUpdate()
    {
        float angle = transform.localEulerAngles.x; 

        if (angle > 180)
            angle -= 360;
        currentOutput = Mathf.Lerp(minMaxOutput.x, minMaxOutput.y, Mathf.InverseLerp(minMaxAngle.x, minMaxAngle.y, angle));
        if (outputTMP)
        {
            outputTMP.text = $"{currentOutput:0}/{minMaxOutput.y}";
        }
    }
}
