using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    float timer;
    float randomTime;
    public void OnEnter(Enemy enemy)
    {
        enemy.eStopMoving();
        timer = 0;
        randomTime = Random.Range(2f, 4f);
    }

    public void OnExecute(Enemy enemy)
    {
        if (timer > randomTime)
        {
            enemy.ChangeState(new PatrolState());
        }
        timer += Time.deltaTime;

    }

    public void OnExit(Enemy enemy)
    {
    }
}
