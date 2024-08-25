using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public Transform mainCamera;
    public Transform midBg;
    public Transform sildeBG;
    public float lenght;

    // Update is called once per frame
    void Update()
    {
        if (mainCamera.position.x > midBg.position.x) 
        {
            UpdateBacground(Vector3.right);
        }else if (mainCamera.position.x < midBg.position.x)
        {
            UpdateBacground(Vector3.left);
        }
    }


    public void UpdateBacground(Vector3 direction) 
    {
        sildeBG.position = midBg.position + direction * lenght;
        Transform temp = midBg;
        midBg = sildeBG;
        sildeBG = temp;
    }
}
