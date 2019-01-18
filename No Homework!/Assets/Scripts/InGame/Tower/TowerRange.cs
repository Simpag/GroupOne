using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerRange : MonoBehaviour {

    private Tower tower;
    private TeacherStats currentTargetStats;

    private void Awake()
    {
        tower = GetComponentInParent<Tower>();
    }

    private void UpdateTarget(TeacherStats _enemy)
    {
        if (tower.target == null)
            SelectTarget(_enemy);

        switch (tower.currentTargetSetting)
        {
            case Tower.TargetSetting.first:
                if (currentTargetStats.Movement.DistanceTraveled < _enemy.Movement.DistanceTraveled)
                {
                    SelectTarget(_enemy);
                }         
                break;

            case Tower.TargetSetting.last:
                if (currentTargetStats.Movement.DistanceTraveled > _enemy.Movement.DistanceTraveled)
                {
                    SelectTarget(_enemy);
                }
                break;

            case Tower.TargetSetting.mostHealth:
                //Select the target with the most health,
                //or if two enemies have the same amount of health then choose
                //the first one.
                if (currentTargetStats.Health == _enemy.Health)
                {
                    if (currentTargetStats.Movement.DistanceTraveled < _enemy.Movement.DistanceTraveled)
                    {
                        SelectTarget(_enemy);
                    }
                }
                else if (currentTargetStats.Health < _enemy.Health)
                {
                    SelectTarget(_enemy);
                }
                break;
        }
    }

    private void SelectTarget(TeacherStats _enemy)
    {
        tower.target = _enemy.transform;
        currentTargetStats = _enemy;
    }

    private void DeselectTarget()
    {
        tower.target = null;
        currentTargetStats = null;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(GameConstants.TEACHER_TAG))
        {
            if (tower.target != null)
            {
                UpdateTarget(collision.GetComponent<TeacherStats>());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(GameConstants.TEACHER_TAG))
        {
            UpdateTarget(other.GetComponent<TeacherStats>());
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag(GameConstants.TEACHER_TAG))
        {
            if (collision.transform == tower.target) //If the target that exited is the current target, deselect it
                DeselectTarget();
        }
    }
}
