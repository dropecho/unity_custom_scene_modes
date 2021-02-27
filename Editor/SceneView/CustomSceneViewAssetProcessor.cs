
using UnityEditor;

namespace Dropecho {
  public class CustomSceneViewAssetPostProcessor : AssetPostprocessor {
    // If you have the scene view open, redraw the gui to load any new assets.
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
      UseCustomSceneDrawMode.RefreshDrawModeList();
    }
  }
}