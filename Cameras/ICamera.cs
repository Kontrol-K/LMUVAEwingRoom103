
namespace LMU.VA.Ewing.Room117.Cameras
{
	public interface ICamera : IPower
	{
		#region Properties

		/// <summary>
		/// Ch5 Index of Camera
		/// </summary>
		uint DeviceIndex { get; set; }

		/// <summary>
		/// Returns the maximum preset count the camera can hold
		/// </summary>
		int MaxPreset { get; }

		/// <summary>
		/// True if the camera supports tracking
		/// </summary>
		bool SupportsTracking { get; }

		bool Tracking { get; set; }

		#endregion Properties

		#region Methods

		void FocusIn(bool state);

		void FocusOut(bool state);

		void Home();

		void MoveDown(bool state);

		void MoveLeft(bool state);

		void MoveRight(bool state);

		void MoveUp(bool state);

		void PresetRecall(int presetIndex);

		void PresetStore(int presetIndex);

		void ZoomIn(bool state);

		void ZoomOut(bool state);

		#endregion Methods
	}
}