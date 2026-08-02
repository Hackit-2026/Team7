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
        selectedBodyPart = bodyPart; // 部位を記憶
        Debug.Log("選択された部位: " + selectedBodyPart);

        // シーンは変えずに、難易度選択画面を表示する
        ShowDifficulty_Level();
    }

    public void StartGameScene(string sceneName)
    {
        // GameManagerに部位のデータも渡しておきたい場合（必要に応じて）
        if (GameManager.Instance != null)
        {
            // GameManager側で部位を保持する変数があればここに渡せます
        }

        // シーンを遷移する
        SceneManager.LoadScene(sceneName);
    }
}