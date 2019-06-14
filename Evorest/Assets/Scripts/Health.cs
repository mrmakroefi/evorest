using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    private Animator anim;
    public CharacterMotor motor;
    public int initialHP = 100;
    public GameObject damagePopUp;
    public Material flashMaterial;
    public float flashTime = 0.1f;

    public int HP { get; private set; }

    private SpriteRenderer spriteRenderer;
    private Material normalMaterial;
    private float currentFlashTime = 0;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        normalMaterial = spriteRenderer.material;
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        HP = initialHP;
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

    private void UpdateHP()
    {
        if (HP <= 0) {
            // dead
            print(transform.name + " was defeated!");
            HP = initialHP;
        }
    }

    public void Damage(int damage)
    {
        GameObject dmg = Instantiate(damagePopUp, transform.position + transform.up * Random.Range(0.3f, 0.5f) + transform.right * Random.Range(-.3f, .3f), Quaternion.identity);

        dmg.GetComponentInChildren<TextMesh>().text = damage.ToString(); ;

        anim.SetTrigger("hurt");
        //print("Aww, recieving " + damage + " damages!");

        spriteRenderer.material = flashMaterial;
        currentFlashTime = flashTime;

        HP -= damage;
        UpdateHP();
    }
    
    public void Knockback(int direction, float amount)
    {
        if (motor != null) {
            motor.Dash(direction, 0.15f, amount, true);
        }
    }

}
