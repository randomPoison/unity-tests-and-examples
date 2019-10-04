using UniRx.Async;
using UnityEngine;

public class ExecutionOrderTest : MonoBehaviour
{
    private void Start()
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
}
