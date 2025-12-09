using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void SimpleEvent();
    public delegate void PositionEvent(Vector3 position);
    public delegate void BooleanEvent(bool value);
    public delegate void StringEvent(string value);
    public delegate void DoubleStringEvent(string str1, string str2);
    public delegate void IntegerEvent(int value);
    public delegate void ActionEvent(Action callBack);
    public delegate bool BooleanReturnEvent();
    public delegate int IntegerReturnEvent();
    public delegate float FloatReturnEvent();
    public delegate int IntegerReturnEventCustom(OreType type);
    public delegate object ObjectReturnEvent();
    public delegate void GenericObjectEvent(object obj);
    public delegate void GenericObjectBoolEvent(object obj, bool value);
    public delegate void RayCastHitEvent(RaycastHit hit);
    public delegate void TransformEvent(Transform newTransform);


    public static event PositionEvent SpawnOreAtPosition;
    public static event GenericObjectEvent oreCollectedEvent;
    public static event GenericObjectBoolEvent oreCollectedEventBool;
    public static event GenericObjectEvent addOreToInventory;
    public static event GenericObjectEvent onStartDiggingEvent;
    public static event GenericObjectEvent onObjectHighlightedEvent;
    public static event SimpleEvent onTouchpadClickedEvent;
    public static event SimpleEvent interactButtonClicked;
    public static event SimpleEvent onRefillBattery;
    public static event SimpleEvent onBuyLamp;
    public static event SimpleEvent onBuyDynamite;
    public static event SimpleEvent onEnablePlayerMovement;
    public static event SimpleEvent onPurchaseShovelUpgrade;
    public static event ReturnBoolEvent isInventoryFull;
    public static event BooleanEvent onShovelButtonPressed;
    public static event BooleanEvent onDrainBattery;
    public static event BooleanEvent onShowInteractButton;
    public static event BooleanEvent onEnableFPSPanel;
    public static event BooleanReturnEvent onGetIfBatteryEmpty;
    public static event IntegerReturnEventCustom onGetItemValueByType;
    public static event RayCastHitEvent spawnShovelAtHitPointEvent;
    public static event StringEvent onSetProgress;
    public static event ObjectReturnEvent onGetInventoryItems;
    public static event ObjectReturnEvent onGetGoldOre;
    public static event SimpleEvent onClearInventory;
    public static event FloatReturnEvent onGetCurrentBatteryPercentage;
    public static event FloatReturnEvent onGetPlayerHeight;

    public static event IntegerReturnEvent onGetCurrentLamps;
    public static event IntegerReturnEvent onGetCurrentDynamites;
    public static event IntegerReturnEvent getInventoryCapactity;

    public static event TransformEvent onChangePlayerPosition;
    public static event TransformEvent onChangePlayerPositionDisableMovement;




    public static event BooleanReturnEvent isPlayerInCave;
    public static event BooleanEvent setPlayerInCave;

    public static void DoFireSetPlayerInCave(bool inCave)
    {
        setPlayerInCave?.Invoke(inCave);
    }

    public static bool DoFirePlayerInCave()
    {
        if (isPlayerInCave != null)
            return isPlayerInCave.Invoke();

        return false;
    }


    public static event SimpleEvent onTreasureChestOresCollected;
    public static event SimpleEvent onTreasureChestRewardClaimed;

    public static void DoFireTreasureChestOresCollected()
    {
        if (onTreasureChestOresCollected != null)
            onTreasureChestOresCollected.Invoke();
    }   
    
    
    public static void DoFireTreasureChestRewardClaimed()
    {
        if (onTreasureChestRewardClaimed != null)
            onTreasureChestRewardClaimed.Invoke();
    }

    public static event SimpleEvent onSpawnPlayerAtGarage;
    public static void DoFireSpawnPlayerAtGarage()
    {
        if (onSpawnPlayerAtGarage != null)
            onSpawnPlayerAtGarage.Invoke();
    }
    public static event StringEvent onLogAnalyticsEvent;
    public static void DoFireLogAnalyticsEvent(string value)
    {
        if (onLogAnalyticsEvent != null)
            onLogAnalyticsEvent.Invoke(value);

        Debug.Log($"AnalyticsEvent: <color=yellow>{value}</color> Logged");

        //Debug.Log("AnalyticsEvent: </color=yellow>" + value + "</color> Logged");
    }   
    
    public static event StringEvent onSetCurrentView;
    public static void DoFireSetCurrentView(string value)
    {
        if (onSetCurrentView != null)
            onSetCurrentView.Invoke(value);

    }


    public static event StringEvent onShowUINotification;


    public static void DoFireShowNotification(string text)
    {
        if (onShowUINotification != null)
            onShowUINotification.Invoke(text);
    }

    public static event TransformEvent onSetCanveEnvPos;
    public static event TransformEvent onSetCaveParticlesPos;

    public static void DoFireSetCaveEnvironmentPos(Transform caveEnvPos)
    {
        if (onSetCanveEnvPos != null)
            onSetCanveEnvPos.Invoke(caveEnvPos);
    }
    public static void DoFireSetCaveParticlesPos(Transform caveParticlesPos)
    {
        if (onSetCaveParticlesPos != null)
            onSetCaveParticlesPos.Invoke(caveParticlesPos);
    }

    public static event BooleanEvent onGoldHintEnabled;

    public static void DoFireGoldHintEnabled(bool value)
    {
        if (onGoldHintEnabled != null)
            onGoldHintEnabled.Invoke(value);
    }

    public static event SimpleEvent onSaveData;


    public static void DoFireSaveData()
    {
        if (onSaveData != null)
            onSaveData.Invoke();
    }



   


    #region Tutorial
    //tutorial
    public static event SimpleEvent onAllTasksUI_TaskComplete;
    public static event IntegerEvent OnSubStateComplete;


    public static void DoFireSubStateComplete(int index)
    {
        OnSubStateComplete?.Invoke(index);
    }

    public static void DoFireAllTasksUI_TaskComplete()
    {
        onAllTasksUI_TaskComplete?.Invoke();
    }
    #endregion

    #region Task_Events
    public static event SimpleEvent tutorialTaskComplete;
    public static event SimpleEvent onResetTaskUI_CompletedTasks;

    public static event DoubleStringEvent onAddSimpleTask;

    public delegate void AddCheckListTask(string headerText, TutorialTask checklistTask);
    public static event AddCheckListTask onAddCheckListTask;

    public delegate void OnTaskProgressUpdated(TaskType taskType, int increment);
    public static event OnTaskProgressUpdated OnTaskProgressUpdatedEvent;

    public static void DoFireAddSimpleTask(string str1, string str2)
    {
        onAddSimpleTask?.Invoke(str1, str2);
    }

    public static void DoFireAddCheckListTask(string headertext, TutorialTask task)
    {
        onAddCheckListTask?.Invoke(headertext, task);
    }

    public static void DoFireTaskUI_TaskComplete()
    {
        tutorialTaskComplete?.Invoke();
    }


    public static void DoFireTriggerTaskProgress(TaskType taskType, int increment)
    {
        OnTaskProgressUpdatedEvent?.Invoke(taskType, increment);
    }

    public static void DoFireResetTaskUI_CompletedTasksCounter()
    {
        onResetTaskUI_CompletedTasks?.Invoke();
    }

    #endregion


    public static void DoFireSetProgressText(string text)
    {
        if (onSetProgress != null)
            onSetProgress.Invoke(text);
    }

    public static void DoFireUpdatePlayerPosition(Transform newTransform)
    {
        if (onChangePlayerPosition != null)
            onChangePlayerPosition.Invoke(newTransform);
    }   
    public static void DoFireUpdatePlayerPositionDisableMovement(Transform newTransform)
    {
        if (onChangePlayerPositionDisableMovement != null)
            onChangePlayerPositionDisableMovement.Invoke(newTransform);
    }


    public static float DoFireGetCurrentBatteryPercentage()
    {
        if (onGetCurrentBatteryPercentage != null)
            return onGetCurrentBatteryPercentage.Invoke();

        return 0;
    }

    public static float DoFireGetPlayerHeight()
    {
        if (onGetPlayerHeight != null)
            return onGetPlayerHeight.Invoke();

        return 0;
    }

    public static int DoFireGetCurrentLamps()
    {
        if (onGetCurrentLamps != null)
            return onGetCurrentLamps.Invoke();

        return 0;
    }
    public static int DoFireGetCurrentDynamites()
    {
        if (onGetCurrentDynamites != null)
            return onGetCurrentDynamites.Invoke();

        return 0;
    }

    public static bool DoFireGetIfBatteryEmpty()
    {
        if (onGetIfBatteryEmpty != null)
            return onGetIfBatteryEmpty.Invoke();

        return false;
    }
    public static object DoFireGetInventoryItems()
    {
        if (onGetInventoryItems != null)
            return onGetInventoryItems.Invoke();

        return false;
    }  
    
    public static object DoFireGetGoldOre()
    {
        if (onGetGoldOre != null)
            return onGetGoldOre.Invoke();

        return false;
    }

    public static int DoFireGetOrePriceByType(OreType type)
    {
        if (onGetItemValueByType != null)
            return onGetItemValueByType.Invoke(type);

        return -1;
    }

    public static int DoFireGetInventoryCapactiy()
    {
        if (getInventoryCapactity != null)
            return getInventoryCapactity.Invoke();

        return -1;
    }

    public static void DoFireSpawnOreAtPosition(Vector3 pos)
    {
        SpawnOreAtPosition?.Invoke(pos);
    }

    public static void DoFireOreCollected(object obj)
    {
        oreCollectedEvent?.Invoke(obj);
    }
    public static void DoFireOreCollectedBool(object obj, bool value)
    {
        oreCollectedEventBool?.Invoke(obj, value);
    }
    public static void DoFireAddOreToInventory(object obj)
    {
        addOreToInventory?.Invoke(obj);
    }
    public static void DoFireObjectHighlighted(object obj)
    {
        onObjectHighlightedEvent?.Invoke(obj);
    }

    public static void DoFireTouchpadClicked()
    {
        onTouchpadClickedEvent?.Invoke();
    }
    public static bool DoFireCheckIfInventoryFull()
    {
        if (isInventoryFull != null)
            return isInventoryFull.Invoke();

        return false;
    }
    public static void DoFireInteractButtonClicked()
    {
        interactButtonClicked?.Invoke();
    }

    public static void DoFireRefillBattery()
    {
        onRefillBattery?.Invoke();
    }
    public static void DoFireBuyLamp()
    {
        onBuyLamp?.Invoke();
    }

    public static void DoFireBuyDynamite()
    {
        onBuyDynamite?.Invoke();
    }

    public static void DoFireClearInventory()
    {
        onClearInventory?.Invoke();
    }

    public static void DoFireEnablePlayerMovement()
    {
        onEnablePlayerMovement?.Invoke();
    }
    
    public static void DoFirePurchaseShovelUpgrade()
    {
        onPurchaseShovelUpgrade?.Invoke();
    }

    public static void DoFireShovelButtonPressed(bool value)
    {
        onShovelButtonPressed?.Invoke(value);
    }
    public static void DoFireShowInteractButton(bool value)
    {
        onShowInteractButton?.Invoke(value);
    }   
    
    public static void DoFireEnableFPSPanel(bool value)
    {
        onEnableFPSPanel?.Invoke(value);
    }
    public static void DoFireDrainBattery(bool value)
    {
        onDrainBattery?.Invoke(value);
    }

    public static void DoFireStartDigging(object shovel)
    {
        onStartDiggingEvent.Invoke(shovel);
    }
    public static void DoFireSpawnShovelAtHitPointEvent(RaycastHit hit)
    {
        spawnShovelAtHitPointEvent?.Invoke(hit);
    }


    #region Ads_Events


    public static event SimpleEvent OnAppOpenLoaded;
    public static event SimpleEvent OnAppOpenFailedToLoadForAllIds;
    public static event SimpleEvent OnAppOpenClosed;
    public static event SimpleEvent OnAppOpenFailedToShow;


    public static void DoFireAppOpenLoaded()
    {
        OnAppOpenLoaded?.Invoke();
    }


    public static void DoFireAppOpenFailedToLoadForAllIDs()
    {
        OnAppOpenFailedToLoadForAllIds?.Invoke();
    }

    public static void DoFireOnAppOpenClosed()
    {
        OnAppOpenClosed?.Invoke();
    }

    public static void DoFireOnAppOpenFailedToShow()
    {
        OnAppOpenFailedToShow?.Invoke();
    }


    public static event SimpleEvent OnShowInterstial;
    public static event BooleanEvent OnShowBanner;
    public static event BooleanEvent OnHideNativeAds;
    public static event ActionEvent OnShowInterstialWithAction;


    public static event SimpleEvent OnHideBanner;
    public delegate void ShowRewarded(Action onComplete, Action onFailed);
    public static event ShowRewarded OnShowRewarded;

    public delegate bool ReturnBoolEvent();
    public static event ReturnBoolEvent OnIsRewardedAvailable;
    public static event ReturnBoolEvent OnIsRewardedInterstitialAvailable;
    public static event ReturnBoolEvent OnIsInterstitialAvailable;

    public static bool DoFireIsRewardedAvailable()
    {

        if (OnIsRewardedAvailable != null)
            return OnIsRewardedAvailable.Invoke();
        else
            return false;
    }

    public static bool DoFireIsInterstitialAvailable()
    {

        if (OnIsInterstitialAvailable != null)
            return OnIsInterstitialAvailable.Invoke();
        else
            return false;
    }

    public static void DoFireShowInterstial()
    {
        OnShowInterstial?.Invoke();
    }

    public static void DoFireShowBanner(bool show)
    {
        OnShowBanner?.Invoke(show);
    }

    public static void DoFIreShowInterstialWithAction(Action onClosed)
    {
        OnShowInterstialWithAction?.Invoke(onClosed);
    }

    public static void DoFireShowRewarded(Action onComplete, Action onFailed)
    {
        OnShowRewarded?.Invoke(onComplete, onFailed);
    }

    public static void DoFireHideBanner()
    {
        OnHideBanner?.Invoke();
    }

    public static void DoFireHideNativeAds(bool hide)
    {
        OnHideNativeAds?.Invoke(hide);
    }

    #endregion

    #region Loading_Settings_Events

    public static event SimpleEvent showSettings;
    public static event SimpleEvent onSettingsUpdated;
    public static event StringEvent loadScene;

    public static void DoFireShowSettings()
    {
        showSettings?.Invoke();
    }
    public static void DoFireSettingsUpdated()
    {
        onSettingsUpdated?.Invoke();
    }

    public static void DoFireLoadScene(string sceneName)
    {
        loadScene?.Invoke(sceneName);
    }

    #endregion

    #region Upgrade_Events
    public static event SimpleEvent upgradeShovelEvent;
    public static event SimpleEvent upgradeInventoryEvent;
    public static event SimpleEvent upgradeBatteryEvent;
    public static event SimpleEvent upgradeJetpackEvent;

    public static void DoFireUpgrade_Shovel()
    {
        upgradeShovelEvent?.Invoke();
    }

    public static void DoFireUpgrade_Inventory()
    {
        upgradeInventoryEvent?.Invoke();
    }

    public static void DoFireUpgrade_Battery()
    {
        upgradeBatteryEvent?.Invoke();
    }

    public static void DoFireUpgrade_Jetpack()
    {
        upgradeJetpackEvent?.Invoke();
    }


    #endregion
}
