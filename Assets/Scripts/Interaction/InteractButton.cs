using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractButton : Interactable
{
    [SerializeField] float onOutput, offOutput;
    [SerializeField] Transform buttonTransform;
    [SerializeField] Vector3 unpressedPosition, pressedPosition;
    [SerializeField] float pressSpeed;
    [SerializeField] bool pressed;
    public override void Interact()
    {
        base.Interact();
        Press(!pressed);
    }
    public void Press(bool pressed)
    {
        this.pressed = pressed;
        StartCoroutine(PressButton());
    }
    IEnumerator PressButton()
    {
        float time = 0;
        Vector3 end = pressed ? ref pressedPosition : ref unpressedPosition;
        Vector3 start = pressed ? ref unpressedPosition : ref pressedPosition;
        float inc = Time.fixedDeltaTime * pressSpeed;
        var w = new WaitForFixedUpdate();
        while (time < 1)
        {
            time += inc;
            buttonTransform.localPosition = Vector3.Lerp(start, end, time);
            yield return w;
        }
        currentOutput = pressed ? onOutput : offOutput;
        yield break;
    }
}
