using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI textTotalThrow;

    public static UIManager Instance 
    {
        get 
        {
            if (instance == null) 
            {
                instance = FindObjectOfType<UIManager>(); 
            }

            return instance;
        }
    }

    private void Awake()
    {
        instance= this;
    }

    [SerializeField] TextMeshProUGUI cointText;

    public void InitTextThrow(int Throw) 
    {
        textTotalThrow.text = Throw.ToString();
    }
    public void SetCoint(int coint) 
    {
        cointText.text = coint.ToString();
    }
}
