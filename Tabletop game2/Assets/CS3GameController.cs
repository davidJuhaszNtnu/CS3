using System.Collections;
using UnityEngine;

public class CS3GameController : MonoBehaviour
{
    public GameObject islandPortal;
    public GameObject farmPortal;
    public GameObject cityPortal;
    public GameObject industryPortal;

    public GameObject islandON;
    public GameObject farmON;
    public GameObject cityCecinaON;
    public GameObject industryRosignanoON;

    private GameObject[] portalGameObjects;
    private GameObject[] sceneGameObjects;

    private int currentPortalIndex = 0;
    private int currentSceneIndex = -1; // -1 represents no scene currently active

    private float elapsedTime = 0f;
    private bool inputEnabled = false;

    void Awake()
    {
        // Manually set the references to portal game objects
        islandPortal = GameObject.Find("islandPortal");
        farmPortal = GameObject.Find("farmPortal");
        cityPortal = GameObject.Find("cityPortal");
        industryPortal = GameObject.Find("industryPortal");

        // Manually set the references to scene game objects
        islandON = GameObject.Find("islandON");
        farmON = GameObject.Find("farmON");
        cityCecinaON = GameObject.Find("cityCecinaON");
        industryRosignanoON = GameObject.Find("industryRosignanoON");

        // Add portal game objects to the array in the desired order
        portalGameObjects = new GameObject[] { islandPortal, farmPortal, cityPortal, industryPortal };

        // Add scene game objects to the array in the desired order
        sceneGameObjects = new GameObject[] { islandON, farmON, cityCecinaON, industryRosignanoON };

        // Ensure only the first portal game object is initially active
        TogglePortalObject(currentPortalIndex);

        // Ensure no scene objects are initially active
        foreach (GameObject sceneGO in sceneGameObjects)
        {
            sceneGO.SetActive(false);
        }
    }

    void Start()
    {
        // Enable input after 1 second for the first sequence
        StartCoroutine(EnableInputAfterDelay(1f));
    }

    IEnumerator EnableInputAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        inputEnabled = true;
    }

    void Update()
    {
        // Increment elapsed time
        elapsedTime += Time.deltaTime;

        // Enable keyboard input only if input is enabled
        if (inputEnabled)
        {
            if (currentSceneIndex != -1 && Input.GetKeyDown(KeyCode.G))
            {
                ResetSequence(); // G key for resetting the sequence to the initial state
            }

            // Check for keyboard input based on the current portal
            if (Input.GetKeyDown(KeyCode.A) && currentPortalIndex == 0)
            {
                StartSceneObject(0); // A key for displaying islandON
            }
            else if (Input.GetKeyDown(KeyCode.S) && currentPortalIndex == 1)
            {
                StartSceneObject(1); // S key for displaying farmON
            }
            else if (Input.GetKeyDown(KeyCode.D) && currentPortalIndex == 2)
            {
                StartSceneObject(2); // D key for displaying cityCecinaON
            }
            else if (Input.GetKeyDown(KeyCode.F) && currentPortalIndex == 3)
            {
                StartSceneObject(3); // F key for displaying industryRosignanoON
            }
        }

        // Check for portal transitions based on elapsed time
        if (elapsedTime >= 10f && currentPortalIndex == 0)
        {
            StartPortalTransition(1); // Transition from islandPortal to farmPortal after 10 seconds
        }
        else if (elapsedTime >= 20f && currentPortalIndex == 1)
        {
            StartPortalTransition(2); // Transition from farmPortal to cityPortal after 20 seconds
        }
        else if (elapsedTime >= 30f && currentPortalIndex == 2)
        {
            StartPortalTransition(3); // Transition from cityPortal to industryPortal after 30 seconds
        }
        else if (elapsedTime >= 40f && currentPortalIndex == 3)
        {
            HidePortalShowIsland(); // Hide industryPortal and show islandPortal after 40 seconds
            HideIndustryRosignano(); // Hide industryRosignanoON after 40 seconds
        }
    }

    void StartSceneObject(int index)
    {
        // Hide the current scene object
        ToggleSceneObject(currentSceneIndex);

        // Set the specified scene object as the current one
        currentSceneIndex = index;
        ToggleSceneObject(currentSceneIndex);

        // Reset elapsed time for the next sequence
        elapsedTime = 0f;
    }

    void ResetSequence()
    {
        // Hide the current scene object
        ToggleSceneObject(currentSceneIndex);

        // Reset the current scene index to none
        currentSceneIndex = -1;

        // Hide industryRosignanoON
        industryRosignanoON.SetActive(false);

        // Reset the sequence to the initial state (islandPortal)
        currentPortalIndex = 0;
        TogglePortalObject(currentPortalIndex);

        // Enable keyboard input for the next sequence
        inputEnabled = true;
    }

    void StartPortalTransition(int index)
    {
        // Hide the current portal object
        TogglePortalObject(currentPortalIndex);

        // Set the specified portal object as the current one
        currentPortalIndex = index;
        TogglePortalObject(currentPortalIndex);

        // Reset elapsed time for the next sequence
        elapsedTime = 0f;
    }

    void HidePortalShowIsland()
    {
        // Hide the current portal object
        TogglePortalObject(currentPortalIndex);

        // Set the specified portal object as the current one (islandPortal)
        currentPortalIndex = 0;
        TogglePortalObject(currentPortalIndex);

        // Reset elapsed time for the next sequence
        elapsedTime = 0f;

        // Enable keyboard input for the next sequence
        inputEnabled = true;
    }

    void HideIndustryRosignano()
    {
        // Hide industryRosignanoON after 40 seconds
        industryRosignanoON.SetActive(false);
    }

    void TogglePortalObject(int index)
    {
        // Ensure index is within bounds
        if (index >= 0 && index < portalGameObjects.Length)
        {
            // Hide all portal objects
            foreach (GameObject portalGO in portalGameObjects)
            {
                portalGO.SetActive(false);
            }

            // Show the specified portal object
            portalGameObjects[index].SetActive(true);
        }
    }

    void ToggleSceneObject(int index)
    {
        // Ensure index is within bounds
        if (index >= 0 && index < sceneGameObjects.Length)
        {
            // Hide all scene objects
            foreach (GameObject sceneGO in sceneGameObjects)
            {
                sceneGO.SetActive(false);
            }

            // Show the specified scene object
            sceneGameObjects[index].SetActive(true);
        }
    }
}
