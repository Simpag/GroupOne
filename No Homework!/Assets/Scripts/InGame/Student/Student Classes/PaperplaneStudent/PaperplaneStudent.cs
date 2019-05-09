using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StudentStats))]
[System.Serializable]
public class PaperplaneStudent : StudentParent {

    public override void Shoot()
    {
        if (stat.Row2Level >= 3)
        {
            if (fireCountdown <= 0)
            {
                AudioManager.Instance.Play("TowerThrowSound");

                for (int i = 0; i < 5; i++)
                {
                    GameObject _bulletGO = Instantiate(stat.CurrentStat.bulletPrefab, stat.FirePoint.position, stat.FirePoint.rotation);
                    ProjectileParent _bullet = _bulletGO.GetComponent<ProjectileParent>();

                    if (_bullet != null)
                    {
                        _bullet.Setup(stat.CurrentStat, stat);
                        _bullet.Seek(stat.target);
                    }
                }

                fireCountdown = 1 / stat.CurrentStat.firerate;
            }
        }
        else
        {
            base.Shoot();
        }
    }

}
