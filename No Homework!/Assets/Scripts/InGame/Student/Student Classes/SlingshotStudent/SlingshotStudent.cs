using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StudentStats))]
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
            Watergun _temp = dmgBox.gameObject.AddComponent<Watergun>();
            _temp.stat = stat;

            dmgBox.size = new Vector3(dmgBox.size.x, dmgBox.size.y, stat.CurrentStat.range);
            dmgBox.center = new Vector3(dmgBox.center.x, dmgBox.center.y, stat.CurrentStat.range / 2f);
        }
    }

    protected override void Update()
    {
        stopTimer -= Time.deltaTime;

        if (stat.target == null || !stat.IsActive)
        {
            if (stopTimer <= 0 && firing)
            {
                stat.UpdateState(StudentStats.State.idle);
                firing = false;

                if (stat.Row2Level >= 3)
                    StopShooting();
            }
            return;
        }

        //Change student state
        if (!firing)
        {
            stat.UpdateState(StudentStats.State.fire);
            firing = true;
            Shoot();
        }

        //Look onto target
        LockOn();

        stopTimer = 0.1f;
        fireCountdown -= Time.deltaTime;
    }

    public override void Shoot()
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
}
