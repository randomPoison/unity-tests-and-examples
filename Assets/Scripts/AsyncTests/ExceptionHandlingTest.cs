#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

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

        await ThrowRecursive(3);
    }

    private async void ThrowVoid()
    {
        throw new Exception("Exception thrown from async void function");
    }

    private async Task ThrowRecursive(int depth)
    {
        if (depth == 0)
        {
            throw new Exception("Exception thrown from deep in the call stack");
        }
        else
        {
            await ThrowRecursive(depth - 1);
        }
    }
}
