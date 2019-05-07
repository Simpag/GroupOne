using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Animations;
using System.Linq;

/*[Serializable] public class TargetSettingDictionary : SerializableDictionary<Tower.TargetSetting, bool> { }
[CustomPropertyDrawer(typeof(TargetSettingDictionary))]
public class TargetSettingDictionaryDrawer : DictionaryDrawer<Tower.TargetSetting, bool> { }*/

[RequireComponent(typeof(ProjectileParent))]
public class StudentStats : MonoBehaviour {

    [System.Serializable]
    public struct StudentStat
    {
        [Header("Animation Avatar")]
        public Avatar avatar;

        [Header("Only fill in the stats that the student will use!")]
        [Header("Normal Stats")]
        public GameObject mesh;
        public float damage;
        public float bulletSpeed;
        public float area;
        public float range;
        public float firerate;
        public float rotationSpeed;
        public GameObject bulletPrefab;

        [Header("AOE Stats")]
        public float AOERadius;

        [Header("Slow Stats")]
        public float slowAmount;
        public float slowDuration;

        [Header("Buff/Money Generation Stats")]
        public float moneyGeneration;
        public float damageBuff;
        public float rangeBuff;
        public float firerateBuff;
    }

    public enum TargetSetting
    {
        first,
        last,
        mostHealth,
        leastHealth
    }

    public enum State
    {
        idle,
        fire
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
    [SerializeField]
    private bool canBeBuffed;
    [SerializeField]
    private bool antiImmune;
    public bool AntiImmune
    {
        get { return antiImmune; }
    }
    [SerializeField]
    private State currentState;

    [Header("General Setup")]
    [SerializeField]
    private Animator anim;
	[SerializeField]
	private Transform pivotPoint;
	[SerializeField]
	private Transform firePoint;
    public GameObject studentArea;
    public GameObject studentBuffArea;
	public Transform rangeView;
	public Material rangeMaterial;
	public Material cantPlaceMaterial;

    [Header("Stats")]
    [SerializeField]
    private StudentStat baseStat;

    [Header("These stats will be added to the base stat")]
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
    public TeacherStats targetStats;
    public InGameShopItemStats shopStats;
    public bool isYours = true;
    public string studentGUID;

    [SerializeField]
    private int row1Level;
    [SerializeField]
    private int row2Level;
    [SerializeField]
    private float buffPercentage;

    [SerializeField]
    private StudentStat currentStat;
    private ProjectileParent bullet;

    private float normalFireRate = 0;
    private float slowTimer = 0;

    private bool isActive = false;

    //Getters
    public bool IsActive { get { return isActive; } }
    public int Row1Level { get { return row1Level; } }
    public int Row2Level { get { return row2Level; } }
    public Transform FirePoint { get { return firePoint; } }
    public Transform PivotPoint { get { return pivotPoint; } }
    public StudentStat CurrentStat { get { return currentStat; } }


    private void Start()
    {
        Setup(true);
    }

    private void Update()
    {
        if (Math.Abs(normalFireRate) > Mathf.Epsilon && slowTimer <= 0)
        {
            ReturnToNormalFireRate();
        }
        else if (Math.Abs(normalFireRate) > Mathf.Epsilon)
        {
            slowTimer -= Time.deltaTime;
        }
    }

    private void Setup(bool isStart)
    {
        if (isStart)
        {
            currentStat = baseStat;

            if (allowedTargetSettings.Count < 1)
                currentTargetSetting = TargetSetting.first;
            else
                currentTargetSetting = allowedTargetSettings.ElementAt(0);

            if (bullet != null)
            {
                bullet = baseStat.bulletPrefab.GetComponent<ProjectileParent>();
            }

            Guid tempGUID = Guid.NewGuid();
            studentGUID = tempGUID.ToString();

            student = GetComponent<StudentParent>();

            row1Level = 0;
            row2Level = 0;
            buffPercentage = 0;
        }

        rangeView.localScale = new Vector3(currentStat.range * 2, rangeView.localScale.y, currentStat.range * 2);
        studentArea.transform.localScale = new Vector3(currentStat.area, studentArea.transform.localScale.y, currentStat.area);

        if (studentBuffArea != null)
            studentBuffArea.transform.localScale = new Vector3(currentStat.area, studentArea.transform.localScale.y, currentStat.area);
            
        //Update animation layer
        anim.avatar = currentStat.avatar;
        anim.speed = currentStat.firerate;

        if (row1Level >= 3)
        {
            anim.SetLayerWeight(anim.GetLayerIndex("Row1"), 1f);
            anim.SetLayerWeight(anim.GetLayerIndex("Row2"), 0f);

            student.MaxUpgrade(1);
        }
        else if (row2Level >= 3)
        {
            anim.SetLayerWeight(anim.GetLayerIndex("Row1"), 0f);
            anim.SetLayerWeight(anim.GetLayerIndex("Row2"), 1f);

            student.MaxUpgrade(2);
        }

        UpdateState(currentState);
    }

