using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEventCall : MonoBehaviour {

    public MeleeAttackController attackController;
    public CharacterMotor motor;

    public void Attack(int index)
    {
        attackController.Attack(index);
    }

    public void Dash(int index)
    {
        float dashTime = attackController.meleeCombos[index].dashTime;
        float dashDistance = attackController.meleeCombos[index].dashDistance;
        if (dashTime > 0) {
            motor.Dash(motor.getFacingDir, dashTime, dashDistance, true);
        }
    }



}
