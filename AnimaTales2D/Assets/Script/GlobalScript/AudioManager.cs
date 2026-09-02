using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer; 
    [SerializeField] private AudioMixerGroup bgmGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    private const float MinDecibel = -80f;
    private const string BgmVolumeParam = "BGMVolume";
    private const string SfxVolumeParam = "SFXVolume";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
        bgmSource.outputAudioMixerGroup = bgmGroup;
        bgmSource.loop = true;
        sfxSource.outputAudioMixerGroup = sfxGroup;
        sfxSource.loop = false;
        // PreferenceData에서 이벤트 구독
        PreferenceData.AddListenerBgmVolumeChangeEvent(ApplyBgmVolume);
        PreferenceData.AddListenerSfxVolumeChangeEvent(ApplySfxVolume);

        // 게임 시작 시 현재 설정 반영
        ApplyAllVolume();
    }

    private void OnDestroy()
    {
        PreferenceData.RemoveListenerBgmVolumeChangeEvent(ApplyBgmVolume);
        PreferenceData.RemoveListenerSfxVolumeChangeEvent(ApplySfxVolume);
    }

    private void ApplyAllVolume()
    {
        ApplyBgmVolume();
        ApplySfxVolume();
    }

    private void ApplyBgmVolume()
    {
        audioMixer.SetFloat(BgmVolumeParam, ToDecibel(PreferenceData.BgmVolume));
    }

    private void ApplySfxVolume()
    {
        audioMixer.SetFloat(SfxVolumeParam, ToDecibel(PreferenceData.SfxVolume));
    }

    private static float ToDecibel(int channelVolumePercent)
    {
        float channel = channelVolumePercent / 100f;
        float master = PreferenceData.MasterVolume / 100f;
        float linear = channel * master;
        return linear <= 0f ? MinDecibel : Mathf.Log10(linear) * 20f;
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip) return;
        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }
}
