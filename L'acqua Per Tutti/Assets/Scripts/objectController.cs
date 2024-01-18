using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class objectController : MonoBehaviour
{
    Data data;
    Vector3[] ground_locations;
    Vector3[] reused_locations;
    Vector3[] solvay_location;
    Vector3[] urban_locations;
    Vector3[] agriculture_locations;
    Vector3[] industry_locations;
    Vector3[] flowmeter_locations;
    Vector3[] conductivity_locations;
    List<Vector2[]> surfaces2D;
    List<Vector3[]> piping_cecina;
    List<Vector3[]> piping_rosignano;
    List<Vector2[]> cecina_mun_surface;
    List<Vector2[]> rosignano_mun_surface;
    List<Vector2[]> sea;
    List<Vector2[]> area210;
    Vector3 lower_left, upper_right;
    public GameObject ground_prefab, groundObject,
    surface_prefab, surfaceObject, rectangle,
    reused_prefab, reusedObject, reusedObject_layer3,
    urban_prefab, urbanObject,
    agriculture_prefab, agricultureObject,
    industry_prefab, industryObject,
    piping_cecina_prefab, piping_rosignano_prefab,
    cecina_mun_surface_prefab, mun_surface_Object,
    rosignano_mun_surface_prefab,
    sea_prefab, sea_Object, piping_Object, 
    arrow_prefab, path_prefab, arrow_parent_layer1, arrow_parent_layer3, arrowPathObject_layer1, arrowPathObject_layer3, arrow_parent_layer4, arrowPathObject_layer4,
    flowmeter_prefab, P1_Object, P2_Object, P3_Object,
    conductivity_prefab, legendCanvasPrefab_P1;
    private GameObject piping_rosignano_Object, piping_cecina_Object;
    public GameObject descriptionCanvas_prefab, imageCanvas_prefab;
    public Sprite solvay_image, asa_image;
    //P1
    public Sprite P1_picture2, P1_picture3, P1_picture4, P1_picture5, P1_picture6;
    public Sprite P2_picture2, P2_picture3, P2_picture4, P2_picture5;
    public Sprite P3_picture1, P3_picture2, P3_picture3, P3_picture4, P3_picture5, P3_picture6, P3_picture7, P3_picture8,
    P3_picture9, P3_picture10;
    public GameObject appPanel, app, worldCanvas;
    Vector3[] corners;

    public LanguageChanger languageChanger;
    public TMP_Dropdown dropdown;
    public Material rectangleMaterial;


    public float actualHeight, scale;
    private Vector3 origin;
    private float width, height, aspect_ratio;

    void Start()
    {
        data = new Data();
        lower_left = new Vector3(1613562.9126609f, 0f, 4790557.9631989552f);
        upper_right = new Vector3(1625967.70790374418720603f, 0f, 4809329.91194767318665981f);

        // lower_left = new Vector3(1613562.912660910049453f, 0f, 4790557.963198949582875f);
        // upper_right = new Vector3(1625967.707903739996254f, 0f, 4809329.911947670392692f);
        corners = new [] {lower_left, new Vector3(upper_right.x, 0f, lower_left.z), upper_right, new Vector3(lower_left.x, 0f, upper_right.z), lower_left};
        origin = new Vector3(lower_left.x+ (upper_right.x - lower_left.x)/2f, 0f, lower_left.z);
        lower_left -= origin;
        upper_right -= origin;
        width = upper_right.x - lower_left.x;
        height = upper_right.z - lower_left.z;
        aspect_ratio = width/height;
        // Debug.Log(aspect_ratio);

        ground_locations = data.getData_ground();
        reused_locations = data.getData_reused();
        solvay_location = new [] {new Vector3(1617562.211854790803045f, 0f, 4804301.483507531695068f)};
        urban_locations = data.getData_urban();
        agriculture_locations = data.getData_agriculture();
        industry_locations = data.getData_industry();
        surfaces2D = data.getData_surface();
        piping_cecina = data.getData_piping_cecina();
        piping_rosignano = data.getData_piping_rosignano();
        cecina_mun_surface = data.getData_cecina_mun_surface();
        rosignano_mun_surface = data.getData_rosignano_mun_surface();
        sea = data.getData_sea();
        flowmeter_locations = data.getData_flowmeter();
        conductivity_locations = data.getData_conductivity();
        // area210 = data.getData_area210();

        piping_Object.SetActive(false);
        piping_cecina_Object = piping_Object.transform.GetChild(0).gameObject;
        piping_rosignano_Object = piping_Object.transform.GetChild(1).gameObject;

        // app.GetComponent<Calibration>().calibrateFirst();
    }

    void Update()
    {
        if(appPanel.GetComponent<ButtonsController>().current_layer == 0 && reusedObject.activeSelf){
            foreach(Transform child in arrow_parent_layer1.transform){
                animateArrow(child.gameObject);
            }
        }
        if(appPanel.GetComponent<ButtonsController>().current_layer == 2){
            foreach(Transform child in arrow_parent_layer3.transform){
                animateArrow(child.gameObject);
            }
        }
        if(appPanel.GetComponent<ButtonsController>().current_layer == 3){
            foreach(Transform child in arrow_parent_layer4.transform){
                animateArrow(child.gameObject);
            }
        }
    }

    public void callibrateCreate(){
        //ground
        ground_locations = callibratePoints(ground_locations);
        createPointObjects(ground_locations, ground_prefab, scale * 0.01f, groundObject);

        //sea
        sea = callibrateSurfaces(sea);
        createSurfaceObjects(sea, sea_prefab, sea_Object);

        //surface
        surfaces2D = callibrateSurfaces(surfaces2D);
        createSurfaceObjects(surfaces2D, surface_prefab, surfaceObject);

        //reused
        reused_locations = callibratePoints(reused_locations);
        createPointObjects(reused_locations, reused_prefab, scale * 0.01f, reusedObject);
        //canvases associated to reused locations
        //cecina 1, rosignano 2, aretusa 0
        createDescriptionCanvas("ARETUSA WASTEWATER TREATMENT PLANT", (new Vector3(-0.082f, 0.01f, 0.671f) * scale), scale, 0.1f, 0.05f, 0.01f, reusedObject, 0);
        createDescriptionCanvas("CECINA WASTEWATER TREATMENT PLANT", (new Vector3(0.189f, 0.01f, 0.289f) * scale), scale, 0.1f, 0.05f, 0.01f, reusedObject, 1);
        createDescriptionCanvas("ROSIGNANO WASTEWATER TREATMENT PLANT", (new Vector3(-0.187f, 0.01f, 0.757f) * scale), scale, 0.1f, 0.05f, 0.01f, reusedObject, 2);
        //creating paths with arrows
        createArrowPaths(reused_locations[1], reused_locations[0], 0.2f, scale, arrowPathObject_layer1, arrow_parent_layer1);
        createArrowPaths(reused_locations[2], reused_locations[0], 0.05f, scale, arrowPathObject_layer1, arrow_parent_layer1);

        //urban
        urban_locations = callibratePoints(urban_locations);
        createPointObjects(urban_locations, urban_prefab, scale * 0.01f, urbanObject);

        //agriculture
        agriculture_locations = callibratePoints(agriculture_locations);
        createPointObjects(agriculture_locations, agriculture_prefab, scale * 0.01f, agricultureObject);

        //industry
        industry_locations = callibratePoints(industry_locations);
        createPointObjects(industry_locations, industry_prefab, scale * 0.01f, industryObject);

        //piping
        piping_cecina = callibrateLines(piping_cecina);
        createLineObjects(piping_cecina, piping_cecina_prefab, piping_cecina_Object);
        piping_rosignano = callibrateLines(piping_rosignano);
        createLineObjects(piping_rosignano, piping_rosignano_prefab, piping_rosignano_Object);
        //reused for the piping layer + solvay
        createPointObjects(reused_locations, reused_prefab, scale * 0.01f, reusedObject_layer3);
        solvay_location = callibratePoints(solvay_location);
        createPointObjects(solvay_location, reused_prefab, scale * 0.01f, reusedObject_layer3);
        reusedObject_layer3.SetActive(true);
        // municipality surfaces
        cecina_mun_surface = callibrateSurfaces(cecina_mun_surface);
        createSurfaceObjects(cecina_mun_surface, cecina_mun_surface_prefab, mun_surface_Object);
        rosignano_mun_surface = callibrateSurfaces(rosignano_mun_surface);
        createSurfaceObjects(rosignano_mun_surface, rosignano_mun_surface_prefab, mun_surface_Object);
        mun_surface_Object.SetActive(true);

        //canvases associated to piping locations
        //cecina 1, rosignano 2, aretusa 0, solvay 3
        createDescriptionCanvas("ARETUSA WASTEWATER RECLAMATION PLANT", (new Vector3(-0.087f, 0.01f, 0.664f) * scale), scale, 0.1f, 0.05f, 0.01f, reusedObject_layer3, 0);
        createDescriptionCanvas("CECINA WASTEWATER TREATMENT PLANT", (new Vector3(0.1997f, 0.01f, 0.2538f) * scale), scale, 0.1f, 0.05f, 0.01f, reusedObject_layer3, 1);
        createDescriptionCanvas("ROSIGNANO WASTEWATER TREATMENT PLANT", (new Vector3(-0.212f, 0.01f, 0.742f) * scale), scale, 0.1f, 0.05f, 0.01f, reusedObject_layer3, 2);
        createDescriptionCanvas("SOLVAY", (new Vector3(-0.067f, 0.01f, 0.732f) * scale), scale, 0.05f, 0.025f, 0.01f, reusedObject_layer3, 3);

        //municipality canvases, rosignano 4, cecina 5
        createDescriptionCanvas("ROSIGNANO MUNICIPALITY", new Vector3(0f, 0.01f, 0.9f) * scale, scale * 3f, 0.05f, 0.025f, 0.005f, reusedObject_layer3, 4);
        createDescriptionCanvas("CECINA MUNICIPALITY", new Vector3(0.2f, 0.01f, 0.7f) * scale, scale * 3f, 0.05f, 0.025f, 0.005f, reusedObject_layer3, 5);

        createPictureCanvas(solvay_image, solvay_location[0] + (new Vector3(0.05f, 0.01f, 0.03f) * scale), scale * 1.5f, reusedObject_layer3);
        createPictureCanvas(asa_image, new Vector3(0.13f, 0.01f, 0.9f) * scale, scale * 3f, reusedObject_layer3);
        createPictureCanvas(asa_image, new Vector3(0.2f, 0.01f, 0.77f) * scale, scale * 3f, reusedObject_layer3);

        //creating paths with arrows
        createArrowPaths(new Vector3(0.1997f, 0.01f, 0.2538f) * scale, new Vector3(-0.087f, 0.01f, 0.664f) * scale, 0.2f, scale, arrowPathObject_layer3, arrow_parent_layer3);
        createArrowPaths(new Vector3(-0.087f, 0.01f, 0.664f) * scale, new Vector3(-0.067f, 0.01f, 0.732f) * scale, -0.01f, scale, arrowPathObject_layer3, arrow_parent_layer3);
        createArrowPaths(new Vector3(-0.067f, 0.01f, 0.732f) * scale, new Vector3(0f, 0.01f, 0.9f) * scale, -0.02f, scale, arrowPathObject_layer3, arrow_parent_layer3);
        createArrowPaths(new Vector3(0f, 0.01f, 0.9f) * scale, new Vector3(-0.212f, 0.01f, 0.742f) * scale, -0.02f, scale, arrowPathObject_layer3, arrow_parent_layer3);
        createArrowPaths(new Vector3(-0.212f, 0.01f, 0.742f) * scale, new Vector3(-0.087f, 0.01f, 0.664f) * scale, -0.02f, scale, arrowPathObject_layer3, arrow_parent_layer3);

        createArrowPaths(new Vector3(-0.067f, 0.01f, 0.732f) * scale, new Vector3(0.2f, 0.01f, 0.7f) * scale, 0.2f, scale, arrowPathObject_layer3, arrow_parent_layer3);
        createArrowPaths(new Vector3(0.2f, 0.01f, 0.7f) * scale, new Vector3(0.1997f, 0.01f, 0.2538f) * scale, 0.2f, scale, arrowPathObject_layer3, arrow_parent_layer3);

        //----------P1------------
        //flowmeter
        flowmeter_locations = callibratePoints(flowmeter_locations);
        createPointObjects(flowmeter_locations, flowmeter_prefab, scale * 0.01f, P1_Object);
        //conductivity
        conductivity_locations = callibratePoints(conductivity_locations);
        createPointObjects(conductivity_locations, conductivity_prefab, scale * 0.01f, P1_Object);
        //reused objects for layer 4
        createPointObjects(reused_locations, reused_prefab, scale * 0.01f, P1_Object);
        //description canvases
        createDescriptionCanvas("ARETUSA WASTEWATER RECLAMATION PLANT", (new Vector3(-0.087f, 0.01f, 0.664f) * scale), scale, 0.1f, 0.05f, 0.01f, P1_Object, 0);
        createDescriptionCanvas("CECINA WASTEWATER TREATMENT PLANT", (new Vector3(0.1997f, 0.01f, 0.2538f) * scale), scale, 0.1f, 0.05f, 0.01f, P1_Object, 1);
        createDescriptionCanvas("ROSIGNANO WASTEWATER TREATMENT PLANT", (new Vector3(-0.212f, 0.01f, 0.742f) * scale), scale, 0.1f, 0.05f, 0.01f, P1_Object, 2);
        //images
        createPictureCanvas(P1_picture6, new Vector3(0.0738f, 0.01f, 0.1958f) * scale, scale * 1.5f, P1_Object);
        createPictureCanvas(P1_picture6, new Vector3(0.0912f, 0.01f, 0.3081f) * scale, scale * 1.5f, P1_Object);
        createPictureCanvas(P1_picture6, new Vector3(-0.0178f, 0.01f, 0.419f) * scale, scale * 1.5f, P1_Object);
        createPictureCanvas(P1_picture6, new Vector3(-0.064f, 0.01f, 0.5901f) * scale, scale * 1.5f, P1_Object);
        createPictureCanvas(P1_picture6, new Vector3(-0.111f, 0.01f, 0.721f) * scale, scale * 1.5f, P1_Object);
        createPictureCanvas(P1_picture6, new Vector3(-0.25f, 0.01f, 0.812f) * scale, scale * 1.5f, P1_Object);
        createPictureCanvas(P1_picture6, new Vector3(-0.338f, 0.01f, 0.905f) * scale, scale * 1.5f, P1_Object);

        createPictureCanvas(P1_picture2, new Vector3(0.097f, 0.01f, 0.861f) * scale, scale * 5f, P1_Object);
        createPictureCanvas(P1_picture3, new Vector3(0.225f, 0.01f, 0.7f) * scale, scale * 5f, P1_Object);
        createPictureCanvas(P1_picture4, new Vector3(0.043f, 0.01f,  0.7f) * scale, scale * 5f, P1_Object);
        createPictureCanvas(P1_picture5, new Vector3(0.071f, 0.01f, 0.512f) * scale, scale * 5f, P1_Object);
        //legend
        var legend = Instantiate(legendCanvasPrefab_P1);
        legend.transform.SetParent(P1_Object.transform, true);
        legend.transform.localScale = Vector3.one * scale;
        legend.transform.position = new Vector3(-0.162f, 0.01f, 0.178f) * scale;
        legend.name = "Legend";

        //----------------P2-------------------
        //solvay
        createPointObjects(solvay_location, reused_prefab, scale * 0.01f, P2_Object);
        createPointObjects(new [] {reused_locations[1]}, reused_prefab, scale * 0.01f, P3_Object);

        createPictureCanvas(P2_picture2, new Vector3(0.116f, 0.01f, 0.887f) * scale, scale * 15f, P2_Object);
        createPictureCanvas(P2_picture3, new Vector3(0.128f, 0.01f, 0.697f) * scale, scale * 10f, P2_Object);
        createPictureCanvas(P2_picture4, new Vector3(-0.227f, 0.01f, 0.822f) * scale, scale * 10f, P2_Object);
        createPictureCanvas(P2_picture5, new Vector3(0.009f, 0.01f, 0.485f) * scale, scale * 10f, P2_Object);

        createDescriptionCanvas("SOLVAY", (new Vector3(-0.073f, 0.01f, 0.764f) * scale), scale, 0.05f, 0.025f, 0.01f, P2_Object, 0);
        createDescriptionCanvas("ARETUSA WATER RECLAMATION PLANT", (new Vector3(-0.135f, 0.01f, 0.673f) * scale), scale, 0.1f, 0.05f, 0.01f, P2_Object, 1);
        createDescriptionCanvas("AOP pilot plant", (new Vector3(-0.185f, 0.01f, 0.956f) * scale), scale, 0.08f, 0.025f, 0.01f, P2_Object, 2);
        createDescriptionCanvas("Softening & clariflocculation pilot plant", (new Vector3(0.197f, 0.01f, 0.937f) * scale), scale, 0.15f, 0.03f, 0.01f, P2_Object, 3);
        createDescriptionCanvas("Adsorption pilot plant", (new Vector3(0.191f, 0.01f, 0.77f) * scale), scale, 0.13f, 0.025f, 0.01f, P2_Object, 4);
        createDescriptionCanvas("RO pilot plant", (new Vector3(0.103f, 0.01f, 0.616f) * scale), scale, 0.07f, 0.025f, 0.01f, P2_Object, 5);

        createArrowPaths((new Vector3(-0.135f, 0.01f, 0.673f) * scale), new Vector3(-0.227f, 0.01f, 0.822f) * scale, 0.2f, scale, arrowPathObject_layer4, arrow_parent_layer4);
        createArrowPaths((new Vector3(-0.135f, 0.01f, 0.673f) * scale), new Vector3(0.116f, 0.01f, 0.887f) * scale, 0.2f, scale, arrowPathObject_layer4, arrow_parent_layer4);
        createArrowPaths((new Vector3(-0.135f, 0.01f, 0.673f) * scale), new Vector3(0.128f, 0.01f, 0.697f) * scale, 0.2f, scale, arrowPathObject_layer4, arrow_parent_layer4);
        createArrowPaths((new Vector3(-0.135f, 0.01f, 0.673f) * scale), new Vector3(0.009f, 0.01f, 0.485f) * scale, -0.2f, scale, arrowPathObject_layer4, arrow_parent_layer4);

        
        //areas
        // area210 = callibrateSurfaces(area210);
        // createSurfaceObjects(area210, surface_prefab, P2_Object);

        //-----------------P3----------------
        createPointObjects(solvay_location, reused_prefab, scale * 0.01f, P3_Object);
        createPictureCanvas(P3_picture1, new Vector3(-0.073f, 0.01f, 0.834f) * scale, scale * 4f, P3_Object);
        createPictureCanvas(P3_picture2, new Vector3(-0.193f, 0.01f, 0.831f) * scale, scale * 3f, P3_Object);
        createPictureCanvas(P3_picture3, new Vector3(-0.187f, 0.01f, 0.712f) * scale, scale * 3f, P3_Object);
        createPictureCanvas(P3_picture4, new Vector3(-0.077f, 0.01f, 0.735f) * scale, scale * 3f, P3_Object);
        createPictureCanvas(P3_picture5, new Vector3(-0.014f, 0.01f, 0.688f) * scale, scale * 2f, P3_Object);
        createPictureCanvas(P3_picture6, new Vector3(0.059f, 0.01f, 0.866f) * scale, scale * 6f, P3_Object);
        createPictureCanvas(P3_picture7, new Vector3(0.205f, 0.01f, 0.861f) * scale, scale * 4f, P3_Object);
        createPictureCanvas(P3_picture8, new Vector3(0.208f, 0.01f, 0.612f) * scale, scale * 4f, P3_Object);
        createPictureCanvas(P3_picture9, new Vector3(0.093f, 0.01f, 0.527f) * scale, scale * 4f, P3_Object);
        createPictureCanvas(P3_picture10, new Vector3(0.255f, 0.01f, 0.343f) * scale, scale * 3f, P3_Object);
        createPictureCanvas(P3_picture2, new Vector3(0.173f, 0.01f, 0.27f) * scale, scale * 3f, P3_Object);

        //scale the infopanels, original width = 1, height = 0.5
        worldCanvas.transform.position -= new Vector3(0f, worldCanvas.GetComponent<RectTransform>().sizeDelta.y*(1-(aspect_ratio*actualHeight))/2f, 0f);
        foreach(GameObject infoPanel in appPanel.GetComponent<ButtonsController>().infoPanels){
            infoPanel.transform.localScale = Vector3.one * aspect_ratio*actualHeight;
        }
        appPanel.GetComponent<ButtonsController>().layers_canvas[2].transform.GetChild(0).transform.localScale = Vector3.one * aspect_ratio*actualHeight;

        if(dropdown.value == 0)
            languageChanger.changetoItalian();
    }

    private void createDescriptionCanvas(string text, Vector3 position, float scale, float w, float h, float textSize, GameObject parent, int index){
        var canvas = Instantiate(descriptionCanvas_prefab);
        canvas.transform.SetParent(parent.transform, true);
        canvas.transform.position = position;
        canvas.GetComponent<Canvas>().sortingOrder = 1;
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
        canvas.transform.localScale = Vector3.one * scale;
        canvas.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
        canvas.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = textSize;
        canvas.name = parent.name + index.ToString();
    }

    private void createPictureCanvas(Sprite source, Vector3 position, float scale, GameObject parent){
        var image = Instantiate(imageCanvas_prefab);
        image.transform.SetParent(parent.transform, true);
        image.transform.position = position;
        image.GetComponent<Canvas>().sortingOrder = -1;
        image.transform.localScale = Vector3.one * scale;
        image.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = source;
    }

    private void createArrowPaths(Vector3 from, Vector3 to, float curvature, float scale, GameObject parent, GameObject arrow_parent){
        curvature *= scale;
        var path = Instantiate(path_prefab);
        var arrow = Instantiate(arrow_prefab);
        arrow.transform.SetParent(arrow_parent.transform, true);
        arrow.transform.localScale = Vector3.one * scale;
        path.transform.SetParent(parent.transform, true);
        path.GetComponent<LineRenderer>().material = new Material (Shader.Find ("Sprites/Default"));
        path.GetComponent<LineRenderer>().material.color = Color.green; 
        //get the points for the bezier curve
        Vector3[] anchors = new Vector3[3];
        anchors[0] = from;
        Vector3 direction = to - from;
        Vector3 normal = new Vector3(-direction.z, 0f, direction.x);
        normal.Normalize();
        anchors[1] = (from + to)/2 + normal * curvature;
        anchors[2] = to;
        arrow.GetComponent<ArrowProps>().points = anchors;
        arrow.GetComponent<ArrowProps>().t=Random.Range(0f, 1f);

        LineRenderer lr = path.GetComponent<LineRenderer>();
        lr.positionCount=10;
        lr.startWidth=0.001f;
        lr.endWidth=0.001f;
        Vector3[] linePos=new Vector3[lr.positionCount];
        for(int i = 0; i < lr.positionCount; i++){
            linePos[i]=(arrow.GetComponent<ArrowProps>().curve((float)i/(lr.positionCount-1)));
        }
        lr.numCornerVertices=10;
        lr.SetPositions(linePos);        
    }

    private void animateArrow(GameObject arrow){
        if(arrow.GetComponent<ArrowProps>().t<=1.0f){
            arrow.GetComponent<ArrowProps>().t+=0.005f;
            arrow.transform.localPosition = arrow.GetComponent<ArrowProps>().curve(arrow.GetComponent<ArrowProps>().t);
            //arrow rotation
            // Vector3 up=Quaternion.AngleAxis(2f, arrow.GetComponent<ArrowProps>().curveDerivative(arrow.GetComponent<ArrowProps>().t))*arrow.GetComponent<ArrowProps>().up;
            // arrow.GetComponent<ArrowProps>().up=up;
            // Debug.Log(arrow.GetComponent<ArrowProps>().curveDerivative(arrow.GetComponent<ArrowProps>().t));
            arrow.transform.localRotation = Quaternion.LookRotation(arrow.GetComponent<ArrowProps>().curveDerivative(arrow.GetComponent<ArrowProps>().t), Vector3.up)*Quaternion.Euler(90,90,0);
        }else arrow.GetComponent<ArrowProps>().t=0.0f;
    }

    private List<Vector3[]> callibrateLines(List<Vector3[]> lines){
        for (int i = 0; i < lines.Count; i++){
            for (int j = 0; j < lines[i].Length; j++){
                lines[i][j] -= origin;
                
                //scale to unit square
                lines[i][j].x /= width;
                lines[i][j].z /= height;
                //scale to rectangle with height = actualHeight
                lines[i][j].z *= actualHeight;
                lines[i][j].x *= aspect_ratio*actualHeight;
            }
        }

        return lines;
    }

    private void createLineObjects(List<Vector3[]> lines, GameObject prefab, GameObject parent_object){
        // int k=0;
        for (int i = 0; i < lines.Count; i++){
            if(lines[i].Length > 2){
                // k++;
                var instance = Instantiate(prefab);
                instance.transform.SetParent(parent_object.transform, true);

                LineRenderer lr = instance.GetComponent<LineRenderer>();
                lr.positionCount=lines[i].Length;
                lr.startWidth=0.001f;
                lr.endWidth=0.001f;
                lr.SetPositions(lines[i]);
            }
        }
        // Debug.Log(k);
    }

    private List<Vector2[]> callibrateSurfaces(List<Vector2[]> s2D){
        for (int i = 0; i < s2D.Count; i++){
            for (int j = 0; j < s2D[i].Length; j++){
                s2D[i][j] -= new Vector2(origin.x, origin.z);
                
                //scale to unit square
                s2D[i][j].x /= width;
                s2D[i][j].y /= height;
                //scale to rectangle with height = actualHeight
                s2D[i][j].y *= actualHeight;
                s2D[i][j].x *= aspect_ratio*actualHeight;
            }
        }

        return s2D;
    }

    private void createSurfaceObjects(List<Vector2[]> s2D, GameObject prefab, GameObject parent_object){
        // Use the triangulator to get indices for creating triangles
        Triangulator tr;
        Mesh msh;

        for (int i = 0; i < s2D.Count; i++){
            tr = new Triangulator(s2D[i]);
            msh = new Mesh();
            int[] indices = tr.Triangulate();
            // Create the Vector3 vertices
            Vector3[] vertices = new Vector3[s2D[i].Length];
            for (int j = 0; j < vertices.Length; j++) {
                vertices[j] = new Vector3(s2D[i][j].x, 0f, s2D[i][j].y);
            }

            // Set up game object with mesh;
            var instance = Instantiate(prefab);
            instance.transform.SetParent(parent_object.transform, true);

            msh.vertices = vertices;
            msh.triangles = indices;
            msh.RecalculateNormals();
            msh.RecalculateBounds();

            instance.GetComponent<MeshFilter>().mesh = msh;
        }

        parent_object.SetActive(false);

        // Vector3[] vertices = new Vector3[s2D[0].Length];
        // for (int j = 0; j < vertices.Length; j++) {
        //     vertices[j] = new Vector3(s2D[0][j].x, 0f, s2D[0][j].y);
        // }
        // rectangle.AddComponent<LineRenderer>();
        // LineRenderer lr = rectangle.GetComponent<LineRenderer>();
        // lr.positionCount=s2D[0].Length;
        // lr.startWidth=0.002f;
        // lr.endWidth=0.002f;
        // lr.SetPositions(vertices);
    }

    private Vector3[] callibratePoints(Vector3[] raw_points){
        for (int i = 0; i < raw_points.Length; i++){
            raw_points[i] -= origin;
            
            //scale to unit square
            raw_points[i].x /= width;
            raw_points[i].z /= height;
            //scale to rectangle with height = actualHeight
            raw_points[i].z *= actualHeight;
            raw_points[i].x *= aspect_ratio*actualHeight;
        }
        
        return raw_points;
    }

    private void createPointObjects(Vector3[] locations, GameObject prefab, float scale, GameObject parent_object){
        for (int i = 0; i < locations.Length; i++){
            var instance = Instantiate(prefab);
            instance.transform.SetParent(parent_object.transform, true);
            instance.transform.localPosition = locations[i];
            instance.transform.localScale = Vector3.one * scale;
        }
        parent_object.SetActive(false);
    }

    public void createRectangle(){
        corners = callibratePoints(corners);
        // Debug.Log((corners[2].x - corners[0].x) + ", " + (corners[2].z - corners[0].z));
        rectangle.AddComponent<LineRenderer>();
        LineRenderer lr = rectangle.GetComponent<LineRenderer>();
        lr.useWorldSpace =false;
        lr.positionCount=5;
        lr.startWidth=0.002f;
        lr.endWidth=0.002f;
        lr.SetPositions(corners);
        lr.material = rectangleMaterial;
    }

    public void changeLanguage(int index){
        if(index == 0){
            languageChanger.changetoItalian();
        }else{
            languageChanger.changetoEnglish();
        }
    }

    public void changeLanguage_welcomePanel(int index){
        if(index == 0){
            languageChanger.changetoItalian_welcomePanel();
        }else{
            languageChanger.changetoEnglish_welcomePanel();
        }
    }
}
