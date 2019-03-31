using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : CharacterController {

    private float horizontalInput;
    private bool jumpInput;
    
    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (!jumpInput) {
            jumpInput = Input.GetKeyDown(KeyCode.Space);
        }

    }

    private void FixedUpdate()
    {
        float targetVelocity = horizontalInput * maxSpeed;
        Move(targetVelocity);
        if (jumpInput) {
            Jump();
            jumpInput = false;
        }
    }

}
