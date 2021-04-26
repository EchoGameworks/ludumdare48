using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState { Title, Tutorial, Level1, Level2, Level3, Level4, Level5, Level6, Level7, Level8, Level9, Level10, Level11, Level12, Level13, Level14, Level15 }
    public bool IsDialog;

    public bool SimulateFullGame;
    public GameState CurrentGameState;
    public bool IsDialogReady;
    public List<string> currentDialog;

    public ReticleController ReticleController;

    public GameObject SpeechCanvas;
    public SpriteRenderer SpeechBubble;
    public TextMeshProUGUI SpeechText;
    public RectTransform ContinueText;

    public bool SkipTransitionIn;

    public event Action onPrepLevel;
    public void TriggerPrepLevel()
    {
        if (onPrepLevel != null)
        {
            onPrepLevel();
        }
    }


    public event Action onLoadTitle;
    public void TriggerLoadTitle()
    {
        if(onLoadTitle != null)
        {
            onLoadTitle();
        }
    }

    public event Action onUnloadTitle;
    public void TriggerUnloadTitle()
    {
        if (onUnloadTitle != null)
        {
            onUnloadTitle();
        }
    }

    public event Action onStartDialog;
    public void TriggerStartDialog()
    {
        if (onStartDialog != null)
        {
            onStartDialog();
        }
    }

    public event Action<GameState> onLoadLevel;
    public void TriggerLoadLevel(GameState gs)
    {
        if (onLoadLevel != null)
        {
            onLoadLevel(gs);
        }
    }

    public event Action<GameState> onUnloadLevel;
    public void TriggerUnloadLevel(GameState gs)
    {
        if (onUnloadLevel != null)
        {
            onUnloadLevel(gs);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    void Start()
    {
        currentDialog = new List<string>();
        LeanTween.delayedCall(0.3f, StartGame);
    }

    void StartGame()
    {
        SpeechCanvas.SetActive(false);
        if (!Application.isEditor || SimulateFullGame)
        {
            CurrentGameState = GameState.Title;
            TriggerPrepLevel();
            //TriggerUnloadLevel(GameState.Level1);
            //TriggerUnloadLevel(GameState.Level2);
            //TriggerUnloadLevel(GameState.Level3);
            SetState(GameState.Title, true);
        }
        else
        {
            SetState(CurrentGameState, true);
        }
    }

    void SetState(GameState gs, bool skipIn = false)
    {
        SkipTransitionIn = skipIn;
        //print("skip GM");
        //Unload Current State
        if (!skipIn)
        {
            switch (CurrentGameState)
            {
                case GameState.Title:
                    TriggerUnloadTitle();
                    break;
                default:
                    TriggerUnloadLevel(CurrentGameState);
                    break;
            }
        }

        CurrentGameState = gs;

        switch (CurrentGameState)
        {
            case GameState.Title:
                CameraController.instance.PaletteToLight();
                TriggerLoadTitle();
                IsDialog = true;
                break;
            case GameState.Tutorial:                
                TriggerLoadLevel(CurrentGameState);
                currentDialog.Add("I've finally escaped that disgusting eye socket of great warlock Skozrak!");
                currentDialog.Add("I'm not safe here, though. I need to move deeper into his lair...");
                currentDialog.Add("I bet I can squeeze through the hole in that drain over there.");
                currentDialog.Add("I need to fling myself over to it by clicking on myself, dragging away, then releasing");
                NextDialog();
                break;
            case GameState.Level1:
                TriggerLoadLevel(CurrentGameState);
                currentDialog.Add("That seemed to do it! It looks like I can see when I'm at maximum flinging power when the full circle surrounds me.");
                break;
            case GameState.Level5:
                TriggerLoadLevel(CurrentGameState);
                currentDialog.Add("It's getting darker as I get further down. No way Skozrak finds me now!");
                break;
            case GameState.Level7:
                TriggerLoadLevel(CurrentGameState);
                currentDialog.Add("This should be far enough! I'm safe here.");
                currentDialog.Add("Thanks for playing!");
                break;
            default:
                TriggerLoadLevel(CurrentGameState);
                break;
        }
        //Load New State
    }

    public void DialogReady()
    {      
        var seq = LeanTween.sequence();
        seq.append(() => SpeechBubble.size = Vector2.zero);
        seq.append(() => ContinueText.localScale = Vector3.zero);
        seq.append(() => SpeechText.text = "");
        seq.append(() => SpeechCanvas.SetActive(true));        
        seq.append(() => GenerateSpeech());
    }

    private void GenerateSpeech()
    {
        if(currentDialog.Count > 0)
        {
            IsDialog = true;
            var seq = LeanTween.sequence();
            seq.append(() => LeanTween.value(SpeechBubble.gameObject, (v) => SpeechBubble.size = v, SpeechBubble.size, new Vector2(10f, 2.5f), 0.3f));
            seq.append(0.3f);
            seq.append(() => SpeechText.text = currentDialog[0]);
            seq.append(() => currentDialog.RemoveAt(0)); //type in??
            seq.append(() => ContinueText.localScale = Vector3.one);
            seq.append(() => IsDialogReady = true);
        }
        else
        {
            IsDialog = false;            
            SpeechCanvas.SetActive(false);
            ReticleController.EnableControl();
        }
    }

    public void NextDialog()
    {
        if (IsDialogReady)
        {
            GenerateSpeech();
        }
    }

    public void GoToNextGameState()
    {
        int gsInt = (int)CurrentGameState + 1;
        SetState((GameState)gsInt);
    }

}
