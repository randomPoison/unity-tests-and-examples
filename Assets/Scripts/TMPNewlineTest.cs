using TMPro;
using UnityEngine;

public class TMPNewlineTest : MonoBehaviour
{
    public string inputText;
    public TextMeshProUGUI display;

    private string _prevInput;

    void Update()
    {
        if (_prevInput != inputText)
        {
            display.text = inputText;
            _prevInput = inputText;
        }
    }
}
