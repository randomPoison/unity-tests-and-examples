using System.Runtime.InteropServices;
using UnityEngine;

public class RustBinding : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern int add(int left, int right);

    private void Start()
    {
        Debug.Log("Gonna do some addition!");
        var result = add(1, 2);
        Debug.Log($"1 + 2 = {result}");
    }
}
