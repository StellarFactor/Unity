using System;
using System.Collections;
using UnityEngine;

public class WaitThenDo
{
    protected readonly MonoBehaviour runner;
    protected readonly Func<bool> finishCondition;
    protected readonly Func<bool> cancelCondition;
    protected readonly Action action;
    protected readonly Action ifCancelled;

    protected Coroutine process;

    public bool IsStarted { get; private set; }
    public bool IsFinished { get; private set; }
    public bool BeenCanceled { get; protected set; }

    public WaitThenDo(
        MonoBehaviour runner,
        Func<bool> finishCondition,
        Func<bool> cancelCondition,
        Action action,
        Action ifCancelled)
    {
        this.runner = runner;
        this.finishCondition = finishCondition;
        this.cancelCondition = cancelCondition;
        this.action = action;
        this.ifCancelled = ifCancelled;
    }

    public void Start()
    {
        if (IsStarted) { return; }

        IsStarted = true;
        IsFinished = false;
        BeenCanceled = false;

        process = runner.StartCoroutine(Routine());
    }

    public void Cancel()
    {
        IsStarted = true;
        IsFinished = true;
        BeenCanceled = true;

        if (process != null)
        {
            runner.StopCoroutine(process);
        }

        process = null;
    }

    private IEnumerator Routine()
    {
        while (!finishCondition())
        {
            if (cancelCondition())
            {
                Cancel();
            }

            if (BeenCanceled)
            {
                ifCancelled?.Invoke();
                yield break;
            }

            Debug.LogWarning($"{this} not done. condition: {finishCondition()}");
            yield return null;
        }

        action.Invoke();
        yield return null;
        IsFinished = true;
    }
}
