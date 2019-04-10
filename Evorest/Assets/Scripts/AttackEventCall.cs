using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEventCall : MonoBehaviour {

    public MeleeAttackController controller;

    public void Attack(int index)
    {
        controller.Attack(index);
    }

}
