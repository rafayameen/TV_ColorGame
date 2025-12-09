using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static partial class GameConstants
{
    public const string TotalLevelsUnlocked = "totalLevelsUnlocked";
    public const string TotalLevelsPlayed = "totalLevelsPlayed";

    public const string CurrentSelectedLevel = "currentSelectedLevel";

    public const string AllLevelsUnlocked = "areAllLevelsUnlocked";

    public const string LevelNamePrefix = "level_";
    public const string gamePrefixForEvents = "GS_"; //Goods Sort

    public const int MAX_PIECE_COUNT = 20;
    public const int TOTAL_TOURNAMENT_MODEL_LEVELS = 6;

    public const int FREE_BOOSTERS_TOGIVE_AFTER_TUTORIAL = 1;

    //scene names
    public const string loadingSceneName = "Loading";
    public const string mainMenuSceneName = "MainMenu";
    public const string cutscene_SceneName = "CutScene";

    public const string gameplayScene_Mode1 = "Gameplay";
    public const string gameplayScene_Mode2 = "Gameplay_Castle";
    public const string gameplayScene_Mode3 = "Gameplay_Island";
    //public const string gameplaySceneName = "Gameplay";
    //public const string gameplaySceneName = "Gameplay";

    //prefs
    public const string RemoveAds = "RemoveAds";
    public const string COINS = "COINS";
    public const string STARS = "STARS";
    public const string LIVES = "LIVES";
    public const string TUTORIAL_COMPLETE = "TUTORIAL_COMPLETE";
    public const string BG_SKIN = "BG_SKIN";
    public const string SELECTED_SKIN_INDEX = "SELECTED_SKIN_INDEX";
    public const string UserDetails = "UserDetails";
    public const string TeamDetails = "TeamDetails";
    public const string Arrow_tag = "Arrow";
    public const string LEADERBOARD_REWARD_COUNT = "LEADERBOARD_REWARD_COUNT";
    public const string VIBRATIONS_ON = "VIBRATIONS_ON";

    public const string SKIN_VIDEOS_WATCHED = "SKIN_VIDEOS_WATCHED";

    public const string INFINITE_LIVES = "INFINITE_LIVES";
    public const string INFINITE_LIVES_TIME = "INFINITE_LIVES_TIME";

    public const string RATEUS_SHOWN_COUNT = "RATEUS_SHOWN_COUNT";
    public const string RATEUS_CLOSED_COUNT = "RATEUS_CLOSED_COUNT";
    public const string RATING_POPUP_SHOWN = "RATING_POPUP_SHOWN";

    public const string REVIEW_POPUP_SHOWN = "REVIEW_POPUP_SHOWN";
    public const string TEAM_JOINED = "TEAM_JOINED";
    public const string ASK_LIVES = "ASK_LIVES";
    public const string NOTIFICAITONS_ALLOWED = "NOTIFICAITONS_ALLOWED";
    public const string Flag_ID = "Flag_ID";

    public const string CurrentTournamentModeLevel = "CurrentTournamentModeLevel";
    public const string Flag_ID_OPPONENT = "Flag_ID_OPPONENT";
    public const string matchWon = "matchWon";

    public const string CONSENT_VALUE_SET = "CONSENT_VALUE_SET";
    public const string CURRENT_ITEM_GROUP_INDEX = "CURRENT_ITEM_GRPUP_INDEX";
    public const string CURRENT_ITEM_GROUP_LEVEL_COUNT = "CURRENT_ITEM_GROUP_LEVEL_COUNT";
    public const string ALL_ITEMS_UNLOCKED = "ALL_ITEMS_UNLOCKED";


    //inapp ids
    public const string IAP_REMOVE_ADS = "com.digging.removeads";

    public const string IAP_x2_2000 = "com.goods.sort.triple.match.puzzle.2000x2";
    public const string IAP_x4_7000 = "com.goods.sort.triple.match.puzzle.7000x4";
    public const string IAP_x8_14000 = "com.goods.sort.triple.match.puzzle.14000x8";
    public const string IAP_x16_30000 = "com.goods.sort.triple.match.puzzle.30000x16";
    public const string IAP_x32_70000 = "com.goods.sort.triple.match.puzzle.70000x32";

    public const string IAP_x2000_li = "com.goods.sort.triple.match.puzzle.2000x2_li";
    public const string IAP_x2000_la = "com.goods.sort.triple.match.puzzle.2000x2_la";

    public const string IAP_coins_1000 = "com.goods.sort.triple.match.puzzle.cx1000";
    public const string IAP_coins_2600 = "com.goods.sort.triple.match.puzzle.cx2600";
    public const string IAP_coins_5400 = "com.goods.sort.triple.match.puzzle.cx5400";
    public const string IAP_coins_12000 = "com.goods.sort.triple.match.puzzle.cx12000";
    public const string IAP_coins_25000 = "com.goods.sort.triple.match.puzzle.cx25000";
    public const string IAP_coins_65000 = "com.goods.sort.triple.match.puzzle.cx65000";
    public const string EASTER_DEAL = "com.goods.sort.triple.match.puzzle.easterdeal";

    //animator strings
    public static int COMBO_TEXT_ANIMATION = Animator.StringToHash("comboAnimation");
    public static int STARS_ANIMATIONS = Animator.StringToHash("starsAnimation");
    public static int MOVE_AWAY_MAGICWAND = Animator.StringToHash("moveAwayMagicWand");
    public static int SCALE_DOWN_GENERIC = Animator.StringToHash("ScaleDown");

    //other strings
    public const string currency_coins = "coins";
    public const string currency_stars = "stars";

    //links
    public const string LINK_MORE_GAMES = "https://play.google.com/store/apps/dev?id=9114084439485872389";
    //public const string LINK_RATE_US = "https://play.google.com/store/apps/dev?id=9114084439485872389";
    //public const string LINK_RATE_US = "market://details?id=" + Application.identifier;
    public const string LINK_PRIVACYPOLICY = "http://theatlantisstudios.com/privacy/";

    //ByteBrew
    public const string level_Id = "level_ID";
    public const string time = "time";
    public const string environmentKey = "environment";
    public const string levelStartKey = "level_start";
    public const string levelCompleteKey = "level_complete";
    public const string levelFailKey = "level_fail";
    public const string levelPauseKey = "level_pause";
    public const string levelResumeKey = "level_resume";
    public const string level_ad_detail = "level_ad_detail";
    public const string level_ad = "level_ad";
    public const string inapp_purchase = "inapp_purchase";
    public const string tutorial_step_complete = "tutorial_step_complete";
    public const string REWARDEDAD = "RewardedAd";
    public const string INTERSTIALAD = "InterstialAd";

    //layers
    public static int bgLayer = 1 << LayerMask.NameToLayer("BG");

    //ad locations
    public const string AL_LEVEL_COMPLETE = "Level_Complete";
    public const string AL_LEVEL_COMPLETE_x2 = "Level_Complete_x2";
    public const string AL_TEA_BREAK = "Tea_Break";
    public const string AL_MAIN_MENU_RETURN = "Main_Menu_Rtn";
    public const string AL_Time_Up = "Time_Up";
    public const string AL_Time_Up_Extra_Time = "Time_Up_Extra_Time";
    public const string AL_Level_Fail = "Level_Fail";
    public const string AL_Level_Fail_Extra_Refresh = "Level_Fail_Extra_Refresh";

    public const string AL_Powerup_Refresh = "Powerup_Refresh";
    public const string AL_Powerup_Hammer_x3 = "Powerup_Hammer_x3";
    public const string AL_Powerup_Hammer_x12 = "Powerup_Hammer_x12";
    public const string AL_Powerup_Change_x9 = "Powerup_Change_x9";

    public const string AL_Powerup_Freeze = "Powerup_Freeze";
    public const string AL_Powerup_Extra_Time = "Powerup_Extra_Time";
    public const string AL_Powerup_Score_x2 = "Powerup_Score_x2";


    public const string AL_Level_Start_Power_Up_Combo = "Level_Start_Power_Up_Combo";
    public const string AL_Skin_Unlock = "Skin_Unlock";

    public const string AL_Free_Coin_Shop = "Free_Coin_Shop";
    public const string AL_Shop = "Shop";
    public const string Out_Of_Lives = "Out_Of_Lives";




}
