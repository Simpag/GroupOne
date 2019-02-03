using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherParent : MonoBehaviour {

    protected TeacherStats stats;

    protected virtual void Awake()
    {
        stats = GetComponent<TeacherStats>();
    }

    public virtual void Died(bool _killed)
    {
        if (_killed)
        {
            AudioManager.Instance.Play("EnemyKnockSound");
            PlayerStats.AddCandyCurrency(stats.Worth);

            Debug.Log("Normal Death");
        }

        Destroy(stats.gameObject);
    }
}
