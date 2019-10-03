using UniRx.Async;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsyncTestController : MonoBehaviour
{
    private async void VoidTask()
    {
        Debug.Log("Doing a void task!");
        await UniTask.Delay(1000);
        Debug.Log("Void task resumed!");
    }

    private async UniTaskVoid Start()
    {
        Debug.Log("About to call VoidTask()");
        VoidTask();
        Debug.Log("Returned from VoidTask()");

        LongRunningTask();

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
}
