using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    [Header("General Tower Properties")]
	[SerializeField]
	private TowerType towerType;
	[SerializeField]
	private float area = 1.4f;
	[SerializeField]
	private float range = 10f;
	[SerializeField]
	private float rotationSpeed = 10f;

	public GameObject towerArea;
	public Transform rangeView;
	public Material rangeMaterial;
	public Material cantPlaceMaterial;
	[SerializeField]
	private Transform pivotPoint;
	[SerializeField]
	private GameObject bulletPrefab;
	[SerializeField]
	private Transform firePoint;

	[Header("Bullet")]
    [SerializeField]
    private float fireRate = 2f;

    [HideInInspector]
    public bool isActive = false;
    private float fireCountdown = 0;

	[Header("Just In-Game Info")]
	public Transform target;

	enum TowerType
	{
		bullet,
		laset,
		AOE
	}

    private void Awake()
    {
        rangeView.localScale = new Vector3(range * 2, rangeView.localScale.y, range * 2);
        towerArea.transform.localScale = new Vector3(area, towerArea.transform.localScale.y, area);
    }

    private void Update()
    {
        if (target == null || !isActive)
            return;

        //Look onto target
        LockOn();

        //Shot target
        Shoot();
    }

    private void Shoot()
    {
        if (fireCountdown <= 0)
        {
            GameObject _bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet _bullet = _bulletGO.GetComponent<Bullet>();

            if (_bullet != null)
                _bullet.Seek(target);

            fireCountdown = 1 / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private void LockOn()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(pivotPoint.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        pivotPoint.rotation = Quaternion.Euler(pivotPoint.rotation.x, rotation.y, pivotPoint.rotation.z);
    }

    //Show range of turret
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, area/2);
    }
}
