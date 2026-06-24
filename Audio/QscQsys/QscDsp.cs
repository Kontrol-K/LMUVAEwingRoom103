using System;
using Crestron.SimplSharp;
using Qsc.Qsys.Crestron;
using LMU.VA.Ewing.Room117;

namespace LMU.VA.Ewing.Room117.Audio.QscQsys
{
    public class QscDsp : DspControl
	{
		#region Fields

		/// <summary>
		/// Gets or sets CoreId of this Dsp
		/// </summary>
		internal readonly string CoreId;

		private const int IpPort = 1710;
		private readonly string _password = "1234";
		private readonly string _username = "crestron";

		#endregion Fields

		#region Constructors

		public QscDsp()
		{
			var ipAddress = RoomInformation.CoreIpAddress;
			CoreId = "Primary";

			Core = new QsysCore
			{
				Debug = 0,
				OnIsConnected = CoreConnected,
				OnIsRegistered = CoreRegistered,
				OnNewCoreStatus = CoreStatus
			};

			
			if (RoomInformation.LevelControls?.Count > 0)
			{
				foreach (var volumeControlPoint in RoomInformation.LevelControls)
				{
					VolumeControls.Add(volumeControlPoint, new QscVolumeControl(volumeControlPoint, this));
				}
			}

			if (RoomInformation.NamedControls?.Count > 0)
			{
				foreach (var controlPoint in RoomInformation.NamedControls)
				{
					NamedControls.Add(controlPoint, new QscNamedcontrol(controlPoint, this));
				}
			}

			Core.Initialize(CoreId, ipAddress, IpPort, _username, _password);

			CrestronConsole.AddNewConsoleCommand(DebugConsoleCommand, "QSYSDebug", "Sets the Q-Sys Debug mode in the program", ConsoleAccessLevelEnum.AccessOperator);
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Hardware interface for QsysCore DSP
		/// </summary>
		private QsysCore Core { get; set; }

		#endregion Properties

		#region Methods

		public override void Dispose()
		{
			Core.Dispose();
		}

		private void CoreConnected(ushort status) => CrestronConsole.PrintLine( $"Core Connected Status: {status}");
		private void CoreRegistered(ushort status) => CrestronConsole.PrintLine($"Core Register Status: {status}");

		private void CoreStatus(SimplSharpString str, ushort val1, ushort val2) => CrestronConsole.PrintLine($"Core String: {str}. Core Value 1: {val1}. Core Value 2: {val2}.");
		
		private void DebugConsoleCommand(string cmdParameters)
		{
			if (Core is Object)
			{
				if (string.IsNullOrEmpty(cmdParameters))
				{
					CrestronConsole.ConsoleCommandResponse($"Debug Mode: {Core.Debug}");
				}
				else if (ushort.Parse(cmdParameters) < 3)
				{
					Core.Debug = (ushort.Parse(cmdParameters));
					CrestronConsole.ConsoleCommandResponse($"Debug Mode: {Core.Debug}");
				}
				else
				{
					CrestronConsole.ConsoleCommandResponse($"Invalid Debug Mode. Please enter 0, 1 or 2");
				}
			}
			else
			{
				CrestronConsole.ConsoleCommandResponse($"The Core is not an Object. Seek Help!");
			}
		}

		#endregion Methods
	}
}