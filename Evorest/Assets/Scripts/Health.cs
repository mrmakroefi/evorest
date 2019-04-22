using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable {

    private Animator anim;
    public GameObject damagePopUp;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Damage(float damage)
    {
        GameObject dmg = Instantiate(damagePopUp, transform.position + transform.up * Random.Range(0.3f,0.5f) + transform.right * Random.Range(-.3f, .3f), Quaternion.identity);

        dmg.GetComponentInChildren<TextMesh>().text = damage.ToString(); ;

        anim.SetTrigger("hurt");
        print("Aww, recieving " + damage + " damages!");
    }
    
}
