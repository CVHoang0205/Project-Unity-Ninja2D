using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform a, b;
    public Vector3 target;
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void MovePlatForm()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, a.position) < 0.1f)
        {
            target = b.position;
        }
        else if (Vector2.Distance(transform.position, b.position) < 0.1f)
        {
            target = a.position;
        }
    }
}
