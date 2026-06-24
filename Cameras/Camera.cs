
namespace LMU.VA.Ewing.Room117.Cameras
{
	public class Camera
	{
		protected bool HasTracking;
		protected bool PowerFeedback;
		protected bool TrackingFeedback;
		
		public uint DeviceIndex { get; set; }
		public int MaxPreset { get; protected set; }
		public bool SupportsTracking => HasTracking;
		
		protected Camera()
		{
		}
	}
}