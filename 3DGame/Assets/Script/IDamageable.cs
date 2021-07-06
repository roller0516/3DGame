using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    // Start is called before the first frame update
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
