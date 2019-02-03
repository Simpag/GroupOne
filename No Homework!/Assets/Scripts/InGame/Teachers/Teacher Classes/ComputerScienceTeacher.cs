using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ComputerScienceTeacher : TeacherParent {

    [Header("Dont forget to add a trigger collider")]

    [Header("Buff amounts to teachers (%)")]
    [SerializeField]
    private float healthBuff = 20f;
    [SerializeField]
    private float speedBuff = 20f;
    [SerializeField]
    private float homeworkBuff = 20f;

    protected override void Awake()
    {
        stats = GetComponentInParent<TeacherStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.TEACHER_TAG))
        {
            TeacherStats _stats = other.GetComponent<TeacherStats>();

            _stats.Health *= 1f + (healthBuff / 100f);
            _stats.Speed *= 1f + (speedBuff / 100f);
            _stats.Homework = (int)(_stats.Homework * (1f + (homeworkBuff / 100f)));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameConstants.TEACHER_TAG))
        {
            TeacherStats _stats = other.GetComponent<TeacherStats>();

            _stats.Health /= 1f + (healthBuff / 100f);
            _stats.Speed /= 1f + (speedBuff / 100f);
            _stats.Homework = (int)(_stats.Homework / (1f + (homeworkBuff / 100f)));
        }
    }
}
