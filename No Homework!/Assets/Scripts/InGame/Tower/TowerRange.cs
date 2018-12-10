using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerRange : MonoBehaviour {

    private static TowerRange self;

	[SerializeField]
	private string enemyTag = "Enemy";

    [SerializeField]
    private SortedDictionary<float, Transform> targetDistanceList;
    //[SerializeField]
    //private SortedDictionary<float, Transform> targetHealthList;

    private Tower tower;
    private int updateTargetList;

    private void Awake()
    {
        tower = GetComponentInParent<Tower>();
        targetDistanceList = new SortedDictionary<float, Transform>();
        //targetHealthList = new SortedDictionary<float, Transform>();
        self = this;
        updateTargetList = 0;
    }

    //private void Update()
    //{
    //    if (enemiesInRange.Count > 0)
    //    {
    //        selectTarget();
    //    }
    //}

    private void UpdateTarget()
    {
        Transform _targetToSelect;

        if (targetDistanceList.Count <= 0)
            return;

        switch (tower.targetSetting)
        {
            case Tower.TargetSetting.first:
                _targetToSelect = targetDistanceList.Values.Last();

                //Debug.Log("Selected Target: " + _targetToSelect.name);
                
                SelectTarget(_targetToSelect);           
                break;

            case Tower.TargetSetting.last: //works
                _targetToSelect = targetDistanceList.Values.First();

                SelectTarget(_targetToSelect);
                break;

            //case Tower.TargetSetting.mostHealth:
            //    _targetToSelect = targetHealthList.Values.Last();

            //    SelectTarget(_targetToSelect);
            //    break;
        }
    }

    private void SelectTarget(Transform _enemy)
    {
        tower.target = _enemy;
    }

    private void DeselectTarget()
    {
        tower.target = null;
    }

    private void AddEnemyToTargetList(GameObject _enemy)
    {
        float _newTargetDistance = WaveSpawner.Instance.enemiesWalkDistance[int.Parse(_enemy.name)];
        float _newTargetHealth = WaveSpawner.Instance.enemiesHealth[int.Parse(_enemy.name)];

        targetDistanceList.Add(_newTargetDistance, _enemy.transform); //Add enemy to target list
        //targetHealthList.Add(_newTargetHealth, _enemy.transform);
    }

    private void UpdateEnemyInTargetList(GameObject _enemy)
    {
        float _lastEnemyDistanceKey = targetDistanceList.FirstOrDefault(x => x.Value == _enemy.transform).Key;
        //float _lastEnemyHealthKey = targetHealthList.FirstOrDefault(x => x.Value == _enemy.transform).Key;

        float _newTargetDistance = WaveSpawner.Instance.enemiesWalkDistance[int.Parse(_enemy.name)];
        //float _newTargetHealth = WaveSpawner.Instance.enemiesHealth[int.Parse(_enemy.name)];

        targetDistanceList.Remove(_lastEnemyDistanceKey); //Remove the last enemy distance
        targetDistanceList.Add(_newTargetDistance, _enemy.transform);

        //targetHealthList.Remove(_lastEnemyHealthKey);
        //targetHealthList.Add(_newTargetHealth, _enemy.transform);

        //Debug.Log("Replaced: " + _lastEnemyKey.ToString() + " With: " + _newTargetDistance.ToString());
    }

    public void RemoveEnemyFromTargetList(GameObject _enemyToRemove)
    {
        float _distanceRemoveKey = targetDistanceList.FirstOrDefault(x => x.Value == _enemyToRemove.transform).Key;
        //float _healthRemoveKey = targetHealthList.FirstOrDefault(x => x.Value == _enemyToRemove.transform).Key;

        targetDistanceList.Remove(_distanceRemoveKey);
        //targetHealthList.Remove(_healthRemoveKey);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            collision.GetComponent<EnemyStats>().AddSeenByTower(tower);

            AddEnemyToTargetList(collision.gameObject);

            UpdateTarget();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (updateTargetList > 0 && other.gameObject.CompareTag(enemyTag))
        {
            UpdateEnemyInTargetList(other.gameObject);

            UpdateTarget();

            updateTargetList--;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            collision.gameObject.GetComponent<EnemyStats>().RemoveSeenByTower(tower);

            RemoveEnemyFromTargetList(collision.gameObject);

            if (collision.transform == tower.target) //If the target that exited is the current target, deselect it
                DeselectTarget();

            UpdateTarget();
        }
    }

    public static void UpdateTowerTargetList()
    {
        if (self == null)
            return;

        self.updateTargetList = self.targetDistanceList.Count + 1;
    }
}
