using System;

namespace LMU.VA.Ewing.Room117.Cameras.Args
{
	public class CameraManagementEventArgs : EventArgs
	{
		#region Properties

		public uint LastUiIndex1Based { get; set; }
		public bool OnlyWhenCombined { get; set; }
		public string CameraName { get; set; }
		public ICamera SelectedCamera { get; set; }

		#endregion Properties
	}
}
