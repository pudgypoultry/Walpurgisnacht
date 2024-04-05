using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    GameObject gameObject { get; }
    Transform transform { get; }
    float BaseHP { get; set; }
    float CurrentHP { get; set; }
    bool IsDead { get; }
    void DamageMe(float damageAmount);
    void KillMe();

}
