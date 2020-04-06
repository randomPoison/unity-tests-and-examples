using System.Collections;
using UnityEngine;

/// <summary>
/// Test to see what happens if you try to stop a corotuine on the same frame that you
/// start it. Does Unity correctly cancel the corotuine, or does it let it keep running?
/// </summary>
public class CoroutineTest : MonoBehaviour
{
    void Start()
    {
        var routine = StartCoroutine(TestRoutine());
        StopCoroutine(routine);
    }

    IEnumerator TestRoutine()
    {
        Debug.Log("Top of routine");

        while (true)
        {
            Debug.Log("Top of loop");
            yield return null;
        }
    }
}
