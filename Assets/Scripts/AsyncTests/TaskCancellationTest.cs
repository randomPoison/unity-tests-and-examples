using System.Threading;
using UniRx.Async;
using UnityEngine;

public class TaskCancellationTest : MonoBehaviour
{
    [SerializeField]
    private bool _cancelOnDestroy = true;

    [SerializeField]
    private float _destroyDelaySeconds = 0.5f;

    private GameObject _prefab = null;

    private void Start()
    {
        TestCancellation().Forget();
        Destroy(gameObject, _destroyDelaySeconds);
    }

    public async UniTaskVoid TestCancellation()
    {
        Debug.Log("Going to simulate loading an asset and doing some additional work...");

        using (var cancellation = new CancellationTokenSource())
        {
            if (_cancelOnDestroy)
            {
                cancellation.RegisterRaiseCancelOnDestroy(gameObject);
            }

            // You can use ConfigureAwait() to use cancellation tokens with coroutine-based
            // async operations.
            _prefab = (GameObject)await Resources
                .LoadAsync<GameObject>("SomeAsset")
                .ConfigureAwait(cancellation: cancellation.Token);
            Debug.Log($"Finished waiting for Resources.Load(): {_prefab}");

            // Simulate waiting for another async operation.
            await UniTask.Delay(1200, cancellationToken: cancellation.Token);
            Debug.Log("Finished waiting for delay");

            if (_prefab == null)
            {
                Debug.Log("Prefab was null");
            }
            else
            {
                Debug.Log($"Loaded prefab with name: {_prefab.name}");
            }
        }
    }
}
