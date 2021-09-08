using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellUI : MonoBehaviour
{
    public Spell spellData;

    public Image cooldownImage;

    [HideInInspector]
    public bool isInCooldown = false;

    public void StartCooldown() {
        if (isInCooldown) return;
        isInCooldown = true;
        StartCoroutine(AnimateSliderOverTime(spellData.cooldown));
    }

    IEnumerator AnimateSliderOverTime(float seconds)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            cooldownImage.fillAmount = Mathf.Lerp(100, 0, lerpValue) / 100f;
            yield return null;
        }
        isInCooldown = false;
    }
}
