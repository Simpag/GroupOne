using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TeacherStats))]
public class TeacherParent : MonoBehaviour {

    protected TeacherStats stats;

    private void Awake()
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

        Destroy(this.gameObject);
    }
}
