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
    private bool towerIsSelected;

    public bool TowerIsSelected
    {
        get { return towerIsSelected; }
    }

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
        towerIsSelected = false;
    }

    private void Update()
    {
        if (towerIsSelected) //Building a tower
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, groundLayer))
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
                    InGameUIManager.ShowTowerInfo(towerInfo);
                }

                towerInfo = null;
            }
        }
    }

    public static void SelectTowerToBuild(InGameShopItemStats _tower)
    {
        Instance.towerToBuild = _tower;
        Instance.towerIsSelected = true;
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
            towerIsSelected = false;
        }
    }

    private void FollowMouse()
    {
        followingTower.position = locationToBuild;
    }
}
