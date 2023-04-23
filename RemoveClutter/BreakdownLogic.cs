namespace RemoveClutter
{
    public static class BreakdownLogic
    {
        // Makes it so that the object is able to be broken down.  Objects without a BreakDownDefinition won't show up under our cursor
        internal static void AddBreakDownComponent(GameObject gameObject, BreakDownDefinition objDef)
        {
            BreakDown breakDown = gameObject.AddComponent<BreakDown>();
            BreakDown.m_BreakDownObjects.Add(breakDown);

            Utils.SetLayer(gameObject, vp_Layer.InteractiveProp);

            breakDown.m_YieldObject = new GameObject[0];
            breakDown.m_YieldObjectUnits = new int[0];
            
            //Time to harvest
            breakDown.m_TimeCostHours = objDef.minutesToHarvest / 60;

            //Harvest sound
            if (objDef.sound.Trim() != "" && objDef.sound != null)
            {
                breakDown.m_BreakDownAudio = "Play_Harvesting" + objDef.sound;
            }
            else
            {
                breakDown.m_BreakDownAudio = "Play_HarvestingGeneric";
            }

            //Display name
            String rawName = objDef.filter.Replace("_", string.Empty);
            String[] objWords = Regex.Split(rawName, @"(?<!^)(?=[A-Z])");
            String objName = String.Join(" ", objWords);
            breakDown.m_LocalizedDisplayName = new LocalizedString() { m_LocalizationID = objName };
        }
    }
}
