using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieState : MonoBehaviour
{
    public virtual ZombieState Tick(ZombieManager zombieManager)
    {
        return this;
    }
}