    //Show range of turret in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, baseStat.range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, baseStat.area/2);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, baseStat.AOERadius);
    }

    public void MovingTower()
    {
        rangeView.GetComponent<MeshRenderer>().enabled = true; //Disable targeting
        isActive = false;

        if (studentBuffArea != null)
            studentBuffArea.SetActive(false);
    }

    public void PlacedTower()
    {
        rangeView.GetComponent<MeshRenderer>().enabled = false; //Disable range view
        isActive = true;

        if (studentBuffArea != null)
            studentBuffArea.SetActive(true);
    }

    public void UpgradeRow1()
    {
        row1Level++;

        //Replace the mesh
        //currentStat.mesh.SetActive(false);
        //Instantiate(row1Stats[row1Level - 1].mesh, pivotPoint);

        //Update the current stat
        AddStat(row1Stats[row1Level - 1]);
    }

    public void UpgradeRow2()
    {
        row2Level++;

        //Replace the mesh
        //currentStat.mesh.SetActive(false);
        //Instantiate(row2Stats[row2Level - 1].mesh, pivotPoint);

        //Update the current stat
        AddStat(row2Stats[row2Level - 1]);
    }

    public void BuffStudent(float _percentage)
    {
        if (buffPercentage >= _percentage || !canBeBuffed)
            return;

        buffPercentage = _percentage;

        currentStat.damage *= 1f + _percentage;
        currentStat.firerate *= 1f + _percentage;

        UpdateSetups();
    }

    public void DebuffStudent(float _percentage)
    {
        if (buffPercentage >= _percentage || !canBeBuffed)
            return;

        buffPercentage = 0;

        currentStat.damage /= 1f + _percentage;
        currentStat.firerate /= 1f + _percentage;

        UpdateSetups();
    }

    private void AddStat(StudentStat _stat)
    {
        currentStat.avatar = _stat.avatar;
        currentStat.mesh.SetActive(false);
        currentStat.mesh = _stat.mesh;
        currentStat.mesh.SetActive(true);
        currentStat.bulletPrefab = _stat.bulletPrefab;
        currentStat.damage += _stat.damage;
        currentStat.bulletSpeed += _stat.bulletSpeed;
        currentStat.AOERadius += _stat.AOERadius;
        currentStat.area += _stat.area;
        currentStat.range += _stat.range;
        currentStat.firerate += _stat.firerate;
        currentStat.rotationSpeed += _stat.rotationSpeed;
        currentStat.slowAmount += _stat.slowAmount;
        currentStat.slowDuration += _stat.slowDuration;

        UpdateSetups();
    }

    private void UpdateSetups()
    {
        Setup(false);

        //Update bullet stats
        if (currentStat.bulletPrefab != null)
            bullet = currentStat.bulletPrefab.GetComponent<ProjectileParent>();
    }

    public void SlowStudent(float _amount, float _time)
    {
        slowTimer = _time;

        if (canBeSlowed && Math.Abs(normalFireRate) < Mathf.Epsilon)
        {
            normalFireRate = currentStat.firerate;
            currentStat.firerate *= 1 - _amount;
        }
    }

    private void ReturnToNormalFireRate()
    {
        if (Math.Abs(normalFireRate) > Mathf.Epsilon)
        {
            currentStat.firerate = normalFireRate;
            normalFireRate = 0;
        }
    }

    public void UpdateState(State _state)
    {
        /*if (currentState == _state)
            return;
            */
        currentState = _state;

        switch (_state)
        {
            case State.idle:
                anim.SetTrigger("idle");
                break;

            case State.fire:
                anim.SetTrigger("fire");
                break;
        }
    }
}