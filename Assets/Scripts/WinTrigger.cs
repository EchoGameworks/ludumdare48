using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public float WinTimer;
    public float WinTimerMax = 1.5f;

    public bool HasClearedLevel;
    public bool HasSentClearCommand;
    void Start()
    {
        WinTimer = WinTimerMax;
        HasClearedLevel = false;
        HasSentClearCommand = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioManager.instance.PlaySound(AudioManager.SoundEffects.EnterWater);
        }       
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !HasClearedLevel)
        {
            WinTimer -= Time.deltaTime;
            if(WinTimer <= 0f)
            {
                HasClearedLevel = true;
                if (!HasSentClearCommand)
                {
                    AudioManager.instance.PlaySound(AudioManager.SoundEffects.CompleteLevel);
                    GameManager.instance.GoToNextGameState();
                }

            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            WinTimer = WinTimerMax;
        }        
    }
}
