using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;
using DG.Tweening;
using System;

public class ScriptEvent : MonoBehaviour
{
    public string[] aims;
    public bool Lvl_12;

    public GameObject[] activateObjects;
    public ParticleSystem[] particleSystems;
    public ParticleSystem[] particleSystems_DisActivate;
    public GameObject[] disactivateObjects;
    public GameObject[] destroyObjects;
    public EnemySpawner[] enemySpawners;
    public CubeStation[] cubeStations;
    public GameObject[] gates;

    public GameObject[] spawnObjects;
    public Transform spawnPoint;

    public CharacterController2D charCntrl;
    public GameController gameController;
    public Transform teleportPoint;

    private BoxCollider2D boxCollider;

    private AudioSource audioS;
    [HideInInspector] public AudioManager audioManager;
    public VolumeManager volumeManager;
    [HideInInspector] public int currentTrack;
    public AudioClip[] tracks;
    public AudioSource[] stopAudioObjects;
    public bool soundActive;
    public bool dontLoopMusic;
    [HideInInspector] public BGmusic backGroundMusic;
    public float timeFadeTrack;
    public float timeExitFadeTrack;
    [HideInInspector] float volumeValue;
    public AudioClip oneShotSound;
    private bool isChangingVolume;

    public CinemachineVirtualCamera vcam;
    public float shakeValue;
    public float shakeIntensity;
    public float shakeTime;
    public float cameraZoomValue;
    public Transform followObject;

    public Animator[] animator;

    public BossCntrl_phase1 boss_phase1;
    public BossCntrl_phase2 boss_phase2;
    public GameObject[] bossHealthBar;
    public WhoThisCntrl whoThis;
    public CollectsForDamageStation collectsCntrl;

    public Exit exit;

    public DialogEdgesActivator DEA;

    public ScriptEvent scriptEvent;

    public float timerValue;
    public AudioClip timerSound;
    public TMP_Text timerText;

    public DataStreamPoint dataStreamPoint;

    private bool enemySpawnerIsPresent, triggerTouched;

    public int index;

