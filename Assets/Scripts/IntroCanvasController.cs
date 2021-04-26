using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System;

public class IntroCanvasController : MonoBehaviour
{
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI InstructionsText;

    void Awake()
    {
        GameManager.instance.onLoadTitle += LoadTitle;
        GameManager.instance.onUnloadTitle += FadeIntro;
    }

    private void OnDisable()
    {
        GameManager.instance.onLoadTitle -= LoadTitle;
        GameManager.instance.onUnloadTitle -= FadeIntro;
    }

    private void LoadTitle()
    {
        TitleText.color = new Color(TitleText.color.r, TitleText.color.g, TitleText.color.b, 0f);
        InstructionsText.color = new Color(InstructionsText.color.r, InstructionsText.color.g, InstructionsText.color.b, 0f);
        gameObject.SetActive(true);
        LeanTween.value(TitleText.gameObject, (a) => TitleText.color = new Color(TitleText.color.r, TitleText.color.g, TitleText.color.b, a), TitleText.color.a, 1f, 1f);
        LeanTween.value(InstructionsText.gameObject, (a) => InstructionsText.color = new Color(InstructionsText.color.r, InstructionsText.color.g, InstructionsText.color.b, a), InstructionsText.color.a, 1f, 1f);
        
    }

    [Button]
    void FadeIntro()
    {
        LeanTween.value(TitleText.gameObject, (a) => TitleText.color = new Color(TitleText.color.r, TitleText.color.g , TitleText.color.b, a), TitleText.color.a, 0f, 1f);
        LeanTween.value(InstructionsText.gameObject, (a) => InstructionsText.color = new Color(InstructionsText.color.r, InstructionsText.color.g, InstructionsText.color.b, a), InstructionsText.color.a, 0f, 1f);
        LeanTween.delayedCall(1.2f, () => gameObject.SetActive(false));
    }

    void FadeColor()
    {

    }

}
