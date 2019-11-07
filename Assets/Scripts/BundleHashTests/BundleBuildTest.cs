using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class BundleBuildTest
{
    private static readonly BuildTarget[] Targets = new BuildTarget[]
    {
        BuildTarget.StandaloneWindows,
        BuildTarget.Android,
    };

    [MenuItem("Assets/Test AssetBundle Hashes")]
    public static void BuildTestBundles()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"Running with active build target: {EditorUserBuildSettings.activeBuildTarget}");

        var bundlePath = Path.Combine("AssetBundles", EditorUserBuildSettings.activeBuildTarget.ToString());
        if (Directory.Exists(bundlePath))
        {
            Directory.CreateDirectory(bundlePath);
        }

        foreach (var target in Targets)
        {
            builder.AppendLine();
            builder.AppendLine($"Testing bundles for target: {target}");
            builder.AppendLine("-------------------------");

            var dryRunHashes = new Dictionary<string, Hash128>();
            var buildHashes = new Dictionary<string, Hash128>();

            var manifest = BuildPipeline.BuildAssetBundles(
                bundlePath,
                BuildAssetBundleOptions.DryRunBuild,
                target);

            foreach (var bundleName in manifest.GetAllAssetBundles())
            {
                dryRunHashes[bundleName] = manifest.GetAssetBundleHash(bundleName);
            }

            manifest = BuildPipeline.BuildAssetBundles(
                bundlePath,
                BuildAssetBundleOptions.None,
                target);

            foreach (var bundleName in manifest.GetAllAssetBundles())
            {
                buildHashes[bundleName] = manifest.GetAssetBundleHash(bundleName);
            }

            foreach (var bundleName in dryRunHashes.Keys)
            {
                builder.AppendLine(
                    $"Bundle: {bundleName}, " +
                    $"dry run hash: {dryRunHashes[bundleName]}, " +
                    $"build hash: {buildHashes[bundleName]}");
            }
        }

        Debug.Log(builder);
    }

    private static BuildTargetGroup GroupForTarget(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.StandaloneWindows: return BuildTargetGroup.Standalone;
            case BuildTarget.Android: return BuildTargetGroup.Android;
            case BuildTarget.iOS: return BuildTargetGroup.iOS;
            default: throw new ArgumentException($"Unknown build target {target}");
        }
    }
}
