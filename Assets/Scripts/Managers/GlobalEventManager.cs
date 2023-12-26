using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventManager
{
    public static UnityEvent<int> OnEnemyKilled = new UnityEvent<int>();

    public static void SendEnemyKilled(int remainingCount)
    {
        OnEnemyKilled.Invoke(remainingCount);
    }
}
