using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] protected GameObject attackArea;
    [SerializeField] GameObject cointPrefabs;
    [SerializeField] GameObject Fire_Bullet;
    [SerializeField] GameObject FlagPrefabs;

    private IState currentState;

    private bool isRight = true;

    private Character target;
    public Character Target => target;

    public bool isBoss = false;

    private void Update()
    {
       if (currentState != null && !IsDead) 
        {
            currentState.OnExecute(this);
        } 
    }
    public override void OnInit()
    {
        base.OnInit();
        if (isBoss) 
        {
            hp = 200;
            healthBar.OnInit(hp, transform);
        }
        ChangeState(new IdleState());
        AnableActiveAttackArea();
    }

    public override void OnDespam()
    {
        base.OnDespam();
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
        Instantiate(cointPrefabs, transform.position, Quaternion.identity);
        if (isBoss) 
        {
            Vector2 flagPosition = transform.position + new Vector3(2f, 0f); // Adjust the offset as needed
            Instantiate(FlagPrefabs, flagPosition, Quaternion.identity);
        }
    }

    //dead
    public override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
    }

    public void ChangeState(IState newState) 
    {
        if (currentState != null) 
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null) 
        {
            currentState.OnEnter(this);
        }
    }

    public void Moving() 
    {
        ChangeAnimation("run");
        rb.velocity = transform.right * moveSpeed;
    }

    public void StopMoving() 
    {
        ChangeAnimation("idle");
        rb.velocity = Vector2.zero;
    }

    public void Attack() 
    {
        ChangeAnimation("attack");
        SetActiveAttackArea();
        Invoke(nameof(AnableActiveAttackArea), 0.5f);
    }

    public void Throw() 
    {
        Vector2 direction = target.transform.position - transform.position;
        ChangeAnimation("throw");
        StartCoroutine(DelayFireBullet(direction));
    }

     IEnumerator DelayFireBullet(Vector2 direction)
    {
        yield return new WaitForSeconds(0.4f); 

        GameObject bullet;
        if (direction.x > 0)
        {
            bullet = Instantiate(Fire_Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 400f);
            bullet.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else
        {
            bullet = Instantiate(Fire_Bullet, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 400f);
            bullet.transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
        }
        Destroy(bullet.gameObject, 5f);
    }

    public void ChangeDirection(bool isRight) 
    {
        this.isRight = isRight;

        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }

    internal void SetTarget(Character character) 
    {
        this.target = character;
        if (IsTargetInRange()) 
        {
            ChangeState(new AttackState());
        }
        else if (target != null) 
        {
            if (isBoss) 
            {
                ChangeState(new ThrowState());
            } 
            else 
            {
                ChangeState(new PatrolState());
            }
        }else 
        {
            ChangeState(new IdleState());
        }
    }

    public bool IsTargetInRange() 
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange) 
        {
            return true;
        }
        else 
        {
            return false;
        }
        
    }

    public void SetActiveAttackArea()
    {
        attackArea.SetActive(true);
    }

    public void AnableActiveAttackArea()
    {
        attackArea.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("changeMoveEnemy")) 
        {
            ChangeDirection(isRight is false);
        }
    }
}
