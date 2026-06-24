using Qsc.Qsys.Crestron;
using System;
using Crestron.SimplSharp;

namespace LMU.VA.Ewing.Room117.Cameras.CameraTypes
{
    public class QscQsysCamera : Camera, ICamera
	{
		#region Fields
		
		private readonly QsysCamera _camera;
		private readonly QsysSnapshot _presets;

		#endregion Fields

		#region Constructors

		public QscQsysCamera(string name, uint index) : base()
		{
			try
			{
				_camera = new QsysCamera();
				_presets = new QsysSnapshot();
				DeviceIndex = index;

				_camera.Initialize("Primary", name);
				_presets.Initialize("Primary", $"{name}-Presets");

				HasTracking = false;
			}
			catch (Exception ex)
			{
				ErrorLog.Exception("Exception in Constructor.", ex);
				//CtiLogger.Error("Exception in Constructor.", ex);
			}
		}

		#endregion Constructors

		#region Properties

		public bool Power
		{
			get => throw new NotSupportedException();
			set
			{
				if (value)
				{
					PowerEvent?.Invoke(this, true);
				}
			}
		}

		public bool Tracking
		{
			get => TrackingFeedback;
			set => throw new NotSupportedException();
		}

		public event EventHandler<bool> PowerEvent;

		#endregion Properties

		#region Methods

		public void FocusIn(bool state) => _camera.FocusIn(state);
		public void FocusOut(bool state) => _camera.FocusOut(state);
		public void Home() => _camera.RecallHome();
		public void MoveDown(bool state) => _camera.TiltDown(state);
		public void MoveLeft(bool state) => _camera.PanLeft(state);
		public void MoveRight(bool state) => _camera.PanRight(state);
		public void MoveUp(bool state) => _camera.TiltUp(state);
		public void PresetRecall(int presetIndex) => _presets.LoadSnapshot((ushort)presetIndex);
		public void PresetStore(int presetIndex) => _presets.SaveSnapshot((ushort)presetIndex);
		public void ZoomIn(bool state) => _camera.ZoomIn(state);
		public void ZoomOut(bool state) => _camera.ZoomOut(state);
		
		#endregion Methods
	}
}