using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestInactivityTest : MonoBehaviour
{
    [SerializeField] private string _url = default;
    [SerializeField] private float _interval = default;

    private IEnumerator Start()
    {
        for (int count = 0; ; count += 1)
        {
            Debug.Log($"Attempt #{count}");

            var request = UnityWebRequest.Get(_url);
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.LogError($"Got an error: {request.error}");
                yield break;
            }

            Debug.Log($"Got {request.responseCode} response, waiting {_interval * count} before next attempt");
            yield return new WaitForSeconds(_interval * count);
        }
    }
}
