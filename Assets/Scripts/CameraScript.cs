using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private InputAction lookAction;
    private GameObject cameraPosition3;
    private GameObject character;
    private Vector3 c;
    private Vector3 cameraAngles, cameraAngles0;
    private bool isFpv;
    private float sensitivityH = 0.15f;
    private float sensitivityV = -0.08f;

    private float minDistance = 0.0f;
    private float maxDistance = 10.0f;

    private AudioSource dayMusic;
    private AudioSource nightMusic;

    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        character = GameObject.Find("Character");
        c = this.transform.position - character.transform.position;
        cameraPosition3 = GameObject.Find("CameraPosition");
        cameraAngles0 = cameraAngles = this.transform.eulerAngles;
        isFpv = true;

        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources == null || audioSources.Length != 2)
        {
            Debug.LogError("LightScript::Start audioSources error");
        }
        else
        {
            dayMusic = audioSources[0];
            nightMusic = audioSources[1];
        }

        SetLightMusic(true);

        GameState.AddChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.musicVolume));

        GameState.AddChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.isSoundsMuted));
    }

    void Update()
    {
        if (Time.timeScale == 0.0f) return;
        if (isFpv)
        {
            float wheel = Input.mouseScrollDelta.y;
            c *= 1 - wheel / 10.0f;

            float distance = c.magnitude;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            c = c.normalized * distance;

            if (distance < 1.0f)
            {
                c = c.normalized * minDistance;
            }

            GameState.isFpv = distance < 0.25f;

            Vector2 lookValue = lookAction.ReadValue<Vector2>();
            cameraAngles.x += lookValue.y * sensitivityV;
            cameraAngles.y += lookValue.x * sensitivityH;

            if (distance == minDistance)
            {
                cameraAngles.x = Mathf.Clamp(cameraAngles.x, -35f, 20f);
            }
            else
            {
                cameraAngles.x = Mathf.Clamp(cameraAngles.x, 35f, 75f);
            }

            this.transform.eulerAngles = cameraAngles;

            this.transform.position = character.transform.position +
                Quaternion.Euler(0, cameraAngles.y - cameraAngles0.y, 0) * c;
            if (Input.GetKeyDown(KeyCode.N))
            {
                SetLightMusic(GameState.isDay);
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isFpv)
            {
                this.transform.position = cameraPosition3.transform.position;
                this.transform.rotation = cameraPosition3.transform.rotation;
            }
            isFpv = !isFpv;
        }
    }

    private void OnSoundsVolumeChanged(string name)
    {
        dayMusic.volume =
            nightMusic.volume = GameState.isSoundsMuted
                ? 0.0f
                : GameState.musicVolume;
    }

    private void OnDestroy()
    {
        GameState.RemoveChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.musicVolume));
        GameState.RemoveChangeListener(
            OnSoundsVolumeChanged,
            nameof(GameState.musicVolume));
    }

    private void SetLightMusic(bool day)
    {
        if (day)
        {
            nightMusic.Stop();
            dayMusic.Play();
        }
        else
        {
            dayMusic.Stop();
            nightMusic.Play();
        }
    }
}
