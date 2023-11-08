using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour
{
    public Button groundBttn, surfaceBttn, seaBttn, reusedBttn, urbanBttn, agricultureBttn, industryBttn;
    public GameObject[] infoPanels;
    public GameObject[] objects;
    public GameObject[] layers, layers_canvas;
    public int current_layer;

    void Start()
    {
        groundBttn.onClick.AddListener(ground);
        surfaceBttn.onClick.AddListener(surface);
        seaBttn.onClick.AddListener(sea);
        reusedBttn.onClick.AddListener(reused);
        urbanBttn.onClick.AddListener(urban);
        agricultureBttn.onClick.AddListener(agriculture);
        industryBttn.onClick.AddListener(industry);

        //ground 0
        //surface 1
        //sea 2
        //reused 3
        //urban 4
        //agriculture 5
        //industry 6
    }

    void Update()
    {
        foreach(GameObject layer in layers)
            layer.SetActive(false);
        layers[current_layer].SetActive(true);
        foreach(GameObject layer_canvas in layers_canvas)
            layer_canvas.SetActive(false);
        layers[current_layer].SetActive(true);
        layers_canvas[current_layer].SetActive(true);
    }

    private void ground(){
        foreach(GameObject infoPanel in infoPanels)
            infoPanel.SetActive(false);

        foreach(GameObject objectItem in objects)
            objectItem.SetActive(false);

        infoPanels[0].SetActive(true);
        objects[0].SetActive(true);
    }

    private void surface(){
        foreach(GameObject infoPanel in infoPanels)
            infoPanel.SetActive(false);
        
        foreach(GameObject objectItem in objects)
            objectItem.SetActive(false);

        infoPanels[1].SetActive(true);
        objects[1].SetActive(true);
    }

    private void sea(){
        foreach(GameObject infoPanel in infoPanels)
            infoPanel.SetActive(false);

        foreach(GameObject objectItem in objects)
            objectItem.SetActive(false);

        infoPanels[2].SetActive(true);
        objects[2].SetActive(true);
    }

    private void reused(){
        foreach(GameObject infoPanel in infoPanels)
            infoPanel.SetActive(false);

        foreach(GameObject objectItem in objects)
            objectItem.SetActive(false);

        infoPanels[3].SetActive(true);
        objects[3].SetActive(true);
    }

    private void urban(){
        foreach(GameObject infoPanel in infoPanels)
            infoPanel.SetActive(false);
        
        foreach(GameObject objectItem in objects)
            objectItem.SetActive(false);

        infoPanels[4].SetActive(true);
        objects[4].SetActive(true);
    }

    private void agriculture(){
        foreach(GameObject infoPanel in infoPanels)
            infoPanel.SetActive(false);

        foreach(GameObject objectItem in objects)
            objectItem.SetActive(false);

        infoPanels[5].SetActive(true);
        objects[5].SetActive(true);
    }

    private void industry(){
        foreach(GameObject infoPanel in infoPanels)
            infoPanel.SetActive(false);
        
        foreach(GameObject objectItem in objects)
            objectItem.SetActive(false);
            
        infoPanels[6].SetActive(true);
        objects[6].SetActive(true);
    }
}
