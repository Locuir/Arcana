using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{

    [Header("Movement Settings")]
    public float moveSpeed;
    public float GroundDrag;

    [Header("Ground Check")]
    public float PlayerHight;
    public LayerMask WhatIsGround;
    bool Grounded;



    public Transform Oriantation;

    float horizontalInput;
    float verticalInput;

    Vector3 MoveDirection;

    Rigidbody rb;




    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    // Update is called once per frame
    void Update()
    {
        Grounded = Physics.Raycast(transform.position, Vector3.down, PlayerHight * .5f + .2f, WhatIsGround);
        ReadInput();

        if (Grounded)
            rb.linearDamping = GroundDrag;
        else
            rb.linearDamping = 0;

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }


    void ReadInput()
    {

        horizontalInput =  Input.GetAxisRaw("Horizontal");
        verticalInput =  Input.GetAxisRaw("Vertical");


    }





    void MovePlayer()
    {

        MoveDirection = Oriantation.forward * verticalInput + Oriantation.right * horizontalInput;

        rb.AddForce(MoveDirection.normalized * moveSpeed * 10f, ForceMode.Force);


    }



    void LimitSpeed()
    {
        Vector3 flatvel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.y);


    }

}
