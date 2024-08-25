using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject hitVFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            collision.GetComponent<Character>().OnHit(10f);
            Instantiate(hitVFX, transform.position, transform.rotation);
        }
        else if (collision.CompareTag("enemy")) 
        {
            collision.GetComponent<Character>().OnHit(12f);
            Instantiate(hitVFX, transform.position, transform.rotation);
        }
    }
}
