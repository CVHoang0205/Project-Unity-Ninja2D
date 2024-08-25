using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;


public class MovingPlayer : Character
{
    [SerializeField] Rigidbody2D rb; 
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] Kunai kunaiPrefabs;
    [SerializeField] Transform pointKunai;
    [SerializeField] GameObject attackArea;


    public bool isGrounded = true; 
    public bool isJumping = false; 
    public bool isAttack = false;
    public bool isThrow = false;

    private float horizontal;
    public int coint = 0;
    private Vector3 savePoint;
    public float canThrow;
    private int totalThrow = 3;
    public GameObject hitVFX;
    private Audio audioManager;

    private void Awake()
    {
        coint = PlayerPrefs.GetInt("coint", 0);
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (IsDead) 
        {
            return;
        }

        if (isGrounded) 
        {

            // state is jumping of Player
            Jumping();


            // state is attack of Player
            Attack();


            // state is throw of Player
            Throw();

        }
        // state is jump Out
        checkFalling();
    }
    void FixedUpdate()
    {
        isGrounded = CheckGrounded();
        

        if (isAttack || isThrow) 
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isGrounded) 
        {
            if (IsDead) { return; }
            if (isJumping) { return; }
            if (isAttack) { return; }
            if (isThrow) { return; }

            // Change Animation of Player
            if (Mathf.Abs(horizontal) > 0.1f) 
            {
                ChangeAnimation("run");
            }else if (isGrounded && !isAttack && !isThrow) //idle
            {
                ChangeAnimation("idle");
                rb.velocity = Vector2.zero;
            }

        }

        // state is move of Player
        Moving();

        //Move of Mobile
        //if (Mathf.Abs(horizontal) > 0.1f)
        //{
        //    rb.velocity = new Vector2(horizontal * Time.deltaTime * 350f, rb.velocity.y);
        //    transform.localScale = new Vector3(horizontal, 1, 1);
        //}
    }


    public override void OnInit()
    {
        base.OnInit();
        isAttack = false;

        transform.position = savePoint;
        ChangeAnimation("idle");
        AnableActiveAttackArea();

        SavePoint();
        UIManager.instance.SetCoint(coint);
    }

    public override void OnDespam()
    {
        base.OnDespam();
        OnInit();
    }

    public override void OnDeath()
    {
        base.OnDeath();
    }

    public void SavePoint() 
    {
        savePoint = transform.position;
    }

    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);

        return hit.collider != null;
    }

    public void Moving()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * Time.deltaTime * 350f, rb.velocity.y);
            transform.localScale = new Vector3(horizontal, 1, 1);
        }
    }

    public void SetMoveButtion(float hozirontal) 
    {
        this.horizontal = hozirontal;
        Debug.Log(horizontal);
    }

    public void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            ChangeAnimation("jump");
            rb.AddForce(400f * Vector2.up);
        }
    }

    public void JumpMobile() 
    {
        isJumping = true;
        ChangeAnimation("jump");
        rb.AddForce(400f * Vector2.up);
    }

    public void checkFalling()
    {
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnimation("fall");
            isJumping = false;
        }
    }
    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.C) && isGrounded)
        {
            ChangeAnimation("attack");
            isAttack = true;
            Invoke(nameof(ResetTriggerAnim), 0.5f);
            SetActiveAttackArea();
            Invoke(nameof(AnableActiveAttackArea), 0.5f);
        }
    }
    public void AttackMobile() 
    {
        ChangeAnimation("attack");
        isAttack = true;
        Invoke(nameof(ResetTriggerAnim), 0.5f);
        SetActiveAttackArea();
        Invoke(nameof(AnableActiveAttackArea), 0.5f);
    }
    public void Throw()
    {
        if (Input.GetKeyDown(KeyCode.V) && isThrow is false)
        {
            if (totalThrow > 0) 
            {
                totalThrow--;
                UIManager.instance.InitTextThrow(totalThrow);
                if (totalThrow == 0) 
                {
                    Invoke(nameof(ResetThrow), 5f);
                }
                ChangeAnimation("throw");
                Invoke(nameof(SpawnKunai), 0.4f);
                isThrow = true;
                StartCoroutine(CanThrow(0.8f));
            }
        }
    }

    public void ResetThrow()
    {
        totalThrow = 3;
        UIManager.instance.InitTextThrow(totalThrow);
    }

    public void ThrowMobile() 
    {
        if(isThrow is false) 
        {
            if (totalThrow > 0)
            {
                totalThrow--;
                UIManager.instance.InitTextThrow(totalThrow);
                if (totalThrow == 0)
                {
                    Invoke(nameof(ResetThrow), 5f);
                }
                ChangeAnimation("throw");
                Invoke(nameof(SpawnKunai), 0.4f);
                isThrow = true;
                StartCoroutine(CanThrow(0.8f));
            }
        }
    }

    public void SpawnKunai() 
    {
        Kunai kunaiInstance = Instantiate(kunaiPrefabs, pointKunai.position, pointKunai.rotation);
        Vector3 kunaiScale = kunaiInstance.transform.localScale;
        kunaiScale.x = transform.localScale.x > 0 ? Mathf.Abs(kunaiScale.x) : -Mathf.Abs(kunaiScale.x);
        kunaiInstance.transform.localScale = kunaiScale;
    }

    IEnumerator CanThrow(float canthrow) 
    {
        this.canThrow = canthrow;
        yield return new WaitForSeconds(canthrow);

        ResetTriggerAnim();
    }

    public void SetActiveOfPlayer() 
    {
        gameObject.SetActive(false);
    }

    public void SetActiveAttackArea() 
    {
        attackArea.SetActive(true);
    }

    public void AnableActiveAttackArea() 
    {
        attackArea.SetActive(false);
    }
  
    public void ResetTriggerAnim()
    {
            ChangeAnimation("idle");
            isAttack = false;
            isThrow = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (isDead is true) { return;} // nếu đã chết không xử lý bất kỳ va chạm nào

        if (collision.CompareTag("coint"))
        {  
            coint++;
            PlayerPrefs.SetInt("coint", coint);
            UIManager.instance.SetCoint(coint);
            audioManager.PlaySFX(audioManager.cointClip);
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("Flag")) 
        {
            audioManager.PlaySFX(audioManager.WinClip);
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("dead")) 
        {
            OnHit(100);
        }

        if (collision.CompareTag("Fire")) 
        {
            OnHit(20);
            Instantiate(hitVFX, transform.position, transform.rotation);
            Destroy(collision.gameObject);
        }
    }





}

