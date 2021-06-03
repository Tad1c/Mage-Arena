using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public static class CurrentPlayerData
{
    public static string playerDisplayName;
    public static string playerPlayFabID;
    public static List<StatisticValue> playerStatistics;
    public static List<ItemInstance> playerInventory;
    public static string entityID;
    public static string entityType;
}
