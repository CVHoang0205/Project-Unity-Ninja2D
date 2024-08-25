using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5;
    [SerializeField] private float Jumpforce = 350;

    public bool isGrounded = true;
    public bool isJumping = false;
    public bool isAttack = false;

    private float horizontal;

    private string currentAnim = "idle";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = CheckGrounded();

        horizontal = Input.GetAxisRaw("Horizontal");

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return; 
        }

        if (isGrounded) 
        {

            if (isJumping)
            {
                return; //dang o tren 0 se khong duoc attack
            }

            //Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }

            //Change anim run
            if (horizontal != 0)
            {
                ChangeAnim("run");
            }

            //attack
            if (Input.GetKeyDown(KeyCode.C) && isGrounded) 
            {
                Attack();
            }

            //throw
            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
            {
                Throw();
            }
        }
        //check falling
        if (isGrounded is false && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }

        //Moving
        if (horizontal != 0) // cần lấy giá trị tuyệt đối Mathf.abs(horizontal > 0.1)
        {
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        //idle
        else if (isGrounded && isAttack is false)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }

    private bool CheckGrounded() 
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);

       RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);

       return hit.collider != null;
    }

    private void Attack() 
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void Throw() 
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void Jump() 
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(Jumpforce * Vector2.up);
    }

    private void ResetAttack() 
    {
        ChangeAnim("idle");
        isAttack = false;
    }

    private void ChangeAnim(string animName) 
    {
        if (currentAnim != animName) 
        {
            animator.ResetTrigger(animName);
            currentAnim = animName;
            animator.SetTrigger(currentAnim);
        }
    }
}
