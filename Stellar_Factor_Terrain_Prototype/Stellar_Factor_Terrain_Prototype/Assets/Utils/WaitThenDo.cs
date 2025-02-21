using System;
using System.Collections;
using UnityEngine;

public class WaitThenDo
{
    private readonly MonoBehaviour runner;
    private readonly Func<bool> condition;
    private readonly Action action;

    private Coroutine process;

    public bool IsStarted { get; private set; }
    public bool IsFinished { get; private set; }

    public WaitThenDo(MonoBehaviour runner, Func<bool> condition, Action action)
    {
        this.runner = runner;
        this.condition = condition;
        this.action = action;
    }

    public void Start()
    {
        IsStarted = true;
        IsFinished = false;

        process = runner.StartCoroutine(Routine());
    }

    public void Cancel()
    {
        IsStarted = true;
        IsFinished = false;

        if (process != null)
        {
            runner.StopCoroutine(process);
        }

        process = null;
    }

    private IEnumerator Routine()
    {
        while (!condition())
        {
            Debug.LogWarning($"{this} not done. condition: {condition()}");
            yield return null;
        }

        action.Invoke();
        yield return null;
        IsFinished = true;
    }
}
