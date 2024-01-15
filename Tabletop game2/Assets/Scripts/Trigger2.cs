using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Trigger2 : MonoBehaviour
{
    public int averageTarget;
    public bool mIsTriggered;
    public bool time_elapsed_pos, time_elapsed_neg;

    private Camera mCamera = null;
    private RectTransform mRectTransform = null;

    public gameController gameController;

    private float time_pos, time_neg;

    //averaging
    public int n;
    private int[] numberOfPoints;
    private int sum;
    public float average;

    public bool isOff, start_measuring, isShowing, finished;
    private bool start_measuring_On, start_measuring_Off;
    private float time_trigOn, time_trigOff;

    //home version
    private bool condition;

    private void Awake(){
        gameController.OnTrigger2Points += OnTrigger2Points;

        mCamera = Camera.main;
        mRectTransform = transform.GetChild(0).GetComponent<RectTransform>();

        time_pos = -99999f;
        time_neg = -99999f;
        time_elapsed_pos = true;
        time_elapsed_neg = true;
        isShowing = false;
        finished = false;
        start_measuring = false;

        numberOfPoints = new int[n];
        for(int i = 0; i < n; i++){
            numberOfPoints[i] = 0;
        }
        sum = 0;

        //home version
        condition = false;
    }

    void Update(){
        //home version
        OnTrigger2();

        if(isShowing){
            if(!start_measuring){
                start_measuring = true;
                time_pos = Time.time;
                gameController.trig2_positiveAction.transform.GetChild(0).gameObject.SetActive(true);
                gameController.trig2_positiveAction.transform.GetChild(1).gameObject.SetActive(true);
            }else if(Time.time - time_pos > 5f){
                gameController.trig2_positiveAction.transform.GetChild(0).gameObject.SetActive(false);
                gameController.trig2_positiveAction.transform.GetChild(1).gameObject.SetActive(false);
                isShowing = false;
                start_measuring = false;
                finished = true;
            }
        }

        if(!start_measuring){
            isOff = true;
        }else isOff = false;
    }

    private void OnDestroy(){
        gameController.OnTrigger2Points -= OnTrigger2Points;
    }

    //home version
    private void OnTrigger2(){
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            condition = condition ? false : true;
            // Debug.Log(condition);
        }

        if(condition){
            if(!start_measuring_On){
                start_measuring_On = true;
                time_trigOn = Time.time;
            }else if((Time.time - time_trigOn) > 2f){
                mIsTriggered = true;
                start_measuring_Off = false;
            }

            // mIsTriggered = true;
        }else{
            if(!start_measuring_Off){
                start_measuring_Off = true;
                time_trigOff = Time.time;
            }else if((Time.time - time_trigOff) > 2f){
                mIsTriggered = false;
                start_measuring_On = false;
                finished = false;
            }

            // mIsTriggered = false;
        }
    }

    private void OnTrigger2Points(List<Vector2> triggerPoints){
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

        if(average <= averageTarget + 10 && average >= averageTarget - 10){
            if(!mIsTriggered){
                time_pos = Time.time;
            }
            if((Time.time - time_pos) < 20f)
                time_elapsed_pos = false;
            else time_elapsed_pos = true;

            mIsTriggered = true;
        }else{
            // Debug.Log(mIsTriggered);
            if(mIsTriggered){
                time_neg = Time.time;
            }
            if((Time.time - time_neg) < 20f)
                time_elapsed_neg = false;
            else time_elapsed_neg = true;

            mIsTriggered = false;
        }

        // if(Input.GetKeyDown(KeyCode.Alpha2))
        //     Debug.Log(average);
    }
}
