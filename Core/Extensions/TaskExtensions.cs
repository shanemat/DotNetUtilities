namespace Shanemat.DotNetUtils.Core.Extensions;

/// <summary>
/// Contains extension methods for <see cref="Task"/> class
/// </summary>
public static class TaskExtensions
{
	#region Methods

	/// <summary>
	/// Allows for using the task in a synchronous context
	/// </summary>
	/// <param name="task">The task to be executed</param>
	/// <param name="continueOnCapturedContext">A value indicating whether an attempt to continue on captured context should be made</param>
	/// <param name="onExceptionCaught">The action to execute if an exception occurs during execution of this task</param>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Major Bug", "S3168:\"async\" methods should not return \"void\"",
		Justification = "Here it is necessary to implement the \"Fire and Forget\" task handling" )]
	public static async void FireAndForget( this Task? task, bool continueOnCapturedContext = false, Action<Exception>? onExceptionCaught = null )
	{
		if( task is null )
			return;

		try
		{
			await task.ConfigureAwait( continueOnCapturedContext );
		}
		catch( Exception exception )
		{
			onExceptionCaught?.Invoke( exception );
		}
	}

	#endregion
}
