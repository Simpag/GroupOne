using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants {

    //Item specific tags
    public const string ITEM_STORE = "STOREITEM";

    //Virtual Currencies
    public const string CURRENCY_CANDY_COIN = "CC";

    //Game scenes
    public const string MAIN_MENU_SCENE = "MainScene";
    public const string GAME_SCENE = "GameScene";

    //Multiplayer constants
    public const string RANDOM_MATCHMAKING_SHORTCODE = "RandomMatchMaking";
    public const int OPCODE_TOWER = 1;
    public const int OPCODE_TOWER_UPGRADE = 2;

    public const uint PACKET_TOWER_ID = 1;
    public const uint PACKET_TOWER_GUID = 2;
    public const uint PACKET_TOWER_POSITION = 3;
}
