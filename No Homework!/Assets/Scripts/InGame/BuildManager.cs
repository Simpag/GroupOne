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
    private LayerMask layerMask;

    [HideInInspector]
    public bool canBuild;
    private Vector3 locationToBuild;
    private Camera cam;

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        towerToBuild = null;
        canBuild = true;
    }

    private void Update()
    {
        if (towerToBuild != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, layerMask))
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
    }

    private void BuildTower()
    {
        if (canBuild)
        {
            InGameShopManager.PurchasedTower(towerToBuild);

            followingTower.position = locationToBuild;
            Tower _towerComponent = followingTower.GetComponent<Tower>();
            _towerComponent.isActive = true;
            _towerComponent.rangeView.gameObject.SetActive(false);
            _towerComponent.towerArea.GetComponent<TowerArea>().TowerPlaced();

            //Reset variables
            followingTower = null;
            towerToBuild = null;
        }
    }

    private void FollowMouse()
    {
        followingTower.position = locationToBuild;
    }
}
