using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {

    private static BuildManager instance;
    public static BuildManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    public InGameShopItem towerToBuild;
    private Transform followingTower;

    [SerializeField]
    private LayerMask groundLayer;

    [HideInInspector]
    public bool canBuild;
    private Vector3 locationToBuild;
    private Camera cam;
    private bool towerSelected;

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
        towerSelected = false;
    }

    private void Update()
    {
        if (towerSelected)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, groundLayer))
            {
                locationToBuild = hit.point;
            }

            if (followingTower == null)
            {
                followingTower = Instantiate(towerToBuild.Prefab, locationToBuild, Quaternion.identity).transform;
            }
            else
            {
                FollowMouse();
            }

            if (Input.GetMouseButtonDown(0))
            {
                BuildTower();
            }
        }
    }

    public static void SelectTower(InGameShopItem _tower)
    {
        Instance.towerToBuild = _tower;
        Instance.towerSelected = true;
    }

    private void BuildTower()
    {
        if (canBuild)
        {
            InGameShopManager.PurchasedTower(towerToBuild);

            followingTower.position = locationToBuild;
            Tower _towerComponent = followingTower.GetComponent<Tower>();
            _towerComponent.isActive = true;
            _towerComponent.rangeView.gameObject.GetComponent<MeshRenderer>().enabled = false;

            //Reset variables
            followingTower = null;
            towerToBuild = null;
            towerSelected = false;
        }
    }

    private void FollowMouse()
    {
        followingTower.position = locationToBuild;
    }
}
