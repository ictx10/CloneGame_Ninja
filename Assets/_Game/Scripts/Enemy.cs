using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float eAttackRange;
    [SerializeField] private float eMoveSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject attackArea;

    private IState currentState;

    bool isRight = true;
    private Character target;
    public Character Target => target;

    private void Update()
    {
        if (currentState != null && !isDead)
        {
            currentState.OnExecute(this);
        }
    }
    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
        DeActiveAttack();
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }
    protected override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
    }

    /// <summary>
    /// Change State of Animation
    /// 
    /// Khi doi sang state moi thi se check xem state cu co bang null hay khong.
    /// Neu no bang null, thoat state cu va gan state moi bang currentState
    /// va se check xem currentState khac bang null thi bat dau truy cap vao state moi
    /// </summary>
    /// <param name="newState"></param>
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
    internal void SetTarget(Character character)
    {
        this.target = character;
        if (IsTargetInRange())
        {
            ChangeState(new AttackState());
        }
        else if (Target != null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new IdleState());
        }
    }

    public void eMoving()
    {
        ChangeAnim("Run");
        rb.velocity = transform.right * eMoveSpeed;
    }
    public void eStopMoving()
    {
        ChangeAnim("Idle");
        rb.velocity = Vector2.zero;
    }
    public void eAttack()
    {
        ChangeAnim("Attack");
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }

    public bool IsTargetInRange()
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= eAttackRange)
        {
            return true;
        }else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyWall")
        {
            ChangeDirrection(!isRight);
        }
    }

    public void ChangeDirrection(bool isRight)
    {
        this.isRight = isRight;

        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }

    void ActiveAttack()
    {
        attackArea.SetActive(true);
    }
    void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }

}
