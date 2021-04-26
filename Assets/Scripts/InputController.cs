using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public LayerMask PlayerLayer;

    GameObject eyeGO;
    EyeController eyeController;
    ReticleController reticleController;
    Camera cam;

    public bool IsEyeActive;
    public Vector2 clickStartPos;
    public Vector2 clickCurrentPos;
    public float dragMag;
    public float dragMagClamped;
    public bool isDragClamped;
    public Vector2 launchDirection;

    void Start()
    {
        cam = Camera.main;
        eyeGO = GameObject.FindGameObjectWithTag("Player");
        eyeController = eyeGO.GetComponent<EyeController>();
        reticleController = GameObject.FindGameObjectWithTag("Reticle").GetComponent<ReticleController>();
        IsEyeActive = false;
    }

    void Update()
    {
        if(GameManager.instance.CurrentGameState == GameManager.GameState.Title)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //print("Input: at title clicking");
                GameManager.instance.GoToNextGameState();
            }
            return;
        }

        if (GameManager.instance.IsDialog)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //print("Input: at title clicking");
                AudioManager.instance.PlaySound(AudioManager.SoundEffects.ClickDialog);
                GameManager.instance.NextDialog();
            }
            return;
        }

        if (Input.GetButtonDown("Fire1") && eyeController.IsInteractable)
        {
            Vector3 worldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPosition.x, worldPosition.y), Vector2.zero,  100, PlayerLayer);
            if (hit)
            {
                IsEyeActive = true;
                clickStartPos = hit.point;
                AudioManager.instance.PlaySound(AudioManager.SoundEffects.ActiveClick);
            }
        }


        if (IsEyeActive)
        {
            clickCurrentPos = cam.ScreenToWorldPoint(Input.mousePosition);
            launchDirection = clickStartPos - clickCurrentPos;
            dragMag = 0.9f * Vector2.SqrMagnitude(launchDirection);
            dragMagClamped = Mathf.Clamp01(dragMag);
            if(dragMag > 1f)
            {
                if (!isDragClamped)
                {
                    //Debug.Log("Effect for max drag");
                    reticleController.EnableDragMax();
                    AudioManager.instance.PlaySound(AudioManager.SoundEffects.MaxPower);
                }
                isDragClamped = true;
                
            }
            else
            {
                if (isDragClamped)
                {
                    reticleController.EnableDragNormalFromMax();
                }
                isDragClamped = false;
            }

            if (Input.GetButtonUp("Fire1"))
            {
                eyeController.Launch(dragMagClamped, launchDirection.normalized);
                AudioManager.instance.PlaySound(AudioManager.SoundEffects.ActiveRelease);
                IsEyeActive = false;
                dragMag = 0;
            }
        }
    }
}
