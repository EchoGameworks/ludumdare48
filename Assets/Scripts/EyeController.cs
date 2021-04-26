using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EyeController : MonoBehaviour
{
    public bool IsInteractable;
    public ReticleController ReticleController;
    public float dragPowerMax = 1f;
    Rigidbody2D rb2D;
    public bool IsMoving;
    [ShowInInspector]
    public float sqrMag;
    public float timerToStop;
    private float timerToStopMax = 0.5f;
    public float minCollisionForce = 1f;
    private float initX;

    private void Awake()
    {
        GameManager.instance.onLoadTitle += LoadTitle;
        GameManager.instance.onUnloadTitle += UnloadTitle;
        rb2D = GetComponent<Rigidbody2D>();
        timerToStop = timerToStopMax;
        initX = this.transform.localPosition.x;
    }

    void Start()
    {

    }

    private void OnDisable()
    {
        GameManager.instance.onLoadTitle -= LoadTitle;
        GameManager.instance.onUnloadTitle -= UnloadTitle;
    }

    private void LoadTitle()
    {
        rb2D.isKinematic = true;
    }

    private void UnloadTitle()
    {
        //Debug.Log("unloading on eye");
        var seq = LeanTween.sequence();
        seq.append(LeanTween.moveLocalX(gameObject, initX + 0.1f, 0.05f));
        seq.append(LeanTween.moveLocalX(gameObject, initX - 0.1f, 0.1f));
        seq.append(LeanTween.moveLocalX(gameObject, initX + 0.1f, 0.1f));
        seq.append(LeanTween.moveLocalX(gameObject, initX - 0.1f, 0.1f));
        seq.append(0.9f);
        seq.append(() => rb2D.isKinematic = false);
        seq.append(() => rb2D.AddForce(new Vector2(-2f, 2f), ForceMode2D.Impulse));
        seq.append(() => AudioManager.instance.PlaySound(AudioManager.SoundEffects.EyePopOut));
    }

    void Update()
    {
        sqrMag = rb2D.velocity.sqrMagnitude;
        if (timerToStop < 0f)
        {
            timerToStop = 0f;
            //rb2D.velocity = Vector2.zero;
        }
        else if (sqrMag < 0.1f && sqrMag >= 0f && timerToStop > 0f)
        {
            timerToStop -= Time.deltaTime;            
        }
        else if(sqrMag > 0.1f)
        {
            timerToStop = timerToStopMax;
        }

        if(timerToStop <= 0f)
        {
            if (IsMoving)
            {
                //Enable Interaction on stop
                IsInteractable = true;
                //Enable Reticle on stop
                ReticleController.EnableControl();
            }
            IsMoving = false;
        }
        else
        {
            if (!IsMoving)
            {
                //Disable Interaction on move
                IsInteractable = false;
                //Disable Recticle on move
                ReticleController.DisableControl();
            }
            IsMoving = true;
        }
    }

    public void Launch(float dragMag, Vector2 launchDirection)
    {
        Vector3 launchVector = dragPowerMax * dragMag * launchDirection;
        //Debug.Log("Launching:" + launchVector + " | " + launchVector.sqrMagnitude);
        rb2D.AddForce(launchVector, ForceMode2D.Impulse);

    }

    public void LoadLevel(Vector3 position)
    {
        //print("loading Level EC");
        //rb2D.velocity = Vector2.zero;
        
        var seq = LeanTween.sequence();
        seq.append(() => rb2D.isKinematic = true);
        seq.append(LeanTween.scale(gameObject, Vector3.zero, 0.3f).setEaseInOutCirc());
        seq.append(ReticleController.LoadLevel);
        seq.append(() => this.transform.position = position);
        seq.append(LeanTween.scale(gameObject, new Vector3(0.5f, 0.5f, 0.5f), 0.3f).setEaseInOutCirc());
        seq.append(() => rb2D.isKinematic = false);
    }

    public void ResetLevel(Vector3 position)
    {
        rb2D.velocity = Vector2.zero;
        var seq = LeanTween.sequence();
        seq.append(LeanTween.scale(gameObject, Vector3.zero, 0.3f).setEaseInOutCirc());
        seq.append(ReticleController.ResetLevel);
        seq.append(() => this.transform.position = position);
        seq.append(LeanTween.scale(gameObject, new Vector3(0.5f, 0.5f, 0.5f), 0.3f).setEaseInOutCirc());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float collisionForce = collision.contacts[0].normalImpulse + collision.contacts[0].tangentImpulse;
        if (collisionForce > minCollisionForce)
        {
            if(collisionForce > 10)
            {
                AudioManager.instance.PlaySound(AudioManager.SoundEffects.HeavyHit);
            }
            else
            {
                AudioManager.instance.PlaySound(AudioManager.SoundEffects.LightHit);
            }
            CameraController.instance.ShakeScreen(collision.contacts[0].normalImpulse);
        }
        
        //print("eye hit:" + collision + " | " + collision.contacts[0].normalImpulse);
    }

}
