using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SwordSoundEffect : MonoBehaviour
{
    private AudioSource audioSource;
    private Collider swordCollider;
    private bool wasColliderEnabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        swordCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (swordCollider != null)
        {
            // コライダーが「OFFからONに切り替わった瞬間」（＝剣を振って判定が出た瞬間）を検出
            if (!wasColliderEnabled && swordCollider.enabled)
            {
                audioSource.Play();
            }
            wasColliderEnabled = swordCollider.enabled;
        }
    }
}