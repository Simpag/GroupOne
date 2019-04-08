using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameManager : MonoBehaviour
{
    private static PreGameManager instance;
    public static PreGameManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    bool foundPartner;
    bool partnerReady;

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

        foundPartner = false;
        partnerReady = false;
    }

    private void Start()
    {
        LookForPartner();
    }

    private void LookForPartner()
    {
        MultiplayerManager.Instance.RandomMatchMaking();
    }

    public void StartGame()
    {
        if (!partnerReady || !foundPartner)
            return;

        if (MultiplayerManager.IsHost)
            MultiplayerManager.SendGameStartToPartner();

        GameManager.StartGame(GameManager.Startmethod.multiplayer); //Start multiplayer match

        partnerReady = false;
        foundPartner = false;
    }

    public void FoundPartner()
    {
        foundPartner = true;
    }

    public void PartnerReady()
    {
        partnerReady = true;
    }
}
