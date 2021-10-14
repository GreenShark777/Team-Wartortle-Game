using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public abstract void Damage(float value, bool knockBack = false, Vector3 knockPos = default, float knockPower = 1);
}
