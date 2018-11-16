using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks;
using GameSparks.Core;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Api.Messages;

public class MulitplayerManager : MonoBehaviour {

    private static MulitplayerManager instance;
    public static MulitplayerManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    private string challengeId;

    public static string ChallengeId
    {
        get { return Instance.challengeId; }
        set { Instance.challengeId = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void Matchmaking()
    {
        Debug.Log("Looking for match...");
        new MatchmakingRequest()
        .SetMatchShortCode(GameConstants.DEFAULT_MATCHMAKING_SHORTCODE)
        .SetSkill(0)
        .Send((response) => {
            GSData scriptData = response.ScriptData;
        });
    }
}
