using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RigidbodyMotor : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider col;
    [SerializeField] float groundMoveForce, airMoveForce, jumpForce, groundDrag, airDrag;
    [SerializeField] float moveMultiplierPerKG, currentInteractedMass;

    [SerializeField] Vector2 lookInput, moveInput, lookSpeed;
    [SerializeField] Transform head;
    Vector2 lookAngle;

    [SerializeField] float groundCheckRadius, groundCheckDistance, maxWalkableSlopeDot;
    [SerializeField] LayerMask groundCheckLayer;
    [SerializeField] bool grounded;

    [SerializeField] float interactRange, interactMaxMass, interactPullForce, interactDamper, interactSpring;
    [SerializeField] LayerMask interactMask;
    [SerializeField] bool interacting;
    [SerializeField] Rigidbody currentInteractedObject;
    ConfigurableJoint joint;
    JointDrive posJointDrive, rotJointDrive;
    [SerializeField] Vector3 interactLocalPosition;
    [SerializeField] float grabBreakDistance;
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
    private void LateUpdate()
    {
        lookAngle += lookInput * lookSpeed * Time.smoothDeltaTime;
        lookAngle.y = Mathf.Clamp(lookAngle.y, -80, 80);
        head.localRotation = Quaternion.Euler(-lookAngle.y,0, 0);
        transform.rotation = Quaternion.Euler(0, lookAngle.x, 0);
    }
    private void FixedUpdate()
    {
        grounded = IsGrounded();
        rb.AddRelativeForce((grounded ? groundMoveForce : airMoveForce) * (interacting ? 1 + (moveMultiplierPerKG * currentInteractedMass) : 1) * new Vector3(moveInput.x, 0, moveInput.y));
        rb.drag = grounded ? groundDrag : airDrag;

        if(interacting && joint)
        {
            Vector3 headForward = (head.forward * interactRange);
            joint.anchor = transform.InverseTransformDirection(headForward);
            if (Vector3.Distance(head.position + headForward, currentInteractedObject.position) > grabBreakDistance)
            {
                print("Grabbed object too far!");
                ReleaseGrab();
            }
        }
    }
    bool IsGrounded()
    {
        return Physics.SphereCast(transform.position, groundCheckRadius, -transform.up, out RaycastHit hit, groundCheckDistance, groundCheckLayer) && Vector3.Dot(hit.normal, transform.up) > maxWalkableSlopeDot;
    }
    public void Jump()
    {
        if (grounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    public void TryInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        if (!interacting) {
            if (Physics.Raycast(head.position, head.forward, out RaycastHit hit, interactRange, interactMask))
            {
                if (hit.rigidbody && hit.rigidbody.mass < interactMaxMass)
                {
                    if (hit.rigidbody.TryGetComponent(out Interactable i))
                    {
                        i.Interact();
                        if (i.CanBeGrabbed)
                        {
                            interacting = true;
                            currentInteractedObject = hit.rigidbody;

                            posJointDrive = new JointDrive()
                            {
                                maximumForce = interactPullForce * interactMaxMass,
                                positionSpring = interactSpring * hit.rigidbody.mass,
                                positionDamper = interactDamper
                            };
                            rotJointDrive = posJointDrive;

                            joint = gameObject.AddComponent<ConfigurableJoint>();
                            joint.connectedBody = currentInteractedObject;
                            joint.autoConfigureConnectedAnchor = false;
                            joint.rotationDriveMode = RotationDriveMode.Slerp;
                            joint.connectedAnchor = Vector3.zero;
                            joint.anchor = transform.InverseTransformDirection(head.forward * interactRange);
                            joint.xDrive = posJointDrive;
                            joint.yDrive = posJointDrive;
                            joint.zDrive = posJointDrive;
                            joint.slerpDrive = rotJointDrive;
                            currentInteractedMass = currentInteractedObject.mass;
                        }
                    }
                }
                else
                {
                    print("Could not interact with this object");
                }
            }
        }
        else
        {
            ReleaseGrab();
        }
    }
    void ReleaseGrab()
    {
        interacting = false;
        Destroy(joint);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
        Gizmos.DrawWireSphere(transform.position - transform.up * groundCheckDistance, groundCheckRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(head.position, head.forward * interactRange);
    }
}
