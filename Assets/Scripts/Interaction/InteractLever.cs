using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractLever : Interactable
{
    [SerializeField] Vector2 minMaxAngle;
    [SerializeField] float offsetAngle;
    [SerializeField] TextMeshPro outputTMP;
    private enum Axis
    {
        x = 0,
        y = 1,
        z = 2,
    }
    [SerializeField]
    private Axis axis;
    [SerializeField] Vector2 minMaxOutput;
    public override void Interact()
    {
        base.Interact();
    }

    private void FixedUpdate()
    {
        float angle = axis switch 
        { 
            Axis.x => transform.localEulerAngles.x, 
            Axis.y => transform.localEulerAngles.y,
            Axis.z => transform.localEulerAngles.z,
            _ => 0 
        };
        currentOutput = Mathf.Lerp(minMaxOutput.x, minMaxOutput.y, Mathf.InverseLerp(minMaxAngle.x, minMaxAngle.y, angle + offsetAngle));
        if (outputTMP)
        {
            outputTMP.text = $"{currentOutput:0}/{minMaxOutput.y}";
        }
    }
}