    public Fader fader;
    public bool playerWalkAfterCompleteLvl;
    public float completeWaitTime;
    public int levelProgressIndex;
    private void Start()
    {
        if (Lvl_12)
        {
            charCntrl = GameObject.Find("Trip(Lvl 12)").GetComponent<CharacterController2D>();
        }
        else
        {
            charCntrl = GameObject.Find("Trip").GetComponent<CharacterController2D>();
        }

        volumeManager = GameObject.Find("VolumeManager").GetComponent<VolumeManager>();

        boxCollider = GetComponent<BoxCollider2D>();
        audioS = GetComponent<AudioSource>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        backGroundMusic = GameObject.Find("AudioManager").GetComponent<BGmusic>();

        if (timerText != null)
        {
            timerText.text = Convert.ToString(timerValue);
        }

        soundActive = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterController2D player) && !triggerTouched)
        {
            triggerTouched = true;
            boxCollider.enabled = false;
            StartEvent();
        }
    }

    public void StartEvent()
    {
        for (int i = 0; i < aims.Length; i++)
        {
            switch (aims[i])
            {
                case "Activate":

                    for (int m = 0; m < activateObjects.Length; m++)
                    {
                        if (activateObjects[m].activeInHierarchy != true)
                        {
                            activateObjects[m].SetActive(true);
                        }
                    }

                    break;

                case "Disactivate":

                    for (int m = 0; m < disactivateObjects.Length; m++)
                    {
                        if (disactivateObjects[m].activeInHierarchy)
                        {
                            disactivateObjects[m].SetActive(false);
                        }
                    }

                    break;

                case "Spawn":

                    for (int m = 0; m < spawnObjects.Length; m++)
                    {
                        GameObject spawnedObject = Instantiate(spawnObjects[m], spawnPoint);
                        if (spawnedObject.TryGetComponent(out AudioSource audio))
                        {
                            audio.volume = GameObject.Find("AudioManager").GetComponent<BGmusic>().soundEffectsFloat;
                        }
                    }

                    break;

                case "Destroy":

                    for (int m = 0; m < destroyObjects.Length; m++)
                    {
                        Destroy(destroyObjects[m]);
                    }

                    break;

                case "ReturnHealth":

                    for (int m = 0; m < charCntrl.healthPoint.Length; m++)
                    {
                        if (!charCntrl.healthPoint[m].activeInHierarchy)
                        {
                            charCntrl.healthPoint[m].SetActive(true);
                        }
                    }

                    charCntrl.healthPartSys.Play();
                    charCntrl.gameController.deaths = 0;
                    charCntrl.animator.SetTrigger("HealthRestored");
                    break;

                case "PlayOneShotSound":
                    audioS.PlayOneShot(oneShotSound, audioManager.GetComponent<BGmusic>().soundEffectsFloat);
                    break;

                case "Sound":

                    if (!audioManager.lvlsEnd)
                    {
                        StartCoroutine(audioManager.ExitFadeTrack(audioManager.music.clip));
                    }

                    if (!dontLoopMusic)
                    {
                        soundActive = true;
                    }

                    StartCoroutine(StartFadeEventTrack(tracks[0]));
                    break;

                case "StopOtherAudioSource":

                    for (int n = 0; n < stopAudioObjects.Length; n++)
                    {
                        if (stopAudioObjects[n].TryGetComponent(out ScriptEvent script))
                        {
                            StartCoroutine(script.ExitFadeEventTrack(script.tracks[script.currentTrack]));
                        }
                        else
                        {
                            for (int m = 0; m < stopAudioObjects.Length; m++)
                            {
                                if (stopAudioObjects[m].enabled)
                                {
                                    stopAudioObjects[m].enabled = false;
                                }

                            }
                        }
                    }

                    break;

                case "CameraZoom":
                    vcam.m_Lens.OrthographicSize = cameraZoomValue;
                    vcam.Follow = followObject;
                    break;

                case "ShakeCamera":
                    StartCoroutine(StartShake());
                    break;

                case "Animation":

                    for (int m = 0; m < animator.Length; m++)
                    {
                        animator[m].SetTrigger("Activate");
                    }

                    break;

                case "Boss_phase1":
                    boss_phase1.Activate();
                    break;

                case "BossHealthBar":

                    for (int m = 0; m < bossHealthBar.Length; m++)
                    {
                        if (bossHealthBar[m].activeInHierarchy == false)
                        {
                            bossHealthBar[m].SetActive(true);
                        }
                    }

                    break;

                case "Boss_phase2":
                    StartCoroutine(boss_phase2.Activate());
                    break;

                case "Exit":
                    exit.active = true;
                    exit.anim.SetBool("OpenOn", true);
                    break;

                case "CutSceneActivate":
                    DEA.DialogActivate();
                    DEA.EdgesActivate();
                    break;

                case "ActivateEnemySpawners":

                    for (int m = 0; m < enemySpawners.Length; m++)
                    {
                        StartCoroutine(enemySpawners[m].SpawnEnemy());
                    }

                    enemySpawnerIsPresent = true;
                    break;

                case "Teleport":
                    StartCoroutine(TeleportPlayer());
                    break;

                case "ActivateCubeStations":

                    for (int m = 0; m < cubeStations.Length; m++)
                    {
                        StartCoroutine(cubeStations[m].SpawnDelay());
                    }

                    break;

                case "OpenGates":

                    for (int m = 0; m < gates.Length; m++)
                    {
                        if (gates[m].TryGetComponent(out GatesCntrl giantGates))
                        {
                            giantGates.Unlocked();
                        }

                        if (gates[m].TryGetComponent(out SmallGates smallGates))
                        {
                            if (gates[m].GetComponent<BoxCollider2D>().enabled == false)
                            {
                                if (!smallGates.dontEnableBoxColliderAfterScriptEvent)
                                {
                                    gates[m].GetComponent<BoxCollider2D>().enabled = true;
                                }
                            }

                            if (smallGates.dontEnableBoxColliderAfterScriptEvent)
                            {
                                smallGates.UnlockGates();
                            }
                        }
                    }

                    for (int n = 0; n < aims.Length; n++)
                    {
                        if (aims[n] == "OpenGates")
                        {
                            aims[n] = "NotOpenGates";
                        }
                    }

                    break;

                case "SmallGatesLocked":

                    for (int m = 0; m < gates.Length; m++)
                    {
                        if (gates[m].TryGetComponent(out SmallGates smallGates))
                        {
                            smallGates.LockGates();
                            gates[m].GetComponent<BoxCollider2D>().enabled = false;
                        }
                    }

                    for (int n = 0; n < aims.Length; n++)
                    {
                        if (aims[n] == "SmallGatesLocked")
                        {
                            aims[n] = "NotSmallGatesLocked";
                        }
                    }

                    break;

                case "ScriptEvent":
                    scriptEvent.StartEvent();
                    break;

                case "DataStream":
                    StartCoroutine(dataStreamPoint.MoveDataStream(dataStreamPoint.moveDistance));
                    break;

                case "ActivateParticles":

                    for (int m = 0; m < particleSystems.Length; m++)
                    {
                        particleSystems[m].Play();
                    }

                    break;

                case "DisactivateParticles":

                    for (int m = 0; m < particleSystems_DisActivate.Length; m++)
                    {
                        particleSystems_DisActivate[m].Stop();
                    }

                    break;

                case "CompleteLevel":

                    if (playerWalkAfterCompleteLvl)
                    {
                        charCntrl.transform.DOMoveX(charCntrl.transform.position.x + 5, 3f);
                    }

                    StartCoroutine(CompleteLevel());
                    break;

                case "ActivateMainComputer":
                    whoThis.isActive = true;
                    whoThis.awake = true;
                    StartCoroutine(collectsCntrl.SpawnCollections());
                    break;

                case "Timer":
                    StartCoroutine(StartTimer());
                    break;
            }
        }
    }

    private void Update()
    {

        if (!isChangingVolume)
        {
            audioS.volume = volumeManager.musicSlider.value;
        }

        volumeValue = audioS.volume;

        if (!audioS.isPlaying && soundActive)
        {
            currentTrack++;
            if (currentTrack >= tracks.Length)
            {
                currentTrack = 0;
            }
            audioS.clip = tracks[currentTrack];
            audioS.Play();
        }

        if (enemySpawnerIsPresent)
        {
            for (int i = 0; i < enemySpawners.Length; i++)
            {
                if (enemySpawners[i].LimitExceeded(true))
                {
                    index = i;
                }
                else
                {
                    break;
                }

                if (i >= enemySpawners.Length - 1)
                {
                    scriptEvent.StartEvent();
                    enemySpawnerIsPresent = false;
                }
            }
        }
    }

    private IEnumerator StartFadeEventTrack(AudioClip newClip)
    {
        isChangingVolume = true;

        float timeToFade = timeFadeTrack;
        float timeElapsed = 0;

        audioS.clip = newClip;
        audioS.Play();

        while (timeElapsed < timeToFade)
        {
            audioS.volume = Mathf.Lerp(0, volumeManager.musicSlider.value, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        isChangingVolume = false;
    }

    public IEnumerator ExitFadeEventTrack(AudioClip newClip)
    {
        soundActive = false;

        float timeToFade = timeExitFadeTrack;
        float timeElapsed = 0;

        audioS.clip = newClip;

        while (timeElapsed < timeToFade)
        {
            audioS.volume = Mathf.Lerp(volumeManager.musicSlider.value, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        audioS.Stop();
        audioS.mute = true;
    }

    private IEnumerator TeleportPlayer()
    {
        gameController.fader.gameObject.SetActive(true);

        yield return StartCoroutine(gameController.fader.Fade(true));

        charCntrl.transform.position = teleportPoint.position;

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(gameController.fader.Fade(false));

        gameController.fader.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
    }

    private void ShakeCamera(float amplitudeGain, float frequencyGain)
    {
        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitudeGain;

        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequencyGain;
    }

    private IEnumerator StartShake()
    {
        ShakeCamera(shakeValue, shakeIntensity);

        yield return new WaitForSeconds(shakeTime);

        ShakeCamera(0, 0);
    }

    private IEnumerator CompleteLevel()
    {
        yield return new WaitForSeconds(completeWaitTime);

        if (PlayerPrefs.GetInt("completeLevels") < SceneManager.GetActiveScene().buildIndex)
        {
            PlayerPrefs.SetInt("completeLevels", SceneManager.GetActiveScene().buildIndex);
        }

        fader.gameObject.SetActive(true);

        yield return StartCoroutine(fader.Fade(true));

        yield return new WaitForSeconds(2f);

        PlayerPrefs.Save();

        if (PlayerPrefs.GetInt("completeLevels") >= 14 && SceneManager.GetActiveScene().buildIndex == 14)
        {
            SceneManager.LoadScene(15);
            yield break;
        }

        SceneManager.LoadScene(0);
    }

    private IEnumerator StartTimer()
    {
        float loopLenght = timerValue;

        for (int i = 0; i < loopLenght; i++)
        {
            yield return new WaitForSeconds(1);
            timerValue--;
            timerText.text = Convert.ToString(timerValue);
            audioS.PlayOneShot(timerSound, audioS.volume);
        }

        timerText.text = null;
        scriptEvent.StartEvent();
    }
}
