using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StudentStats))]
[RequireComponent(typeof(BoxCollider))]
[System.Serializable]
public class SlingshotStudent : StudentParent {

    [Header("MAKE COLLIDER TRIGGER!!")]
    [SerializeField]
    private BoxCollider dmgBox;
    [SerializeField]
    private GameObject waterParticle;

    private void Start()
    {
        dmgBox.enabled = false;
        waterParticle.SetActive(false);
    }

    public override void MaxUpgrade(int _row)
    {
        if (_row == 2)
        {
            dmgBox.size = new Vector3(dmgBox.size.x, dmgBox.size.y, stat.CurrentStat.range);
            dmgBox.center = new Vector3(dmgBox.center.x, dmgBox.center.y, stat.CurrentStat.range / 2f);
        }
    }

    protected override void Update()
    {
        if (stat.target == null || !stat.IsActive)
        {
            stat.UpdateState(StudentStats.State.idle);

            if (stat.Row2Level >= 3)
                StopShooting();
            return;
        }

        //Change student state
        stat.UpdateState(StudentStats.State.fire);

        //Look onto target
        LockOn();

        //Shot target
        Shoot();
    }

    protected override void Shoot()
    {
        if (stat.Row2Level >= 3)
        {
            dmgBox.enabled = true;
            waterParticle.SetActive(true);
        }
        else
        {
            base.Shoot();
        }
    }

    private void StopShooting()
    {
        dmgBox.enabled = false;
        waterParticle.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (stat.Row2Level < 3)
            return;

        if (other.CompareTag(GameConstants.TEACHER_TAG))
        {
            stat.targetStats.TakeDamage(stat.CurrentStat.damage, stat);
        }
    }
}
