using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.ViewModels.Properties;

namespace Shanemat.DotNetUtils.Wpf.Tests.ViewModels.Properties.SimpleViewModelProperty;

/// <summary>
/// Contains tests for <see cref="SimpleViewModelProperty{T}.DisplayName"/> property
/// </summary>
internal sealed class DisplayNameTests
{
	#region Sources

	private static IEnumerable<Func<string>> DisplayNameGetters => Sources.DisplayNameGetters;

	#endregion

	#region Tests

	[Test]
	public void ShouldReturnEmptyStringByDefault()
	{
		var property = new SimpleViewModelProperty<int>();

		Assert.That( property.DisplayName, Is.EqualTo( string.Empty ) );
	}

	[Test]
	[TestCaseSource( nameof( DisplayNameGetters ) )]
	public void ShouldUseTheGivenGetter( Func<string> displayNameGetter )
	{
		var property = new SimpleViewModelProperty<int>
		{
			DisplayNameGetter = displayNameGetter,
		};

		Assert.That( property.DisplayName, Is.EqualTo( displayNameGetter.Invoke() ) );
	}

	[Test]
	public void ShouldNotNeedlesslyQueryTheValue()
	{
		const string result = "Success";

		var hasBeenCalled = false;

		var property = new SimpleViewModelProperty<int>
		{
			DisplayNameGetter = GetDisplayName,
		};

		Assert.That( property.DisplayName, Is.EqualTo( result ) );
		Assert.That( property.DisplayName, Is.EqualTo( result ) );

		string GetDisplayName()
		{
			if( !hasBeenCalled )
				return result;

			hasBeenCalled = true;

			throw new InvalidOperationException();
		}
	}

	#endregion
}
