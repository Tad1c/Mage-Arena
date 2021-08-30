using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Vignette effect")]
    public Image vignetteImg;
    public float minimumVignetteOpacity = 0.6f;
    public BoolVariable lookingForMatch;

    [Header("Welcome")]
    public TextMeshProUGUI welcomeLabel;

    private void Start()
    {
        welcomeLabel.text = $"Welcome, {CurrentPlayerData.playerDisplayName}";
        lookingForMatch.AddListener(LookingForMatchChanged);
    }

    void LookingForMatchChanged()
    {
        vignetteImg.enabled = lookingForMatch.Value;
    }

    void FixedUpdate()
    {
        if (lookingForMatch.Value)
        {
            var existingColor = vignetteImg.color;
            vignetteImg.color = new Color(existingColor.r, existingColor.g, existingColor.b, Mathf.PingPong(Time.time * 0.5f, 1f - minimumVignetteOpacity) + minimumVignetteOpacity);
        }
    }
}
