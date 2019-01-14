using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ComputerScienceTeacher : TeacherParent {

    [Header("Dont forget to add a trigger collider")]

    [Header("Buff amounts (%)")]
    [SerializeField]
    private float healthBuff = 100f;
    [SerializeField]
    private float speedBuff = 100f;
    [SerializeField]
    private float homeworkBuff = 100f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.TEACHER_TAG))
        {
            TeacherStats _stats = other.GetComponent<TeacherStats>();

            _stats.Health *= healthBuff / 100f;
            _stats.Speed *= speedBuff / 100f;
            _stats.Homework = (int)(_stats.Homework * (homeworkBuff / 100f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameConstants.TEACHER_TAG))
        {
            TeacherStats _stats = other.GetComponent<TeacherStats>();

            _stats.Health /= healthBuff / 100f;
            _stats.Speed /= speedBuff / 100f;
            _stats.Homework = (int)(_stats.Homework / (homeworkBuff / 100f));
        }
    }
}
