using System;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro.CrestronThread;

namespace LMU.VA.Ewing.Room103
{
    public class Shutdown
    {
        const int LastCalledDefault = 28;
        private readonly ControlSystem _controlSystem;
        private CTimer _timer;
        private readonly Hour _early = new Hour(23);
        private readonly Hour _late = new Hour(0);
        private int _lastCalled = LastCalledDefault;

        private class Hour
        {
            private int _val;

            public Hour(int val)
            {
                Val = val;
            }

            public int Val
            {
                set
                {
                    if (value < 0)
                    {
                        _val = 23;
                    }
                    else if (value > 23)
                    {
                        _val = 0;
                    }
                    else
                    {
                        _val = value;
                    }
                }
                get => _val;
            }

            public ushort Clock
            {
                get
                {
                    switch (_val)
                    {
                        case 0: 
                            return 12;
                        case 12:
                            return 12; 
                        default:
                        {
                            if (_val > 12)
                                return (ushort)(_val - 12);
                            else
                                return (ushort)_val;
                        }
                    }
                }
            }

            public string AmPm
            {
                get
                {
                    switch (_val)
                    {
                        case 0:
                            return "AM";
                        case 12:
                            return "PM";
                        default:
                        {
                            return _val > 12 ? "PM" : "AM";
                        }
                    } 
                }
            }
        }
        
        public Shutdown(ControlSystem controlSystem)
        {
            _controlSystem = controlSystem;
            _timer = new CTimer(Check, 0, 10000, 60000);
        }

        private void Check(object userObject)
        {
            var curHour = DateTime.Now.Hour;

            if (_lastCalled != LastCalledDefault)
            {
                if (_lastCalled != curHour)
                {
                    _lastCalled = LastCalledDefault;
                }
            }

            if (!_controlSystem.SystemPower)
            {
                return;
            }

            if (curHour == _early.Val && _lastCalled != curHour)
            {
                //Logger.DebugWriteLine("Call Early Shutdown");
                _controlSystem.ShutdownThread = new Thread(this.Count, 0);
                _lastCalled = curHour;
            }

            if (curHour == _late.Val && _lastCalled != curHour)
            {
                //Logger.DebugWriteLine("Call Late Shutdown");
                _controlSystem.ShutdownThread = new Thread(this.Count, 0);
                _lastCalled = curHour;
            }
        }

        private object Count(object time)
        {
            Hour current = new Hour(DateTime.Now.Hour);

            _controlSystem.Panel.BoolFeedBack(74, true);
            _controlSystem.Panel.AnaFeedBack(48, current.Clock);
            _controlSystem.Panel.TxtFeedBack(50, current.AmPm);
            
            for (int i1 = 90; i1 > -1; i1--)
            {
                _controlSystem.Panel.TxtFeedBack(96, i1.ToString());
                Thread.Sleep(1000);
            }

            _controlSystem.Panel.BoolFeedBack(74, false);
            _controlSystem.SystemOff();
            return new object();
        }

        internal void EarlySet(int change)
        {
            _early.Val += change;
            
            _controlSystem.Panel.TxtFeedBack(43, _early.Val.ToString());
            _controlSystem.Panel.TxtFeedBack(41, _early.AmPm);
        }

        internal void LateSet(int change)
        {
            _late.Val += change;
           
            _controlSystem.Panel.TxtFeedBack(44, _late.Clock.ToString());
            _controlSystem.Panel.TxtFeedBack(42, _late.AmPm);
        }
    }
}