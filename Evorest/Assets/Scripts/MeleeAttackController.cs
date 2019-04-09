using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackController : MonoBehaviour {

    private Animator anim;

    [System.Serializable]
    public struct Combos
    {
        public Bounds attackArea;
        public int damage;
        public float knockbackPower;
    };

    public Combos[] meleeCombos;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();    
    }

    private void Update()
    {
        if (PlayerInput.lightAttackInput) {
            Attack();
        }
    }

    protected void Attack()
    {
        anim.SetBool("L", true);
    }

}
