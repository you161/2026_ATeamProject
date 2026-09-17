using UnityEngine;
using UnityEngine.UI;

public class gaugeTest : MonoBehaviour
{
    [SerializeField] private Image gaugeImage = null;
    [SerializeField] private float coolDownTime = 0.0f;

    public float countTime = 0.0f;

    void Start()
    {
        gaugeImage.fillAmount = 0.0f;
    }

    void Update()
    {
        countTime += Time.deltaTime;
        gaugeImage.fillAmount += 1.0f / coolDownTime * Time.deltaTime;

        if (countTime >= coolDownTime) countTime = coolDownTime;
    }
}
