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

        // ★追加：GameManager側にも現在の難易度を伝える
        if (GameManager.Instance != null)
        {
            string newDifficulty = isHard ? "Hard" : "Normal";
            GameManager.Instance.SetDifficulty(newDifficulty);
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
