using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeMotorController : MonoBehaviour
{
    [SerializeField] HingeJoint hinge;
    [SerializeField] JointMotor hingeMotor;
    [SerializeField] float motorForce;
    [SerializeField] float motorTargetVelocity;
    [SerializeField] bool motorFreeSpin;
    public bool usingMotor;
    public void SetMotor(bool activated)
    {
        hingeMotor = new()
        {
            force = motorForce,
            freeSpin = motorFreeSpin,
            targetVelocity = motorTargetVelocity
        };

        usingMotor = activated;
        hinge.motor = hingeMotor;
        hinge.useMotor = usingMotor;
    }
}
