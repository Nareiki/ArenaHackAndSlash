using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public float HealthPoints { get; }
    public bool TakeHit();

}
