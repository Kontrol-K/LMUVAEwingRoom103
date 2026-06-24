
using System;
using Independentsoft.Exchange;

namespace LMU.VA.Ewing.Room117.Audio
{
	public interface INamedControl
	{
	#region Events

	event EventHandler<bool> BoolEvent;
	event EventHandler<ushort> IntEvent;

	#endregion Events

	#region Properties

	bool BoolValue { get; set; }
	int IntValue { get; set; }

	#endregion Properties
	}
}