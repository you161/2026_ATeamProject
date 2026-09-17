using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float targetPosY;
    [SerializeField] private GameObject crown;
    [SerializeField] private float crownPos;
    [SerializeField] private float crownSpeed;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private ResaultSceneManager resaultSceneManager = null;
    private Coroutine resultCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(resultCoroutine != null)
        {
            return;
        }
        else
        {
            resultCoroutine = StartCoroutine(ResultEffects());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (mainCamera.transform.position.y < targetPosY)
        //{
        //    if (crown.transform.position.y < crownPos)
        //    {
        //        crown.transform.position = new Vector3(0, moveSpeed, 0) * Time.deltaTime;
        //    }
        //    return;
        //}
        //else
        //{
        //    mainCamera.transform.position += new Vector3(0, moveSpeed, 0) * Time.deltaTime;
        //}
    }
    private IEnumerator ResultEffects()
    {
        while(mainCamera.transform.position.y > targetPosY)
        {
            mainCamera.transform.Translate(0, moveSpeed * Time.deltaTime, 0);
            //crown.transform.Translate(0, moveSpeed, 0);
            yield return new WaitForSeconds(0.01f);
        }
        playerAnimator.SetTrigger("pickup");
        while(crown.transform.position.y > crownPos)
        {
            crown.transform.Translate(0, crownSpeed * Time.deltaTime, 0);
            yield return new WaitForSeconds(0.01f);

        }
        resaultSceneManager.IsWait = true;
    }
}
