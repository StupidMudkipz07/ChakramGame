using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public interface IDamageable
{   
    void TakeDamage(float damage);
}