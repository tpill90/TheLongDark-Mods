namespace RemoveClutter
{
    public static class Logging
    {
        //TODO cleanup
        public static MelonLogger.Instance Logger2 = Melon<RemoveClutter>.Logger;
    }

    public static class StopwatchExtensions
    {
        /// <summary>
        /// Formats the elapsed time, omitting any leading 0's.
        ///
        /// For example, if the total elapsed time 15 minutes, the result would be "15:00.00
        /// </summary>
        public static string FormatElapsedString(this Stopwatch stopwatch)
        {
            TimeSpan elapsed = stopwatch.Elapsed;
            if (elapsed.TotalHours > 1)
            {
                return elapsed.ToString(@"h\:mm\:ss\.ff");
            }
            if (elapsed.TotalMinutes > 1)
            {
                return elapsed.ToString(@"mm\:ss\.ff");
            }
            return elapsed.ToString(@"ss\.ffff");
        }
    }

    internal class Utils
    {
        internal static List<GameObject> GetRootObjects()
        {
            List<GameObject> rootObj = new List<GameObject>();

            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);

                GameObject[] sceneObj = scene.GetRootGameObjects();

                foreach (GameObject obj in sceneObj)
                {
                    rootObj.Add(obj);
                }
            }

            return rootObj;
        }

        internal static void GetChildrenWithName(GameObject obj, string name, List<GameObject> result)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                GameObject child = obj.transform.GetChild(i).gameObject;

                if (child.name.Contains(name))
                {
                    result.Add(child);
                    continue;
                }

                GetChildrenWithName(child, name, result);
            }
        }

        internal static void SetLayer(GameObject gameObject, int layer)
        {
            ChangeLayer changeLayer = gameObject.AddComponent<ChangeLayer>();
            changeLayer.Layer = layer;
            changeLayer.Recursively = false;
        }

        internal static RaycastHit DoRayCast(Vector3 start, Vector3 direction)
        {
            Physics.Raycast(start, direction, out RaycastHit result, float.PositiveInfinity);

            return result;
        }

        internal static GameObject GetFurnitureRoot(GameObject gameObject)
        {
            if (gameObject.GetComponent<LODGroup>() != null)
            {
                return gameObject;
            }

            return GetFurnitureRoot(gameObject.transform.parent.gameObject);
        }
    }

    public class ChangeLayer : MonoBehaviour
    {
        public int Layer;
        public bool Recursively;

        public ChangeLayer(IntPtr intPtr) : base(intPtr) { }

        public void Start()
        {
            this.Invoke("SetLayer", 1);
        }

        internal void SetLayer()
        {
            vp_Layer.Set(this.gameObject, Layer, Recursively);
            Destroy(this);
        }
    }
}
