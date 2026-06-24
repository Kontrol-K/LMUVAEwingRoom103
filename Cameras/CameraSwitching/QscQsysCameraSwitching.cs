using Qsc.Qsys.Crestron;
using System;
using Crestron.SimplSharp;

namespace LMU.VA.Ewing.Room117.Cameras.CameraSwitching
{
    public class QscQsysCameraSwitching : CameraSwitchingBase, ICameraSwitching
	{
		#region Fields
		private readonly QsysRouter _cameraSwitching;
		
		#endregion Fields

		#region Constructors

		public QscQsysCameraSwitching(string paramCoreId, string paramRouterName, int paramOutputIndex)
		{
			try
			{
				_cameraSwitching = new QsysRouter();
				_cameraSwitching.NewRouterInputChange += CameraInputChange;
				_cameraSwitching.Initialize(paramCoreId, paramRouterName, paramOutputIndex);
			}
			catch (Exception ex)
			{
				ErrorLog.Exception("Exception in Constructor.", ex);
				//CtiLogger.Error("Exception in Constructor.", ex);
			}
		}

		#endregion Constructors

		#region Properties

		public uint ActiveCamera
		{
			get => CameraFeedback;
			set => _cameraSwitching.InputSelect((int)value);
		}

		#endregion Properties
		
		#region Methods
		
		private void CameraInputChange(ushort input) => CameraFeedback = input;
		
		#endregion Methods
	}
}