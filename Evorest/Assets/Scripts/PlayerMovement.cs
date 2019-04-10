﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMotor {

    private Animator anim;
    private bool canDash = false;

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
    }

    protected void FixedUpdate()
    {
        Move(PlayerInput.horizontalInput);
        if (PlayerInput.jumpInput) {
            Jump();
        }

        if (PlayerInput.dashInput && !isDashing || canDash && isDashing) {
            canDash = Dash(getFacingDir);
        }
    }

}