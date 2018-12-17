using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    private static WaveSpawner instance;
    public static WaveSpawner Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    [SerializeField]
    private Transform enemyPrefab;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private float firstTimeBeforeSpawn;

    [SerializeField]
    private float algTimeBeforeStart;
    [SerializeField]
    private float algSpawnDelay;

    [SerializeField]
    private int waveIndex = 0;
    private int enemyIndex = 0;

    private bool isSpawning;
    private float countDown;
    private int totalEnemiesSpawned;

    [SerializeField]
    public List<float> enemiesWalkDistance;
    public static float GetEnemyDistance(int _eIndex) { return Instance.enemiesWalkDistance[_eIndex]; }
    public static void EnemyWalkDistanceToList(int _eIndex, float _distance) { if (Instance.enemiesWalkDistance.Count <= _eIndex) { Instance.enemiesWalkDistance.Add(_distance); } else { Instance.enemiesWalkDistance[_eIndex] = _distance; } }

    [SerializeField]
    public List<float> enemiesHealth;
    public static float GetEnemyHealth(int _eIndex) { return Instance.enemiesHealth[_eIndex]; }
    public static void EnemyHealthToList(int _eIndex, float _health) { if (Instance.enemiesHealth.Count <= _eIndex) { Instance.enemiesHealth.Add(_health); } else { Instance.enemiesHealth[_eIndex] = _health; } }

    [SerializeField]
    private WaveStats[] waves;

    private void Awake()
    {
        //Create singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        countDown = firstTimeBeforeSpawn;
        totalEnemiesSpawned = 0;
    }

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
        if (waves[waveIndex].isBossRound) //If its a boss round play bossround sound
        {
            AudioManager.Instance.Stop("InGameMusic");
            AudioManager.Instance.Play("BossRoundSound");
        }
        else
        {
            if (AudioManager.Instance.isPlaying("BossRoundSound"))
            {
                AudioManager.Instance.Stop("BossRoundSound");
                AudioManager.Instance.Play("InGameMusic");
            }

            AudioManager.Instance.Play("EndOfRoundSound"); //replace with round start sound later
        }

        isSpawning = true;
        enemyIndex = 0;
        countDown = waves[waveIndex].timeBeforeStart;

        for (int i = 0; i < waves[waveIndex].EnemyPrefabs.Length; i++)
        {
            SpawnEnemy(waves[waveIndex].EnemyPrefabs[enemyIndex]);
            enemyIndex++;
            yield return new WaitForSeconds(waves[waveIndex].spawnDelay); // wait to spawn next enemy
        }

        isSpawning = false;
        waveIndex++;
    }

    private IEnumerator SpawnWaveFromAlg()
    {
        if (AudioManager.Instance.isPlaying("BossRoundSound"))
        {
            AudioManager.Instance.Stop("BossRoundSound");
            AudioManager.Instance.Play("InGameMusic");
        }

        AudioManager.Instance.Play("EndOfRoundSound"); //replace with round start sound later

        //0.03x^2+2sin(x)+5
        waveIndex++;
        isSpawning = true;
        enemyIndex = 0;
        countDown = algTimeBeforeStart;

        int _amountOfEnemies = (int)(Mathf.RoundToInt((float)(0.03 * Mathf.Pow(waveIndex, 2) + Mathf.Sin(waveIndex) + 5)) * 2);

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
        _spawned.name = totalEnemiesSpawned.ToString();
        totalEnemiesSpawned++;
    }
}
