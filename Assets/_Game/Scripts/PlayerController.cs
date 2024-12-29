using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 350;

    [SerializeField] private Kunai kunaiPrefabs;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;

    private bool isGrounded = true;
    private bool isJumping;
    private bool isAttack;
    private bool isDeath;

    private float horizontal;

    private int coin = 0;

    private Vector3 savePoint;

    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
    }
    void Update()
    {
        if (isDead)
        {
            return;
        }
        isGrounded = CheckGround();
        //horizontal = Input.GetAxisRaw("Horizontal");

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (isGrounded)
        {
            if (isJumping) { return; }

            //To Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Attack();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                Throw();
            }

            //Change animation to Run
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("Run");
            }
        }

        //Change animation to Fall
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("Fall");
            isJumping = false;
        }

        //To Run
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            //===>>>> ChangeAnim("Run");


            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            //horizontal > 0 => trục y trả về 0, nếu horizontal < 0 => trục y trả về 180
            transform.rotation = Quaternion.Euler(new Vector2(0, horizontal > 0 ? 0 : 180));
        }
        //To Idle
        else if (isGrounded)
        {
            ChangeAnim("Idle");
            rb.velocity = Vector2.zero;
        }
    }
    public override void OnInit()
    {
        base.OnInit();
        isAttack = false;
        
        transform.position = savePoint;
        ChangeAnim("Idle");
        DeActiveAttack();

        SavePoint();
        UIManager.instance.SetCoin(coin);
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
    }
    // Update is called once per frame
    /*    void Update()
        {
            if (isGrounded)
            {
                if (isJumping) { return; }

                //To Jump
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Debug.Log("Atk");
                    Attack();
                }
            }
        }*/
 
    private bool CheckGround()
    {

        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);

        /*if (hit != null)
        {
            return true;
        }
        else
        {
            return false;
        }*/

        return hit.collider != null;
    }
  
    public void Attack()
    {
        ChangeAnim("Attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }
    public void Throw()
    {
        ChangeAnim("Throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiPrefabs, throwPoint.position, throwPoint.rotation);
    }
    private void ResetAttack()
    {
        //BUG: Invoke call to this func, but can't change anim to Idle
        ChangeAnim("Idle");
        isAttack = false;
    }
    public void Jump()
    {
        isJumping = true;
        ChangeAnim("Jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    internal void SavePoint()
    {
        savePoint = transform.position;
    }
  
    void ActiveAttack()
    {
        attackArea.SetActive(true);
    }
    void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }
    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.instance.SetCoin(coin);
            Destroy(collision.gameObject);
        }
        if (collision.tag == "DeathZone")
        {
            isDeath = true;
            ChangeAnim("Die");

            Invoke(nameof(OnInit), 1f);
        }
    }
}
