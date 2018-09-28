using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour {

	[SerializeField]
	private string enemyTag = "Enemy";
    [SerializeField]
    private List<GameObject> enemiesInRange;

    private Tower tower;

    private void Awake()
    {
        tower = GetComponentInParent<Tower>();
        enemiesInRange = new List<GameObject>();
    }

    private void Update()
    {
        if (enemiesInRange.Count > 0)
        {
            selectTarget();
        }
    }

    private void selectTarget()
    {
        switch (tower.targetSetting)
        {
            case Tower.TargetSetting.first:
                tower.target = enemiesInRange[0].transform;
                break;

            case Tower.TargetSetting.last:
                tower.target = enemiesInRange[enemiesInRange.Count - 1].transform;
                break;

            case Tower.TargetSetting.mostHealth:
                float _mostHealth = -Mathf.Infinity;
                Transform _winner = null;
                foreach (GameObject _enemy in enemiesInRange)
                {
                    EnemyStats _e = _enemy.GetComponent<EnemyStats>();
                    if (_e.Health > _mostHealth)
                    {
                        _mostHealth = _e.Health;
                        _winner = _enemy.transform;
                    }
                }
                tower.target = _winner;
                break;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            if (!enemiesInRange.Contains(collision.gameObject))
            {
                collision.GetComponent<EnemyStats>().AddSeenByTower(tower);
                enemiesInRange.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            if (enemiesInRange.Contains(collision.gameObject)) //If the enemy is listed, remove it from the list on the tower and the enemy
            {
                collision.gameObject.GetComponent<EnemyStats>().RemoveSeenByTower(tower);
                enemiesInRange.Remove(collision.gameObject);
            }

            if (collision.transform == tower.target) //If the target that exited is the current target, deselect it
                tower.target = null;
        }
    }

    public void RemoveEnemyFromEnemiesInRange(GameObject _enemy)
    {
        if (enemiesInRange.Contains(_enemy))
            enemiesInRange.Remove(_enemy);
    }
}
