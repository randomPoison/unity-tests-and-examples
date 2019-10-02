using UniRx.Async;
using UnityEngine;

public class AsyncTestController : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("About to call VoidTask()");
        VoidTask();
        Debug.Log("Returned from VoidTask()");
    }

    private async void VoidTask()
    {
        Debug.Log("Doing a void task!");
        await UniTask.Delay(1000);
        Debug.Log("Void task resumed!");
    }

    private async void Start()
    {
        LongRunningTask();
        TimingTest();

        Debug.Log("AsyncTestController.Start()");

        Debug.Log($"Destroying game object {this}");
        Destroy(gameObject);

        await UniTask.Delay(1000);
        Debug.Log($"After delay in Start(), game object: {this}");
    }

    private async UniTask<string> StringTask()
    {
        Debug.Log("StringTask(), before await");
        await UniTask.Delay(10);
        Debug.Log("StringTask(), after await");
        return "Did a thing!";
    }

    private async void LongRunningTask()
    {
        var start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - start < 100f)
        {
            var randomNumber = await SlowRandom();
            Debug.Log($"Random number was {randomNumber}");
        }
    }

    private async UniTask<int> SlowRandom()
    {
        await UniTask.Delay(3000);
        return Random.Range(0, 10);
    }

    private async void TimingTest()
    {
        Debug.Log($"Simulating loading an asset for the first time, frame: {Time.frameCount}");
        var asset = await LoadAssetCached(false);
        Debug.Log($"Got asset on frame {Time.frameCount}: {asset}");

        Debug.Log($"Simulating load from cache, frame: {Time.frameCount}");
        asset = await LoadAssetCached(true);
        Debug.Log($"Got asset on frame {Time.frameCount}: {asset}");

        async UniTask<string> LoadAssetCached(bool isCached)
        {
            if (!isCached)
            {
                await UniTask.Delay(1000);
            }

            return "I'm an asset!";
        }
    }
}
