using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class CountdownTimer
{
    protected readonly MonoBehaviour runner;
    protected readonly float duration;

    protected Coroutine process;

    protected float timeRemaining;

    public bool IsStarted { get; private set; }
    public bool IsFinished { get; private set; }
    public bool BeenCanceled { get; private set; }

    public CountdownTimer(MonoBehaviour runner, float duration)
    {
        this.runner = runner;
        this.duration = duration;
        timeRemaining = duration;
    }

    public void Start()
    {
        if (IsStarted) { return; }

        IsStarted = true;
        IsFinished = false;
        BeenCanceled = false;

        timeRemaining = duration;

        process = runner.StartCoroutine(Routine());
    }

    public void Cancel()
    {
        IsStarted = true;
        IsFinished = false;
        BeenCanceled = true;

        if (process != null)
        {
            runner.StopCoroutine(process);
        }

        process = null;
    }

    protected virtual IEnumerator Routine()
    {
        while (timeRemaining > 0f && !BeenCanceled)
        {
            int truncatedRemaining = (int)timeRemaining;
            timeRemaining -= Time.deltaTime;

            // If the second has ticked over into the whole second
            // after subtracting frame time,
            if (truncatedRemaining != (int)timeRemaining)
            {
                Debug.LogWarning(
                    $"Time Remaining: {Mathf.Round(timeRemaining):F2}",
                    runner);
            }

            yield return null;
        }

        yield return null;
        IsFinished = true;
    }
}
