using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bullet))]
public class Tower : MonoBehaviour {
    [Header("General Tower Properties")]
	[SerializeField]
	private TowerType towerType;
    public int numberOfUpgrades = 1;
    [SerializeField]
    private float damage = 50f;
    [SerializeField]
    private float AOE = -1f; //Area of effect
    [SerializeField]
    private float bulletSpeed = 70f;
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
    public GameObject towerArea;
	public Transform rangeView;
	[SerializeField]
	private Transform pivotPoint;
	[SerializeField]
	private Transform firePoint;
	public Material rangeMaterial;
	public Material cantPlaceMaterial;

    [Header("Upgraded Setup")]
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

	[Header("Bullet")]
    [SerializeField]
    private float fireRate = 2f;
	[SerializeField]
	private GameObject bulletPrefab;

    [Header("Information")]
    public string towerName;
    public string towerDescription;

	[Header("Just In-Game Info")]
	public Transform target;
    [HideInInspector]
    public InGameShopItemStats shopStats;
    [HideInInspector]
    public int towerLevel = 0;

    private float fireCountdown = 0;
    private Bullet bullet;

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
        if (target == null)
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
            bullet.speed = bulletSpeed;
            bullet.damage = damage;
            bullet.AOE = AOE;
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
        rangeView.GetComponent<MeshRenderer>().enabled = true; //Enable range view
    }

    public void PlacedTower()
    {
        rangeView.GetComponent<MeshRenderer>().enabled = false; //Disable range view
        rangeView.GetComponent<TowerRange>().enabled = true; //Enable targeting script
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
