using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable {
    
    public void Damage(float damage)
    {
        print("Aww, recieving " + damage + " damages!");
    }
    
}
