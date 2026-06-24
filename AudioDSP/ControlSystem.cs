// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlSystem.cs" company="CTI">
//   CPR
// </copyright>
// <summary>
//   Defines the ControlSystem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using Base_Touch_Panel;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.CrestronThread;
using Crestron.SimplSharpPro.UI;
using LMU.VA.Ewing.Room103.Audio;
using LMU.VA.Ewing.Room103.Audio.QscQsys;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LMU.VA.Ewing.Room103
{
    /// <summary>
    /// The control system.
    /// </summary>
    public partial class ControlSystem : CrestronControlSystem
    {
        /// <summary>
        /// The panel.
        /// </summary>
        internal BasePanel Panel;

        /// <summary>
        /// The touch panel.
        /// </summary>
        private Ts1070 _tp;

        /// <summary>
        /// The advanced mode status.
        /// </summary>
        private bool _advancedMode;

        /// <summary>
        /// The selected input.
        /// </summary>
        private int _selectedInput;

        /// <summary>
        /// The on call.
        /// </summary>
        private bool _onCall;

        /// <summary>
        /// The counting.
        /// </summary>
        private bool _counting;

        internal bool SystemPower;

        internal Thread ShutdownThread;
        
        private Shutdown _shutdown;

        /// <summary>
        /// ControlSystem Constructor. Starting point for the SIMPL#Pro program.
        /// Use the constructor to:
        /// * Initialize the maximum number of threads (max = 400)
        /// * Register devices
        /// * Register event handlers
        /// * Add Console Commands
        /// 
        /// Please be aware that the constructor needs to exit quickly; if it doesn't
        /// exit in time, the SIMPL#Pro program will exit.
        /// 
        /// You cannot send / receive data in the constructor
        /// </summary>
        public ControlSystem()
            : base()
        {
            try
            {
                Thread.MaxNumberOfUserThreads = 20;
                //var currentDomain = AppDomain.CurrentDomain;
                //currentDomain.UnhandledException += this.MyHandler;
            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in the constructor: {0}", e.Message);

            }
        }
        /// <summary>
        /// InitializeSystem - this method gets called after the constructor 
        /// has finished. 
        /// 
        /// Use InitializeSystem to:
        /// * Start threads
        /// * Configure ports, such as serial and verisports
        /// * Start and initialize socket connections
        /// Send initial device configurations
        /// 
        /// Please be aware that InitializeSystem needs to exit quickly also; 
        /// if it doesn't exit in time, the SIMPL#Pro program will exit.
        /// </summary>

       private CTimer _startupTimer;

        public override void InitializeSystem()
        {
            try
            {
                ErrorLog.Notice("LMU Room: Starting initialization timer Kalonji Nicholson of ISS (678) 230-7112 knicholson@kontrolk.com https://issavinc.com.");

                _startupTimer = new CTimer(StartupTimerCallback,null,30000,Timeout.Infinite);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in InitializeSystem: {0}",e.Message);
            }
        }

        private void StartupTimerCallback(object o)
        {
            _startupTimer?.Dispose();
            _startupTimer = null;

            SystemSetup();
        }

        private object SystemSetup()
        {
            try
            {
                ErrorLog.Notice("Step 1");
                _shutdown = new Shutdown(this);
                ErrorLog.Notice("Step 2");
                this.Panel = new BasePanel(new Ts1070(03, this), RoomInformation.SgdFile);
                ErrorLog.Notice("Step 3");
                this._tp = this.Panel.Panel as Ts1070;
                ErrorLog.Notice("Step 4");
                ErrorLog.Notice("Initializing Audio Manager");
                var dsp = AudioManager.Dsp;
                ErrorLog.Notice("Audio Manager Initialized");
                if (this._tp != null)
                {
                    ErrorLog.Notice("Step 5");
                    this._tp.ExtenderApplicationControlReservedSigs.Use();
                    ErrorLog.Notice("Step 6");
                    this._tp.ExtenderApplicationControlReservedSigs.HideOpenedApplication();
                    ErrorLog.Notice("Step 7");
                    this._tp.ExtenderZoomRoomAppReservedSigs.Use();
                    ErrorLog.Notice("Step 8");
                    this._tp.ExtenderZoomRoomAppReservedSigs.DeviceExtenderSigChange +=
                        this.ExtenderZoomRoomAppReservedSigs_DeviceExtenderSigChange;
                    ErrorLog.Notice("Step 9");
                }
                ErrorLog.Notice("Step 10");
                this.SetUpPanel();
                ErrorLog.Notice("Step 11");
            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in InitializeSystem: {0}", e.ToString());
                //ErrorLog.Error("Error in InitializeSystem: {0}", e.Message);
            }
            CrestronConsole.PrintLine(RoomInformation.RoomName + " -- System Setup Complete");
            return null;
        }

        private void SetRoute(int value, bool output = false)
        {
            const uint offset = 22;
            if (!this._advancedMode)
            {
                if (this._onCall)
                {
                    if (value == RoomInformation.ZoomInput)
                    {
                        return;
                    }
                    AudioManager.Dsp.NamedControls["Zoom1"].IntValue = value;
                    AudioManager.Dsp.NamedControls["ZoomShare"].IntValue = 1;
                    return;
                }

                this.RouteAll(value);
                return;
            }
            if (output)
            {
                if (RoomInformation.Outputs[value].Contains("Zoom") && _selectedInput == RoomInformation.ZoomInput)
                {
                    return;
                }
                AudioManager.Dsp.NamedControls[RoomInformation.Outputs[value]].IntValue = _selectedInput;

                var text = value + offset;
                this.Panel.TxtFeedBack((ushort)text, RoomInformation.Inputs[this._selectedInput]);

                switch (value)
                {
                    case RoomInformation.ZoomShareValue:
                        AudioManager.Dsp.NamedControls["ZoomShare"].IntValue = 1;
                        break;
                    case 1:
                        this.Panel.TxtFeedBack(2, RoomInformation.Inputs[this._selectedInput]);
                        AudioManager.Dsp.NamedControls["MediaSite1"].IntValue = _selectedInput;
                        break;
                }

                return;
            }
            this._selectedInput = value;
        }

        private void RouteAll(int value)
        {
            const uint offset = 22;

            foreach (var output in RoomInformation.Outputs.Where(output => output != "None"))
            {
                if (output.Contains("Zoom") && value == RoomInformation.ZoomInput)
                {
                    continue;
                }
                AudioManager.Dsp.NamedControls[output].IntValue = value;
            }

            for (ushort i = 0; i < RoomInformation.Outputs.Count; i++)
            {
                this.Panel.TxtFeedBack(offset + i, RoomInformation.Inputs[value]);
            }

            this.Panel.TxtFeedBack(2, RoomInformation.Inputs[value]);
            AudioManager.Dsp.NamedControls["MediaSite1"].IntValue = value;
        }

        private void SystemOn()
        {
            this.Panel.BoolFeedBack(16, false);
            this.Panel.BoolFeedBack(11, true);
            this.Panel.BoolFeedBack(10, false);
            
            AudioManager.Dsp.NamedControls["SystemOn"].BoolValue = true;

            for (var i = 1; i <= RoomInformation.NumberOfDisplays; i++)
            {
                AudioManager.Dsp.NamedControls[$"Display{i}"].BoolValue = true;
            }
            
            SystemPower = true;
        }
        
        public void SystemOff()
        {
            SystemPower = false;
            
            this._tp.ExtenderZoomRoomAppReservedSigs.ZoomRoomEndCall();
            
            this.Panel.BoolFeedBack(11, false);
            this.Panel.BoolFeedBack(10, true);
            var workerThread = new Thread(this.CountDown, 20);
            this.Panel.BoolFeedBack(19, false);
            this.Panel.BoolFeedBack(80, true);
            this.Panel.BoolFeedBack(81, false);
            this.SubSelect(EPages.None);
            this.ButtonSelect(ESourceButton.None);
            
 
            AudioManager.Dsp.NamedControls["SystemOff"].BoolValue = true;
            AudioManager.Dsp.NamedControls["Zoom1"].IntValue = 0;

            foreach (var output in RoomInformation.Outputs.Where(output => output != "None"))
            {
                AudioManager.Dsp.NamedControls[output].IntValue = 1;
            }

            this._advancedMode = false;

            for (var i = 1; i <= RoomInformation.NumberOfDisplays; i++)
            {
                AudioManager.Dsp.NamedControls[$"Display{i}"].BoolValue = false;
            }
        }
    }
}