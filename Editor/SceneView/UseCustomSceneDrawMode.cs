/* Copyright 2018 Ming Wai Chan
https://cmwdexint.com/2018/07/01/custom-sceneview-drawmode/ */

#if UNITY_EDITOR
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

//====================== The custom draw modes =========================
//internal static readonly PrefColor kSceneViewWire = new PrefColor("Scene/Wireframe", 0.0f, 0.0f, 0.0f, 0.5f);
//https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/Annotation/SceneRenderModeWindow.cs
//https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/SceneView/SceneView.cs
//GameView? https://forum.unity.com/threads/creating-a-totally-custom-scene-editor-in-the-editor.118065/

[InitializeOnLoad]
public class UseCustomSceneDrawMode {
  private static Camera cam;
  private static bool delegateSceneView = false;
  private static CustomSceneDrawMode[] drawModes;

  static UseCustomSceneDrawMode() {
    RefreshDrawModeList();

    EditorApplication.update += Update;
    EditorApplication.projectChanged += RefreshDrawModeList;
  }

  public static void RefreshDrawModeList() {
    drawModes = AssetDatabase.FindAssets("t:" + typeof(CustomSceneDrawMode).Name)
        .Distinct()
        .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
        .Select(path => AssetDatabase.LoadAssetAtPath<CustomSceneDrawMode>(path))
        .ToArray();

    // Clear the list and add the custom modes.
    SceneView.ClearUserDefinedCameraModes();
    foreach (var mode in drawModes) {
      if (mode.shader != null) {
        SceneView.AddCameraMode(mode.name, string.IsNullOrWhiteSpace(mode.category) ? "Custom" : mode.category);
      }
    }

    // Fix the currently selected item in the drop down, if it was deleted, or this package was removed.
    foreach (SceneView view in SceneView.sceneViews) {
      var currentName = view.cameraMode.name;
      if (drawModes.FirstOrDefault(x => x.name == currentName) == null) {
        view.cameraMode = SceneView.GetBuiltinCameraMode(DrawCameraMode.Textured);
      }
    }
  }

  static void Update() {
    if (!delegateSceneView && SceneView.sceneViews.Count > 0) {
      SceneView.duringSceneGui += DuringSceneGui;
      delegateSceneView = true;
    }

    if (SceneView.sceneViews.Count == 0) {
      SceneView.duringSceneGui -= DuringSceneGui;
      delegateSceneView = false;
    }
  }

  private static void DuringSceneGui(SceneView sceneview) {
    CustomSceneDrawMode currentMode = null;

    foreach (var mode in drawModes) {
      if (mode.name == sceneview.cameraMode.name) {
        currentMode = mode;
        break;
      }
    }

    if (currentMode != null && currentMode.shader != null) {
      sceneview.SetSceneViewShaderReplace(currentMode.shader, "");
    } else {
      sceneview.SetSceneViewShaderReplace(null, "");
    }
  }
}
#endif