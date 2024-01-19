using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Trigger4 : MonoBehaviour
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

    public bool isOff, start_measuring_pos, isShowing_pos, finished_pos;
    public bool start_measuring_neg, isShowing_neg, finished_neg, negative_activated;
    private bool start_measuring_On, start_measuring_Off;
    private float time_trigOn, time_trigOff;

    //home version
    private bool condition;

    private void Awake(){
        gameController.OnTrigger4Points += OnTrigger4Points;

        mCamera = Camera.main;
        mRectTransform = transform.GetChild(0).GetComponent<RectTransform>();

        time_pos = -99999f;
        time_neg = -99999f;
        time_elapsed_pos = true;
        time_elapsed_neg = true;
        isShowing_pos = false;
        finished_pos = false;
        finished_neg = false;
        start_measuring_pos = false;

        isShowing_neg = false;
        finished_neg = false;
        start_measuring_neg = false;
        negative_activated = false;

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
        // OnTrigger4();

        if(isShowing_pos){
            if(!start_measuring_pos){
                start_measuring_pos = true;
                time_pos = Time.time;
                gameController.trig4_positiveAction.transform.GetChild(0).gameObject.SetActive(true);
                gameController.trig4_positiveAction.transform.GetChild(1).gameObject.SetActive(true);
            }else if(Time.time - time_pos > 10f){
                gameController.trig4_positiveAction.transform.GetChild(0).gameObject.SetActive(false);
                gameController.trig4_positiveAction.transform.GetChild(1).gameObject.SetActive(false);
                isShowing_pos = false;
                start_measuring_pos = false;
                finished_pos = true;
            }
        }

        if(isShowing_neg){
            if(!start_measuring_neg){
                start_measuring_neg = true;
                time_neg = Time.time;
                gameController.trig4_negativeAction.transform.GetChild(0).gameObject.SetActive(true);
                gameController.trig4_negativeAction.transform.GetChild(1).gameObject.SetActive(true);
            }else if(Time.time - time_neg > 10f){
                gameController.trig4_negativeAction.transform.GetChild(0).gameObject.SetActive(false);
                gameController.trig4_negativeAction.transform.GetChild(1).gameObject.SetActive(false);
                isShowing_neg = false;
                start_measuring_neg = false;
                finished_neg = true;
                negative_activated = false;
            }
        }

        if(!start_measuring_pos && !start_measuring_neg){
            isOff = true;
        }else isOff = false;
    }

    private void OnDestroy(){
        gameController.OnTrigger4Points -= OnTrigger4Points;
    }

    //home version
    private void OnTrigger4(){
        if(Input.GetKeyDown(KeyCode.Alpha4)){
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
                finished_neg = false;
            }

            // mIsTriggered = true;
        }else{
            if(!start_measuring_Off){
                start_measuring_Off = true;
                time_trigOff = Time.time;
            }else if((Time.time - time_trigOff) > 2f){
                if(mIsTriggered)
                    negative_activated = true;
                mIsTriggered = false;
                start_measuring_On = false;
                finished_pos = false;
            }

            // mIsTriggered = false;
        }
    }

    private void OnTrigger4Points(List<Vector2> triggerPoints){
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
            if(!start_measuring_On){
                start_measuring_On = true;
                time_trigOn = Time.time;
            }else if((Time.time - time_trigOn) > 2f){
                mIsTriggered = true;
                start_measuring_Off = false;
                finished_neg = false;
            }
        }else{
            if(!start_measuring_Off){
                start_measuring_Off = true;
                time_trigOff = Time.time;
            }else if((Time.time - time_trigOff) > 2f){
                if(mIsTriggered)
                    negative_activated = true;
                mIsTriggered = false;
                start_measuring_On = false;
                finished_pos = false;
            }
        }

        // if(Input.GetKeyDown(KeyCode.Alpha4))
        //     Debug.Log(average);
    }
}