using UnityEditor;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.U2D;

[CreateAssetMenu(
    fileName = "BuildScriptFastModeWithAtlasSpriteAddresses.asset",
    menuName = "Addressables/Custom Build/Fast Mode with Atlas Sprite Addresses")]
public class BuildScriptFastModeWithAtlasSpriteAddresses : BuildScriptFastMode
{
    public override string Name => "Use Asset Database (faster, with atlas sprite addresses)";

    protected override string ProcessGroup(AddressableAssetGroup assetGroup, AddressableAssetsBuildContext context)
    {
        Debug.Log($"Processing group: {assetGroup}");

        foreach (var entry in assetGroup.entries)
        {
            if (entry.MainAsset == null)
            {
                continue;
            }

            var type = entry.MainAsset.GetType();
            Debug.Log($"Checking entry {entry}");

            // Add special handling for SpriteAtlas assets by adding extra catalog
            // entries for each of the sprites in the atlas.
            if (type == typeof(SpriteAtlas))
            {
                Debug.Log($"Adding sprite entries for atlas {entry.MainAsset}");

                var atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(entry.AssetPath);
                var sprites = new Sprite[atlas.spriteCount];
                atlas.GetSprites(sprites);

                var keyList = entry.CreateKeyList();

                for (int i = 0; i < atlas.spriteCount; i++)
                {
                    var spriteName = sprites[i].name;

                    // TODO: Do we want/need this logic here? This was copied from the original
                    // sub-asset implementation for sprite atlases, but I'm not sure we need to
                    // modify the atlas names here.
                    if (spriteName.EndsWith("(Clone)"))
                    {
                        spriteName = spriteName.Replace("(Clone)", "");
                    }

                    context.locations.Add(new ContentCatalogDataEntry(
                        typeof(Sprite),
                        spriteName,
                        typeof(AtlasSpriteProvider).FullName,
                        new object[] { spriteName },
                        new object[] { keyList[0] }));
                }

                context.providerTypes.Add(typeof(AtlasSpriteProvider));
            }
        }

        return base.ProcessGroup(assetGroup, context);
    }
}
