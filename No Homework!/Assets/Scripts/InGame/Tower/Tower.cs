using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Bullet))]
public class Tower : MonoBehaviour {
    [Header("General Tower Properties")]
	[SerializeField]
	private TowerType towerType;
    public int numberOfUpgrades = 1;
    [SerializeField]
	private float area = 1.4f;
	[SerializeField]
	private float range = 10f;
	[SerializeField]
	private float rotationSpeed = 10f;
    public TargetSetting targetSetting;

    [Header("General Setup")]
    [SerializeField]
    private GameObject standardMesh;
	[SerializeField]
	private Transform pivotPoint;
	[SerializeField]
	private Transform firePoint;
    public GameObject towerArea;
	public Transform rangeView;
	public Material rangeMaterial;
	public Material cantPlaceMaterial;

	[Header("Stats")]
    [SerializeField]
    private float damage = 50f;
    [SerializeField]
    private float AOE = -1f; //Area of effect
    [SerializeField]
    private float bulletSpeed = 70f;
    [SerializeField]
    private float fireRate = 2f;
	[SerializeField]
	private GameObject bulletPrefab;

    [Header("Upgraded Stats")]
    [SerializeField]
    private GameObject upgradedMesh;
    [SerializeField]
    private float upgradedDamage = 75f;
    [SerializeField]
    private float upgradedBulletSpeed = 100f;
    [SerializeField]
    private float upgradedArea = 1.4f;
    [SerializeField]
    private float upgradedRange = 15f;
    [SerializeField]
    private float upgradedFireRate = 4f;

    [Header("Information")]
    public string towerName;
    public string towerDescription;

	[Header("Just In-Game Info")]
	public Transform target;
    public InGameShopItemStats shopStats;
    public int towerLevel = 0;
    public bool isYours = true;
    public string towerGUID;

    private float fireCountdown = 0;
    private Bullet bullet;

    private bool isActive = false;

	private enum TowerType
	{
		bullet,
		AOE
	}

    public enum TargetSetting
    {
        first,
        last,
        mostHealth
    }

    private void Awake()
    {
        Setup(true);
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

    private void Setup(bool isStart)
    {
        rangeView.localScale = new Vector3(range * 2, rangeView.localScale.y, range * 2);
        towerArea.transform.localScale = new Vector3(area, towerArea.transform.localScale.y, area);

        if (isStart)
        {
            bullet = bulletPrefab.GetComponent<Bullet>();
            bullet.Setup(bulletSpeed, damage, AOE);

            Guid tempGUID = Guid.NewGuid();
            towerGUID = tempGUID.ToString();
        }
    }

    private void Shoot()
    {
        if (fireCountdown <= 0)
        {
            AudioManager.Instance.Play("TowerThrowSound");

            GameObject _bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet _bullet = _bulletGO.GetComponent<Bullet>();

            if (_bullet != null)
            {
                _bullet.Seek(target, bulletSpeed, damage, AOE);
            }

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

    //Show range of turret in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, area/2);
    }

    public void MovingTower()
    {
        rangeView.GetComponent<TowerRange>().enabled = false; //Disable targeting
        isActive = false;
    }

    public void PlacedTower()
    {
        rangeView.GetComponent<MeshRenderer>().enabled = false; //Disable range view
        isActive = true;
    }

    public void UpgradeTower()
    {
        towerLevel++;
        this.damage = upgradedDamage;
        this.bulletSpeed = upgradedBulletSpeed;
        this.range = upgradedRange;
        this.fireRate = upgradedFireRate;
        this.area = upgradedArea;

        Setup(false);

        //Replace the mesh
        standardMesh.SetActive(false);
        Instantiate(this.upgradedMesh, pivotPoint);
    }
}
