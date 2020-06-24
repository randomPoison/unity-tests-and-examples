using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SpriteAtlasTest : MonoBehaviour
{
    [SerializeField] private Image _displayByAtlas = default;
    [SerializeField] private Image _displayByCoolness = default;

    private void Start()
    {
        {
            var request = Addressables.LoadAssetAsync<Sprite>("Adventurer[adventurer-attack1-00]");
            request.Completed += handle =>
            {
                _displayByAtlas.sprite = handle.Result;
            };
        }

        {
            var request = Addressables.LoadAssetAsync<Sprite>("adventurer-attack1-00");
            request.Completed += handle =>
            {
                _displayByCoolness.sprite = handle.Result;
            };
        }
    }
}
