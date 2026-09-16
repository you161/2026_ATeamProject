using UnityEngine;

enum BGMType
{
    Title,
    Tutorial,
    Main,
    Result
}

public class BGMManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource bgmAudioSource=null;
    [SerializeField]
    private BGMType bgmType;
    [SerializeField]
    private AudioClip titleBGM = null;
    [SerializeField]
    private AudioClip tutorialBGM = null;
    [SerializeField]
    private AudioClip mainBGM = null;
    [SerializeField]
    private AudioClip resultBGM = null;
    private void Start()
    {
        switch(bgmType)
        {
            case BGMType.Title:
                PlayTitleBGM();
                break;
            case BGMType.Tutorial:
                PlayTutorialBGM();
                break;
            case BGMType.Main:
                PlayMainBGM();
                break;
            case BGMType.Result:
                PlayResultBGM();
                break;
        }
    }
    public void PlayTitleBGM()
    {
        bgmAudioSource.clip = titleBGM;
        bgmAudioSource.Play();
    }
    public void PlayTutorialBGM()
    {
        bgmAudioSource.clip = tutorialBGM;
        bgmAudioSource.Play();
    }
    public void PlayMainBGM()
    {
        bgmAudioSource.clip = mainBGM;
        bgmAudioSource.Play();
    }
    public void PlayResultBGM()
    {
        bgmAudioSource.clip = resultBGM;
        bgmAudioSource.Play();
    }
}
