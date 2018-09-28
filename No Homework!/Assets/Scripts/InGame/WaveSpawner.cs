using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    [SerializeField]
    private Transform enemyPrefab;
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private float waveTimeDelay = 5f;
    [SerializeField]
    private float spawnTimeDelay = 0.5f;
    private float countDown = 2f;

    [SerializeField]
    private int waveIndex = 0;

    private void Update()
    {
        if (countDown <= 0)
        {
            StartCoroutine(SpawnWave());
            countDown = waveTimeDelay;
        }

        countDown -= Time.deltaTime;
    }

    private IEnumerator SpawnWave()
    {
        waveIndex++;

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnTimeDelay);
        }
    }

    private void SpawnEnemy()
    {
        GameObject _spawned = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        _spawned.name = "Enemy" + Random.Range(-100, 100);
    }
}
