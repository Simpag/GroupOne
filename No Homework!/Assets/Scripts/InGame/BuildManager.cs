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
                SendTowerToPartner(followingTower, locationToBuild);
            }

            followingTowerTransform.position = locationToBuild;
            followingTower.PlacedTower();

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
            SendTowerUpgradeToPartner(_towerInfo);
        }
    }

    private void SendTowerToPartner(Tower _tower, Vector3 _position)
    {
        // for all RT-data we are sending, we use an instance of the RTData object //
        // this is a disposable object, so we wrap it in this using statement to make sure it is returned to the pool //
        using (RTData data = RTData.Get())
        {
            data.SetString(GameConstants.PACKET_TOWER_ID, _tower.shopStats.TowerId); // we add the message data to the RTPacket at key '1', so we know how to key it when the packet is receieved
            data.SetString(GameConstants.PACKET_TOWER_GUID, _tower.towerGUID);
            data.SetVector3(GameConstants.PACKET_TOWER_POSITION, _position); // we are also going to send the time at which the user sent this message

            Debug.Log("Sending tower data to partner");
            // for this example we are sending RTData, but there are other methods for sending data we will look at later //
            // the first parameter we use is the op-code. This is used to index the type of data being send, and so we can identify to ourselves which packet this is when it is received //
            // the second parameter is the delivery intent. The intent we are using here is 'reliable', which means it will be send via TCP. This is because we aren't concerned about //
            // speed when it comes to these chat messages, but we very much want to make sure the whole packet is received //
            // the final parameter is the RTData object itself //
            MultiplayerManager.Instance.GetRTSession.SendData(GameConstants.OPCODE_TOWER, GameSparksRT.DeliveryIntent.RELIABLE, data);
        }
    }

    private void SendTowerUpgradeToPartner(Tower _towerInfo)
    {
        using (RTData data = RTData.Get())
        {
            data.SetString(GameConstants.PACKET_TOWER_GUID, _towerInfo.towerGUID);

            Debug.Log("Sending tower data to partner");
            MultiplayerManager.Instance.GetRTSession.SendData(GameConstants.OPCODE_TOWER_UPGRADE, GameSparksRT.DeliveryIntent.RELIABLE, data);
        }
    }

    public void ReceivedTowerFromPartner(RTPacket _packet)
    {
        AudioManager.Instance.Play("TowerPlacedSound");
        GameObject _prefab = null;

        string _towerId = (string)_packet.Data.GetString(GameConstants.PACKET_TOWER_ID);
        string _towerGUID = (string)_packet.Data.GetString(GameConstants.PACKET_TOWER_GUID);
        Vector3 _position = (Vector3)_packet.Data.GetVector3(GameConstants.PACKET_TOWER_POSITION);

        //Find the right tower prefab based on towerId
        foreach (InGameShopItemStats _stat in InGameShopManager.Instance.allShopItems)
        {
            if(_stat.TowerId == _towerId)
            {
                _prefab = _stat.TowerPrefab;
            }
        }

        //Instantiate partners tower
        Transform _tower = Instantiate(_prefab, _position, Quaternion.identity, towerContainer).transform;
        _tower.GetComponent<Tower>().PlacedTower();
        _tower.GetComponent<Tower>().isYours = false;
        _tower.GetComponent<Tower>().towerGUID = _towerGUID;

        //Debug.Log("Recived tower with GUID: " + _towerGUID);
    }

    public void RecivedTowerUpgradeFromPartner(RTPacket _packet)
    {
        string _guid = _packet.Data.GetString(GameConstants.PACKET_TOWER_GUID);

        Tower[] _towers = towerContainer.GetComponentsInChildren<Tower>();

        foreach (Tower _tower in _towers)
        {
            if (_tower.towerGUID == _guid)
            {
                _tower.UpgradeTower();
                return;
            } else
            {
                Debug.Log("No tower found with GUID: " + _guid);
            }
        }
    }
}
