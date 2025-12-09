using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region enums
public enum CurrentStageype
{
    BEACH, CITY
}

public enum ModeType
{
    SUNNY, EVENING, NIGHT
}

public enum MissionObjectiveType
{
    FINISHLINE, OVERTAKECARS, DRIVETODISTANCE, WHEELIE, SCORE, HIGHSPEED, NEARMISS, WRONGWAY, COMBO, WHEELIEOVERTAKE
}

public enum VEHICLETYPE
{
    BUS, CAR, FIRETRUCK, POLICECAR, SPORTSCAR, TAXI, VAN, PICKUP, NONE
}

public enum LevelCompletedResult
{
    COMPLETE, CRASHED, TIMEUP
}

public enum WayType { OneWay, TwoWay }

public enum GameOverType { TimeUp, Crashed, LevelComplete }

public enum AxisState
{
    Idle,
    Down,
    Held,
    Up
}

public enum BikeSpeedType
{
    ANALOGUE,
    DIGITAL,
    NONE
}

public enum BikeGearType
{
    ANALOGUE,
    DIGITAL,
    NONE
}

public enum RoadPatchType
{
    BEACH,
    CITY
}

public enum SettingsToggleType
{
    GRAPHICS,
    SOUNDS,
    MUSIC,
    HELMET,
    NONE
}

public enum LoopingSFX
{
    COINCOUNTER
}

public enum TrafficState
{
    // Token: 0x04000950 RID: 2384
    GoingForward,
    // Token: 0x04000951 RID: 2385
    ChangingLane
}

public enum GraphicsQuality
{
    HIGH, 
    LOW
}

public enum ShadowsToUse
{
    REALTIME,
    PLANAR
}

public enum HornType
{
    // Token: 0x04000953 RID: 2387
    Car,
    // Token: 0x04000954 RID: 2388
    Bus,
    // Token: 0x04000955 RID: 2389
    Truck
}
#endregion

public class GlobalStringsContainer : MonoBehaviour
{

    public static class USERPROPERTIES
    {
        public static string USERCOUNTRY = "user_country";
    }

    //prefs
    public static string RATEUS_PREF = "GameRated";
    public static string UNLOCKEVERYTHING_PREF = "EverythingUnlocked";
    public static string ALLSKINSUNLOCKED = "AllSkinsUnlocked";
    public static string SKINUNLOCKED = "SkinUnlocked";
    public static string QUALITYLEVELPREF = "QualityLevel";

    public static string ALLCARSUNLOCKED = "AllCarsUnlocked";
    public static string ALLLEVELS_UNLOCKED = "AllLevelsUnlocked";
    public static string EVERYTHINGUNLOCKED = "EverythingUnlocked";

    public static string UnlockedLevelCount = "LevelsUnlocked";

    public static string RemoveAdsPref = "RemoveAds";
    public static string levelPref = "Level";
    public static string currentLevelPref = "CurrentLevel";
    public static string currentSkyBoxPref = "CurrentSkybox";
    public static string PREVIOUSMODEPREF = "PreviouseMode";

    public static string SELECTEDMODEPREF = "SelectedModeIndex";
    public static string SELECTEDWAYTYPEPREF = "SelectedWayType";
    public static string SELECTEDPLAYERPREF = "SelectedPlayerCarIndex";

    public static string LEVELCOUNTERPREF = "LEVELCOUNTER";
    public static string SPEEDUIPREF = "SPEEDUI";
    public static string BRAKESUIPREF = "BRAKESUI";
    public static string WHEELIEUIPREF = "TIMEUI";
    public static string TWOWAYMODECOUNTPREF = "TwoWay";
    //SCENE_NAMES
    public static string HOMESCENENAME = "Splash";
    public static string MAINMENU = "MainMenu";
    public static string LOADINGSCENENAME = "Loading";
    public static string GAMEPLAY = "Motel_NEW";
    public static string CaveSceneName = "Cave";
   // public static string GAMEPLAY = "TrafficModule";

