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
                followingTower.position = locationToBuild;
                Debug.Log("Following");
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
        InGameShopManager.PurchasedTower(towerToBuild);

        followingTower.position = locationToBuild;
        followingTower.GetComponent<Tower>().isActive = true;

        //Reset variables
        followingTower = null;
        towerToBuild = null;
    }
}
