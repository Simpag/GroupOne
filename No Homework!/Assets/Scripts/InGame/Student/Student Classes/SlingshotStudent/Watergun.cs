using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Watergun : MonoBehaviour
{
    public StudentStats stat;
    private float timer = 0;
    List<TeacherStats> inside = new List<TeacherStats>();

    private void Update()
    {
        timer -= Time.deltaTime;

        if (stat.CurrentState == StudentStats.State.idle)
            inside.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.TEACHER_TAG))
        {
            TeacherStats _temp = other.GetComponent<TeacherStats>();
            if (!inside.Contains(_temp))
                inside.Add(_temp);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameConstants.TEACHER_TAG))
        {
            TeacherStats _temp = other.GetComponent<TeacherStats>();
            if (inside.Contains(_temp))
                inside.Remove(_temp);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (timer > 0)
            return;

        if (other.CompareTag(GameConstants.TEACHER_TAG))
        {
            foreach (TeacherStats _t in inside)
            {
                if (_t != null)
                    _t.TakeDamage(stat.CurrentStat.damage, stat);
            }

            timer = 1 / stat.CurrentStat.firerate;
        }
    }
}
