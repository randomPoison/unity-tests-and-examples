using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Linq;

public class SpriteAtlasTest : MonoBehaviour
{
    [SerializeField] private Image _displayByAtlas = default;
    [SerializeField] private Image _displayByCoolness = default;

    private async void Start()
    {
        // Register a location map that tries to point the sprite name to the location
        // within the atlas.
        var adventurerLocations = await Addressables.LoadResourceLocationsAsync("Adventurer");
        var spriteLocations = new ResourceLocationMap("Atlas Sprite Locations");
        spriteLocations.Add(
            "adventurer-attack1-00",
            new ResourceLocationBase(
                "Adventurer[adventurer-attack1-00]",
                "Adventurer[adventurer-attack1-00]",
                typeof(AtlasSpriteProvider).FullName,
                typeof(Sprite),
                adventurerLocations.ToArray()));
        Addressables.AddResourceLocator(spriteLocations);

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

///// <summary>
///// Provides sprite from atlases when sprites are referenced directly.
///// </summary>
//[DisplayName("Directly-Referenced Sprites from Atlases Provider")]
//public class DirectAtlasSpriteProvider : ResourceProviderBase
//{
//    public override void Provide(ProvideHandle providerInterface)
//    {
//        var atlas = providerInterface.GetDependency<SpriteAtlas>(0);
//        if (atlas == null)
//        {
//            providerInterface.Complete<Sprite>(null, false, new System.Exception($"Sprite atlas failed to load for location {providerInterface.Location.PrimaryKey}."));
//            return;
//        }

//        var key = providerInterface.ResourceManager.TransformInternalId(providerInterface.Location);
//        string spriteKey = string.IsNullOrEmpty(subKey) ? mainKey : subKey;

//        var sprite = atlas.GetSprite(spriteKey);
//        providerInterface.Complete(sprite, sprite != null, sprite != null ? null : new System.Exception($"Sprite failed to load for location {providerInterface.Location.PrimaryKey}."));
//    }
//}
