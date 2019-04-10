using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackController : MonoBehaviour {

    private Animator anim;

    [System.Serializable]
    public struct Combos
    {
        [Tooltip("just for naming the move, can be blank.")]
        public string name;
        public Rect attackArea;
        public int damage;
        public float knockbackPower;
    };

    public Combos[] meleeCombos;

    [HideInInspector]
    private bool preview = false;
    [HideInInspector]
    private Combos previewCombo;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();    
    }

    public void Attack()
    {
        anim.SetBool("L", true);
    }

    public void Attack(int index)
    {
        Vector2 hitboxPos = new Vector2(
            GameManager.gm.getPlayer.getFacingDir * meleeCombos[index].attackArea.center.x,
            meleeCombos[index].attackArea.center.y);
        Collider2D[] colliders = Physics2D.OverlapBoxAll( (Vector2)transform.position + hitboxPos, meleeCombos[index].attackArea.size, 0);
        for (int i = 0;i < colliders.Length; i++) {
            // do something here on hit something

            IDamageable obj = colliders[i].GetComponent<IDamageable>();
            if (obj != null) {
                // apply damage here
                obj.Damage(meleeCombos[index].damage);
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
