namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels;

/// <summary>
/// Contains the source values to use in view model tests
/// </summary>
internal static class Sources
{
	#region Properties

	internal static IEnumerable<int?> Values => [-57, 0, 49, null];

	internal static IEnumerable<string> PropertyNames => ["Property", "Value", string.Empty];

	#endregion
}
