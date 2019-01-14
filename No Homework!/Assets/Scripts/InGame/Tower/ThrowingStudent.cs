using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
[System.Serializable]
public class ThrowingStudent : MonoBehaviour {

    private Tower tower;

    private void Awake()
    {
        tower = GetComponent<Tower>();
        SetupTargetSettings();
    }

    private void SetupTargetSettings()
    {
        tower.allowedTargetSettings.Add(Tower.TargetSetting.first);
        tower.allowedTargetSettings.Add(Tower.TargetSetting.last);
        tower.allowedTargetSettings.Add(Tower.TargetSetting.mostHealth);
    }

}
