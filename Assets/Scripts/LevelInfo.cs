using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using Sirenix.OdinInspector;

public class LevelInfo : MonoBehaviour
{
    public float delayTransition;
    public float delayForDialog;
    public GameState myGameState;
    public GameObject StartingPosition;
    public GameObject RespawnPosition;
    private GameObject eyeball;
    private EyeController eyeballController;
    public ColorPalette MyColorPalette;

    private void Awake()
    {
        gameObject.transform.position = new Vector3(0f, -20f, 0f);
        eyeball = GameObject.FindGameObjectWithTag("Player");
        eyeballController = eyeball.GetComponent<EyeController>();
        GameManager.instance.onPrepLevel += PrepLevel;
        GameManager.instance.onLoadLevel += LoadLevel;
        GameManager.instance.onUnloadLevel += UnloadLevel;        
    }

    private void OnDisable()
    {
        GameManager.instance.onPrepLevel -= PrepLevel;
        GameManager.instance.onLoadLevel -= LoadLevel;
        GameManager.instance.onUnloadLevel -= UnloadLevel;
    }

    public void LoadLevel(GameState gs)
    {
        if (gs != myGameState) return;
        //print("LI: GM Skip: " + GameManager.instance.SkipTransitionIn);
        var seq = LeanTween.sequence();
        if (!GameManager.instance.SkipTransitionIn)
        {
            seq.append(delayTransition);
        }
        
        seq.append(() => gameObject.transform.position = new Vector3(0f, -20f, 0f));
        seq.append(() => gameObject.SetActive(true));
        CameraController.instance.SwapPalette(MyColorPalette);
        if (GameManager.instance.SkipTransitionIn)
        {
            //print("here1");
            seq.append(() => gameObject.transform.position = Vector3.zero);
        }
        else
        {
            //print("here2");
            seq.append(LeanTween.move(gameObject, Vector3.zero, 3f).setEaseInOutCubic());
        }
        
        seq.append(() => eyeballController.LoadLevel(StartingPosition.transform.position));
        seq.append(delayForDialog);
        seq.append(GameManager.instance.DialogReady);
        
    }

    public void UnloadLevel(GameState gs)
    {
        if (gs != myGameState) return;
        var seq = LeanTween.sequence();
        seq.append(LeanTween.move(gameObject, new Vector3(0f, 20f, 0f), 3f).setEaseInOutCubic());
        seq.append(() => gameObject.SetActive(false));
    }

    public void PrepLevel()
    {
        gameObject.transform.position = new Vector3(0f, -20f, 0f);
    }

    public void RestartLevel()
    {
        eyeballController.ResetLevel(RespawnPosition.transform.position);
    }
}
