using System.Collections.Generic;

namespace LMU.VA.Ewing.Room103
{
    public static class RoomInformation
    {
        public const string CoreIpAddress = "172.30.0.105";
        public const string RoomName = "Room 103";
        
        public const int NumberOfDisplays = 6;
        public const int NumberOfCameras = 2;
        public const int NumberOfWallPlates = 4;
        
        public const int ZoomShareValue = 7;
        
        public const string SgdFile = "Assets/LMU_DVTC_Microscope_Room103_v2_0_1.sgd";

        public const int ZoomInput = 12;
        public const int AirMediaInput = 4;
        public const int MicroscopeInput = 1;
        public const int FirstWallPlateInput = 5;
        public const int RoomPcInput = 2;
        public const int LaptopInput = 3;
        public const int FirstCameraInput = 9;

        
        public static readonly List<string> Outputs = new List<string>()
        {
            "None",
            "Samsung1",
            "Samsung2",
            "Samsung3",
            "Samsung4",
            "Samsung5",
            "Samsung6",
            "Zoom1",
            "Audio"
        };
        
        public static readonly List<string> LevelControls = new List<string>()
        {
            "Mic1",
            "Mic2",
            "Program",
            "Ceiling"
        };
        
        public static readonly List<string> NamedControls = new List<string>()
        {
            "Audio",
            "ZoomShare","Zoom1","MediaSite1",
            "Samsung1","Samsung2","Samsung3","Samsung4","Samsung5","Samsung6",
            "Display1", "Display2", "Display3", "Display4", "Display5", "Display6",
            "SystemOn", "SystemOff"
        };

        public static readonly List<string> Inputs = new List<string>()
        { 
            "None", 
            "Microscope", 
            "PC", 
            "Laptop", 
            "Air Media", 
            "Wallplate 1", 
            "Wallplate 2",
            "Wallplate 3", 
            "Wallplate 4", 
            "Audience Camera", 
            "Presenter Camera", 
            "MediaSite", 
            "Zoom",
            "OFF"
        };
    }
}