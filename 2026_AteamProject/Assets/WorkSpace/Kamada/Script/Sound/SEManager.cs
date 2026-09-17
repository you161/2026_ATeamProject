using UnityEngine;
using UnityEngine.Audio;

public class SEManager : MonoBehaviour
{
    [Header("AudioSourse")]
    [SerializeField] private AudioSource audioSource = null;
    [Header("スタートボタンSE")]
    [SerializeField] private AudioClip gameStartButtonSE = null;
    [SerializeField, Range(0.01f, 10f)] private float gameStartButtonSEVolume = 1f;
    [Header("ページ変更SE")]
    [SerializeField] private AudioClip nextPageSE = null;
    [SerializeField, Range(0.01f, 10f)] private float nextPageSEVolume = 1f;
    [Header("ゲームスタートSE")]
    [SerializeField] private AudioClip gameStartSE = null;
    [SerializeField, Range(0.01f, 10f)] private float gameStartSEVolume = 1f;
    [Header("プレイヤージャンプSE")]
    [SerializeField] private AudioClip playerJumpSE = null;
    [SerializeField, Range(0.01f, 10f)] private float playerJumpSEVolume = 1f;
    [Header("プレイヤーダイブSE")]
    [SerializeField] private AudioClip playerDiveSE = null;
    [SerializeField, Range(0.01f, 10f)] private float playerDiveSEVolume = 1f;
    [Header("爆弾構えSE")]
    [SerializeField] private AudioClip bombPickupSE = null;
    [SerializeField, Range(0.01f, 10f)] private float bombPickupSEVolume = 1f;
    [Header("爆発SE")]
    [SerializeField] private AudioClip bombExplodeSE = null;
    [SerializeField, Range(0.01f, 10f)] private float bombExplodeSEVolume = 1f;
    [Header("ラウンド終了SE")]
    [SerializeField] private AudioClip roundOverSE = null;
    [SerializeField, Range(0.01f, 10f)] private float roundOverSEVolume = 1f;
    public void GameStartButtonSE()
    {
        audioSource.clip = gameStartButtonSE;
        audioSource.volume = gameStartButtonSEVolume;
        audioSource.pitch = 1.0f;
        audioSource.PlayOneShot(gameStartButtonSE);
    }

    public void NextPageSE()
    {
        audioSource.clip = nextPageSE;
        audioSource.volume = nextPageSEVolume;
        audioSource.pitch = 1.0f;
        audioSource.PlayOneShot(nextPageSE);
    }

    public void GameStartSE()
    {
        audioSource.clip = gameStartSE;
        audioSource.volume = gameStartSEVolume;
        audioSource.pitch = 1.0f;
        audioSource.PlayOneShot(gameStartSE);
    }

    public void PlayerJumpSE()
    {
        audioSource.clip = playerJumpSE;
        audioSource.volume = playerJumpSEVolume;
        audioSource.pitch = 1.0f;
        audioSource.PlayOneShot(playerJumpSE);
    }

    public void PlayerDiveSE()
    {
        audioSource.clip = playerDiveSE;
        audioSource.volume = playerDiveSEVolume;
        audioSource.pitch = 1.0f;
        audioSource.PlayOneShot(playerDiveSE);
    }

    public void BombPickupSE()
    {
        audioSource.clip = bombPickupSE;
        audioSource.volume = bombPickupSEVolume;
        audioSource.pitch = 1.0f;
        audioSource.PlayOneShot(bombPickupSE);
    }

    public void BombExplodeSE()
    {
        audioSource.clip = bombExplodeSE;
        audioSource.volume = bombExplodeSEVolume;
        audioSource.pitch = 1.0f;
        audioSource.PlayOneShot(bombExplodeSE);
    }

    public void RoundOverSE()
    {
        audioSource.clip = roundOverSE;
        audioSource.volume = roundOverSEVolume;
        audioSource.pitch = 1.0f;
        audioSource.PlayOneShot(roundOverSE);
    }
}