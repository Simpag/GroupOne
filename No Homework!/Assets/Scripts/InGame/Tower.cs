using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    [Header("Just ingame info")]
    [SerializeField]
    private Transform target;

    [Header("Tower Properties")]
    [SerializeField]
    private float fireRate = 2f;
    [SerializeField]
    private float range = 10f;
    [SerializeField]
    private float rotationSpeed = 10f;

    public Color cantPlaceTint;

    [HideInInspector]
    public bool isActive = false;
    private float fireCountdown = 0;

    [Header("Drag-n-drop")]
    [SerializeField]
    private Transform pivotPoint;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform firePoint;

    public GameObject towerArea;
    public Transform rangeView;


    [SerializeField]
    private float checkNewTargetInterval = 0.5f;

    private void Awake()
    {
        InvokeRepeating("UpdateTarget" , 0f, checkNewTargetInterval);
        rangeView.localScale = new Vector3(range * 2, rangeView.localScale.y, range * 2);
    }

    private void Update()
    {
        if (target == null || !isActive)
            return;

        //Look onto target
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(pivotPoint.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        pivotPoint.rotation = Quaternion.Euler(pivotPoint.rotation.x, rotation.y, pivotPoint.rotation.z);

        if (fireCountdown <= 0)
        {
            Shoot();
            fireCountdown = 1 / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        GameObject _bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet _bullet = _bulletGO.GetComponent<Bullet>();

        if (_bullet != null)
            _bullet.Seek(target);
    }

    private void UpdateTarget()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject _enemy in WaveSpawner.aliveEnemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, _enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = _enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    //Show range of turret
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
