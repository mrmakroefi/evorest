using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : CharacterMotor {

    public float horizontalInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool dashInput { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        GameManager.gm.player = this;
    }


    protected override void Update()
    {
        base.Update();
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (!dashInput && !isDashing) {
            dashInput = Input.GetKeyDown(KeyCode.K);
        }
        
        if (!jumpInput) {
            jumpInput = Input.GetKeyDown(KeyCode.Space);
        }

    }

    protected void FixedUpdate()
    {
        float targetVelocity = horizontalInput * maxSpeed;
        Move(targetVelocity);
        if (jumpInput) {
            Jump();
            jumpInput = false;
        }

        if (dashInput) {
            dashInput = Dash(facingRight ? 1 : -1);
        }
    }

}
