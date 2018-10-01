using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    [SerializeField]
    private Transform enemyPrefab;
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private float algTimeBeforeStart;
    [SerializeField]
    private float algSpawnDelay;

    [SerializeField]
    private int waveIndex = 0;
    private int enemyIndex = 0;

    private bool isSpawning;
    private float countDown = 2f;

    [SerializeField]
    private WaveStats[] waves;

    private void Update()
    {
        if (countDown <= 0 && waveIndex < waves.Length)
        {
            StartCoroutine(SpawnWaveFromArray());
        }
        else if (countDown <= 0)
        {
            StartCoroutine(SpawnWaveFromAlg());
        }

        if (!isSpawning)
            countDown -= Time.deltaTime; //Decrement time by one each second
    }

    private IEnumerator SpawnWaveFromArray()
    {
        waveIndex++;
        isSpawning = true;
        enemyIndex = 0;
        countDown = waves[waveIndex-1].timeBeforeStart;

        for (int i = 0; i < waves[waveIndex-1].EnemyPrefabs.Length; i++)
        {
            SpawnEnemy(waves[waveIndex-1].EnemyPrefabs[enemyIndex]);
            enemyIndex++;
            yield return new WaitForSeconds(waves[waveIndex-1].spawnDelay); // wait to spawn next enemy
        }

        isSpawning = false;
    }

    private IEnumerator SpawnWaveFromAlg()
    {
        //0.03x^2+2sin(x)+5
        waveIndex++;
        isSpawning = true;
        enemyIndex = 0;
        countDown = algTimeBeforeStart;

        int _amountOfEnemies = Mathf.RoundToInt((float)(0.03 * Mathf.Pow(waveIndex, 2) + Mathf.Sin(waveIndex) + 5));

        for (int i = 0; i < _amountOfEnemies; i++)
        {
            SpawnEnemy(waves[0].EnemyPrefabs[0]); //Replace with enemy prefab lateron
            yield return new WaitForSeconds(algSpawnDelay); // wait to spawn next enemy
        }

        isSpawning = false;
    }

    private void SpawnEnemy(GameObject _prefab)
    {
        GameObject _spawned = Instantiate(_prefab, spawnPoint.position, spawnPoint.rotation);
        _spawned.name = "Enemy " + enemyIndex.ToString();
    }
}
