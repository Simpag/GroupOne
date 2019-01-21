using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StudentStats))]
[System.Serializable]
public class ThrowingStudent : StudentParent {

    private StudentStats student;

    private void Awake()
    {
        student = GetComponent<StudentStats>();
        SetupTargetSettings();
    }

    private void SetupTargetSettings()
    {
        student.allowedTargetSettings.Add(StudentStats.TargetSetting.first);
        student.allowedTargetSettings.Add(StudentStats.TargetSetting.last);
        student.allowedTargetSettings.Add(StudentStats.TargetSetting.mostHealth);
    }

}
