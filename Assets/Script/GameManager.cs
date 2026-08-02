using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string currentDifficulty = "Normal";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ここの頭に 「public」 がついているか確認！
    public void SetDifficulty(string difficulty)
    {
        currentDifficulty = difficulty;
        Debug.Log("現在の難易度: " + currentDifficulty);
    }
}