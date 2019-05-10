using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : ProjectileParent
{
    public override void Setup(StudentStats.StudentStat _stat, StudentStats _stats)
    {
        _stats.Student.FireFollow(transform);
        base.Setup(_stat, _stats);
    }
}
