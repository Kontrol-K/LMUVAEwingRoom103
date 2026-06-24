using System;
using System.Collections.Generic;
using System.Linq;

namespace LMU.VA.Ewing.Room117.Audio
{
	public abstract class DspControl : IDisposable
	{
		#region Constructors

		protected DspControl()
		{
			VolumeControls = new Dictionary<string, IVolumeControl>();
			NamedControls = new Dictionary<string, INamedControl>();
		}

		#endregion Constructors

		#region Properties

		public IVolumeControl PrivacyControl { get; internal set; }

		/// <summary>
		/// Dictionary of Control Points for Level Control
		/// </summary>
		public Dictionary<string, IVolumeControl> VolumeControls { get; private set; }
		public Dictionary<string, INamedControl> NamedControls { get; private set; }

		#endregion Properties

		#region Methods

		public abstract void Dispose();


		#endregion Methods
	}
}