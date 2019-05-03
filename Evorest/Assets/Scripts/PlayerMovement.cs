using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMotor {
    
    protected override void Awake()
    {
        base.Awake();
        GameManager.gm.motor = this;
    }

    protected override void Update()
    {
        base.Update();
        anim.SetFloat("move", Mathf.Abs(rb2D.velocity.x) * 0.5f);
        anim.SetBool("grounded", isGrounded);
        anim.SetFloat("verticalVelocity", rb2D.velocity.y);
        anim.SetBool("isDashing", isDashing);
        anim.SetBool("isMoving", PlayerInput.horizontalInput != 0);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float moveInput = 0;
        if (!GameManager.gm.attackController.isAttacking) {
            moveInput = PlayerInput.horizontalInput;
        }
        Move(moveInput);

        if (PlayerInput.jumpInput) {
            Jump();
        }

        if (PlayerInput.dashInput && !isDashing) {
            Dash(getFacingDir, dashTime, dashDistance, false, true);
        }
    }

}
