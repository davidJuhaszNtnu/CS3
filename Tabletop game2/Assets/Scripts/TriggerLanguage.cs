using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriggerLanguage : MonoBehaviour
{
    public int averageTarget;
    private bool mIsTriggered;

    private Camera mCamera = null;
    private RectTransform mRectTransform = null;

    private Color c;
    public gameController gameController;

    //averaging
    public int n;
    private int[] numberOfPoints;
    private int sum;
    public float average;

    //language
    private bool isItalian, time_elapsed;
    public GameObject text_ita, text_eng;
    private float time_start;

    public Texture idle_title;

    private void Awake(){
        gameController.OnTrigger5Points += OnTrigger5Points;

        mCamera = Camera.main;
        mRectTransform = transform.GetChild(0).GetComponent<RectTransform>();

        numberOfPoints = new int[n];
        for(int i = 0; i < n; i++){
            numberOfPoints[i] = 0;
        }
        sum = 0;
        time_start = -99999f;

        isItalian = true;
        text_eng.SetActive(false);
        text_ita.SetActive(true);
    }

    private void OnDestroy(){
        gameController.OnTrigger5Points -= OnTrigger5Points;
    }

    private void OnTrigger5Points(List<Vector2> triggerPoints){
        if(!enabled)
            return;
        
        int count = 0;

        foreach(Vector2 point in triggerPoints){
            Vector2 flippedY = new Vector2(point.x, mCamera.pixelHeight - point.y);

            if(RectTransformUtility.RectangleContainsScreenPoint(mRectTransform, flippedY))
                count++;
        }

        //average
        sum = sum - numberOfPoints[n - 1] + count;
        average = sum/((float)n);
        for(int i = n - 2; i >= 0; i--){
            numberOfPoints[i + 1] = numberOfPoints[i];
        }
        numberOfPoints[0] = count;

        if((Time.time - time_start) < 1f)
            time_elapsed = false;
        else time_elapsed = true;

        if(time_elapsed){
            if(average <= averageTarget + 50 && average >= averageTarget - 50){
                if(!mIsTriggered){
                    time_start = Time.time;
                    if(isItalian){
                        changeToEnglish();
                        text_eng.SetActive(true);
                        text_ita.SetActive(false);
                        // Debug.Log("english");
                    }else{
                        changeToItalian();
                        text_eng.SetActive(false);
                        text_ita.SetActive(true);
                        // Debug.Log("italian");
                    }
                }

                mIsTriggered = true;
            }else{
                if(mIsTriggered){
                    if(isItalian)
                        isItalian = false;
                    else isItalian = true;
                }

                mIsTriggered = false;
            }
        }

        // if(Input.GetKeyDown(KeyCode.Alpha5))
        //     Debug.Log(average);
    }

    private void changeToEnglish(){
        
    }

    private void changeToItalian(){
        
    }
}
