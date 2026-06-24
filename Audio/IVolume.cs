using System;

namespace CTIDeploy.Devices
{
	public interface IVolume
	{
		#region Events

		event EventHandler<ushort> VolumeEvent;

		#endregion Events

		#region Properties

		ushort Volume { get; set; }

		#endregion Properties

		#region Methods

		void VolumeDown();

		void VolumeUp();

		#endregion Methods
	}
}