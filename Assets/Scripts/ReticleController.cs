using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class ReticleController : MonoBehaviour
{
    public Transform Eyeball;
    public GameObject ReticleNormal;
    public GameObject ReticleMax;

    public bool IsResetting = false;
    public float ScaleTransitionTime = 0.3f;
    public float RotateSpeed = 1f;

    void Awake()
    {
        GameManager.instance.onUnloadTitle += UnloadTitle;
        ReticleNormal.transform.localScale = Vector3.zero;
        ReticleMax.transform.localScale = Vector3.zero;
    }

    private void UnloadTitle()
    {
        DisableControl();
    }

    void Update()
    {
        this.transform.position = Eyeball.position;
    }

    public void LoadLevel()
    {
        DisableControl();
    }

    public void ResetLevel()
    {
        //print("resetting level at reticle");
        IsResetting = true;
        DisableControl();
        LeanTween.delayedCall(1f, () => IsResetting = false).setOnComplete(EnableControl);
    }

    [Button]
    public void EnableControl()
    {
        //print("trying to enable control");
        if (GameManager.instance.IsDialog || IsResetting) return;
        //print("enabling control");
        ReticleNormal.SetActive(true);
        //ReticleNormal.transform.rotation = Quaternion.identity;
        LeanTween.scale(ReticleNormal, new Vector3(1f, 1f, 1f), ScaleTransitionTime).setEaseOutBack();
    }

    [Button]
    public void DisableControl()
    {
        LeanTween.scale(ReticleNormal, Vector3.zero, ScaleTransitionTime).setEaseOutCirc();
        LeanTween.scale(ReticleMax, Vector3.zero, ScaleTransitionTime).setEaseOutCirc();
    }

    [Button]
    public void EnableDragNormalFromIdle()
    {
        LeanTween.scale(ReticleNormal, new Vector3(1.2f, 1.22f, 1.2f), ScaleTransitionTime).setLoopPingPong(1);
    }

    [Button]
    public void EnableDragNormalFromMax()
    {
        ReticleNormal.gameObject.SetActive(true);
        LeanTween.scale(ReticleMax, Vector3.zero, ScaleTransitionTime).setEaseOutCirc();
    }

    [Button]
    public void EnableDragMax()
    {
        LeanTween.scale(ReticleMax, new Vector3(1f, 1f, 1f), ScaleTransitionTime).setEaseOutCirc()
            .setOnComplete(() => ReticleNormal.gameObject.SetActive(false));
    }
}
