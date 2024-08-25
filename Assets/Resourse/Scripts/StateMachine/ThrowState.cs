using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowState : IState
{
    float timer;
    private bool hasThrown = false;
    public void OnEnter(Enemy enemy)
    {
        hasThrown = false;
        if (enemy.Target != null && !hasThrown)
        {
            enemy.Throw();
            hasThrown= true;
        }
        
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer >= 0.5f) 
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Enemy enemy)
    {
        timer = 0;
    }
}
