﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MeleeAttackController {

    protected override void Awake()
    {
        base.Awake();

        GameManager.gm.getPlayerAttack = this;
    }

    private void Update()
    {
        if (PlayerInput.lightAttackInput) {
            Attack();
        }
    }
}
