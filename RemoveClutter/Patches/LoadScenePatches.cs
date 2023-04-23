namespace RemoveClutter.Patches
{
    [HarmonyPatch]
    public static class LoadScenePatches
    {
        /// <summary>
        /// Triggers on initial game save load.  Also triggered by transitioning indoors or outdoors, or to a different zone.
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(SaveGameSystem), nameof(SaveGameSystem.LoadSceneData))]
        public static void LoadSceneData(ref string name)
        {
            if (InterfaceManager.IsMainMenuEnabled() || GameManager.IsOutDoorsScene(GameManager.m_ActiveScene)
                                                         && !RemoveClutter.notReallyOutdoors.Contains(GameManager.m_ActiveScene))
            {
                Logger2.Msg($"{GameManager.m_ActiveScene} is outdoor scene, mod disabled.");
                return;
            }

            PatchSceneObjects();
            PatchSceneDecals();

            string? serializedSaveData = RemoveClutter.dataManager.Load(RemoveClutter.SaveDataTag);

            if (!string.IsNullOrEmpty(serializedSaveData))
            {
                JSON.MakeInto(JSON.Load(serializedSaveData), out RemoveClutter.buriedCorpses);
            }

            if (RemoveClutter.buriedCorpses.ContainsKey(GameManager.m_ActiveScene))
            {
                foreach (string s in RemoveClutter.buriedCorpses[GameManager.m_ActiveScene])
                {
                    ContainerManager.FindContainerByGuid(s)?.gameObject.SetActive(false);
                }
            }
        }

        public static void PatchSceneObjects()
        {
            var timer = Stopwatch.StartNew();

            var rootObjects = Utils.GetRootObjects()
                .Where(e => !e.name.Contains("XPZ"))
                .ToList();

            // Clear patched objects from the previous scene
            RemoveClutter.itemList.Clear();

            int objectsSetupCount = 0;
            int totalLoops = 0;
            var results = new List<GameObject>();

            foreach (GameObject rootObj in rootObjects)
            {
                Utils.GetChildrenWithName(rootObj, $"OBJ_", results);
            }

            foreach (BreakDownDefinition breakdownDefinition in RemoveClutter.breakdownDefinitions)
            {
                foreach (GameObject child in results)
                {
                    totalLoops++;

                    if (child.name.Contains("_xpzclutter"))
                    {
                        continue;
                    }

                    if (!child.name.Contains(breakdownDefinition.filter))
                    {
                        continue;
                    }
                    if (child.GetComponent("RepairableContainer") == null)
                    {
                        child.name += "_xpzclutter";

                        PrepareGameObject(child, breakdownDefinition);

                        objectsSetupCount++;
                    }
                }

            }

            Logger2.Msg($"{objectsSetupCount} objects setup for removal. Took: {timer.FormatElapsedString()}");
            Logger2.Msg($"Total iterations : {totalLoops}");
        }

        internal static void PrepareGameObject(GameObject gameObject, BreakDownDefinition objDef)
        {
            LODGroup lodObject = gameObject.GetComponent<LODGroup>();

            if (lodObject == null)
            {
                lodObject = gameObject.GetComponentInChildren<LODGroup>();
            }

            if (lodObject != null)
            {
                gameObject = lodObject.gameObject;
            }

            Renderer renderer = Il2Cpp.Utils.GetLargestBoundsRenderer(gameObject);
            if (renderer == null)
            {
                return;
            }

            //Check if it has collider, add one if it doesn't
            Collider collider = gameObject.GetComponent<Collider>();
            if (collider == null)
            {
                collider = gameObject.GetComponentInChildren<Collider>();
            }

            if (gameObject.name.StartsWith("Decal-"))
            {
                gameObject.transform.localRotation = Quaternion.identity;
                GameObject collisionObject = new GameObject("PaperDecalRemover-" + gameObject.name);
                collisionObject.transform.parent = gameObject.transform.parent;
                collisionObject.transform.position = gameObject.transform.position;

                gameObject.transform.parent = collisionObject.transform;

                gameObject = collisionObject;
            }

            if (collider == null)
            {
                Bounds bounds = renderer.bounds;

                BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                boxCollider.size = bounds.size;
                boxCollider.center = bounds.center - gameObject.transform.position;
            }

            BreakdownLogic.AddBreakDownComponent(gameObject, objDef);

            //Set children to interactive layer
            if (gameObject.transform.childCount > 0)
            {
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    if (gameObject.transform.GetChild(i).gameObject.name.StartsWith("Decal-"))
                    {
                        continue;
                    }

                    Utils.SetLayer(gameObject.transform.GetChild(i).gameObject, vp_Layer.InteractiveProp);
                }
            }
        }

        internal static void PatchSceneDecals()
        {
            var timer = Stopwatch.StartNew();

            Melon<RemoveClutter>.Logger.Msg(nameof(PatchSceneDecals));
            GameObject[] rObjs = Utils.GetRootObjects().ToArray();

            foreach (GameObject rootObj in rObjs)
            {
                MeshRenderer childRenderer = rootObj.GetComponent<MeshRenderer>();
                MeshRenderer[] allRenderers = rootObj.GetComponentsInChildren<MeshRenderer>();
                allRenderers.AddItem(childRenderer);

                foreach (MeshRenderer renderer in allRenderers)
                {
                    if (renderer.gameObject.name.ToLower().Contains("decal-"))
                    {
                        renderer.receiveShadows = true;
                        qd_Decal decal = renderer.GetComponent<qd_Decal>();
                        if (decal != null && (decal.texture.name.StartsWith("FX_DebrisPaper") || decal.texture.name.StartsWith("FX_DebrisMail") || decal.texture.name.StartsWith("FX_DebriPaper")))
                        {

                            BreakDownDefinition bdDef = new BreakDownDefinition
                            {
                                filter = "Paper",
                                minutesToHarvest = 1f,
                                sound = "Paper"
                            };

                            PrepareGameObject(renderer.gameObject, bdDef);

                        }

                        continue;
                    }
                }
            }

            Logger2.Msg($"Patched scene decals in : {timer.FormatElapsedString()}");
            //BreakDown.DeserializeAllAdditive(sceneBreakDownData);
        }
    }
}
