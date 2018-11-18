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
        switch (_packet.OpCode)
        {
            case GameConstants.OPCODE_TOWER:
                string _towerId = (string)_packet.Data.GetString(GameConstants.PACKET_TOWER_ID);
                Vector3 _pos = (Vector3)_packet.Data.GetVector3(GameConstants.PACKET_TOWER_POSITION);

                BuildManager.Instance.ReceivedTowerFromPartner(_towerId, _pos);
                break;
        }
    }
    #endregion
}
