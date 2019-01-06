using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using GameSparks.RT;

public class BuildManager : MonoBehaviour {

    private static BuildManager instance;
    public static BuildManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    [SerializeField]
    private Transform towerContainer;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask towerLayer;

    public List<Tower> builtTowers;

    //Building variables
    public InGameShopItemStats towerToBuild;
    private Transform followingTowerTransform;
    private Tower followingTower;
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
    private Tower tower;

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
        builtTowers = new List<Tower>();
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
            }

            if (followingTowerTransform == null)
            {
                followingTowerTransform = Instantiate(towerToBuild.TowerPrefab, locationToBuild, Quaternion.identity, towerContainer).transform;
                followingTower = followingTowerTransform.GetComponent<Tower>();
                followingTower.MovingTower();
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
        else // Tower info
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100f, towerLayer))
                {
                    Tower _tower = hit.transform.GetComponentInParent<Tower>();

                    if (_tower.isYours == true)
                    {
                        tower = _tower;
                    }
                }

                if (tower != null)
                {
                    InGameUIManager.ShowTowerInfo(tower);
                }

                tower = null;
            }
        }
    }

    public static void SelectTowerToBuild(InGameShopItemStats _tower)
    {
        Instance.towerToBuild = _tower;
        Instance.towerIsSelected = true;
    }

    private void FollowMouse()
    {
        followingTowerTransform.position = locationToBuild;
    }

    private void BuildTower()
    {
        if (canBuild)
        {
            AudioManager.Instance.Play("TowerPlacedSound");
            InGameShopManager.PurchasedTower(towerToBuild);

            if (GameManager.IsMultiplayer)
            {
                MultiplayerManager.SendTowerToPartner(followingTower, locationToBuild);
            }

            followingTowerTransform.position = locationToBuild;
            followingTower.PlacedTower();

            builtTowers.Add(followingTower);

            //Reset variables
            followingTowerTransform = null;
            towerToBuild = null;
            towerIsSelected = false;
        }
    }

    public void UpgradeTower(Tower _towerInfo)
    {
        bool _success = InGameShopManager.UpgradeTower(_towerInfo);
        
        if (_success && GameManager.IsMultiplayer)
        {
            MultiplayerManager.SendTowerUpgradeToPartner(_towerInfo);
        }
    }

    public void BuildPartnerTower(GameObject _prefab, Vector3 _position, string _towerGUID)
    {
        AudioManager.Instance.Play("TowerPlacedSound");

        Transform _tower = Instantiate(_prefab, _position, Quaternion.identity, towerContainer).transform;
        _tower.GetComponent<Tower>().PlacedTower();
        _tower.GetComponent<Tower>().isYours = false;
        _tower.GetComponent<Tower>().towerGUID = _towerGUID;

        builtTowers.Add(_tower.GetComponent<Tower>());

        //Debug.Log("Recived tower with GUID: " + _towerGUID);
    }
}
