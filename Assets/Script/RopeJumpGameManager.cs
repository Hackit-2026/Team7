using UnityEngine;
using TMPro;

public class RopeJumpGameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [Tooltip("Target number of jumps to clear the game")]
    [SerializeField] private int targetJumps = 10;

    [Header("UI Reference")]
    [Tooltip("Drag and drop your Canvas TextMeshPro here")]
    [SerializeField] private TextMeshProUGUI counterText;

    private int currentJumps = 0;
    private bool isGameCleared = false;

    void Start()
    {
        UpdateUI();
    }

    // Called from the Player script when a jump is detected
    public void OnPlayerJumped()
    {
        if (isGameCleared) return;

        currentJumps++;

        // 1. Check if the target is reached BEFORE updating the UI
        if (currentJumps >= targetJumps)
        {
            GameClear();
        }
        else
        {
            // 2. If not cleared yet, just update the remaining count
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (counterText == null) return;

        int remaining = targetJumps - currentJumps;
        counterText.text = "あと " + remaining + " 回！";
    }

    void GameClear()
    {
        isGameCleared = true;

        // 3. Immediately change the text to the clear message
        if (counterText != null)
        {
            counterText.text = "クリアお疲れ様！";
        }

        Debug.Log("Game Cleared!");
    }
}
