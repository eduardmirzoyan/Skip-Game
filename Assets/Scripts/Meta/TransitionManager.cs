using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Data")]
    [SerializeField] private float transitionTime = 1f;

    private Coroutine coroutine;
    public static TransitionManager instance;
    private void Awake()
    {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        animator = GetComponentInChildren<Animator>();
    }

    public int GetSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void OpenScene()
    {
        // Play animation
        animator.Play("Transition In");

        // Play background music
        AudioManager.instance.Play("Background " + GetSceneIndex());
    }

    public void LoadNextScene()
    {
        int index = GetSceneIndex();

        // Stop any background music
        AudioManager.instance.Stop("Background " + index);

        // Stop any transition if one was happening
        if (coroutine != null) StopCoroutine(coroutine);

        // Transition to next scene
        coroutine = StartCoroutine(LoadScene(index + 1));
    }

    public void LoadPreviousScene()
    {
        int index = GetSceneIndex();

        // Stop any background music
        AudioManager.instance.Stop("Background " + index);

        // Stop any transition if one was happening
        if (coroutine != null) StopCoroutine(coroutine);

        // Transition to next scene
        coroutine = StartCoroutine(LoadScene(index - 1));
    }

    public void ReloadScene()
    {
        int index = GetSceneIndex();

        // Stop any background music
        AudioManager.instance.Stop("Background " + index);

        // Stop any transition if one was happening
        if (coroutine != null) StopCoroutine(coroutine);

        // Transition to same scene
        coroutine = StartCoroutine(LoadScene(index));
    }

    public void LoadSceneAt(int nextIndex)
    {
        int index = GetSceneIndex();

        // Stop any background music
        AudioManager.instance.Stop("Background " + index);

        // Stop any transition if one was happening
        if (coroutine != null) StopCoroutine(coroutine);

        // Transition to next scene
        coroutine = StartCoroutine(LoadScene(nextIndex));
    }

    public void LoadMainMenuScene()
    {
        // Stop any background music
        AudioManager.instance.Stop("Background " + GetSceneIndex());

        // Stop any transition if one was happening
        if (coroutine != null) StopCoroutine(coroutine);

        // Transition to main menu, scene 0
        coroutine = StartCoroutine(LoadScene(0));
    }

    private IEnumerator LoadScene(int index)
    {
        // Play animation
        animator.Play("Transition Out");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load scene
        SceneManager.LoadScene(index);
    }
}