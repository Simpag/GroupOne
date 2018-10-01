using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveStats {
    [Header("Time before next wave starts after last enemy in previous wave spawned")]
    public float timeBeforeStart;
    public float spawnDelay;

    public GameObject[] EnemyPrefabs;
}
