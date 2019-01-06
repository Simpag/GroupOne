using GameSparks.Api.Responses;
using GameSparks.Core;
using GameSparks.RT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerManager : MonoBehaviour {

    private static MultiplayerManager instance;
    public static MultiplayerManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    //Ref to gamesparks own rt script
    private GameSparksRTUnity gameSparksRTUnity;
    public GameSparksRTUnity GetRTSession
    {
        get { return gameSparksRTUnity; }
    }

    //Ref to custom session info class
    private RTSessionInfo sessionInfo;
    public RTSessionInfo GetSessionInfo
    {
        get { return sessionInfo; }
    }

    private void Awake()
    {
        //Create singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // this listener will update the text in the player-list field if no match was found //
        GameSparks.Api.Messages.MatchNotFoundMessage.Listener = (message) => {
            Debug.Log("No Match Found...");
        };

        GameSparks.Api.Messages.MatchFoundMessage.Listener += OnMatchFound;
    }

    public void StartNewRTSession(RTSessionInfo _info)
    {
        Debug.Log("Creating New RT Session Instance...");
        sessionInfo = _info;
        gameSparksRTUnity = this.gameObject.AddComponent<GameSparksRTUnity>(); // Adds the RT script to the game
        // In order to create a new RT game we need a 'FindMatchResponse' //
        // This would usually come from the server directly after a successful MatchmakingRequest //
        // However, in our case, we want the game to be created only when the first player decides using a button //
        // therefore, the details from the response is passed in from the gameInfo and a mock-up of a FindMatchResponse //
        // is passed in. //
        GSRequestData mockedResponse = new GSRequestData()
                                            .AddNumber("port", (double)_info.GetPortID)
                                            .AddString("host", _info.GetHostURL)
                                            .AddString("accessToken", _info.GetAccessToken); // construct a dataset from the game-details

        FindMatchResponse response = new FindMatchResponse(mockedResponse); // create a match-response from that data and pass it into the game-config
        // So in the game-config method we pass in the response which gives the instance its connection settings //
        // In this example, I use a lambda expression to pass in actions for 
        // OnPlayerConnect, OnPlayerDisconnect, OnReady and OnPacket actions //
        // These methods are self-explanatory, but the important one is the OnPacket Method //
        // this gets called when a packet is received //

        gameSparksRTUnity.Configure(response,
            (peerId) => { OnPlayerConnectedToGame(peerId); },
            (peerId) => { OnPlayerDisconnected(peerId); },
            (ready) => { OnRTReady(ready); },
            (packet) => { OnPacketReceived(packet); });
        gameSparksRTUnity.Connect(); // when the config is set, connect the game

    }

    #region Matchmaking Request
    /// <summary>
    /// This will request a match between as many players you have set in the match.
    /// When the max number of players is found each player will receive the MatchFound message
    /// </summary>
    public void RandomMatchMaking()
    {
        Debug.Log("Attempting Matchmaking...");
        new GameSparks.Api.Requests.MatchmakingRequest()
            .SetMatchShortCode(GameConstants.RANDOM_MATCHMAKING_SHORTCODE) // set the shortCode to be the same as the one we created in the first tutorial
            .SetSkill(0) // in this case we assume all players have skill level zero and we want anyone to be able to join so the skill level for the request is set to zero
            .Send((response) => {
                if (response.HasErrors)
                { // check for errors
                    Debug.LogError("MatchMaking Error \n" + response.Errors.JSON);
                }
            });
    }
    #endregion


    #region Callbacks

    private void OnMatchFound(GameSparks.Api.Messages.MatchFoundMessage _message)
    {
        Debug.Log("Match Found!...");
        sessionInfo = new RTSessionInfo(_message); // we'll store the match data until we need to create an RT session instance 
        StartNewRTSession(sessionInfo);
    }

    private void OnPlayerConnectedToGame(int _peerId)
    {
        Debug.Log("Player Connected, " + _peerId);
    }

    private void OnPlayerDisconnected(int _peerId)
    {
        Debug.Log("Player Disconnected, " + _peerId);
    }

    private void OnRTReady(bool _isReady)
    {
        if (_isReady)
        {
            Debug.Log("RT Session Connected...");
            GameManager.StartGame(true); //Start multiplayer match
        }

    }

    private void OnPacketReceived(RTPacket _packet)
    {
        Debug.Log("Packet recived with code: " + _packet.OpCode.ToString());

        switch (_packet.OpCode)
        {
            case GameConstants.OPCODE_TOWER:
                Instance.ReceivedTowerFromPartner(_packet);
                break;

            case GameConstants.OPCODE_TOWER_UPGRADE:
                Instance.RecivedTowerUpgradeFromPartner(_packet);
                break;
        }
    }
    #endregion

    #region Multiplayer

    public static void SendTowerToPartner(Tower _tower, Vector3 _position)
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

    public static void SendTowerUpgradeToPartner(Tower _towerInfo)
    {
        using (RTData data = RTData.Get())
        {
            data.SetString(GameConstants.PACKET_TOWER_GUID, _towerInfo.towerGUID);

            Debug.Log("Sending tower data to partner");
            MultiplayerManager.Instance.GetRTSession.SendData(GameConstants.OPCODE_TOWER_UPGRADE, GameSparksRT.DeliveryIntent.RELIABLE, data);
        }
    }

    private void ReceivedTowerFromPartner(RTPacket _packet)
    {
        AudioManager.Instance.Play("TowerPlacedSound");
        GameObject _prefab = null;

        string _towerId = (string)_packet.Data.GetString(GameConstants.PACKET_TOWER_ID);
        string _towerGUID = (string)_packet.Data.GetString(GameConstants.PACKET_TOWER_GUID);
        Vector3 _position = (Vector3)_packet.Data.GetVector3(GameConstants.PACKET_TOWER_POSITION);

        //Find the right tower prefab based on towerId
        foreach (InGameShopItemStats _stat in InGameShopManager.Instance.allShopItems)
        {
            if (_stat.TowerId == _towerId)
            {
                _prefab = _stat.TowerPrefab;
            }
        }

        //Instantiate partners tower
        BuildManager.Instance.BuildPartnerTower(_prefab, _position, _towerGUID);
    }

    private void RecivedTowerUpgradeFromPartner(RTPacket _packet)
    {
        string _guid = _packet.Data.GetString(GameConstants.PACKET_TOWER_GUID);

        foreach (Tower _tower in BuildManager.Instance.builtTowers)
        {
            if (_tower.towerGUID == _guid)
            {
                _tower.UpgradeTower();
                return;
            }
            else
            {
                Debug.Log("No tower found with GUID: " + _guid);
            }
        }
    }

    #endregion
}
