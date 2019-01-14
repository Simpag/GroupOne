using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using System.Diagnostics;
using UnityEditor;

/*[Serializable] public class TargetSettingDictionary : SerializableDictionary<Tower.TargetSetting, bool> { }
[CustomPropertyDrawer(typeof(TargetSettingDictionary))]
public class TargetSettingDictionaryDrawer : DictionaryDrawer<Tower.TargetSetting, bool> { }*/

[RequireComponent(typeof(Bullet))]
public class Tower : MonoBehaviour {

    [Header("General Tower Properties")]
	[SerializeField]
	private TowerType towerType;
    public List<TargetSetting> allowedTargetSettings;
    public TargetSetting currentTargetSetting;
    [SerializeField]
    private bool canBeSlowed;
    public int numberOfUpgrades;
    [SerializeField]
	private float area;
	[SerializeField]
	private float range;
	[SerializeField]
	private float rotationSpeed;

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
    private float damage;
    [SerializeField]
    private float AOE; //Area of effect
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float fireRate;
	[SerializeField]
	private GameObject bulletPrefab;

    [Header("Upgraded Stats")]
    [SerializeField]
    private GameObject upgradedMesh;
    [SerializeField]
    private float upgradedDamage;
    [SerializeField]
    private float upgradedBulletSpeed;
    [SerializeField]
    private float upgradedArea;
    [SerializeField]
    private float upgradedRange;
    [SerializeField]
    private float upgradedFireRate;

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
    private float baseFireRate;

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
        mostHealth,
        leastHealth
    }

    private void Start()
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
            if (allowedTargetSettings.Count < 1)
                currentTargetSetting = TargetSetting.first;
            else
                currentTargetSetting = allowedTargetSettings.ElementAt(0);

            bullet = bulletPrefab.GetComponent<Bullet>();
            bullet.Setup(bulletSpeed, damage, AOE);

            Guid tempGUID = Guid.NewGuid();
            towerGUID = tempGUID.ToString();

            baseFireRate = fireRate;
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

    public void Slow(float _amount)
    {
        if (canBeSlowed)
            fireRate *= _amount;
    }

    public void RemoveSlow()
    {
        fireRate = baseFireRate;
    }
}