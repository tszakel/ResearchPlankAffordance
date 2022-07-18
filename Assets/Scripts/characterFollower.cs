using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterFollower : MonoBehaviour
{
    private float fallingSpeed;
    private float gravity = -9.81f;
    private float additionalHeight = 0.2f;
    public CharacterController character;

    public LayerMask groundLayer;
    public Transform headRig;

    private void Update()
    {
        if (headRig.parent.parent.transform.position.y != 0)
            headRig.parent.parent.transform.position = Vector3.zero;
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadSet();

        //gravity
        bool isGrounded = CheckIfGrounded();

        if (isGrounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;

        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    private void CapsuleFollowHeadSet()
    {
        character.height = headRig.position.y + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(headRig.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }

    private bool CheckIfGrounded()
    {
        // tells us if on ground
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
