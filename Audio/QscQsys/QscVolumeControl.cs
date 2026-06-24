using System;
using System.Timers;
using Qsc.Qsys.Crestron;

namespace LMU.VA.Ewing.Room117.Audio.QscQsys
{
    public class QscVolumeControl : VolumeControl, IVolumeControl
	{
		#region Fields

		private const int IncrementStep = 3000;
		private readonly QsysGainComponent _gainComponent;
		private readonly Timer _volumeDownTimer;
		private readonly Timer _volumeUpTimer;
		private bool _muteFeedback;
		private ushort _volumeFeedback;

		#endregion Fields

		#region Constructors

		public QscVolumeControl(string namedComponent, QscDsp paramDsp) : base()
		{
			_gainComponent = new QsysGainComponent();
			_gainComponent.Initialize(paramDsp.CoreId, namedComponent);

			_gainComponent.OnMuteEvent += GainComponent_OnMuteEvent;
			_gainComponent.OnGainEvent += GainComponent_OnVolumeEvent;

			_volumeUpTimer = new Timer(10)
			{
				AutoReset = true
			};
			_volumeUpTimer.Elapsed += VolumeUpRepeat;

			_volumeDownTimer = new Timer(10)
			{
				AutoReset = true
			};
			_volumeDownTimer.Elapsed += VolumeDownRepeat;
			
		}

		#endregion Constructors

		#region Events

		public event EventHandler<bool> MuteEvent;

		public event EventHandler<ushort> VolumeEvent;

		#endregion Events

		#region Properties

		public bool Mute
		{
			get => _muteFeedback;
			set => _gainComponent.SetBoolean(value ? 1 : 0, QsysGainComponent.eControlName.Mute);
		}

		public ushort Volume
		{
			get => _volumeFeedback;
			set => _gainComponent.SetInteger(value, 0, QsysGainComponent.eControlName.Gain);
		}

		#endregion Properties

		#region Methods

		public void VolumeDown(bool paramPress)
		{
			switch (paramPress)
			{
				case true:
					_volumeDownTimer.Start();
					break;

				case false:
					_volumeDownTimer.Stop();
					break;
			}
		}

		public void VolumeUp(bool paramPress)
		{
			switch (paramPress)
			{
				case true:
					_volumeUpTimer.Start();
					break;

				case false:
					_volumeUpTimer.Stop();
					break;
			}
		}

		private void GainComponent_OnMuteEvent(bool value, string name)
		{
			//CtiLogger.Debug($"Mute Feedback Event: {name} = {value}");
			_muteFeedback = value;
			MuteEvent?.Invoke(name, value);
		}

		private void GainComponent_OnVolumeEvent(ushort value, string name)
		{
			//CtiLogger.Debug($"Gain Feedback Event: {name} = {value}");
			_volumeFeedback = value;
			VolumeEvent?.Invoke(name, value);
		}

		private void VolumeDownRepeat(Object source, ElapsedEventArgs e)
		{
			if (_volumeFeedback - IncrementStep < 0)
			{
				_gainComponent.SetInteger(0, 1, QsysGainComponent.eControlName.Gain);
			}
			else
			{
				_gainComponent.SetInteger((ushort)(Volume - IncrementStep), 1, QsysGainComponent.eControlName.Gain);
			}
		}

		private void VolumeUpRepeat(Object source, ElapsedEventArgs e)
		{
			if (_volumeFeedback + IncrementStep > 65535)
			{
				_gainComponent.SetInteger(65535, 1, QsysGainComponent.eControlName.Gain);
			}
			else
			{
				_gainComponent.SetInteger((ushort)(Volume + IncrementStep), 1, QsysGainComponent.eControlName.Gain);
			}
		}

		#endregion Methods
	}
}