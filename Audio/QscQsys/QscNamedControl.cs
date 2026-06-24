using System;
using Crestron.SimplSharp;
using Qsc.Qsys.Crestron;

namespace LMU.VA.Ewing.Room117.Audio.QscQsys
{
    public class QscNamedcontrol : NamedControl, INamedControl
	{
		#region Fields

		private readonly QsysNamedControl _namedControl;
		private bool _boolFeedback;
		private ushort _intFeedback;

		#endregion Fields

		#region Constructors

		public QscNamedcontrol(string namedControl, QscDsp paramDsp) : base()
		{
			_namedControl = new QsysNamedControl();
			_namedControl.Initialize(paramDsp.CoreId, namedControl, 1);
			_namedControl.NewNamedControlChange += OnControlChange;
		}

		#endregion Constructors

		#region Events
		public event EventHandler<bool> BoolEvent;
		public event EventHandler<ushort> IntEvent;

		#endregion Events

		#region Properties

		public bool BoolValue
		{
			get => _boolFeedback;
			set => _namedControl.SetBoolean(value ? 1 : 0);
		}

		public int IntValue
		{
			get => _intFeedback;
			set => _namedControl.SetInteger(value, 0);
		}

		#endregion Properties

		#region Methods
		
		private void OnControlChange(ushort intdata, SimplSharpString stringdata, string cname)
		{
			_boolFeedback = Convert.ToBoolean(intdata);
			BoolEvent?.Invoke(cname, Convert.ToBoolean(intdata));
			_intFeedback = intdata;
			IntEvent?.Invoke(cname, intdata);
		}
	

		#endregion Methods



	}
}