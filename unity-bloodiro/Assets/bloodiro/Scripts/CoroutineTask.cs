using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTask
{
    public bool isRunning { get; private set; }
    public bool isFinished { get; private set; }
    IEnumerator _routine;
    System.Action _onFinishFunction;

    public CoroutineTask(IEnumerator routine,  MonoBehaviour monoBehaviour, System.Action onFinishFunction = null)
    {
        _routine = routine;
        _onFinishFunction = onFinishFunction;
        monoBehaviour.StartCoroutine(WrapperRoutine());
    }

    IEnumerator WrapperRoutine()
    {
        isRunning = true;
        yield return _routine;
        isRunning = false;
        isFinished = true;
        _onFinishFunction();
    }
}