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

[RequireComponent(typeof(ProjectileParent))]
public class StudentStats : MonoBehaviour {

    [System.Serializable]
    public struct StudentStat
    {
        public GameObject mesh;
        public float damage;
        public float bulletSpeed;
        public float AOERadius;
        public float area;
        public float range;
        public float fireRate;
        public float rotationSpeed;
        public GameObject bulletPrefab;
    }
    public enum TargetSetting
    {
        first,
        last,
        mostHealth,
        leastHealth
    }

    private StudentParent student;
    public StudentParent Student
    {
        get { return student; }
    }

    [Header("General Tower Properties")]
    public List<TargetSetting> allowedTargetSettings;
    public TargetSetting currentTargetSetting;
    [SerializeField]
    private bool canBeSlowed;

    [Header("General Setup")]
	[SerializeField]
	private Transform pivotPoint;
	[SerializeField]
	private Transform firePoint;
    public GameObject studentArea;
	public Transform rangeView;
	public Material rangeMaterial;
	public Material cantPlaceMaterial;

    [Header("Stats")]
    [SerializeField]
    private StudentStat baseStat;

    [Header("Row 1 Upgrades Stats")]
    [SerializeField]
    private StudentStat[] row1Stats;

    [Header("Row 2 Upgrades Stats")]
    [SerializeField]
    private StudentStat[] row2Stats;

    [Header("Information")]
    public string studentName;
    [TextArea]
    public string studentDescription;

	[Header("Just In-Game Info")]
	public Transform target;
    public InGameShopItemStats shopStats;
    public bool isYours = true;
    public string studentGUID;

    [SerializeField]
    private int row1Level;
    [SerializeField]
    private int row2Level;

    private StudentStat currentStat;
    private ProjectileParent bullet;
    private float baseFireRate;

    private bool isActive = false;

    //Getters
    public bool IsActive { get { return isActive; } }
    public int Row1Level { get { return row1Level; } }
    public int Row2Level { get { return row2Level; } }
    public Transform FirePoint { get { return firePoint; } }
    public Transform PivotPoint { get { return PivotPoint; } }
    public StudentStat CurrentStat { get { return currentStat; } }


    private void Start()
    {
        Setup(true);
    }

    private void Setup(bool isStart)
    {
        rangeView.localScale = new Vector3(currentStat.range * 2, rangeView.localScale.y, currentStat.range * 2);
        studentArea.transform.localScale = new Vector3(currentStat.area, studentArea.transform.localScale.y, currentStat.area);

        if (isStart)
        {
            currentStat = baseStat;

            if (allowedTargetSettings.Count < 1)
                currentTargetSetting = TargetSetting.first;
            else
                currentTargetSetting = allowedTargetSettings.ElementAt(0);

            bullet = baseStat.bulletPrefab.GetComponent<ProjectileParent>();
            bullet.Setup(baseStat.bulletSpeed, baseStat.damage, baseStat.AOE);

            Guid tempGUID = Guid.NewGuid();
            studentGUID = tempGUID.ToString();

            baseFireRate = 0;

            student = GetComponent<StudentParent>();

            row1Level = 0;
            row2Level = 0;
        }
    }

    //Show range of turret in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, baseStat.range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, baseStat.area/2);
    }

    public void MovingTower()
    {
        rangeView.GetComponent<StudentRange>().enabled = false; //Disable targeting
        isActive = false;
    }

    public void PlacedTower()
    {
        rangeView.GetComponent<MeshRenderer>().enabled = false; //Disable range view
        isActive = true;
    }

    public void UpgradeRow1()
    {
        row1Level++;

        //Replace the mesh
        currentStat.mesh.SetActive(false);
        Instantiate(row1Stats[row1Level - 1].mesh, pivotPoint);

        //Update the current stat
        currentStat = row1Stats[row1Level - 1];
        Setup(false);

        //Update bullet stats
        bullet = currentStat.bulletPrefab.GetComponent<ProjectileParent>();
        bullet.Setup(currentStat.bulletSpeed, currentStat.damage, currentStat.AOE);
    }

    public void UpgradeRow2()
    {
        row2Level++;

        //Replace the mesh
        currentStat.mesh.SetActive(false);
        Instantiate(row2Stats[row2Level - 1].mesh, pivotPoint);

        //Update the current stat
        currentStat = row2Stats[row2Level - 1];
        Setup(false);

        //Update bullet stats
        bullet = currentStat.bulletPrefab.GetComponent<ProjectileParent>();
        bullet.Setup(currentStat.bulletSpeed, currentStat.damage, currentStat.AOE);
    }

    public void Slow(float _amount, float _time)
    {
        if (canBeSlowed && baseFireRate == 0)
        {
            baseFireRate = currentStat.fireRate;
            currentStat.fireRate *= _amount;
            Invoke("RemoveSlow", _time);
        }
    }

    private void RemoveSlow()
    {
        if (baseFireRate != 0)
        {
            currentStat.fireRate = baseFireRate;
            baseFireRate = 0;
        }
    }
}