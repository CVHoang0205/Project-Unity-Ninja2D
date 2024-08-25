using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink_Dead : GameController
{

    private void Start()
    {
        transform.position = a.position;
        target = b.position;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatForm();
    }
}
