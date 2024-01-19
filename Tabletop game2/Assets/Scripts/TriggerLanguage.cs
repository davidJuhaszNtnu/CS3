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

    //idle
    public GameObject idle_cloud;
    public Texture idle_title, idle_text;
    public Texture idle_title_it, idle_text_it;

    //other clouds
    public GameObject lake_cloud_pos, wwtp_cloud_pos, house_cloud_pos, island_cloud_pos;
    public GameObject lake_cloud_neg, wwtp_cloud_neg, house_cloud_neg, island_cloud_neg;
    public Texture lake_tex_pos, wwtp_tex_pos, house_tex_pos, island_tex_pos;
    public Texture lake_tex_neg, wwtp_tex_neg, house_tex_neg, island_tex_neg;
    public Texture lake_tex_pos_it, wwtp_tex_pos_it, house_tex_pos_it, island_tex_pos_it;
    public Texture lake_tex_neg_it, wwtp_tex_neg_it, house_tex_neg_it, island_tex_neg_it;

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

        changeToItalian();
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
        //idle
        idle_cloud.transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_BaseMap", idle_title);
        idle_cloud.transform.GetChild(1).GetComponent<Renderer>().material.SetTexture("_BaseMap", idle_text);
        //other clouds
        lake_cloud_pos.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", lake_tex_pos);
        wwtp_cloud_pos.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", wwtp_tex_pos);
        house_cloud_pos.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", house_tex_pos);
        island_cloud_pos.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", island_tex_pos);

        lake_cloud_neg.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", lake_tex_neg);
        wwtp_cloud_neg.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", wwtp_tex_neg);
        house_cloud_neg.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", house_tex_neg);
        island_cloud_neg.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", island_tex_neg);
    }

    private void changeToItalian(){
        //idle
        idle_cloud.transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_BaseMap", idle_title_it);
        idle_cloud.transform.GetChild(1).GetComponent<Renderer>().material.SetTexture("_BaseMap", idle_text_it);
        //other clouds
        lake_cloud_pos.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", lake_tex_pos_it);
        wwtp_cloud_pos.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", wwtp_tex_pos_it);
        house_cloud_pos.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", house_tex_pos_it);
        island_cloud_pos.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", island_tex_pos_it);

        lake_cloud_neg.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", lake_tex_neg_it);
        wwtp_cloud_neg.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", wwtp_tex_neg_it);
        house_cloud_neg.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", house_tex_neg_it);
        island_cloud_neg.transform.GetComponent<Renderer>().material.SetTexture("_BaseMap", island_tex_neg_it);
    }
}
