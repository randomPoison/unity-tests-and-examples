using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Pipeline;
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

        foreach (var target in Targets)
        {
            builder.AppendLine();
            builder.AppendLine($"Testing bundles for target: {target}");
            builder.AppendLine("-------------------------");

            var dryRunHashes = new Dictionary<string, Hash128>();
            var buildHashes = new Dictionary<string, Hash128>();
            var sbpDryRunHashes = new Dictionary<string, Hash128>();
            var sbpBuildHashes = new Dictionary<string, Hash128>();

            // Run asset bundle builds using the built-in build pipeline.
            // ----------------------------------------------------------

            var bundlePath = Path.Combine("AssetBundles", target.ToString());
            if (!Directory.Exists(bundlePath))
            {
                Directory.CreateDirectory(bundlePath);
            }

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

            // Run asset bundle builds using the Scriptable Build Pipeline.
            // ------------------------------------------------------------

            var sbpBundlePath = Path.Combine("SbpAssetBundles", target.ToString());
            if (!Directory.Exists(sbpBundlePath))
            {
                Directory.CreateDirectory(sbpBundlePath);
            }

            var sbpManifest = CompatibilityBuildPipeline.BuildAssetBundles(
                sbpBundlePath,
                BuildAssetBundleOptions.DryRunBuild,
                target);

            foreach (var bundleName in sbpManifest.GetAllAssetBundles())
            {
                sbpDryRunHashes[bundleName] = sbpManifest.GetAssetBundleHash(bundleName);
            }

            sbpManifest = CompatibilityBuildPipeline.BuildAssetBundles(
                sbpBundlePath,
                BuildAssetBundleOptions.None,
                target);

            foreach (var bundleName in sbpManifest.GetAllAssetBundles())
            {
                sbpBuildHashes[bundleName] = sbpManifest.GetAssetBundleHash(bundleName);
            }

            // Build the results table for the current build target.
            // -----------------------------------------------------

            foreach (var bundleName in dryRunHashes.Keys)
            {
                builder.AppendLine(
                    $"Bundle: {bundleName},\t" +
                    $"dry run hash: {dryRunHashes[bundleName]},\t" +
                    $"build hash: {buildHashes[bundleName]},\t" +
                    $"SBP dry run: {sbpDryRunHashes[bundleName]},\t" +
                    $"SBP build: {sbpBuildHashes[bundleName]}");
            }
        }

        Debug.Log(builder);
    }

    [MenuItem("Assets/Test SBP Dry Run")]
    public static void TestSbpDryRun()
    {
        foreach (var target in Targets)
        {
            var sbpBundlePath = Path.Combine("SbpAssetBundles", target.ToString());
            var sbpManifest = CompatibilityBuildPipeline.BuildAssetBundles(
                sbpBundlePath,
                BuildAssetBundleOptions.DryRunBuild,
                target);
        }
    }
}
