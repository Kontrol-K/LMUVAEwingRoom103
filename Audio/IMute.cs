using System;

namespace LMU.VA.Ewing.Room117.Audio
{
	public interface IMute
	{
		#region Events

		event EventHandler<bool> MuteEvent;

		#endregion Events

		#region Properties

		bool Mute { get; set; }

		#endregion Properties

	}
}