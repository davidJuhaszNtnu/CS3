using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Trigger1 : MonoBehaviour
{
    // [Range(0, 10)]
    // public int mSensitivity_pos, mSensitivity_neg;
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

    public bool isOff, isShowing;

    //home version
    private bool condition;

    private void Awake(){
        gameController.OnTrigger1Points += OnTrigger1Points;

        mCamera = Camera.main;
        mRectTransform = transform.GetChild(0).GetComponent<RectTransform>();

        time_pos = -99999f;
        time_neg = -99999f;
        time_elapsed_pos = true;
        time_elapsed_neg = true;

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
        OnTrigger1();

        if(!time_elapsed_pos || !time_elapsed_neg)
            isShowing = true;
        else isShowing = false;

        if(mIsTriggered){
            if(!time_elapsed_pos){
                //turn on pos
                gameController.trig1_positiveAction.transform.GetChild(0).gameObject.SetActive(true);
                gameController.trig1_positiveAction.transform.GetChild(1).gameObject.SetActive(true);

                //turn off neg
                gameController.trig1_negativeAction.transform.GetChild(0).gameObject.SetActive(false);
                // gameController.trig1_negativeAction.transform.GetChild(1).gameObject.SetActive(false);
            }else{
                //turn off pos
                gameController.trig1_positiveAction.transform.GetChild(0).gameObject.SetActive(false);
                gameController.trig1_positiveAction.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else{
            if(!time_elapsed_neg){
                //turn on neg
                gameController.trig1_negativeAction.transform.GetChild(0).gameObject.SetActive(true);
                // gameController.trig1_negativeAction.transform.GetChild(1).gameObject.SetActive(true);

                //turn off pos
                gameController.trig1_positiveAction.transform.GetChild(0).gameObject.SetActive(false);
                gameController.trig1_positiveAction.transform.GetChild(1).gameObject.SetActive(false);
            }else{
                //turn off neg
                gameController.trig1_negativeAction.transform.GetChild(0).gameObject.SetActive(false);
                // gameController.trig1_negativeAction.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        if(time_elapsed_pos && time_elapsed_neg){
            if(!isOff){
                isOff = true;
            }
        }else if(isOff){
                isOff = false;
        }

        if(gameController.transform.GetComponent<gameController>().allIsTriggered || gameController.transform.GetComponent<gameController>().allIsNotTriggered){
            // gameController.trig1_negativeAction.transform.GetChild(1).gameObject.SetActive(false);
            gameController.trig1_positiveAction.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void OnDestroy(){
        gameController.OnTrigger1Points -= OnTrigger1Points;
    }

    //home version
    private void OnTrigger1(){
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            condition = condition ? false : true;
            // Debug.Log(condition);
        }

        // if(!gameController.GetComponent<gameController>().showing[1] && !gameController.GetComponent<gameController>().showing[2] && !gameController.GetComponent<gameController>().showing[3])
        if(condition){
            if(!mIsTriggered){
                time_pos = Time.time;
            }
            if((Time.time - time_pos) < 5f){
                time_elapsed_pos = false;
            }else time_elapsed_pos = true;

            mIsTriggered = true;
        }else{
            if(mIsTriggered){
                time_neg = Time.time;
            }
            if((Time.time - time_neg) < 5f)
                time_elapsed_neg = false;
            else time_elapsed_neg = true;

            mIsTriggered = false;
        }
    }

    private void OnTrigger1Points(List<Vector2> triggerPoints){
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

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            mIsTriggered = mIsTriggered ? false : true;
            Debug.Log(mIsTriggered);
        }

        if(average <= averageTarget + 10 && average >= averageTarget - 10){
            if(!mIsTriggered){
                time_pos = Time.time;
            }
            if((Time.time - time_pos) < 20f)
                time_elapsed_pos = false;
            else time_elapsed_pos = true;

            mIsTriggered = true;
        }else{
            if(mIsTriggered){
                time_neg = Time.time;
            }
            if((Time.time - time_neg) < 20f)
                time_elapsed_neg = false;
            else time_elapsed_neg = true;

            mIsTriggered = false;
        }

        // if(Input.GetKeyDown(KeyCode.Alpha1))
        //     Debug.Log(average);
    }
}
