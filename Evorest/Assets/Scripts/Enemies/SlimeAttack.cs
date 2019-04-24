using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : AttackController
{
    private SlimeMovement movement;

    private float attackCooldown = 0;

    protected override void Awake()
    {
        base.Awake();
        movement = (SlimeMovement)motor;
    }

    private void Update()
    {
        if (movement.GetDistanceToTargetX() <= movement.minDistance && Time.time >= attackCooldown) {
            Attack();
            attackCooldown = Time.time + 2f;
        }
    }
}
