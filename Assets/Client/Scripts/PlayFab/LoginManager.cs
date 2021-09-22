using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [Header("Login")]
    public InputField loginUsername;
    public InputField loginPassword;

    [Header("Registration")]
    public InputField registerUsername;
    public InputField registerEmail;
    public InputField registerPassword;

    public void Login()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = loginUsername.text,
            Password = loginPassword.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetUserInventory = true, GetPlayerStatistics = true, GetPlayerProfile = true }
        },
        result =>
        {
            CurrentPlayerData.playerDisplayName = result.InfoResultPayload.PlayerProfile.DisplayName;
            CurrentPlayerData.playerPlayFabID = result.PlayFabId;
            CurrentPlayerData.playerStatistics = new List<StatisticValue>(result.InfoResultPayload.PlayerStatistics);
            CurrentPlayerData.playerInventory = new List<ItemInstance>(result.InfoResultPayload.UserInventory);
            CurrentPlayerData.entityID = result.EntityToken.Entity.Id;
            CurrentPlayerData.entityType = result.EntityToken.Entity.Type;
            SceneManager.LoadScene("Main Menu");
            //load next scene
        },
        error =>
        {
            Debug.LogError(error.ErrorMessage);
            //error handling
        });
    }

    public void Register()
    {
        PlayFabClientAPI.RegisterPlayFabUser(
            new RegisterPlayFabUserRequest
            {
                Username = registerUsername.text,
                DisplayName = registerUsername.text,
                Password = registerPassword.text,
                Email = registerEmail.text
            },
            result =>
            {
                Debug.Log("Successfully registered! You may now log in.");
                registerUsername.text = "";
                registerPassword.text = "";
                registerEmail.text = "";
            },
            error =>
            {
                Debug.LogError(error.ErrorMessage);
            });
    }
}
