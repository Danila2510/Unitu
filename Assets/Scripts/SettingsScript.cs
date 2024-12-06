using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    private GameObject content;

    #region soundEffectsSlider

    private Slider soundEffectsSlider;
    private void initSoundEffectsSlider()
    {
        soundEffectsSlider = transform
            .Find("Content/Sound/EffectsSlider")
            .GetComponent<Slider>();
        GameState.effectsVolume = soundEffectsSlider.value;
    }
    public void OnSoundEffectsChanged(System.Single value)
    {
        GameState.effectsVolume = value;
    }

    #endregion

    #region soundAmbientSlider

    private Slider soundAmbientSlider;
    private void initSoundAmbientSlider()
    {
        soundAmbientSlider = transform
            .Find("Content/Sound/AmbientSlider")
            .GetComponent<Slider>();
        GameState.ambientVolume = soundAmbientSlider.value;
    }
    public void OnSoundAmbientChanged(System.Single value)
    {
        GameState.ambientVolume = value;
    }

    #endregion

    #region soundMusicSlider

    private Slider soundMusicSlider;
    private void initSoundMusicSlider()
    {
        soundMusicSlider = transform
            .Find("Content/Sound/MusicSlider")
            .GetComponent<Slider>();
        GameState.musicVolume = soundMusicSlider.value;
    }
    public void OnSoundMusicChanged(System.Single value)
    {
        GameState.musicVolume = value;
    }

    #endregion

    #region soundsMuteToggle

    private Toggle soundsMuteToggle;
    private void initSoundsMuteToggle()
    {
        soundsMuteToggle = transform
            .Find("Content/Sound/MuteToggle")
            .GetComponent<Toggle>();
        GameState.isSoundsMuted = soundsMuteToggle.isOn;
    }
    public void OnSoundsMuteToggle(System.Boolean value)
    {
        GameState.isSoundsMuted = value;
    }

    #endregion

    void Start()
    {
        initSoundEffectsSlider();
        initSoundAmbientSlider();
        initSoundsMuteToggle();
        initSoundMusicSlider();

        content = transform
            .Find("Content")
            .gameObject;

        if (content.activeInHierarchy)
        {
            Time.timeScale = 0.0f;
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Time.timeScale = content.activeInHierarchy ? 1.0f : 0.0f;
            content.SetActive(!content.activeInHierarchy);
        }
    }
    
}
