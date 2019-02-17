using UnityEditor;
using Mirror.MigrationUtilities;

public class Windows : EditorWindow {

    [MenuItem("Tools/Mirror/Migrate UNET components on Prefabs")]
    private static void ReplaceComponentsOnPrefabs() {
        if (EditorUtility.DisplayDialog("Prefabs Converter",
            "Are you sure you want to convert prefabs of your project from UNET to Mirror?\nNote: Depending on your project size, it could take lot of time. Please don't close Unity during the process to avoid corrupted project.\nAlso, please be sure you made a backup of your project, just in case.",
            "Yes, farewell UNET!", "Cancel")) {
            int netComponentObsolete = 0;

            Components.FindAndReplaceUnetComponents(out netComponentObsolete);

            if (netComponentObsolete > 0) {
                EditorUtility.DisplayDialog("Warning",
                    "Please check your console logs, obsolete components found.",
                    "OK");
            }
        }
    }

    [MenuItem("Tools/Mirror/Migrate UNET components on Scene")]
    private static void ReplaceComponentsOnScene() {
        if (EditorUtility.DisplayDialog("Scene GameObjects Converter",
            "Are you sure you want to convert GameObjects of your scene from UNET to Mirror?\nNote: Depending on your scene size, it could take lot of time. Please don't close Unity during the process to avoid corrupted scene.\nAlso, please be sure you made a backup of your project, just in case.",
            "Yes, farewell UNET!", "Cancel")) {
            int netComponentObsolete = 0;

            Components.FindAndReplaceUnetSceneGameObject(out netComponentObsolete);

            if (netComponentObsolete > 0) {
                EditorUtility.DisplayDialog("Warning",
                    "Please check your console logs, obsolete components found.",
                    "OK");
            }
        }
    }
}
