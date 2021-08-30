using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab.MultiplayerModels;
using ScriptableObjectArchitecture;

public class MainMenuHandler : MonoBehaviour
{
    public BoolVariable looingForMatch;

    public void StartMatching()
    {
        looingForMatch.Value = true;

        Matchmaker.StartMatchmaking("DEVQ", (ticket) =>
        {
            StartCoroutine(TicketLoop(ticket));
        },
        () =>
        {
            // error
        });
    }

    IEnumerator TicketLoop(string ticketId)
    {
        Debug.Log("[MATCHMAKER] Running loop...");
        PlayFabMultiplayerAPI.GetMatchmakingTicket(
            new GetMatchmakingTicketRequest
            {
                TicketId = ticketId,
                QueueName = "DEVQ"
            },
            result =>
            {
                Debug.Log("Checking match ticket...");
                Debug.Log("[MATCHMAKER] Ticket Status: " + result.Status);
                if (result.Status == "Matched")
                {
                    looingForMatch.Value = false;

                    PlayFabMultiplayerAPI.GetMatch(
                        new GetMatchRequest
                        {
                            MatchId = result.MatchId,
                            QueueName = result.QueueName
                        },
                        res =>
                        {
                            Debug.Log("[MATCHMAKER] Match Found!");
                            Debug.Log("[MATCHMAKER] Connecting to " + res.ServerDetails.IPV4Address);
                            Matchmaker.ip = res.ServerDetails.IPV4Address;
                            foreach (var p in res.ServerDetails.Ports)
                            {
                                if (p.Name == "port_tcp")
                                    Matchmaker.tcp_port = p.Num;
                                else if (p.Name == "port_udp")
                                    Matchmaker.udp_port = p.Num;
                            }
                            Debug.Log("[MATCHMAKER] Ports: tcp " + Matchmaker.tcp_port + " udp = " + Matchmaker.udp_port);
                            StopAllCoroutines();
                            SceneManager.LoadScene("Client");
                        },
                        err =>
                        {
                            looingForMatch.Value = false;
                            Debug.LogError("[ Matchmaker ] ERROR: " + err.ErrorMessage);
                        });
                }
            },
            error =>
            {
                looingForMatch.Value = false;
                Debug.LogError("[ Matchmaker ] ERROR: " + error.ErrorMessage);
            });
        yield return new WaitForSeconds(6.2f);
        StartCoroutine(TicketLoop(ticketId));
    }
}
