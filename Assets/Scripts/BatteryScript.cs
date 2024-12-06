using UnityEngine;
using UnityEngine.InputSystem;

public class BatteryScript : MonoBehaviour
{
    [SerializeField]
    private float batteryLvl = 1;
    private void OnTriggerEnter(Collider other)
    {
        GameState.TriggerEvent("Battery", new GameEvents.BatteryEvent { message = $"Вы взяли батарейку. Текущий заряд фонарика: {batteryLvl}" });
        GameState.Collect(gameObject.name);
        Destroy(gameObject);
    }
}
