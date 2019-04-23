﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMotor {

    private Animator anim;

    protected override void Awake()
    {
        base.Awake();
        GameManager.gm.getPlayer = this;
        // find first animator in children
        anim = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        anim.SetFloat("move", Mathf.Abs(getRb2D.velocity.x) * 0.5f);
        anim.SetBool("grounded", isGrounded);
        anim.SetFloat("verticalVelocity", getRb2D.velocity.y);
        anim.SetBool("isDashing", isDashing);
        anim.SetBool("isMoving", PlayerInput.horizontalInput != 0);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float moveInput = 0;
        if (!GameManager.gm.getPlayerAttack.isAttacking) {
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
