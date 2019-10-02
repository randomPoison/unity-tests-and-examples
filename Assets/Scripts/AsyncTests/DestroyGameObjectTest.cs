using UniRx.Async;
using UnityEngine;

public class DestroyGameObjectTest : MonoBehaviour
{
    private async void Start()
    {
        Debug.Log("AsyncTestController.Start()");

        Debug.Log($"Destroying game object {this}");
        Destroy(gameObject);

        await UniTask.Delay(1000);
        Debug.Log($"After delay in Start(), game object: {this}");
    }
}
