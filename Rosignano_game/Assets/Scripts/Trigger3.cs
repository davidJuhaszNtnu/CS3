using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Trigger3 : MonoBehaviour
{
    public int averageTarget_pos, averageTarget_neg;
    private bool mIsTriggered_pos, time_elapsed_pos;
    private bool mIsTriggered_neg, time_elapsed_neg;

    private Camera mCamera = null;
    private RectTransform mRectTransform_pos = null;
    private RectTransform mRectTransform_neg = null;

    private Color c;
    public gameController gameController;

    private float t_pos, t_neg;
    private float time_pos, time_neg;

    //averaging
    public int n;
    private int[] numberOfPoints_pos;
    private int sum_pos;
    private float average_pos;
    private int[] numberOfPoints_neg;
    private int sum_neg;
    private float average_neg;

    private void Awake(){
        gameController.OnTrigger3Points += OnTrigger3Points;

        mCamera = Camera.main;
        mRectTransform_pos = transform.GetChild(0).GetComponent<RectTransform>();
        mRectTransform_neg = transform.GetChild(1).GetComponent<RectTransform>();

        t_pos = 0f;
        t_neg = 0f;
        time_pos = Time.time;
        time_neg = Time.time;

        numberOfPoints_pos = new int[n];
        numberOfPoints_neg = new int[n];
        for(int i = 0; i < n; i++){
            numberOfPoints_pos[i] = 0;
            numberOfPoints_neg[i] = 0;
        }
        sum_pos = 0;
        sum_neg = 0;
    }

    void Update(){
        if(mIsTriggered_pos && !time_elapsed_pos){
            if(t_pos < 1f){
                t_pos += 0.01f;
                //image
                c = gameController.trig3_positiveAction.transform.GetChild(0).GetComponent<Image>().color;
                c.a = t_pos;
                gameController.trig3_positiveAction.transform.GetChild(0).GetComponent<Image>().color = c;
                //text
                c = gameController.trig3_positiveAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
                c.a = t_pos;
                gameController.trig3_positiveAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = c;
            }
        }else{
            if(t_pos > 0f){
                t_pos -= 0.01f;
                //image
                c = gameController.trig3_positiveAction.transform.GetChild(0).GetComponent<Image>().color;
                c.a = t_pos;
                gameController.trig3_positiveAction.transform.GetChild(0).GetComponent<Image>().color = c;
                //text
                c = gameController.trig3_positiveAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
                c.a = t_pos;
                gameController.trig3_positiveAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = c;
            }
        }
        if(mIsTriggered_neg && !time_elapsed_neg){
            if(t_neg < 1f){
                t_neg += 0.01f;
                //image
                c = gameController.trig3_negativeAction.transform.GetChild(0).GetComponent<Image>().color;
                c.a = t_pos;
                gameController.trig3_negativeAction.transform.GetChild(0).GetComponent<Image>().color = c;
                //text
                c = gameController.trig3_negativeAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
                c.a = t_pos;
                gameController.trig3_negativeAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = c;
            }
        }else{
            if(t_neg > 0f){
                t_neg -= 0.01f;
                //image
                c = gameController.trig3_negativeAction.transform.GetChild(0).GetComponent<Image>().color;
                c.a = t_pos;
                gameController.trig3_negativeAction.transform.GetChild(0).GetComponent<Image>().color = c;
                //text
                c = gameController.trig3_negativeAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
                c.a = t_pos;
                gameController.trig3_negativeAction.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = c;
            }
        }
    }

    private void OnDestroy(){
        gameController.OnTrigger3Points -= OnTrigger3Points;
    }

    private void OnTrigger3Points(List<Vector2> triggerPoints){
        if(!enabled)
            return;
        
        int count_pos = 0;
        int count_neg = 0;

        foreach(Vector2 point in triggerPoints){
            Vector2 flippedY = new Vector2(point.x, mCamera.pixelHeight - point.y);

            if(RectTransformUtility.RectangleContainsScreenPoint(mRectTransform_pos, flippedY))
                count_pos++;

            if(RectTransformUtility.RectangleContainsScreenPoint(mRectTransform_neg, flippedY))
                count_neg++;
        }

        //average
        sum_pos = sum_pos - numberOfPoints_pos[n - 1] + count_pos;
        average_pos = sum_pos/((float)n);
        for(int i = n - 2; i >= 0; i--){
            numberOfPoints_pos[i + 1] = numberOfPoints_pos[i];
        }
        numberOfPoints_pos[0] = count_pos;

        sum_neg = sum_neg - numberOfPoints_neg[n - 1] + count_neg;
        average_neg = sum_neg/((float)n);
        for(int i = n - 2; i >= 0; i--){
            numberOfPoints_neg[i + 1] = numberOfPoints_neg[i];
        }
        numberOfPoints_neg[0] = count_neg;

        if(average_pos <= averageTarget_pos + 5 && average_pos >= averageTarget_pos - 5){
            if(!mIsTriggered_pos){
                time_pos = Time.time;
            }
            if((Time.time - time_pos) < 5f)
                time_elapsed_pos = false;
            else time_elapsed_pos = true;

            mIsTriggered_pos = true;
        }else{
            mIsTriggered_pos = false;
        }

        if(average_neg <= averageTarget_neg + 7 && average_neg >= averageTarget_neg - 7){
            if(!mIsTriggered_neg){
                time_neg = Time.time;
            }
            if((Time.time - time_neg) < 5f)
                time_elapsed_neg = false;
            else time_elapsed_neg = true;

            mIsTriggered_neg = true;
        }else mIsTriggered_neg = false;

        if(Input.GetKeyDown(KeyCode.Alpha3))
            Debug.Log(average_pos + ", " + average_neg);
    }
}
