using UnityEngine;
using System.Collections;

public interface IDamageable
{
    void Damage(float damage);
    void Knockback(int direction, float amount);
}
