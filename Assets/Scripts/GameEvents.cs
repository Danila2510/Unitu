using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public interface IMessage { string message { get; } }
    public class KeyPointEvent : IMessage
    {
        public string keyName { get; set; }
        public bool isInTime { get; set; }
        public string message { get; set; }

    }

    public class GateEvent : IMessage 
    {
        public string gateName { get; set; }
        public string message { get; set; }
    }

    public class BatteryEvent : IMessage
    {
        public string batteryName { get; set; }
        public float batteryLevel { get; set; }
        public string message { get; set; }

    }
}
