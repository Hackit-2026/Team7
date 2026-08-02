using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Meta.XR.MRUtilityKit; // Required for MR Utility Kit (MRUK)

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject title;
    public GameObject Game_Select;
    public GameObject Difficulty_Level;

    [SerializeField] private GameObject normalObject;
    [SerializeField] private GameObject hardObject;

    [Header("MR / Surface Visual Effects")]
    [Tooltip("Optional: Particle effect spawned on real-world walls/floors when a button is clicked.")]
    [SerializeField] private GameObject clickSurfaceEffectPrefab;

    private bool isHard = false;
    private string selectedBodyPart = "";

    void Start()
    {
        Showtitle();

        // Ensure MRUK is ready in the scene
        if (MRUK.Instance == null)
        {
            Debug.LogWarning("MRUK Instance not found. Spatial features will be disabled.");
        }
    }

    public void Showtitle()
    {
        title.SetActive(true);
        Game_Select.SetActive(false);
        Difficulty_Level.SetActive(false);
    }

    public void ShowGame_Select()
    {
        title.SetActive(false);
        Game_Select.SetActive(true);
        Difficulty_Level.SetActive(false);
        TriggerSurfaceEffect(); // Spawn effect on room surface
    }

    public void ShowDifficulty_Level()
    {
        title.SetActive(false);
        Game_Select.SetActive(false);
        Difficulty_Level.SetActive(true);
        TriggerSurfaceEffect();
    }

    public void ToggleDifficulty()
    {
        isHard = !isHard;

        if (normalObject != null) normalObject.SetActive(!isHard);
        if (hardObject != null) hardObject.SetActive(isHard);

        if (GameManager.Instance != null)
        {
            string newDifficulty = isHard ? "Hard" : "Normal";
            GameManager.Instance.SetDifficulty(newDifficulty);
        }
        TriggerSurfaceEffect();
    }

    public void SelectBodyPart(string bodyPart)
    {
        selectedBodyPart = bodyPart;
        Debug.Log("Selected Part: " + selectedBodyPart);

        ShowDifficulty_Level();
    }

    public void StartGameSceneAuto()
    {
        if (string.IsNullOrEmpty(selectedBodyPart))
        {
            Debug.LogError("No body part selected!");
            return;
        }

        string sceneName = "";

        switch (selectedBodyPart)
        {
            case "Arm":
                sceneName = "Sword Fight";
                break;
            case "Leg":
                sceneName = "Jump Rope";
                break;
            case "Shoulder":
                sceneName = "Javelin Throw";
                break;
            case "Stomach":
                sceneName = "Strength Training";
                break;
            default:
                Debug.LogError("Unknown body part: " + selectedBodyPart);
                return;
        }

        if (GameManager.Instance != null)
        {
            Debug.Log("Part: " + selectedBodyPart + " / Difficulty: " + GameManager.Instance.currentDifficulty + " -> Launching Game");
        }

        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// MR Feature: Finds the closest real-world room surface (floor) and spawns a visual effect there.
    /// </summary>
    private void TriggerSurfaceEffect()
    {
        if (MRUK.Instance == null || clickSurfaceEffectPrefab == null) return;

        // Get the active room scanned by Meta Quest
        MRUKRoom currentRoom = MRUK.Instance.GetCurrentRoom();
        if (currentRoom == null) return;

        Vector3 menuPosition = transform.position;
        Ray ray = new Ray(menuPosition, Vector3.down);

        // Fix: Create assignable variables for the out parameters
        RaycastHit hit;
        MRUKAnchor anchor;

        // Correct MRUK Raycast call for Unity 6
        if (currentRoom.Raycast(ray, Mathf.Infinity, LabelFilter.Included(MRUKAnchor.SceneLabels.FLOOR), out hit, out anchor))
        {
            // Spawn particle effect right on your real living room floor!
            Instantiate(clickSurfaceEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}