    //inapps
    public static string REMOVEADS_IAP = "com.rds.bike.racer.highway.rider.traffic.bike.racing.game.noads";
    public static string BUNDLECOINS_IAP = "com.rds.bike.racer.highway.rider.traffic.bike.racing.game.bundlecoins";
    public static string CHESTCOINS_IAP = "com.rds.bike.racer.highway.rider.traffic.bike.racing.game.chestcoins";
    public static string HANDFULCOINS_IAP = "com.rds.bike.racer.highway.rider.traffic.bike.racing.game.handfulcoins";
    public static string SACKCOINS_IAP = "com.rds.bike.racer.highway.rider.traffic.bike.racing.game.sackofcoins";

    public static string UNLOCKEVERYTHING_IAP = "com.rds.bike.racer.highway.rider.traffic.bike.racing.game.allunlock";

    public static string UNLOCKALLBIKES_IAP = "com.rds.bike.racer.highway.rider.traffic.bike.racing.game.unlockbikes";


    public static string UNLOCKALLLEVELS_IAP = "com.rds.bike.racer.highway.rider.traffic.bike.racing.game.unlocklevels";

    //public static string UNLOCKTHISBIKE_IAP = "com.rds.bike.racer.highway.rider.traffic.bike.racing.game.unlockcar";
    //public static string UNLOCKALLSKINS_IAP = "com.rds.bike.racer.highway.rider.traffic.bike.racing.game.unlockallskins";
    //public static string UNLOCKTHISSKIN_IAP = "com.rds.bike.racer.highway.rider.traffic.bike.racing.game.unlockskin";

    //public static string UNLOCKTHISROBOT_IAP = "com.fgz.us.police.car.transform.robot.flying.car.games.unlockrobot";
    //public static string UNLOCKALLROBOTS_IAP = "com.fgz.us.police.car.transform.robot.flying.car.games.unlockallrobots";

    //public static string UNLOCKALLEAGLES_IAP = "com.fgz.us.police.eagle.robot.transform.flying.bike.game.unlockalleagles";
    //public static string UNLOCKTHISEAGLE_IAP = "com.fgz.us.police.eagle.robot.transform.flying.bike.game.unlockeagle";


    //links
    public static string RATEUS = "market://details?id=com.rds.bike.racer.highway.rider.traffic.bike.racing.game";
    public static string MOREAPPS = "https://play.google.com/store/apps/developer?id=Roadster%20Inc%20-%203D%20Games%20Action%20%26%20Simulation&hl=en";
    public static string PRIVACYPOLICYLINK = "http://www.roadsterinc.co/privacy/privacypage.htm";


    //tags
    public static string PICKUP_LOCATION_TAG = "PickUp";
    public static string DROP_LOCATION_TAG = "Drop";
    public static string PLAYERTAG = "Player";
    public static string LEFTRAIL_TAG = "LeftRail";
    public static string RIGHTRAIL_TAG = "RightRail";

    //animatior paramerters
    public static int MOVEMENT_FLOAT = Animator.StringToHash("Movement");
    public static int ACCELERATIONFLOAT = Animator.StringToHash("AccelInput");
    public static int CHANGINGGEARBOOL = Animator.StringToHash("ChangingGear");
    public static int WHEELIEANIMATORSTRING = Animator.StringToHash("Wheelie");
    public static int DEADANIMATORSTRING = Animator.StringToHash("Dead");
    public static int LOADINGANIMATORSTRING = Animator.StringToHash("unfill");

    //general
    public static string TESTDEVICEID = "FA10960869FB60D2AD53D0B2AE307A8C";
    public static int NATIVEADINDEX = 6;
    public static int TOPSMARTBANNERINDEX = 0;
    public static int TOPSIMPLEBANNERINDEX = 1;
    public static int BOTTOMSIMPLEBANNERINDEX = 3;
    public static int MEDIUMRECTBANNERINDEX = 2;
    public static int DISTANCEMP = 320;


    //Traffic Variables
    public static float msToKmhFactor = 100;
    public static float oneOverMsToKmhFactor = 1/100;
    public static int trafficDensityForRelaxMode = 8;


    //firebase remote config variables
    public static string LEVELORDERFIREBASE = "levelsOrder";
    public static string APPLYCHANGESFIREBASE = "applyChanges";

    //AudioClip names
    public static string MainMenuMusic = "MainMenuMusic";
    public static string GamePlayMusic = "GamePlayMusic";
    public static string ButtonClickSound = "ButtonClickSound";
    public static string MissionStartSound = "MissionStartSound";
    public static string HeadShotSound = "HeadShotSound";
    
}
