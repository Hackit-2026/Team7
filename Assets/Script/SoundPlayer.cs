using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("鳴らす効果音ファイル")]
    public AudioClip jumpSound;

    void Start()
    {
        // AudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();
    }

    // ジャンプしたとき（外部やイベントから呼び出す用）
    public void PlayJumpSound()
    {
        if (jumpSound != null && audioSource != null)
        {
            // 音を1回再生する
            audioSource.PlayOneShot(jumpSound);
        }
    }
}