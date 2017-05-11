// <copyright file="GPGSDependencies.cs" company="Google Inc.">
// Copyright (C) 2015 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using UnityEditor;

[InitializeOnLoad]
public class HeyzapGPSDependencies : AssetPostprocessor {
#if UNITY_ANDROID
  /// <summary>Instance of the PlayServicesSupport resolver</summary>
  public static object svcSupport;
#endif  // UNITY_ANDROID

  /// Initializes static members of the class.
  static HeyzapGPSDependencies() {
        RegisterDependencies();
  }


  /// <summary>
  /// Registers the dependencies needed by this plugin.
  /// </summary>
  public static void RegisterDependencies() {
#if UNITY_ANDROID
    RegisterAndroidDependencies();
#elif UNITY_IOS
#endif
  }

  /// <summary>
  /// Registers the android dependencies.
  /// </summary>
  public static void RegisterAndroidDependencies() {

    // Setup the resolver using reflection as the module may not be
    // available at compile time.
    Type playServicesSupport = Google.VersionHandler.FindClass(
      "Google.JarResolver", "Google.JarResolver.PlayServicesSupport");
    if (playServicesSupport == null) {
      return;
    }
    svcSupport = svcSupport ?? Google.VersionHandler.InvokeStaticMethod(
      playServicesSupport, "CreateInstance",
      new object[] {
          "GooglePlayGames",
          EditorPrefs.GetString("AndroidSdkRoot"),
          "ProjectSettings"
      });

    // For example to depend on play-services-games version 9.6.0  you need to specify the
    // package, artifact, and version as well as the packageId from the SDK manager in case
    // a newer version needs to be downloaded to build.

    Google.VersionHandler.InvokeInstanceMethod(
      svcSupport, "DependOn",
      new object[] {
      "com.google.android.gms",
      "play-services-ads",
      "10.2.1" },
      namedArgs: new Dictionary<string, object>() {
          {"packageIds", new string[] { "extra-google-m2repository" } }
      });
  }

  // Handle delayed loading of the dependency resolvers.
  private static void OnPostprocessAllAssets(
      string[] importedAssets, string[] deletedAssets,
      string[] movedAssets, string[] movedFromPath) {
    foreach (string asset in importedAssets) {
      if (asset.Contains("IOSResolver") ||
        asset.Contains("JarResolver")) {
        RegisterDependencies();
        break;
      }
    }
  }
}

