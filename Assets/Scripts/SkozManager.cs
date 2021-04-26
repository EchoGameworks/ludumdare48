using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkozManager : MonoBehaviour
{
    public SpriteRenderer skozSprite;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.onLoadTitle += LoadTitle;
        GameManager.instance.onUnloadTitle += UnloadTitle;
    }

    private void OnDisable()
    {
        GameManager.instance.onLoadTitle -= LoadTitle;
        GameManager.instance.onUnloadTitle -= UnloadTitle;
    }

    private void LoadTitle()
    {
        skozSprite.color = new Color(skozSprite.color.r, skozSprite.color.g, skozSprite.color.b, 0f);
        gameObject.SetActive(true);
        LeanTween.color(gameObject, new Color(skozSprite.color.r, skozSprite.color.g, skozSprite.color.b, 1f), 2f);
    }

    private void UnloadTitle()
    {
        LeanTween.delayedCall(2f, () => 
            LeanTween.color(gameObject, new Color(skozSprite.color.r, skozSprite.color.g, skozSprite.color.b, 0f), 2f)
            .setOnComplete(() => gameObject.SetActive(false)));
    }


}
