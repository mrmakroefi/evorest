using UnityEngine;
using System.Collections;

public interface IDamageable
{
    void Damage(int damage);
    void Knockback(int direction, float amount);
}
