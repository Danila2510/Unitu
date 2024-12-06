using System.Linq;
using UnityEngine;

public class LightScript : MonoBehaviour
{

    private Light[] dayLights;
    private Light[] nightLights;
    private AudioSource dayAmbient;
    private AudioSource nightAmbient;
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources == null || audioSources.Length != 2)
        {
            Debug.LogError("LightScript::Start audioSources error");
        }
        else
        {
            dayAmbient = audioSources[0];
            nightAmbient = audioSources[1];
        }

        dayLights = GameObject
            .FindGameObjectsWithTag("DayLight")
            .Select(x => x.GetComponent<Light>())
            .ToArray();

        nightLights = GameObject
            .FindGameObjectsWithTag("NightLight")
            .Select(x => x.GetComponent<Light>())
            .ToArray();

        GameState.isDay = true;
        SetLights(GameState.isDay);

        GameState.AddChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.ambientVolume));

        GameState.AddChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.isSoundsMuted));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameState.isDay = !GameState.isDay;
            SetLights(GameState.isDay);
        }
    }

    private void SetLights(bool day)
    {
        foreach (Light light in dayLights)
            light.enabled = day;

        foreach (Light light in nightLights)
            light.enabled = !day;
        if (day)
        {
            nightAmbient.Stop();
            dayAmbient.Play();
        }
        else
        {
            dayAmbient.Stop();
            nightAmbient.Play();
        }
    }

    private void OnSoundsVolumeChanged(string name)
    {
        dayAmbient.volume =
            nightAmbient.volume = GameState.isSoundsMuted
                ? 0.0f
                : GameState.ambientVolume;
    }
    private void OnDestroy()
    {
        GameState.RemoveChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.ambientVolume));
        GameState.RemoveChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.isSoundsMuted));
    }
}
