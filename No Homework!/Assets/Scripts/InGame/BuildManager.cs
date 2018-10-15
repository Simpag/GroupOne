using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour {

    private static BuildManager instance;
    public static BuildManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask towerLayer;

    //Building variables
    public InGameShopItemStats towerToBuild;
    private Transform followingTower;
    [HideInInspector]
    public bool canBuild;
    private Vector3 locationToBuild;
    private Camera cam;
    private bool towerSelectedToBuy;

    //Tower info variables
    private Tower towerInfo;

    void Start()
    {
        cam = Camera.main;
    }

    private void Awake()
    {
        //Create singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        canBuild = true;
        towerSelectedToBuy = false;
    }

    private void Update()
    {
        if (towerSelectedToBuy) //Building a tower
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f, groundLayer))
            {
                locationToBuild = hit.point;
                Debug.Log("hit");
            }

            if (followingTower == null)
            {
                followingTower = Instantiate(towerToBuild.TowerPrefab, locationToBuild, Quaternion.identity).transform;
                followingTower.GetComponent<Tower>().MovingTower();
            }
            else if (Input.GetMouseButton(0))
            {
                FollowMouse();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                BuildTower();
            }
        }
        else //Tower info
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100f, towerLayer))
                {
                    towerInfo = hit.transform.GetComponentInParent<Tower>();
                }

                if (towerInfo != null)
                {
                    //Show tower info ui here
                    Debug.Log(towerInfo.name);
                }

                towerInfo = null;
            }
        }
    }

    public static void SelectTowerToBuild(InGameShopItemStats _tower)
    {
        Instance.towerToBuild = _tower;
        Instance.towerSelectedToBuy = true;
    }

    private void BuildTower()
    {
        if (canBuild)
        {
            InGameShopManager.PurchasedTower(towerToBuild);

            followingTower.position = locationToBuild;
            followingTower.GetComponent<Tower>().PlacedTower();

            //Reset variables
            followingTower = null;
            towerToBuild = null;
            towerSelectedToBuy = false;
        }
    }

    private void FollowMouse()
    {
        followingTower.position = locationToBuild;
    }
}
