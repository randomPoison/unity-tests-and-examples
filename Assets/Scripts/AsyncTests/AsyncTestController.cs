using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsyncTestController : MonoBehaviour
{
    private async void Start()
    {
        var start = Time.time;
        while (Time.time - start < 100f)
        {
            var randomNumber = await SlowRandom();
            Debug.Log($"Random number was {randomNumber}");
        }
    }

    private async Task<int> SlowRandom()
    {
        Debug.Log("Generating a random number!");
        await UniTask.Delay(3000);
        return Random.Range(0, 10);
    }
}
