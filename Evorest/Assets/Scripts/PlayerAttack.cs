using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MeleeAttackController {

    private void Update()
    {
        if (PlayerInput.lightAttackInput) {
            Attack();
        }
    }
}
