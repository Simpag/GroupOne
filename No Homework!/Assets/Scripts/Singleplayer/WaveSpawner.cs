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
            SpawnEnemie();
            yield return new WaitForSeconds(spawnTimeDelay);
        }

        Debug.Log("Wave: " + waveIndex);
    }

    private void SpawnEnemie()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
