using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.MultiplayerModels;
using System.Timers;

public static class Matchmaker
{
    static string ticketId;
    static string queueName;


    public static string ip;
    public static int tcp_port = 30000;
    public static int udp_port = 30001;

    public static void StartMatchmaking(string queue, System.Action<string> fin, System.Action err)
    {
        queueName = queue;
        Debug.Log("[MATCHMAKER] Started Matching");
        PlayFabMultiplayerAPI.CreateMatchmakingTicket(new CreateMatchmakingTicketRequest
        {
            QueueName = queue,
            GiveUpAfterSeconds = 60,
            MembersToMatchWith = null,
            Creator = new MatchmakingPlayer
            {
                Entity = new PlayFab.MultiplayerModels.EntityKey
                {
                    Id = CurrentPlayerData.entityID,
                    Type = CurrentPlayerData.entityType
                },
                Attributes = new MatchmakingPlayerAttributes
                {
                    DataObject = new PlayerAttributeDataObject
                    { latencies = new Latency[] { new Latency { region = "NorthEurope", latency = 100 } } }
                }
            }
        },
        result =>
        {
            Debug.Log("[MATCHMAKER] Ticket Created");
            ticketId = result.TicketId;
            fin?.Invoke(ticketId);
        },
        error =>
        {
            Debug.LogError("[ Matchmaker ] ERROR: " + error.ErrorMessage);
            err?.Invoke();
        });
    }

}

public class PlayerAttributeDataObject
{
    public Latency[] latencies;
}

public class Latency
{
    public string region;
    public int latency;
}