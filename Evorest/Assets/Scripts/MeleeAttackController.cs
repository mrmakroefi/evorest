using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackController : MonoBehaviour {

    public enum TargetTag { Player, Enemy }

    private Animator anim;
    public CharacterMotor motor { get; private set; }

    [System.Serializable]
    public struct Combos
    {
        [Tooltip("just for naming the move, can be blank.")]
        public string name;
        public Rect attackArea;
        public int damage;
        public float knockbackPower;
        public float dashTime;
        public float dashDistance;
    };

    public TargetTag target;
    public Combos[] meleeCombos;

    private bool preview = false;
    private Combos previewCombo;
    [HideInInspector]
    public bool isAttacking;

    protected virtual void Awake()
    {
        motor = GetComponent<CharacterMotor>();
        anim = GetComponentInChildren<Animator>();    
    }

    public void Attack()
    {
        anim.SetBool("L", true);
    }
    
    public void Attack(int index)
    {
        Vector2 hitboxPos = new Vector2(
            motor.getFacingDir * meleeCombos[index].attackArea.center.x,
            meleeCombos[index].attackArea.center.y);
        Collider2D[] colliders = Physics2D.OverlapBoxAll( (Vector2)transform.position + hitboxPos, meleeCombos[index].attackArea.size, 0);
        for (int i = 0;i < colliders.Length; i++) {
            // do something here on hit something

            if ((target == TargetTag.Enemy && colliders[i].CompareTag("Enemy")) ||
                (target == TargetTag.Player && colliders[i].CompareTag("Player"))) {
                IDamageable obj = colliders[i].GetComponent<IDamageable>();
                if (obj != null) {
                    // apply damage here
                    obj.Damage(meleeCombos[index].damage);
                    obj.Knockback(motor.getFacingDir, meleeCombos[index].knockbackPower);
                }
            }
        }
    }

    public void UpdatePreviewHitbox(Combos combo, bool preview)
    {
        previewCombo = combo;
        this.preview = preview;
    }

    private void OnDrawGizmos()
    {
        if (preview) {
            Gizmos.color = new Color(0, 1f, 0, 0.2f);
            Gizmos.DrawCube(new Vector3(transform.position.x + previewCombo.attackArea.center.x, transform.position.y + previewCombo.attackArea.center.y),
                new Vector3(previewCombo.attackArea.width, previewCombo.attackArea.height));
        }
    }
}
