using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharp;
using LMU.VA.Ewing.Room117;
using LMU.VA.Ewing.Room117.Cameras.Args;
using LMU.VA.Ewing.Room117.Cameras.CameraSwitching;

namespace LMU.VA.Ewing.Room117.Cameras
{
    public static class CameraManager
    {
        #region Fields

        private static readonly ICameraSwitching CameraSwitching;

        private static ICamera _selectedPrevious;

        #endregion Fields

        #region Constructors

        static CameraManager()
        {
            try
            {
                Cameras = new Dictionary<string, ICamera>();

                for (ushort i = 1; i <= RoomInformation.NumberOfCameras; i++)
                {
                    Cameras.Add($"Camera{i}", CameraFactory(i, $"Camera{i}"));
                }

                SetSelected(Cameras.Values.First());
                
                CameraSwitching = CameraSwitcherFactory();
            }
            catch (Exception ex)
            {
                ErrorLog.Exception("Error in constructor", ex);
            }
        }

        private static ICameraSwitching CameraSwitcherFactory()
        {
            return new QscQsysCameraSwitching("Primary", "CameraRouter", 1);
        }

        #endregion Constructors

        #region Events

        public static event EventHandler<CameraManagementEventArgs> OnSelectedCamera;

        #endregion Events

        #region Properties

        private static Dictionary<string, ICamera> Cameras { get; set; }

        public static int Count => Cameras.Count;

        public static ICamera Selected { get; private set; }

        #endregion Properties

        #region Methods

        private static ICamera CameraFactory(ushort i, string name)
        {
            return new CameraTypes.QscQsysCamera(name, i);
        }

        public static void SetSelected(uint paramUiIndex1Based)
        {
            SetSelected(Cameras.Values.First(x => x.DeviceIndex == paramUiIndex1Based));
        }
        private static void SetSelected(ICamera paramCamera)
        {
            _selectedPrevious = Selected;
            Selected = paramCamera;

            CameraManagementEventArgs args = new CameraManagementEventArgs
            {
                SelectedCamera = Selected
            };

            if (_selectedPrevious != null)
            {
                args.LastUiIndex1Based = _selectedPrevious.DeviceIndex;
            }

            if (CameraSwitching != null)
            {
                CameraSwitching.ActiveCamera = Selected.DeviceIndex;
            }

            OnSelectedCamera?.Invoke(null, args);

            foreach (var camera in Cameras)
            {
                camera.Value.Power = true;
            }

            //CtiLogger.Debug($"{Selected.Label} has been selected.");
        }

        #endregion Methods
    }
}