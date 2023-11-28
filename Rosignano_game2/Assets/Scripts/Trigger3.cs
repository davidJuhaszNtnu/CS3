using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Trigger3 : MonoBehaviour
{
    public int averageTarget;
    public bool mIsTriggered;
    private bool time_elapsed_pos, time_elapsed_neg;

    private Camera mCamera = null;
    private RectTransform mRectTransform = null;

    private Color c;
    public gameController gameController;

    private float t_pos, t_neg;
    private float time_pos, time_neg;

    //averaging
    public int n;
    private int[] numberOfPoints;
    private int sum;
    public float average;

    public bool isOff;

    private void Awake(){
        gameController.OnTrigger3Points += OnTrigger3Points;

        mCamera = Camera.main;
        mRectTransform = transform.GetChild(0).GetComponent<RectTransform>();

        t_pos = 0f;
        t_neg = 0f;
        time_pos = -99999f;
        time_neg = -99999f;
        time_elapsed_pos = true;
        time_elapsed_neg = true;

        numberOfPoints = new int[n];
        for(int i = 0; i < n; i++){
            numberOfPoints[i] = 0;
        }
        sum = 0;
    }

    private void fade(float t, GameObject action){
        //image
        c = action.transform.GetChild(0).GetComponent<Image>().color;
        c.a = t;
        action.transform.GetChild(0).GetComponent<Image>().color = c;
        //text
        c = action.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
        c.a = t;
        action.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = c;
    }

    void Update(){
        if(gameController.transform.GetComponent<gameController>().allIsTriggered || gameController.transform.GetComponent<gameController>().allIsNotTriggered){
            gameController.trig3_positiveAction.transform.GetChild(1).gameObject.SetActive(false);
            time_elapsed_pos = true;
            time_elapsed_neg = true;
        }

        if(mIsTriggered){
            if(!time_elapsed_pos){
                //fade in pos
                if(t_pos < 1f){
                    t_pos += 0.01f;
                    fade(t_pos, gameController.trig3_positiveAction);
                }
                //3d model
                gameController.trig3_positiveAction.transform.GetChild(1).gameObject.SetActive(true);

                //fade out neg
                if(t_neg > 0f){
                    t_neg -= 0.01f;
                    fade(t_neg, gameController.trig3_negativeAction);
                }
            }else{
                //fade out pos
                if(t_pos > 0f){
                    t_pos -= 0.01f;
                    fade(t_pos, gameController.trig3_positiveAction);
                }
                //3d model
                gameController.trig3_positiveAction.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else{
            if(!time_elapsed_neg){
                //fade in neg
                if(t_neg < 1f){
                    t_neg += 0.01f;
                    fade(t_neg, gameController.trig3_negativeAction);
                }
                //fade out pos
                if(t_pos > 0f){
                    t_pos -= 0.01f;
                    fade(t_pos, gameController.trig3_positiveAction);
                }
                //3d model
                gameController.trig3_positiveAction.transform.GetChild(1).gameObject.SetActive(false);
            }else{
                //fade out neg
                if(t_neg > 0f){
                    t_neg -= 0.01f;
                    fade(t_neg, gameController.trig3_negativeAction);
                }
            }
        }
        if(t_pos <= 0f && t_neg <= 0f){
            if(!isOff){
                isOff = true;
            }
        }else if(isOff){
                isOff = false;
        }

        if(gameController.transform.GetComponent<gameController>().allIsTriggered || gameController.transform.GetComponent<gameController>().allIsNotTriggered){
            gameController.trig3_positiveAction.transform.GetChild(1).gameObject.SetActive(false);
            time_elapsed_pos = true;
            time_elapsed_neg = true;
        }
    }

    private void OnDestroy(){
        gameController.OnTrigger3Points -= OnTrigger3Points;
    }

    private void OnTrigger3Points(List<Vector2> triggerPoints){
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
            if(mIsTriggered){
                time_neg = Time.time;
            }
            if((Time.time - time_neg) < 20f)
                time_elapsed_neg = false;
            else time_elapsed_neg = true;

            mIsTriggered = false;
        }

        // if(Input.GetKeyDown(KeyCode.Alpha3))
        //     Debug.Log(average);
    }
}
