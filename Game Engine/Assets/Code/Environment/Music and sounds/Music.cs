using System.Collections;
using UnityEngine;

public class Music : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private AudioClip defaultSong;
    [SerializeField] private AudioClip battleSong;

    [Header("References")]
    [SerializeField] private AudioSource audioSource;

    public static Music Instance;

    private void Start()
    {
        Instance = this;

        if (defaultSong == null || battleSong == null)
        {
            Debug.Log("Null songs found. Disabling music.");
            enabled = false;
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
            StartCoroutine(startSong("default"));
        }
    }

    public void StartSong(string type)
    {
        if (enabled)
        {
            StopAllCoroutines();
            StartCoroutine(startSong(type));
        }
    }

    private IEnumerator startSong(string type)
    {
        AudioClip songSelected = type == "default" ? defaultSong : battleSong;
        float fadeInSpeed = type == "default" ? 0.1f : 0.01f;
        float fadeOutSpeed = type == "default" ? 0.005f : 0.01f;

        while (audioSource.volume > 0)
        {
            yield return new WaitForSeconds(fadeOutSpeed);
            audioSource.volume -= 0.01f;
        }

        if (audioSource.clip != songSelected)
        {
            audioSource.clip = songSelected;
            audioSource.Play();
        }

        while (audioSource.volume < 1) // 1 should be replaced wwith max music volume by player options
        {
            yield return new WaitForSeconds(fadeInSpeed);
            audioSource.volume += 0.01f;
        }
    }
}