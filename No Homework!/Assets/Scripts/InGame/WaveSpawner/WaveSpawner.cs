using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    [System.Serializable]
    public struct WaveStats
    {
        [Header("Time before next wave starts after last enemy in previous wave spawned")]
        public float spawnDelay;

        [Header("Boss-wave sound")]
        public bool isBossRound;

        public GameObject[] TeacherPrefabs;
    }

    private static WaveSpawner instance;
    public static WaveSpawner Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    [SerializeField]
    private int waveIndex;

    [Header("Setup")]
    [SerializeField]
    private GameObject[] teacherPrefabs;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private Transform teacherContainer;

    [SerializeField]
    private WaveStats[] waves;

    [SerializeField]
    private float algSpawnDelay;

    //Private variables
    private bool isSpawning;
    private int totalTeachersSpawned;
    private int teachersOnScreen;
    private int ticketSum;

    public int WaveIndex
    {
        get { return waveIndex; }
        set { waveIndex = value; }
    }
    public int TeachersOnScreen
    {
        get { return teachersOnScreen; }
        set
        {
            teachersOnScreen = value;

            if (teachersOnScreen <= 0 && GameManager.IsMultiplayer)
                MultiplayerManager.SendRoundEndInformation(PlayerStats.Homework);
        } 
    }

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

        totalTeachersSpawned = 0;
        teachersOnScreen = 0;
        ticketSum = 0;
        waveIndex = 0;

        CalculateTicketSum();
    }

    public void NextRound()
    {
        if (isSpawning || !MultiplayerManager.Instance.IsPartnerRoundDone || teachersOnScreen > 0)
            return;

        if (GameManager.IsMultiplayer)
            MultiplayerManager.SendStartOfRound();

        waveIndex++;

        if (waveIndex < waves.Length)
        {
            StartCoroutine(SpawnWaveFromArray());
        }
        else
        {
            SpawnWaveFromAlg();
        }

        InGameUIManager.UpdateWaveIndex();
    }

    private IEnumerator SpawnWaveFromArray()
    {
        //Play the wave start sound
        if (waves[waveIndex-1].isBossRound) //If its a boss round play bossround sound
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

        //Spawn enemies
        isSpawning = true;

        for (int i = 0; i < waves[waveIndex-1].TeacherPrefabs.Length; i++)
        {
            SpawnTeacher(waves[waveIndex-1].TeacherPrefabs[i], teacherContainer);
            yield return new WaitForSeconds(waves[waveIndex-1].spawnDelay); // wait to spawn next enemy
        }

        isSpawning = false;
    }

    private void SpawnWaveFromAlg()
    {
        if (AudioManager.Instance.isPlaying("BossRoundSound"))
        {
            AudioManager.Instance.Stop("BossRoundSound");
            AudioManager.Instance.Play("InGameMusic");
        }

        AudioManager.Instance.Play("EndOfRoundSound"); //replace with round start sound later

        //0.03x^2+2sin(x)+5
        int _budget = (Mathf.RoundToInt(0.03f * Mathf.Pow(waveIndex, 2) + 2f * Mathf.Sin(waveIndex) + 5) * 100);

        StartCoroutine(SpawnTeachers(_budget, teacherContainer, algSpawnDelay));
    }

    private void CalculateTicketSum()
    {
        foreach (GameObject _tGo in teacherPrefabs)
        {
            ticketSum += _tGo.GetComponent<TeacherStats>().TicketsForSpawning;
        }
    }

    public IEnumerator SpawnTeachers(int _budget, Transform _container, float _delay)
    {
        isSpawning = true;

        List<GameObject> _teachersToSpawn = new List<GameObject>();

        while (_budget > 0)
        {
            GameObject _teacher = TeacherLottery();
            _budget -= (int)_teacher.GetComponent<TeacherStats>().Worth;

            _teachersToSpawn.Add(_teacher);
        }

        for (int i = 0; i < _teachersToSpawn.Count; i++)
        {
            SpawnTeacher(_teachersToSpawn[i], _container); //Replace with enemy prefab lateron
            yield return new WaitForSeconds(_delay); // wait to spawn next enemy
        }

        isSpawning = false;
    }

    private GameObject TeacherLottery()
    {
        GameObject _winner = teacherPrefabs[0]; //Default enemy fallback;

        int _index = 0;
        int _winningTicket = Random.Range(1, ticketSum);

        while (_winningTicket > 0)
        {
            _winner = teacherPrefabs[_index];

            _index++;
            if (_index >= teacherPrefabs.Length)
            {
                _index = 0;
            }

            _winningTicket -= _winner.GetComponent<TeacherStats>().TicketsForSpawning;
        }


        return _winner;
    }

    private void SpawnTeacher(GameObject _prefab, Transform _container)
    {
        GameObject _spawned = Instantiate(_prefab, spawnPoint.position, spawnPoint.rotation, _container);
        _spawned.name = totalTeachersSpawned.ToString();
        totalTeachersSpawned++;
        teachersOnScreen++;
    }
}
