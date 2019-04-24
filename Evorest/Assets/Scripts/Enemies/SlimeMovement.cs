using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : CharacterMotor
{   
    public GameObject target { get; private set; }

    public float minDistance = 0.3f;

    protected override void Awake()
    {
        base.Awake();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        int direction = (int)Mathf.Sign(target.transform.position.x - transform.position.x); ;
        if (GetDistanceToTargetX() <= minDistance && (direction + getFacingDir != 0)) {
            direction = 0;
        }

        Move(direction);
    }

    protected override void Update()
    {
        base.Update();
        anim.SetFloat("move", Mathf.Abs(getRb2D.velocity.x) * 0.5f);
    }

    public float GetDistanceToTargetX()
    {
        return Mathf.Abs(transform.position.x - target.transform.position.x);
    }
    
}
