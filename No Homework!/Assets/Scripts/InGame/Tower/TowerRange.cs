using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerRange : MonoBehaviour {

	[SerializeField]
	private string enemyTag = "Enemy";

    [SerializeField]
    private SortedDictionary<float*, Transform> targetList;

    private Tower tower;

    private void Awake()
    {
        tower = GetComponentInParent<Tower>();
        targetList = new SortedDictionary<float*, Transform>(); ;
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

        switch (tower.targetSetting)
        {
            case Tower.TargetSetting.first:
                _targetToSelect = targetList.Values.First();

                Debug.Log("Selected Target: " + _targetToSelect.name);
                
                SelectTarget(_targetToSelect);           
                break;

            case Tower.TargetSetting.last: //works
                _targetToSelect = targetList.Values.Last();

                SelectTarget(_targetToSelect);
                break;

            case Tower.TargetSetting.mostHealth:
                //float _mostHealth = -Mathf.Infinity;
                //Transform _winnerTransform = null;
                //foreach (GameObject _enemy in enemiesInRange)
                //{
                //    EnemyStats _e = _enemy.GetComponent<EnemyStats>();
                //    if (_e.Health > _mostHealth)
                //    {
                //        _mostHealth = _e.Health;
                //        _winnerTransform = _enemy.transform;
                //    }
                //}
                //tower.target = _winnerTransform;
                break;
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
        unsafe
        {
            float* _newTargetDistance = &WaveSpawner.Instance.enemiesWalkDistance[int.Parse(_enemy.name)];

            targetList.Add(_newTargetDistance, _enemy.transform); //Add enemy to target list
        }


        Debug.Log("Added");

        foreach (Transform _tower in targetList.Values)
        {
            Debug.Log("List: " + _tower.name);
        }
    }

    public void RemoveEnemyFromTargetList(GameObject _enemyToRemove)
    {
        float _removeKey = WaveSpawner.GetEnemiesDistance(int.Parse(_enemyToRemove.name));

        targetList.Remove(_removeKey);

        Debug.Log("Removed");

        foreach (Transform _tower in targetList.Values)
        {
            Debug.Log("RList: " + _tower.name);
        }
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
}
