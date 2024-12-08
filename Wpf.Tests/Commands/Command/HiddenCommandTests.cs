using NUnit.Framework;
using Shanemat.DotNetUtils.Wpf.Commands;

namespace Shanemat.DotNetUtils.Wpf.Tests.Commands.Command;

/// <summary>
/// Contains tests for <see cref="Command.Hidden"/> property
/// </summary>
internal sealed class HiddenCommandTests
{
	#region Tests

	[Test]
	public void ShouldNotBeExecutable()
	{
		Assert.That( Wpf.Commands.Command.Hidden.CanExecute, Is.False );
	}

	#endregion
}
