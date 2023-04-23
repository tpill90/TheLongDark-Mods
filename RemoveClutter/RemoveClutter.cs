namespace RemoveClutter
{
    class RemoveClutter : MelonMod
    {
        #region Config

        private static readonly string MODS_FOLDER_PATH = Path.GetFullPath(typeof(MelonMod).Assembly.Location + @"\..\..\..\Mods\remove-clutter");

        public static readonly string SaveDataTag = "removeClutter";
        public static ModDataManager dataManager = new ModDataManager("RemoveClutter");

        #endregion

        //TODO refactor
        #region members

        public static Dictionary<string, List<string>> buriedCorpses = new Dictionary<string, List<string>>();

        private static bool interrupted;
        public static bool inProgress;

        public static List<GameObject> itemList = new List<GameObject>();
        public static List<BreakDownDefinition> breakdownDefinitions = new List<BreakDownDefinition>();

        public static string sceneBreakDownData = null;

        //TODO set back to false later
        public static bool verbose = true;

        public static List<string> notReallyOutdoors = new List<string>
        {
            "DamTransitionZone"
        };

        #endregion

        public override void OnInitializeMelon()
        {
            Melon<RemoveClutter>.Logger.Msg($"Version {Assembly.GetExecutingAssembly().GetName().Version}");

            ClassInjector.RegisterTypeInIl2Cpp<ChangeLayer>();
            Settings.OnLoad();

            LoadBreakDownDefinitions();
        }

        private static void LoadBreakDownDefinitions()
        {
            var timer = Stopwatch.StartNew();

            string definitionsFolder = Path.Combine(MODS_FOLDER_PATH, "definitions");

            foreach (var file in Directory.GetFiles(definitionsFolder, "*.json"))
            {
                try
                {
                    var deserialized = JSON.Load(File.ReadAllText(file));
                    breakdownDefinitions.AddRange(deserialized.Make<List<BreakDownDefinition>>());

                    Logger2.Msg($"{Path.GetFileName(file)} definitions loaded ");
                }
                catch (FormatException e)
                {
                    Logger2.Error($"{Path.GetFileName(file)} incorrectly formatted.");
                }
            }

            Logger2.Msg($"Loaded clutter definitions in {timer.FormatElapsedString()}");
        }

        public static IEnumerator RemoveObject(GameObject corpse)
        {
            GameManager.GetPlayerVoiceComponent().BlockNonCriticalVoiceForDuration(10f);

            interrupted = false;
            inProgress = true;

            var progressBarPanel = InterfaceManager.GetPanel<Panel_GenericProgressBar>();
            //TODO change the amount of time taken for each object to be different
            progressBarPanel.Launch("Remove", 1, 1f, 0, true, null);

            while (inProgress)
            {
                yield return new WaitForEndOfFrame();
            }

            if (!interrupted)
            {
                corpse.active = false;
                Carrion crows = corpse.GetComponent<Carrion>();
                if (crows != null) crows.Destroy();
                string guid = ObjectGuid.GetGuidFromGameObject(corpse.gameObject);
                if (buriedCorpses.ContainsKey(GameManager.m_ActiveScene)) buriedCorpses[GameManager.m_ActiveScene].Add(guid);
                else buriedCorpses[GameManager.m_ActiveScene] = new List<string> { guid };

            }
            yield break;
        }

        [HarmonyPatch(typeof(Panel_GenericProgressBar), nameof(Panel_GenericProgressBar.ProgressBarEnded))]
        public class ProgressBarCallback
        {
            public static void Prefix(ref bool success, ref bool playerCancel)
            {
                if (!inProgress)
                {
                    return;
                }

                if (!success) interrupted = true;
                inProgress = false;
            }
        }

        [HarmonyPatch(typeof(SaveGameSystem), nameof(SaveGameSystem.SaveSceneData))]
        private static class SaveHarvestTimes
        {
            internal static void Prefix(ref SlotData slot)
            {
                string serializedSaveData = JSON.Dump(buriedCorpses);

                dataManager.Save(serializedSaveData, SaveDataTag);
            }
        }
        
    }
}