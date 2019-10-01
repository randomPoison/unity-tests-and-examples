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
}
