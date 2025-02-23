// ReSharper disable FieldCanBeMadeReadOnly.Global - Will break how the settings work if you mark these fields as readonly or constants
// ReSharper disable ConvertToConstant.Global

namespace FasterActions
{
    internal class FasterActionSettings : JsonModSettings
    {
        #region Food + Drink

        [Name("Eating Speedup")]
        [Description("How much faster food will be consumed.  Higher is faster.  Game default: 1")]
        [Slider(1, 15f)]
        public float EatingSpeedup = 1;

        [Name("Drinking Speed")]
        [Description("The number of real world seconds it takes to drink.  Lower is faster.  Game default: 4s")]
        [Slider(1, 4f)]
        public float DrinkingSpeedSeconds = 4;

        [Name("Can Open Speed")]
        [Description("The number of real world seconds it takes to open a can.  Lower is faster.  Game default: 6s")]
        [Slider(0.1f, 6f)]
        public float CanOpeningSeconds = 6;

        [Name("Recipe Prep Speed")]
        [Description("The number of real world seconds it takes to prepare a recipe.  Lower is faster.  Game default: 5s")]
        [Slider(0.1f, 5f)]
        public float RecipePrepSpeed = 5;

        #endregion

        #region Campcraft

        [Name("Charcoal Mapping Speed")]
        [Description("The number of real world seconds it takes to survey.  Lower is faster.  Game default: 3s")]
        [Slider(0.1f, 3f)]
        public float CharcoalMappingSeconds = 3f;

        [Name("Fire Start Speedup")]
        [Description("How much faster fires will be started.  Higher is faster.  Game default: 1")]
        [Slider(1, 15f)]
        public float FireStartSpeedup = 1;

        #endregion

        #region Crafting + Repair

        [Name("Crafting Speed")]
        [Description("The number of real world seconds it takes to craft items.  Lower is faster.  Game default: 5s")]
        [Slider(0.1f, 5f)]
        public float CraftingTimeSeconds = 5;

        [Name("Sharpening Speed")]
        [Description("The number of real world seconds it takes to sharpen knives/axes.  Lower is faster.  Game default: 5s")]
        [Slider(0.1f, 5f)]
        public float SharpeningTimeSeconds = 5;

        [Name("Repair Speed")]
        [Description("The number of real world seconds it takes to repair items.  Lower is faster.  Game default: 5s")]
        [Slider(0.1f, 5f)]
        public float RepairTimeSeconds = 5;

        #endregion

        [Name("Harvesting Speed")]
        [Description("The number of real world seconds it takes to harvest/breakdown items like clothing.  Lower is faster.  Game default: 5s")]
        [Slider(0.1f, 5f)]
        public float HarvestTimeSeconds = 5;

        [Name("Breakdown Speed")]
        [Description("The number of real world seconds it takes to breakdown world items like furniture.  Lower is faster.  Game default: 3s")]
        [Slider(0.1f, 3f)]
        public float BreakdownTimeSeconds = 3;

    }

}