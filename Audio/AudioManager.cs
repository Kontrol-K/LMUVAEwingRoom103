using System;
using Crestron.SimplSharp;
using LMU.VA.Ewing.Room117.Audio.QscQsys;

namespace LMU.VA.Ewing.Room117.Audio
{
	public static class AudioManager
	{
		#region Constructors

		static AudioManager()
		{
			try
			{
				Dsp = new QscDsp();
			}
			catch (Exception ex)
			{
				ErrorLog.Error("Exception during construction", ex);
			}
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// DSP Class
		/// </summary>
		public static DspControl Dsp { get; private set; }

		#endregion Properties
	}
}