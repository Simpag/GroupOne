using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitBallProjectile : ProjectileParent {

    protected override void DamageEnemy(TeacherStats _teacher)
    {
        base.DamageEnemy(_teacher);

        SlowTeacher(_teacher);
    }

    private void SlowTeacher(TeacherStats _teacher)
    {
        _teacher.SlowTeacher(currentStat.slowAmount / 100f, currentStat.slowDuration);
    }
}
