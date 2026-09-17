using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class gaugeTest : MonoBehaviour
{
    [SerializeField] private Image gaugeImage = null;
    [SerializeField] private float cooldownTime = 0.0f;

    private bool isCoolDown = false;

    void Update()
    {
        Vector3 imagePos = gaugeImage.rectTransform.position;
        imagePos = transform.position;
        imagePos.y = gaugeImage.rectTransform.position.y;
        gaugeImage.rectTransform.position = imagePos;

        if (isCoolDown)
        {
            gaugeImage.fillAmount += 1.0f / cooldownTime * Time.deltaTime;

            if (gaugeImage.fillAmount >= 1.0f)
            {
                isCoolDown = false;
            }
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            gaugeImage.fillAmount = 0.0f;
            isCoolDown = true;
        }
    }
}
