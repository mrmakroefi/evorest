using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SlimeMovement))]
public class SlimeAttack : AttackController
{
    private SlimeMovement movement;

    private float attackCooldown = 0;

    protected  void Start()
    {
        movement = (SlimeMovement)motor;
    }

    private void Update()
    {
        if (movement.GetDistanceToTargetX() <= movement.minDistance && Time.time >= attackCooldown && movement.inRange) {
            Attack();
            attackCooldown = Time.time + 2f;
        }
    }
}
