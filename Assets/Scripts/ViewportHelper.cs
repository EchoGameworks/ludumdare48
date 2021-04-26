using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportHelper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = new Vector3(-100f, 0f, 0f);
            Rigidbody2D rb2D = collision.gameObject.GetComponent<Rigidbody2D>();
            rb2D.isKinematic = true;
            rb2D.velocity = Vector2.zero;

        }
    }
}
