using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : AttackController {

    protected override void Awake()
    {
        base.Awake();

        GameManager.gm.attackController = this;
    }

    private void Update()
    {
        //print(isAttacking);
        if (PlayerInput.lightAttackInput) {
            Attack();
        }
    }
}
