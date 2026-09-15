using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StockFader : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private Image crown;
    [SerializeField] private AudioClip SEAudio;
    [SerializeField] private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FadeRoutine());
        audioSource.PlayOneShot(SEAudio);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //クラウンが出現したときの処理
    private IEnumerator FadeRoutine()
    {
        Color color = crown.color;
        float startAlpha = color.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            // 経過時間に応じてアルファ値を 1（完全不透明）に向かって補間
            color.a = Mathf.Lerp(startAlpha, 1f, time / duration);
            crown.color = color;
            yield return null;
        }

        // 最後に確実に完全に不透明にする
        color.a = 1f;
        crown.color = color;
    }
}
