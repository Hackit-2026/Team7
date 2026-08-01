using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("ï\é¶ÇêÿÇËë÷Ç¶ÇÈÉpÉlÉã")]
    public GameObject title;
    public GameObject Game_Select;
    public GameObject Difficulty_Level;

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
}
