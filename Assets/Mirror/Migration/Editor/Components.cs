﻿using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnetNetworkAnimator = UnityEngine.Networking.NetworkAnimator;
using UnetNetworkIdentity = UnityEngine.Networking.NetworkIdentity;
using UnetNetworkManager = UnityEngine.Networking.NetworkManager;
using UnetNetworkProximityChecker = UnityEngine.Networking.NetworkProximityChecker;
using UnetNetworkStartPosition = UnityEngine.Networking.NetworkStartPosition;
using UnetNetworkTransform = UnityEngine.Networking.NetworkTransform;
using UnetNetworkTransformChild = UnityEngine.Networking.NetworkTransformChild;

using UnetNetworkDiscovery = UnityEngine.Networking.NetworkDiscovery;

using MirrorNetworkAnimator = Mirror.NetworkAnimator;
using MirrorNetworkIdentity = Mirror.NetworkIdentity;
using MirrorNetworkManager = Mirror.NetworkManager;
using MirrorNetworkProximityChecker = Mirror.NetworkProximityChecker;
using MirrorNetworkStartPosition = Mirror.NetworkStartPosition;
using MirrorNetworkTransform = Mirror.NetworkTransform;
using MirrorNetworkTransformChild = Mirror.NetworkTransformChild;

namespace Mirror.MigrationUtilities {
    public class Components : MonoBehaviour {

        public static void FindAndReplaceUnetComponents(out int netComponentObsolete) {
            int fileCounter = 0; // files on the project
            netComponentObsolete = 0; // obsolete components found (like lobby)
            int netIdComponentsCount = 0; // network identities
            int netComponentCount = 0; // networking components
            string logErrors = ""; // error message

            string[] files = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);
            int gameObjectCount = files.Length;

            try {
                foreach (string file in files) {
                    fileCounter++;
                    EditorUtility.DisplayProgressBar("Mirror Migration Progress", string.Format("{0} of {1} files scanned...", fileCounter, gameObjectCount), fileCounter / gameObjectCount);

                    string relativepath = "Assets" + file.Substring(Application.dataPath.Length);
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(relativepath);

                    IEnumerable<Transform> childsAndParent = prefab.GetComponentsInChildren<Transform>(true);

                    foreach (Transform actualChild in childsAndParent) {
                        // replace UNET components with their mirror counterpart
                        netComponentCount += ReplaceEveryNetworkComponent(actualChild.gameObject);

                        // always replace NetworkIdentity as last element, due to dependencies
                        netIdComponentsCount += ReplaceEveryNetworkIdentity(actualChild.gameObject);

                        // check for obsolete components
                        int compObsolete = 0;
                        logErrors += CheckObsoleteComponents(actualChild.gameObject, out compObsolete);
                        netComponentObsolete += compObsolete;
                    }
                }

                Debug.LogFormat("Searched {0} Prefabs, found {1} UNET NetworkIdentity, {2} Components and replaced them with Mirror components.\nAlso found {3} now deprecated components.", gameObjectCount, netIdComponentsCount, netComponentCount, netComponentObsolete);

                if (netComponentObsolete > 0)
                    Debug.LogWarningFormat("List of now deprecated components found on your project:\n{0}", logErrors);
            } catch (System.Exception e) {
                Debug.LogError("[Mirror Migration Tool] Encountered an exception!");
                Debug.LogException(e);
            } finally {
                EditorUtility.ClearProgressBar();
            }
        }

        public static void FindAndReplaceUnetSceneGameObject(out int netComponentObsolete) {
            int convertedGoCounter = 0; // counter of converted game objects
            netComponentObsolete = 0; // obsolete components found (like lobby)
            int netIdComponentsCount = 0; // network identities
            int netComponentCount = 0; // networking components
            string logErrors = ""; // error message

            // safest way to get all gameObjects on the scene instead of FindObjectOfType()
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            int gameObjectCount = allObjects.Length;

            foreach (GameObject currentGameObject in allObjects) {
                if (currentGameObject.scene.isLoaded) {
                    convertedGoCounter++;
                    EditorUtility.DisplayProgressBar("Mirror Migration Progress", string.Format("{0} of {1} game object scanned...", convertedGoCounter, gameObjectCount), convertedGoCounter / gameObjectCount);

                    IEnumerable<Transform> childsAndParent = currentGameObject.GetComponentsInChildren<Transform>(true);

                    foreach (Transform actualChild in childsAndParent) {
                        // replace UNET components with their mirror counterpart
                        netComponentCount += ReplaceEveryNetworkComponent(actualChild.gameObject);

                        // always replace NetworkIdentity as last element, due to dependencies
                        netIdComponentsCount += ReplaceEveryNetworkIdentity(actualChild.gameObject);

                        // check for obsolete components
                        int compObsolete = 0;
                        logErrors += CheckObsoleteComponents(actualChild.gameObject, out compObsolete);
                        netComponentObsolete += compObsolete;
                    }
                }
            }

            EditorUtility.ClearProgressBar();
            Debug.LogFormat("Searched {0} GameObjects, found {1} UNET NetworkIdentity, {2} Components and replaced them with Mirror components.\nAlso found {3} now deprecated components.", convertedGoCounter, netIdComponentsCount, netComponentCount, netComponentObsolete);

            if (netComponentObsolete > 0)
                Debug.LogWarningFormat("List of now deprecated components found on your project:\n {0}", logErrors);
        }

        static int ReplaceEveryNetworkComponent(GameObject go) {
            int compCount = 0;

            compCount += Utils.ReplaceNetworkComponent<UnetNetworkAnimator, MirrorNetworkAnimator>(go);
            compCount += Utils.ReplaceNetworkComponent<UnetNetworkTransform, MirrorNetworkTransform>(go);
            compCount += Utils.ReplaceNetworkComponent<UnetNetworkManager, MirrorNetworkManager>(go);
            compCount += Utils.ReplaceNetworkComponent<UnetNetworkProximityChecker, MirrorNetworkProximityChecker>(go);
            compCount += Utils.ReplaceNetworkComponent<UnetNetworkStartPosition, MirrorNetworkStartPosition>(go);
            compCount += Utils.ReplaceNetworkComponent<UnetNetworkTransformChild, MirrorNetworkTransformChild>(go);

            return compCount;
        }

        static int ReplaceEveryNetworkIdentity(GameObject go) {
            int niCount = 0;
            niCount += Utils.ReplaceNetworkComponent<UnetNetworkIdentity, MirrorNetworkIdentity>(go);

            return niCount;
        }

        static string CheckObsoleteComponents(GameObject go, out int compObsolete) {
            string errors = "";
            compObsolete = 0;

            // TODO: others obsolete components from original UNET (and not HLAPI_CE) like lobby
            if (go.GetComponent<UnetNetworkDiscovery>()) {
                compObsolete++;
                errors += go.name + "\n";
            }

            return errors;
        }
    }
}