using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

public class LoadTestBundle : MonoBehaviour
{
    [SerializeField]
    private AssetReference _testAsset = null;

    [SerializeField]
    private TextMeshProUGUI _resultText = null;

    private IEnumerator Start()
    {
        //var bundleUrl = Path.Combine(_baseUrl, Application.platform.ToString(), _bundleName);
        //_resultText.text = $"Downloading bundle from {bundleUrl}\n";

        //var request = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl);
        //yield return request.SendWebRequest();

        //if (request.isNetworkError || request.isHttpError)
        //{
        //    _resultText.text += $"Error downloading bundle: {request.error}\n";
        //    yield break;
        //}

        //var bundle = DownloadHandlerAssetBundle.GetContent(request);
        //_resultText.text += "Finished downloading bundle!\n";

        //var testGameObject = bundle.LoadAsset<GameObject>("Test Prefab");
        //var testAsset = testGameObject.GetComponent<ScriptSerializationTest>();

        var handle = _testAsset.LoadAssetAsync<GameObject>();
        yield return handle;

        var testAsset = handle.Result.GetComponent<ScriptSerializationTest>();
        if (testAsset != null)
        {
            _resultText.text += "Loaded Test Prefab asset!\n";
            _resultText.text += $"Int field: {testAsset.IntField}\n";
            _resultText.text += $"String field: {testAsset.StringField}\n";
        }
        else
        {
            _resultText.text += "Test Prefab asset was null\n";
        }
    }
}
