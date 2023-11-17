using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Grounded : MonoBehaviour
{
     public bool isGrounded;

    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckDistance, whatIsGround);
    }
}
