using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants {

    //Student constants
    public const int NUMBER_OF_UPGRADES = 3;

    //Item specific tags
    public const string ITEM_STORE = "STOREITEM";

    //Virtual Currencies
    public const string CURRENCY_CANDY_COIN = "CC";

    //Game scenes
    public const string MAIN_MENU_SCENE = "MainScene";
    public const string GAME_SCENE = "GameScene";

    //Game constants
    public const string TEACHER_TAG = "Teacher";
    public const string STUDENT_AREA_TAG = "StudentArea";
    public const string STUDENT_BUFF_AREA = "StudentBuffArea";
    public const string CAN_PLACE_STUDENT = "CanPlaceStudent";

    //Multiplayer constants
    public const string RANDOM_MATCHMAKING_SHORTCODE = "RandomMatchMaking";
    public const int OPCODE_STUDENT_BUILT = 1;
    public const int OPCODE_STUDENT_UPGRADE = 2;

    public const uint PACKET_STUDENT_SHORTCODE = 1;
    public const uint PACKET_STUDENT_GUID = 2;
    public const uint PACKET_STUDENT_POSITION = 3;
    public const uint PACKET_STUDENT_UPGRADE_ROW = 4;
}
