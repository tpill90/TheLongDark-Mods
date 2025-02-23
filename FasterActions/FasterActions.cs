namespace FasterActions
{
    internal class FasterActions : MelonMod
    {
        internal static FasterActionSettings Settings;

        public override void OnInitializeMelon()
        {
            Debug.Log($"[{Info.Name}] version {Info.Version} loaded!");

            Settings = new FasterActionSettings();
            Settings.AddToModSettings("Faster Actions");
        }
    }
}