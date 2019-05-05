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

    private bool isHost, isPartnerRoundDone;
    private string partnerName;

    public bool IsPartnerRoundDone { get { return isPartnerRoundDone; } }
    public static bool IsHost { get { return instance.isHost; } }
    public string PartnerName { get { return partnerName; } }

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

        isHost = true;
        isPartnerRoundDone = true;
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

            if (sessionInfo.GetPlayerList.Find(x => x.id == AccountInfo.Instance.UserId).peerId == 1)
            {
                isHost = true;
            }
            else
            {
                isHost = false;
            }

            partnerName = sessionInfo.GetPlayerList.Find(x => x.id != AccountInfo.Instance.UserId).displayName;

            //Debug.Log("Is Host: " + isHost);

            PreGameManager.Instance.FoundPartner();
        }
    }

    private void OnPacketReceived(RTPacket _packet)
    {
        Debug.Log("Packet recived with code: " + _packet.OpCode.ToString());

        switch (_packet.OpCode)
        {
            //PreGame
            case GameConstants.OPCODE_READY:
                Instance.RecivedReadyFromPartner();
                break;
            case GameConstants.OPCODE_START_GAME:
                Instance.RecivedStartGameFromPartner();
                break;

            //InGame
            case GameConstants.OPCODE_STUDENT_BUILT:
                Instance.ReceivedTowerFromPartner(_packet);
                break;

            case GameConstants.OPCODE_STUDENT_UPGRADE:
                Instance.RecivedTowerUpgradeFromPartner(_packet);
                break;

            case GameConstants.OPCODE_RANDOM_SEED:
                Instance.RecivedRandomSeed(_packet);
                break;

            case GameConstants.OPCODE_ROUND_END_INFO:
                Instance.RecivedRoundEndInfo(_packet);
                break;

            case GameConstants.OPCODE_START_NEW_ROUND:
                Instance.RecivedStartOfRound();
                break;

            case GameConstants.OPCODE_WRONGLY_PLACED_STUDENT:
                Instance.ReceivedWronglyPlacedStudentFromPartner(_packet);
                break;

            case GameConstants.OPCODE_SOLD_STUDENT:
                Instance.ReceivedSoldStudent(_packet);
                break;

            case GameConstants.OPCODE_SEND_MONEY:
                Instance.ReceivedMoneyFromPartner(_packet);
                break;
        }
    }
    #endregion

    #region PreGame Multiplayer

    public static void SendReadyToPartner()
    {
        using (RTData data = RTData.Get())
        {
            Debug.Log("Sending ready statement to partner");
            MultiplayerManager.Instance.GetRTSession.SendData(GameConstants.OPCODE_READY, GameSparksRT.DeliveryIntent.RELIABLE, data);
        }
    }

    public static void SendGameStartToPartner()
    {
        using (RTData data = RTData.Get())
        {
            Debug.Log("Sending ready statement to partner");
            MultiplayerManager.Instance.GetRTSession.SendData(GameConstants.OPCODE_START_GAME, GameSparksRT.DeliveryIntent.RELIABLE, data);
        }
    }

    private void RecivedReadyFromPartner()
    {
        PreGameManager.Instance.PartnerReady();
    }

    private void RecivedStartGameFromPartner()
    {
        PreGameManager.Instance.StartGame();
    }

    #endregion

    #region InGame Multiplayer 

    public static void Disconnect()
    {
        Instance.GetRTSession.Disconnect();
        GameManager.EndGame(false);
    }

    public static void SendTowerToPartner(StudentStats _tower, Vector3 _position)
    {
        // for all RT-data we are sending, we use an instance of the RTData object //
        // this is a disposable object, so we wrap it in this using statement to make sure it is returned to the pool //
        using (RTData data = RTData.Get())
        {
            data.SetString(GameConstants.PACKET_STUDENT_SHORTCODE, _tower.shopStats.ShortCode); // we add the message data to the RTPacket at key '1', so we know how to key it when the packet is receieved
            data.SetString(GameConstants.PACKET_STUDENT_GUID, _tower.studentGUID);
            data.SetVector3(GameConstants.PACKET_STUDENT_POSITION, _position); // we are also going to send the time at which the user sent this message

            Debug.Log("Sending tower data to partner");
            // for this example we are sending RTData, but there are other methods for sending data we will look at later //
            // the first parameter we use is the op-code. This is used to index the type of data being send, and so we can identify to ourselves which packet this is when it is received //
            // the second parameter is the delivery intent. The intent we are using here is 'reliable', which means it will be send via TCP. This is because we aren't concerned about //
            // speed when it comes to these chat messages, but we very much want to make sure the whole packet is received //
            // the final parameter is the RTData object itself //
            MultiplayerManager.Instance.GetRTSession.SendData(GameConstants.OPCODE_STUDENT_BUILT, GameSparksRT.DeliveryIntent.RELIABLE, data);
        }
    }

    public static void SendTowerUpgradeToPartner(StudentStats _towerInfo, int _row)
    {
        using (RTData data = RTData.Get())
        {
            data.SetString(GameConstants.PACKET_STUDENT_GUID, _towerInfo.studentGUID);
            data.SetInt(GameConstants.PACKET_STUDENT_UPGRADE_ROW, _row);

            Debug.Log("Sending tower data to partner");
            MultiplayerManager.Instance.GetRTSession.SendData(GameConstants.OPCODE_STUDENT_UPGRADE, GameSparksRT.DeliveryIntent.RELIABLE, data);
        }
    }

    public static void SendRoundEndInformation(int _homework)
    {
        using (RTData data = RTData.Get())
        {
            data.SetInt(GameConstants.PACKET_ROUND_END_HOMEWORK, _homework);
            data.SetInt(GameConstants.PACKET_ROUND_INDEX, WaveSpawner.Instance.WaveIndex);

            Debug.Log("Sending end round");
            MultiplayerManager.Instance.GetRTSession.SendData(GameConstants.OPCODE_ROUND_END_INFO, GameSparksRT.DeliveryIntent.RELIABLE, data);
        }
    }

    public static void SendStartOfRound()
    {
        using (RTData data = RTData.Get())
        {
            Debug.Log("Sending start round");
            MultiplayerManager.Instance.GetRTSession.SendData(GameConstants.OPCODE_START_NEW_ROUND, GameSparksRT.DeliveryIntent.RELIABLE, data);
        }
    }

    public static void SendRandomSeed()
    {
        using (RTData data = RTData.Get())
        {
            data.SetInt(GameConstants.PACKET_RANDOM_SEED, Random.seed);

            Debug.Log("Sending seed");
            MultiplayerManager.Instance.GetRTSession.SendData(GameConstants.OPCODE_RANDOM_SEED, GameSparksRT.DeliveryIntent.RELIABLE, data);
        }
    }

    public static void SendWronglyPlacedStudent(string _guid)
    {
        using (RTData data = RTData.Get())
        {
            data.SetString(GameConstants.PACKET_STUDENT_GUID, _guid);

            Debug.Log("Sending tower data to partner");
            MultiplayerManager.Instance.GetRTSession.SendData(GameConstants.OPCODE_WRONGLY_PLACED_STUDENT, GameSparksRT.DeliveryIntent.RELIABLE, data);
        }
    }

    public static void SendSoldStudent(string _guid)
    {
        using (RTData data = RTData.Get())
        {
            data.SetString(GameConstants.PACKET_STUDENT_GUID, _guid);

            Debug.Log("Sending tower data to partner");
            MultiplayerManager.Instance.GetRTSession.SendData(GameConstants.OPCODE_SOLD_STUDENT, GameSparksRT.DeliveryIntent.RELIABLE, data);
        }
    }

    public static void SendMoneyToPartner(float _amount)
    {
        using (RTData data = RTData.Get())
        {
            data.SetFloat(GameConstants.PACKET_SEND_MONEY, _amount);

            Debug.Log("Sending tower data to partner");
            MultiplayerManager.Instance.GetRTSession.SendData(GameConstants.OPCODE_SEND_MONEY, GameSparksRT.DeliveryIntent.RELIABLE, data);
        }
    }

    private void ReceivedTowerFromPartner(RTPacket _packet)
    {
        AudioManager.Instance.Play("TowerPlacedSound");
        GameObject _prefab = null;

        string _towerSC = (string)_packet.Data.GetString(GameConstants.PACKET_STUDENT_SHORTCODE);
        string _towerGUID = (string)_packet.Data.GetString(GameConstants.PACKET_STUDENT_GUID);
        Vector3 _position = (Vector3)_packet.Data.GetVector3(GameConstants.PACKET_STUDENT_POSITION);

        //Find the right tower prefab based on towerId
        foreach (InGameShopItemStats _stat in InGameShopManager.Instance.allShopItems)
        {
            if (_stat.ShortCode == _towerSC)
            {
                _prefab = _stat.TowerPrefab;
                break;
            }
        }

        //Instantiate partners tower
        BuildManager.Instance.BuildPartnerTower(_prefab, _position, _towerGUID, isHost);
    }

    private void RecivedTowerUpgradeFromPartner(RTPacket _packet)
    {
        string _guid = _packet.Data.GetString(GameConstants.PACKET_STUDENT_GUID);

        foreach (StudentStats _tower in BuildManager.Instance.builtTowers)
        {
            if (_tower.studentGUID == _guid)
            {
                int? _row = _packet.Data.GetInt(GameConstants.PACKET_STUDENT_UPGRADE_ROW);
                switch (_row)
                {
                    case 1:
                        _tower.UpgradeRow1();
                        break;
                    case 2:
                        _tower.UpgradeRow2();
                        break;
                    default:
                        Debug.LogError("No upgrade row found");
                        break;
                }
                return;
            }
            else
            {
                Debug.Log("No tower found with GUID: " + _guid);
            }
        }
    }

    private void RecivedRoundEndInfo(RTPacket _packet)
    {
        int _partnerHomework = (int)_packet.Data.GetInt(GameConstants.PACKET_ROUND_END_HOMEWORK);
        int _partnerRound = (int)_packet.Data.GetInt(GameConstants.PACKET_ROUND_INDEX);

        isPartnerRoundDone = true;

        if (_partnerHomework < PlayerStats.Homework)
        {
            PlayerStats.SetHomework(_partnerHomework);
        }

        if (_partnerRound < WaveSpawner.Instance.WaveIndex)
        {
            WaveSpawner.Instance.WaveIndex = _partnerRound;
        }
    }

    private void RecivedStartOfRound()
    {
        WaveSpawner.Instance.NextRound();
        isPartnerRoundDone = false;
    }

    private void RecivedRandomSeed(RTPacket _packet)
    {
        if (!isHost) //If the player is not the host, set the seed to the host seed
            Random.InitState((int)_packet.Data.GetInt(GameConstants.PACKET_RANDOM_SEED));
    }

    private void ReceivedWronglyPlacedStudentFromPartner(RTPacket _packet)
    {
        string _guid = _packet.Data.GetString(GameConstants.PACKET_STUDENT_GUID);

        foreach (StudentStats _tower in BuildManager.Instance.builtTowers)
        {
            if (_tower.studentGUID == _guid)
            {
                BuildManager.Instance.WronglyPlacedTower(_tower);
                return;
            }
            else
            {
                Debug.Log("No tower found with GUID: " + _guid);
            }
        }
    }

    private void ReceivedSoldStudent(RTPacket _packet)
    {
        string _guid = _packet.Data.GetString(GameConstants.PACKET_STUDENT_GUID);

        foreach (StudentStats _tower in BuildManager.Instance.builtTowers)
        {
            if (_tower.studentGUID == _guid)
            {
                BuildManager.Instance.SellStudent(_tower, false);
                return;
            }
            else
            {
                Debug.Log("No tower found with GUID: " + _guid);
            }
        }
    }

    private void ReceivedMoneyFromPartner(RTPacket _packet)
    {
        float _amount = (float)_packet.Data.GetFloat(GameConstants.PACKET_SEND_MONEY);
        PlayerStats.AddCandyCurrency(_amount);
    }

    #endregion
}
