using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    private Animator anim;
    public CharacterMotor motor;
    public GameObject damagePopUp;
    public Material flashMaterial;
    public float flashTime = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Material normalMaterial;
    private float currentFlashTime = 0;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        normalMaterial = spriteRenderer.material;
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (currentFlashTime > 0) {
            currentFlashTime -= Time.deltaTime;
            if (currentFlashTime <= 0) {
                spriteRenderer.material = normalMaterial;
            }
        }
    }

    public void Damage(float damage)
    {
        GameObject dmg = Instantiate(damagePopUp, transform.position + transform.up * Random.Range(0.3f, 0.5f) + transform.right * Random.Range(-.3f, .3f), Quaternion.identity);

        dmg.GetComponentInChildren<TextMesh>().text = damage.ToString(); ;

        anim.SetTrigger("hurt");
        //print("Aww, recieving " + damage + " damages!");

        spriteRenderer.material = flashMaterial;
        currentFlashTime = flashTime;
    }
    
    public void Knockback(int direction, float amount)
    {
        motor.Dash(direction, 0.15f, amount, true);
    }

}
