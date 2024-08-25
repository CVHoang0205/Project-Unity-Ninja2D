using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public GameObject hitVFX;
    [SerializeField] Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        Oninit();
    }

    public void Oninit() 
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f;

        rb.velocity = new Vector2(5f * direction, rb.velocity.y);

        Invoke(nameof(Ondespam), 4f);
    }

    public void Ondespam() 
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy")) 
        {
            collision.GetComponent<Character>().OnHit(15f);
            Instantiate(hitVFX, transform.position, transform.rotation);
            Ondespam();
        }
    }
}
