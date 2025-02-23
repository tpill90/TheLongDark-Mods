using ModSettings;

namespace SonicMode;

internal sealed class Settings : JsonModSettings
{
	public static Settings Instance { get; } = new();

	[Name("Crouch Speed Multiplier")]
	[Description("Scales the speed at which the player moves when crouched. (Default = 1)")]
	[Slider(0.05f, 10f, 200, NumberFormat = "{0:F2}")]
	public float crouchSpeedMultiplier = 1f;

	[Name("Walk Speed Multiplier")]
	[Description("Scales the speed at which the player moves when walking normally. (Default = 1)")]
	[Slider(0.05f, 10f, 200, NumberFormat = "{0:F2}")]
	public float walkSpeedMultiplier = 1f;

	[Name("Sprint Speed Multiplier")]
	[Description("Scales the speed at which the player moves when sprinting. (Default = 1)")]
	[Slider(0.1f, 30f, 300)]
	public float sprintSpeedMultiplier = 1f;

	[Name("Recharge Multiplier")]
	[Description("Scales the speed at which the stamina bar refills. (Default = 1)")]
	[Slider(0.1f, 20f, 200)]
	public float rechargeScalar = 1f;

	[Name("Seconds Before Recovery Multiplier")]
	[Description("Scales the number of seconds before the stamina bar starts refilling. (Default = 1)")]
	[Slider(0f, 2f, 201, NumberFormat = "{0:F2}")]
	public float secondsScalar = 1f;
}