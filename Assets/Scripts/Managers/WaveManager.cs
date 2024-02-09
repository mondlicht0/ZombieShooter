using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    public static int EnemyCount;

    private void Start()
    {
        //EnemyCount = FindObjectsOfType<NPCAgent>().Length;
    }
}
