using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] private GameObject countImageObject = null;
    [SerializeField] private Image currentImage = null;
    [SerializeField] private Sprite[] countDownSprite = null;
    [SerializeField] private int count = 3;
    [SerializeField] private SEManager seManager = null;

    private float currentTime = 0;
    private int currentCount = 0;

    private bool isPlayCount = false;
    private bool isGo = false;
    public bool IsPlayCount { get => isPlayCount; }
    public bool IsGo { get => isGo; }

    private void Start()
    {
        currentTime = 0;
        currentCount = 0;

        countImageObject.SetActive(false);

        StartCountDown();
    }

    private void Update()
    {
        if (!isPlayCount)
        {
            return;
        }

        currentTime -= Time.deltaTime;

        if (currentTime > 0.0f)
        {
            return;
        }

        currentTime = 1.0f;

        //GO表示中
        if (isGo)
        {
            isPlayCount = false;
            isGo = false;

            countImageObject.SetActive(false);

            return;
        }

        currentCount--;

        if (currentCount > 0)
        {
            //3 → 2 → 1
            currentImage.sprite = countDownSprite[count - currentCount];
        }
        else
        {
            //GO
            currentImage.sprite = countDownSprite[3];
            isGo = true;
        }
    }

    public void StartCountDown()
    {
        if (isPlayCount)
        {
            return;
        }

        isPlayCount = true;
        isGo = false;

        currentTime = 1.0f;
        currentCount = count;

        countImageObject.SetActive(true);

        //最初に3を表示
        currentImage.sprite = countDownSprite[0];

        seManager.GameStartSE();
    }
}