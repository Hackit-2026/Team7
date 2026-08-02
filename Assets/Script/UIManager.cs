using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("表示を切り替えるパネル")]
    public GameObject title;
    public GameObject Game_Select;
    public GameObject Difficulty_Level;

    [SerializeField] private GameObject normalObject; // Normalの表示物
    [SerializeField] private GameObject hardObject;   // Hardの表示物

    private bool isHard = false; // 現在の難易度（false: Normal, true: Hard）

    private string selectedBodyPart = "";

    void Start()
    {
        Showtitle();
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
    }

    public void ShowDifficulty_Level()
    {
        title.SetActive(false);
        Game_Select.SetActive(false);
        Difficulty_Level.SetActive(true);
    }

    public void ToggleDifficulty()
    {
        isHard = !isHard; // 状態を反転

        // それぞれの表示を切り替え
        if (normalObject != null) normalObject.SetActive(!isHard);
        if (hardObject != null) hardObject.SetActive(isHard);

        // GameManager側にも現在の難易度を伝える
        if (GameManager.Instance != null)
        {
            string newDifficulty = isHard ? "Hard" : "Normal";
            GameManager.Instance.SetDifficulty(newDifficulty);
        }
    }

    public void SelectBodyPart(string bodyPart)
    {
        selectedBodyPart = bodyPart; // 例: "Arm", "Leg" などを記憶
        Debug.Log("選択された部位: " + selectedBodyPart);

        // 難易度選択画面を表示する
        ShowDifficulty_Level();
    }

    public void StartGameSceneAuto()
    {
        if (string.IsNullOrEmpty(selectedBodyPart))
        {
            Debug.LogError("部位が選択されていません！");
            return;
        }

        string sceneName = "";

        // 選択された部位（selectedBodyPart）に応じて、飛ぶシーンを分岐させる
        switch (selectedBodyPart)
        {
            case "Arm":
                sceneName = "Sword Fight"; // 実際の腕のシーン名に変更してください
                break;
            case "Leg":
                sceneName = "Jump Rope";       // 実際の脚のシーン名に変更してください
                break;
            case "Shoulder":
                sceneName = "Javelin Throw";  // 実際の肩のシーン名に変更してください
                break;
            case "Stomach":
                sceneName = "Strength Training";    // 実際の腹のシーン名に変更してください
                break;
            default:
                Debug.LogError("未知の部位が選択されています: " + selectedBodyPart);
                return;
        }

        // GameManagerに難易度を渡す処理（必要に応じて）
        if (GameManager.Instance != null)
        {
            Debug.Log("部位: " + selectedBodyPart + " / 難易度: " + GameManager.Instance.currentDifficulty + " でゲーム開始");
        }

        // シーンを遷移
        SceneManager.LoadScene(sceneName);
    }
}