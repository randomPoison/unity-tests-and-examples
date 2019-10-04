using System;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;

public class ExceptionHandlingTest : MonoBehaviour
{
    private async void Start()
    {
        UniTaskScheduler.UnobservedExceptionWriteLogType = LogType.Exception;

        ThrowVoid();
        ThrowUniTaskVoid().Forget();

        try
        {
            await UniTaskThrowDeep(10);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        try
        {
            await RawTaskThrowDeep(10);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private async void ThrowVoid()
    {
        throw new Exception("Exception thrown from async void function");
    }

    private async UniTaskVoid ThrowUniTaskVoid()
    {
        throw new Exception("Exception thrown from async UniTaskVoid function");
    }

    private async UniTask UniTaskThrowDeep(int depth)
    {
        if (depth == 0)
        {
            throw new Exception("Exception thrown from deep in the call stack");
        }
        else
        {
            await UniTaskThrowDeep(depth - 1);
        }
    }

    private async Task RawTaskThrowDeep(int depth)
    {
        if (depth == 0)
        {
            throw new Exception("Exception thrown from deep in the call stack");
        }
        else
        {
            await RawTaskThrowDeep(depth - 1);
        }
    }
}
