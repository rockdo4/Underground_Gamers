using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    abstract public void Enter();
    abstract public void Exit();
    abstract public void Update();
}