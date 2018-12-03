using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour {

	[SerializeField]
	private string enemyTag = "Enemy";
    [SerializeField]
    private EnemyMovement targetMovementScript;
    [SerializeField]
    private List<GameObject> enemiesInRange;

    private Tower tower;

    private void Awake()
    {
        tower = GetComponentInParent<Tower>();
        enemiesInRange = new List<GameObject>();
        targetMovementScript = null;
    }

    //private void Update()
    //{
    //    if (enemiesInRange.Count > 0)
    //    {
    //        selectTarget();
    //    }
    //}

    private void UpdateTarget(EnemyMovement _enemy)
    {
        if (tower.target == null)
        {
            tower.target = _enemy.transform;
            targetMovementScript = _enemy;
            return;
        }

        switch (tower.targetSetting)
        {
            case Tower.TargetSetting.first:
                tower.target = enemiesInRange[0].transform;
                break;

            case Tower.TargetSetting.last: //works
                if (_enemy.DistanceTraveled < targetMovementScript.DistanceTraveled)
                {
                    tower.target = _enemy.transform;
                    targetMovementScript = _enemy;
                }

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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            if (!enemiesInRange.Contains(collision.gameObject))
            {
                enemiesInRange.Add(collision.gameObject);
            }

            EnemyMovement _enmy = collision.GetComponent<EnemyMovement>();

            collision.GetComponent<EnemyStats>().AddSeenByTower(tower);

            UpdateTarget(_enmy);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            if (enemiesInRange.Contains(collision.gameObject)) //If the enemy is listed, remove it from the list on the tower and the enemy
            {
                enemiesInRange.Remove(collision.gameObject);
            }

            EnemyMovement _enmy = collision.GetComponent<EnemyMovement>();

            collision.gameObject.GetComponent<EnemyStats>().RemoveSeenByTower(tower);

            if (collision.transform == tower.target) //If the target that exited is the current target, deselect it
                tower.target = null;

            UpdateTarget(_enmy);
        }
    }

    public void RemoveEnemyFromTargeting(GameObject _enemy)
    {
        if (enemiesInRange.Contains(_enemy))
        {
            enemiesInRange.Remove(_enemy);
        }
    }
}
