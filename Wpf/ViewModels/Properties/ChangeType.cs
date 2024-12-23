namespace Shanemat.DotNetUtils.Wpf.ViewModels.Properties;

/// <summary>
/// Represents a type of change of a view model property
/// </summary>
[Flags]
public enum ChangeType
{
	None = 0x000,
	ApplicableStatus = 0x0001,
	ReadOnlyStatus = 0x0010,
	DisplayName = 0x0100,
	Value = 0x1000,
	All = ~None,
}
