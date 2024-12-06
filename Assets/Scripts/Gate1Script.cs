using UnityEngine;

public class Gate1Script : MonoBehaviour
{
    [SerializeField] private string key;
    [SerializeField] private string author;
    private AudioSource closedSound;
    private AudioSource openingSound;
    private float openingTime = 3.0f;
    private float timeout = 0f;
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        closedSound = audioSources[0];
        openingSound = audioSources[1];
        GameState.AddChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.effectsVolume));

        GameState.AddChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.isSoundsMuted));
    }

    void Update()
    {
        if (timeout > 0f)
        {
            timeout -= Time.deltaTime;
            transform.Translate(0, 0, Time.deltaTime / openingTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Character")
        {
            if (GameState.collectedItems.ContainsKey(key))
            {
                if (timeout == 0f)
                {
                    GameState.TriggerEvent("Gate",
                                        new GameEvents.GateEvent { message = "Двері відчиняються" });
                    timeout = openingTime;
                    openingSound.Play();
                }
            }
            else
            {
                GameState.TriggerEvent("Gate", new GameEvents.GateEvent { message = $"{author} : Необхідний ключ №{key}. Продовжуйте пошуки!" });
                closedSound.Play();
            }
        }
    }
    private void OnSoundsVolumeChanged(string name)
    {
        closedSound.volume =
            openingSound.volume = GameState.isSoundsMuted
                ? 0.0f
                : GameState.musicVolume;
    }

    private void OnDestroy()
    {
        GameState.RemoveChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.effectsVolume));
        GameState.RemoveChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.isSoundsMuted));
    }
}
