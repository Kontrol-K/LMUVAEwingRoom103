using System;

namespace LMU.VA.Ewing.Room117.Cameras
{
	public interface IPower
	{
		#region Events

		event EventHandler<bool> PowerEvent;

		#endregion Events

		#region Properties

		bool Power { get; set; }

		#endregion Properties


	}
}