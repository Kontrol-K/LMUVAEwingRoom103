using System;

namespace LMU.VA.Ewing.Room117.Audio
{
	public interface IVolumeWithHold
	{
		#region Events

		event EventHandler<ushort> VolumeEvent;

		#endregion Events

		#region Properties

		ushort Volume { get; set; }

		#endregion Properties

		#region Methods

		void VolumeDown(bool paramPress);

		void VolumeUp(bool paramPress);

		#endregion Methods
	}
}