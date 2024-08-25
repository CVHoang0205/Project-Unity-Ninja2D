using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string currentAnimation = "idle";
    [SerializeField] protected HealthyBar healthBar;
    [SerializeField] protected CombatText combatTextPrefabs;
    public float hp;

    public bool IsDead => hp <= 0;

    private void Start()
    {
        OnInit();
    }
    //OnInit ham khoi tao
    public virtual void OnInit() 
    {
        hp = 100;
        healthBar.OnInit(hp, transform);
    }
    public virtual void OnDeath()
    {
        ChangeAnimation("dead");
        Invoke(nameof(OnDespam), 2f);
    }

    //onDespam ham huy
    public virtual void OnDespam() 
    {

    }

    protected void ChangeAnimation(string animName)
    {
        //Debug.Log("animator: " + currentAnimation + "To: " + animName);
        if (currentAnimation != animName)
        {
            animator.ResetTrigger(animName);
            currentAnimation = animName;
            animator.SetTrigger(currentAnimation);
        }
    }

    public void OnHit(float damage) 
    {
        Debug.Log("hit");
        if (!IsDead) 
        {
            hp -= damage;

            if (IsDead) 
            {
                hp = 0;
                OnDeath();
            }
            healthBar.SetNewHp(hp);
            Instantiate(combatTextPrefabs, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        }
    }
    
}
