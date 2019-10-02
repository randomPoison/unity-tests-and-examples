using System.Collections;
using UniRx.Async;
using UnityEngine;

public class AsyncTimingTest : MonoBehaviour
{
    private void Start()
    {
        TestTimingAsync().Forget();
        StartCoroutine(TestTimingRoutine());
    }

    private async UniTaskVoid TestTimingAsync()
    {
        // Simulate initial asset load taking 1 second.
        {
            var frameBefore = Time.frameCount;
            var asset = await SimulateLoadAssetAsync(false);
            var frameAfter = Time.frameCount;
            Debug.Log($"[Async] Initial asset load took {frameAfter - frameBefore} frames (started {frameBefore}, resumed {frameAfter})");
        }

        // Simulate loading cached asset, where asset is available immediately.
        {
            var frameBefore = Time.frameCount;
            var asset = await SimulateLoadAssetAsync(true);
            var frameAfter = Time.frameCount;
            Debug.Log($"[Async] Cached asset load took {frameAfter - frameBefore} frames (started {frameBefore}, resumed {frameAfter})");
        }

        // Simulate awaiting a very brief delay, something that becomes available on the
        // same frame that it's requested.
        {
            var frameBefore = Time.frameCount;
            await UniTask.Delay(1);
            var frameAfter = Time.frameCount;
            Debug.Log($"[Async] Waiting 1ms took {frameAfter - frameBefore} frames (started {frameBefore}, resumed {frameAfter})");
        }
    }

    private IEnumerator TestTimingRoutine()
    {
        // Simulate initial asset load taking 1 second.
        {
            var frameBefore = Time.frameCount;
            yield return SimulateLoadAssetRoutine(false);
            var frameAfter = Time.frameCount;
            Debug.Log($"[Coroutine] Initial asset load took {frameAfter - frameBefore} frames (started {frameBefore}, resumed {frameAfter})");
        }

        // Simulate loading cached asset, where asset is available immediately.
        {
            var frameBefore = Time.frameCount;
            yield return SimulateLoadAssetRoutine(true);
            var frameAfter = Time.frameCount;
            Debug.Log($"[Coroutine] Cached asset load took {frameAfter - frameBefore} frames (started {frameBefore}, resumed {frameAfter})");
        }

        // Simulate awaiting a very brief delay, something that becomes available on the
        // same frame that it's requested.
        {
            var frameBefore = Time.frameCount;
            yield return new WaitForSeconds(0.001f);
            var frameAfter = Time.frameCount;
            Debug.Log($"[Coroutine] Waiting 1ms took {frameAfter - frameBefore} frames (started {frameBefore}, resumed {frameAfter})");
        }
    }

    private async UniTask<string> SimulateLoadAssetAsync(bool isCached)
    {
        if (!isCached)
        {
            await UniTask.Delay(1000);
        }

        return "I'm an asset!";
    }

    private IEnumerator SimulateLoadAssetRoutine(bool isCached)
    {
        if (!isCached)
        {
            yield return new WaitForSeconds(1f);
        }
    }
}
