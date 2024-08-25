using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public Transform a, b;
    public bool direction = true;
    Vector3 target;

    void Start()
    {
        //transform.position = a.position;
        //target = b.position;
    }
    void Update()
    {
        //MoveTwo();
        MoveOne();
    }

    public void MoveOne() 
    {
        if (direction)
        {
            transform.position = Vector3.MoveTowards(transform.position, b.position, 0.01f);
            if (transform.position == b.position)
            {
                direction = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, a.position, 0.01f);
            if (transform.position == a.position)
            {
                direction = true;
            }
        }
    }

    public void MoveTwo() 
    {
        transform.position = Vector3.MoveTowards(transform.position, target, 1f * Time.deltaTime);

        if (Vector2.Distance(transform.position, a.position) < 0.1f) 
        {
            Debug.Log("2");
            target = b.position;
        }else if (Vector2.Distance(transform.position, b.position) < 0.1f) 
        {
            Debug.Log("1");
            target = a.position;
        }
    }
}
