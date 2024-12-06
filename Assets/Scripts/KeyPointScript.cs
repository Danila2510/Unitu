using UnityEngine;

public class KeyPointScript : MonoBehaviour
{
    [SerializeField]
    private string keyName = "1";

    [SerializeField]
    private float timeout = 5.0f;
    private float timeLeft;

    public float part => timeLeft / timeout;

    void Start()
    {
        timeLeft = timeout;
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) { timeLeft = 0; }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Character")
        {
            GameState.collectedItems.Add(keyName, part);
            GameState.TriggerEvent("KeyPoint", new GameEvents.KeyPointEvent
            {
                keyName = keyName,
                isInTime = part > 0,
                message = $"Найден ключ № '{keyName}' " + (part > 0 ? "вовремя" : "не вовремя")
            });
            Destroy(gameObject);
        }
    }
}
