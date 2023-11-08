using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectController : MonoBehaviour
{
    Data data;
    Vector3[] ground_locations;
    Vector3[] reused_locations;
    Vector3[] solvay_location;
    Vector3[] urban_locations;
    Vector3[] agriculture_locations;
    Vector3[] industry_locations;
    List<Vector2[]> surfaces2D;
    List<Vector3[]> piping_cecina;
    List<Vector3[]> piping_rosignano;
    List<Vector2[]> cecina_mun_surface;
    List<Vector2[]> rosignano_mun_surface;
    List<Vector2[]> sea;
    Vector3 lower_left, upper_right;
    public float groundObjectScale;
    public GameObject ground_prefab, groundObject,
    surface_prefab, surfaceObject, rectangle,
    reused_prefab, reusedObject, reusedObject_layer3,
    urban_prefab, urbanObject,
    agriculture_prefab, agricultureObject,
    industry_prefab, industryObject,
    piping_cecina_prefab, piping_cecina_Object,
    piping_rosignano_prefab, piping_rosignano_Object,
    cecina_mun_surface_prefab, mun_surface_Object,
    rosignano_mun_surface_prefab,
    sea_prefab, sea_Object,
    arrow_prefab, path_prefab, arrow_parent_layer1, arrow_parent_layer3, arrowPathObject_layer1, arrowPathObject_layer3;
    public GameObject[] description_canvases_layer1, description_canvases_layer3;
    public GameObject appPanel, app, worldCanvas;
    Vector3[] corners;


    public float actualHeight;
    private Vector3 origin;
    private float width, height, aspect_ratio;

    void Start()
    {
        data = new Data();
        lower_left = new Vector3(1613562.9126609f, 0f, 4790557.9631989552f);
        upper_right = new Vector3(1625967.70790374418720603f, 0f, 4809329.91194767318665981f);
        corners = new [] {lower_left, new Vector3(upper_right.x, 0f, lower_left.z), upper_right, new Vector3(lower_left.x, 0f, upper_right.z), lower_left};
        origin = new Vector3(lower_left.x+ (upper_right.x - lower_left.x)/2f, 0f, lower_left.z);
        lower_left -= origin;
        upper_right -= origin;
        width = upper_right.x - lower_left.x;
        height = upper_right.z - lower_left.z;
        aspect_ratio = width/height;

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

        // app.GetComponent<Calibration>().calibrate();
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
    }

    public void callibrateCreate(){
        //ground
        ground_locations = callibratePoints(ground_locations);
        createPointObjects(ground_locations, ground_prefab, groundObject);

        //sea
        sea = callibrateSurfaces(sea);
        createSurfaceObjects(sea, sea_prefab, sea_Object);

        //surface
        surfaces2D = callibrateSurfaces(surfaces2D);
        createSurfaceObjects(surfaces2D, surface_prefab, surfaceObject);

        //reused
        reused_locations = callibratePoints(reused_locations);
        createPointObjects(reused_locations, reused_prefab, reusedObject);
        //canvases associated to reused locations
        //cecina 1, rosignano 2, aretusa 0
        description_canvases_layer1[0].transform.position = reused_locations[0] + new Vector3(0f, 0.01f, -0.03f);
        description_canvases_layer1[1].transform.position = reused_locations[1] + new Vector3(0f, 0.01f, 0.03f);
        description_canvases_layer1[2].transform.position = reused_locations[2] + new Vector3(-0.03f, 0.01f, 0.03f);
        //creating paths with arrows
        createArrowPaths(reused_locations[1], reused_locations[0], 0.2f, arrow_prefab, path_prefab, arrowPathObject_layer1, arrow_parent_layer1);
        createArrowPaths(reused_locations[2], reused_locations[0], 0.05f, arrow_prefab, path_prefab, arrowPathObject_layer1, arrow_parent_layer1);

        //urban
        urban_locations = callibratePoints(urban_locations);
        createPointObjects(urban_locations, urban_prefab, urbanObject);

        //agriculture
        agriculture_locations = callibratePoints(agriculture_locations);
        createPointObjects(agriculture_locations, agriculture_prefab, agricultureObject);

        //industry
        industry_locations = callibratePoints(industry_locations);
        createPointObjects(industry_locations, industry_prefab, industryObject);

        //piping
        piping_cecina = callibrateLines(piping_cecina);
        createLineObjects(piping_cecina, piping_cecina_prefab, piping_cecina_Object);
        piping_rosignano = callibrateLines(piping_rosignano);
        createLineObjects(piping_rosignano, piping_rosignano_prefab, piping_rosignano_Object);
        //reused for the piping layer + solvay
        createPointObjects(reused_locations, reused_prefab, reusedObject_layer3);
        solvay_location = callibratePoints(solvay_location);
        createPointObjects(solvay_location, reused_prefab, reusedObject_layer3);
        reusedObject_layer3.SetActive(true);
        // municipality surfaces
        cecina_mun_surface = callibrateSurfaces(cecina_mun_surface);
        createSurfaceObjects(cecina_mun_surface, cecina_mun_surface_prefab, mun_surface_Object);
        rosignano_mun_surface = callibrateSurfaces(rosignano_mun_surface);
        createSurfaceObjects(rosignano_mun_surface, rosignano_mun_surface_prefab, mun_surface_Object);
        mun_surface_Object.SetActive(true);
        //canvases associated to piping locations
        //cecina 1, rosignano 2, aretusa 0, solvay 3
        description_canvases_layer3[0].transform.position = reused_locations[0] + new Vector3(0f, 0.01f, -0.03f);
        description_canvases_layer3[1].transform.position = reused_locations[1] + new Vector3(0f, 0.01f, 0.03f);
        description_canvases_layer3[2].transform.position = reused_locations[2] + new Vector3(-0.03f, 0.01f, 0.03f);
        description_canvases_layer3[3].transform.position = solvay_location[0] + new Vector3(0.04f, 0.01f, 0f);
        //municipality canvases, rosignano 4, cecina 5
        description_canvases_layer3[4].transform.position = new Vector3(-0.1f, 0.01f, 0.8f);
        description_canvases_layer3[5].transform.position = new Vector3(0.22f, 0.01f, 0.36f);
        //creating paths with arrows
        createArrowPaths(description_canvases_layer3[1].transform.position, description_canvases_layer3[0].transform.position, 0.2f, arrow_prefab, path_prefab, arrowPathObject_layer3, arrow_parent_layer3);
        createArrowPaths(description_canvases_layer3[0].transform.position, description_canvases_layer3[3].transform.position, -0.01f, arrow_prefab, path_prefab, arrowPathObject_layer3, arrow_parent_layer3);
        createArrowPaths(description_canvases_layer3[3].transform.position, description_canvases_layer3[4].transform.position, -0.02f, arrow_prefab, path_prefab, arrowPathObject_layer3, arrow_parent_layer3);
        createArrowPaths(description_canvases_layer3[4].transform.position, description_canvases_layer3[2].transform.position, -0.02f, arrow_prefab, path_prefab, arrowPathObject_layer3, arrow_parent_layer3);
        createArrowPaths(description_canvases_layer3[2].transform.position, description_canvases_layer3[0].transform.position, -0.02f, arrow_prefab, path_prefab, arrowPathObject_layer3, arrow_parent_layer3);

        createArrowPaths(description_canvases_layer3[3].transform.position, description_canvases_layer3[5].transform.position, 0.2f, arrow_prefab, path_prefab, arrowPathObject_layer3, arrow_parent_layer3);
        createArrowPaths(description_canvases_layer3[5].transform.position, description_canvases_layer3[1].transform.position, 0.2f, arrow_prefab, path_prefab, arrowPathObject_layer3, arrow_parent_layer3);

        //scale the infopanels, original width = 1, height = 0.5
        worldCanvas.transform.position -= new Vector3(0f, worldCanvas.GetComponent<RectTransform>().sizeDelta.y*(1-(aspect_ratio*actualHeight))/2f, 0f);
        foreach(GameObject infoPanel in appPanel.GetComponent<ButtonsController>().infoPanels){
            infoPanel.transform.localScale = Vector3.one * aspect_ratio*actualHeight;
        }
        appPanel.GetComponent<ButtonsController>().layers_canvas[2].transform.GetChild(0).transform.localScale = Vector3.one * aspect_ratio*actualHeight;
    }

    private void createArrowPaths(Vector3 from, Vector3 to, float curvature, GameObject arrow_prefab, GameObject path_prefab, GameObject parent, GameObject arrow_parent){
        var path = Instantiate(path_prefab);
        var arrow = Instantiate(arrow_prefab);
        arrow.transform.SetParent(arrow_parent.transform, true);
        path.transform.SetParent(parent.transform, true);
        //get the points for the bezier curve
        Vector3[] anchors = new Vector3[3];
        anchors[0] = from;
        Vector3 direction = to - from;
        Vector3 normal = new Vector3(-direction.z, 0f, direction.x);
        normal.Normalize();
        anchors[1] = (from + to)/2 + normal * curvature;
        anchors[2] = to;
        arrow.GetComponent<ArrowProps>().points = anchors;
        arrow.GetComponent<ArrowProps>().t=0.0f;

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
            arrow.transform.localRotation = Quaternion.LookRotation(arrow.GetComponent<ArrowProps>().curveDerivative(arrow.GetComponent<ArrowProps>().t), Vector3.up)*Quaternion.Euler(90,180,0);
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

    private void createPointObjects(Vector3[] locations, GameObject prefab, GameObject parent_object){
        for (int i = 0; i < locations.Length; i++){
            var instance = Instantiate(prefab);
            instance.transform.SetParent(parent_object.transform, true);
            instance.transform.localPosition = locations[i];
            instance.transform.localScale = Vector3.one * groundObjectScale;
        }
        parent_object.SetActive(false);
    }

    public void createRectangle(){
        corners = callibratePoints(corners);
        rectangle.AddComponent<LineRenderer>();
        LineRenderer lr = rectangle.GetComponent<LineRenderer>();
        lr.useWorldSpace =false;
        lr.positionCount=5;
        lr.startWidth=0.002f;
        lr.endWidth=0.002f;
        lr.SetPositions(corners);
    }
}
