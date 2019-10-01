using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BundleCachingTest : MonoBehaviour
{
    [SerializeField]
    private string _bundleUrl = null;

    [SerializeField]
    private bool _runInLoop = false;

    private IEnumerator Start()
    {
        // Define the caching information once. The same caching information is used for
        // every load, so we know the same version will be cached after the first load
        // completes.
        var cacheInfo = new CachedAssetBundle(_bundleUrl, Hash128.Compute(_bundleUrl));

        do
        {
            // Ensure that we're starting with a clean cache each time.
            while (!Caching.ClearCache())
            {
                yield return true;
            }

            // Load the bundle once to populate the cache. We make sure this load
            // finishes before starting subsequent loads.
            Debug.Log("Loading bundle first time");
            yield return LoadBundleRoutine(cacheInfo, false);
            Debug.Assert(
                Caching.IsVersionCached(cacheInfo),
                "Bundle was not cached after first load");

            // Load the bundle from the cache twice concurrently. We use SpawnCoroutine()
            // to ensure that both loads will happen concurrently. The inner coroutine
            // yields while waiting for the bundle to load, so the third load is
            // guaranteed to start while the second one is in progress.
            Debug.Log("Spawning second load");
            var secondLoad = StartCoroutine(LoadBundleRoutine(cacheInfo, true));
            Debug.Assert(
                Caching.IsVersionCached(cacheInfo),
                "Bundle was not cached after second load");

            Debug.Log("Spawning third load");
            var thirdLoad = StartCoroutine(LoadBundleRoutine(cacheInfo, true));
            Debug.Assert(
                Caching.IsVersionCached(cacheInfo),
                "Bundle was not cached after third load");

            // Make sure all loading has finished before starting the next iteration.
            yield return secondLoad;
            yield return thirdLoad;
        } while (_runInLoop);
    }

    private IEnumerator LoadBundleRoutine(CachedAssetBundle cacheInfo, bool shouldBeCached)
    {
        var isCached = Caching.IsVersionCached(cacheInfo);
        Debug.Assert(
            isCached == shouldBeCached,
            $"Bundle was not cached when it should have been, should be cached: {shouldBeCached}, is cached: {isCached}");

        using (var request = UnityWebRequestAssetBundle.GetAssetBundle(_bundleUrl, cacheInfo))
        {
            yield return request.SendWebRequest();

            var bundle = DownloadHandlerAssetBundle.GetContent(request);
            bundle.Unload(true);
        }
    }
}
