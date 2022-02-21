using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private GameManager gameManager;
    private LevelManager levelManager;

    [SerializeField] private Animator animator;
    private float transitionTime = 2f;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        levelManager = FindObjectOfType<LevelManager>();
        //animator = GetComponentInChildren<Animator>();
    }

    public void LoadLevel(int levelIndex)
    {
        StartCoroutine(Load(levelIndex));
    }

    private IEnumerator Load(int levelIndex)
    {
        // Play animation
        animator.SetTrigger("Start");

        //wait
        yield return new WaitForSeconds(transitionTime);

        //load scene
        SceneManager.LoadScene(levelIndex);
    }
}
