using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPaperplaneProjectile : ProjectileParent
{
    public override void Seek(Transform _target)
    {
        if (WaveSpawner.Instance.TeacherStatsOnScreen.Count < 1)
            return;

        int _random = Random.Range(0, WaveSpawner.Instance.TeacherStatsOnScreen.Count - 1);
        teacherTarget = WaveSpawner.Instance.TeacherStatsOnScreen[_random];
        target = teacherTarget.transform;
    }
}
