using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour
{
    [SerializeField] Text hpText;
    public void OnInit(float damage) 
    {
        this.hpText.text = damage.ToString();
        Invoke(nameof(OnDespam), 1f);
    }

    public void OnDespam() 
    {
        Destroy(gameObject);
    }
}
