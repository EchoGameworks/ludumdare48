using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackout : MonoBehaviour
{
    public Image bo;

    // Start is called before the first frame update
    void Start()
    {
        LeanTween.delayedCall(1f, () => LeanTween.scale(bo.gameObject, Vector3.zero, 1f).setOnComplete(() => gameObject.SetActive(false)));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
