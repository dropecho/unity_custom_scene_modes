using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;
using UnityEditor;

[CreateAssetMenu(fileName = "CustomSceneDrawMode", menuName = "Dropecho/CustomSceneDrawMode", order = 1)]
public class CustomSceneDrawMode : ScriptableObject {
  public string category;
  public Shader shader;

  void OnValidate() {
    UseCustomSceneDrawMode.RefreshDrawModeList();
  }
}