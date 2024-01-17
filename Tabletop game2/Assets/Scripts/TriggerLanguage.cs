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

    public TextMeshProUGUI text_trigger1_pos, text_trigger2_pos, text_trigger3_pos, text_trigger4_pos;
    public TextMeshProUGUI text_trigger1_neg, text_trigger2_neg, text_trigger3_neg, text_trigger4_neg;
    public TextMeshProUGUI text_idle1, text_idle2;

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
        // text_trigger1_pos.text = "This pristine lake in Cecina City is full, and its waters are teeming with life. Lush vegetation surrounds the lake, providing habitat for diverse wildlife.";
        // text_trigger1_neg.text = "Eliminating the city's lake would disrupt the water cycle, reducing the amount of water evaporating into the atmosphere. This, in turn will decrease rainfall in Cecina, with dire consequences.";

        // text_trigger2_pos.text = "This water treatment facility in Rosignano is operational, ensuring clean and safe tap water for the residents. Industries continue to flourish, providing jobs and contributing to the local economy.";
        // text_trigger2_neg.text = "Removing Rosignano's water treatment facility means using regular tap water to sustain jobs and industries. However, this will result in a decrease in the supply of drinkable water.";

        // text_trigger3_pos.text = "This greenhouse is a beacon of sustainable farming. Crops within it thrive, and the controlled water and environment ensures healthy yields and produce. ";
        // text_trigger3_neg.text = "Greenhouse removal threatens agriculture, endangers food supply by\ndisrupting water control for plant life, and jeopardizes community\nsustenance.";

        // text_trigger4_pos.text = "This island is intact, serving as a thriving ecosystem. The island maintains a stable water flow and climate in the region, and local fishing communities have abundant catches, supporting their livelihoods.";
        // text_trigger4_neg.text = "Taking away this island will greatly impact its habitat and disrupt the water balance and local ecosystem that it crucially supports.";

        // text_idle1.text = "Water symbiosis in Cecina and Rosignano: A Journey to Reconnect with Water";
        // text_idle2.text = "In the idyllic towns of Cecina and Rosignano is a story of hope amidst a global issue of water imbalance.  These communities, nestled along the pristine Mediterranean coast, had once grappled with environmental discord from rapid development, embarked on a transformative journey.\n\nBy raising awareness and dismantling barriers through the ULTIMATE Water project, we emphasize the need to preserve the balance of life-sustaining waters. Join us in our call for action and play the interactive game to restore our planet's water equilibrium and safeguard precious ecosystems. Together, we can make a lasting impact.";
    }

    private void changeToItalian(){
        // text_trigger1_pos.text = "This pristine lake in Cecina City is full, and its waters are teeming with life. Lush vegetation surrounds the lake, providing habitat for diverse wildlife. (italian)";
        // text_trigger1_neg.text = "Eliminating the city's lake would disrupt the water cycle, reducing the amount of water evaporating into the atmosphere. This, in turn will decrease rainfall in Cecina, with dire consequences. (italian)";

        // text_trigger2_pos.text = "This water treatment facility in Rosignano is operational, ensuring clean and safe tap water for the residents. Industries continue to flourish, providing jobs and contributing to the local economy. (italian)";
        // text_trigger2_neg.text = "Removing Rosignano's water treatment facility means using regular tap water to sustain jobs and industries. However, this will result in a decrease in the supply of drinkable water. (italian)";

        // text_trigger3_pos.text = "This greenhouse is a beacon of sustainable farming. Crops within it thrive, and the controlled water and environment ensures healthy yields and produce. (italian)";
        // text_trigger3_neg.text = "Greenhouse removal threatens agriculture, endangers food supply by\ndisrupting water control for plant life, and jeopardizes community\nsustenance. (italian)";

        // text_trigger4_pos.text = "This island is intact, serving as a thriving ecosystem. The island maintains a stable water flow and climate in the region, and local fishing communities have abundant catches, supporting their livelihoods. (italian)";
        // text_trigger4_neg.text = "Taking away this island will greatly impact its habitat and disrupt the water balance and local ecosystem that it crucially supports. (italian)";

        // text_idle1.text = "Water symbiosis in Cecina and Rosignano: A Journey to Reconnect with Water (italian)";
        // text_idle2.text = "In the idyllic towns of Cecina and Rosignano is a story of hope amidst a global issue of water imbalance.  These communities, nestled along the pristine Mediterranean coast, had once grappled with environmental discord from rapid development, embarked on a transformative journey.\n\nBy raising awareness and dismantling barriers through the ULTIMATE Water project, we emphasize the need to preserve the balance of life-sustaining waters. Join us in our call for action and play the interactive game to restore our planet's water equilibrium and safeguard precious ecosystems. Together, we can make a lasting impact. (italian)";
    }
}
