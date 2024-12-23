using System.Windows;

namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels.Properties;

/// <summary>
/// Contains the source values to use in property view model tests
/// </summary>
internal static class Sources
{
	#region Properties

	internal static IEnumerable<int> Values => [-47, 0, 139];

	internal static IEnumerable<Func<bool>> BooleanGetters => [() => true, () => false];

	internal static IEnumerable<Func<string>> DisplayNameGetters => [() => string.Empty, () => "Foo", () => "BAR"];

	internal static IEnumerable<Visibility> Visibilities => Enum.GetValues<Visibility>();

	#endregion
}
