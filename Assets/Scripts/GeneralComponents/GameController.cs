using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [HideInInspector] public int deaths = 0;
    public Transform spawnPoint;
    public Fader fader;
   
    public int DeathCount
    {
        get => deaths;
        private set => deaths = value;
    }
    public void AddDeath()
    {
        deaths++;
    }
    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(StartRoutine());
    }

    public void LoseGame()
    {
        StartCoroutine(LoseRoutine());
    }

    public IEnumerator StartRoutine()
    {
        yield return StartCoroutine(fader.Fade(false));

        fader.gameObject.SetActive(false);
    }

    public IEnumerator LoseRoutine()
    {
        fader.gameObject.SetActive(true);
        fader.reload.SetActive(true);

        yield return StartCoroutine(fader.Fade(true));
        
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
