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
    public const string PRE_GAME_SCENE = "PreGameScene";

    //Game constants
    public const string TEACHER_TAG = "Teacher";
    public const string STUDENT_AREA_TAG = "StudentArea";
    public const string STUDENT_BUFF_AREA = "StudentBuffArea";
    public const string CAN_PLACE_STUDENT = "CanPlaceStudent";
    public const string COMPLETED_MAP = "Won_Game";

    //Multiplayer constants
    public const string RANDOM_MATCHMAKING_SHORTCODE = "RandomMatchMaking";
    public const int OPCODE_STUDENT_BUILT = 1;
    public const int OPCODE_STUDENT_UPGRADE = 2;
    public const int OPCODE_ROUND_END_INFO = 3;
    public const int OPCODE_START_NEW_ROUND = 4;
    public const int OPCODE_RANDOM_SEED = 5;
    public const int OPCODE_WRONGLY_PLACED_STUDENT = 6;
    public const int OPCODE_SOLD_STUDENT = 7;
    public const int OPCODE_READY = 8;
    public const int OPCODE_START_GAME = 9;
    public const int OPCODE_SEND_MONEY = 10;

    public const uint PACKET_STUDENT_SHORTCODE = 1;
    public const uint PACKET_STUDENT_GUID = 2;
    public const uint PACKET_STUDENT_POSITION = 3;
    public const uint PACKET_STUDENT_UPGRADE_ROW = 4;
    public const uint PACKET_ROUND_END_HOMEWORK = 5;
    public const uint PACKET_RANDOM_SEED = 6;
    public const uint PACKET_ROUND_INDEX = 7;
    public const uint PACKET_SEND_MONEY = 8;
}
