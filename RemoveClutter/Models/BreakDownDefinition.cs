namespace RemoveClutter.Models
{
    class BreakDownDefinition
    {
        /// <summary>
        /// Item name / identifier.  Must be an exact match
        /// https://github.com/ds5678/ModComponent/blob/master/docs/Item-Names.md
        /// </summary>
        //TODO rename
        public string filter = "";
        public string sound = "";
        public float minutesToHarvest = 1f;

        public override string ToString()
        {
            return filter;
        }
    }
}
