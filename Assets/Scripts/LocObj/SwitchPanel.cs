using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SwitchPanel : MonoBehaviour
{
    private Animator anim;
    public string[] aims;
    private bool near;

    public ScriptEvent scriptEvent;
    public DialogEdgesActivator DEA;
    public GameObject[] swapObjs;
    public GameObject[] destroyObj;
    public GameObject[] spawnObj;
    public GatesCntrl[] gates;
    public GameObject emailBot;
    public AudioClip emailBotDeadSound;
    public List<Transform> emailBotPoints;
    public List<Transform> trollEnemyPoints;
    public GameObject trollEnemy;
    public AudioClip trollEnemyHitSound;
    public Transform objSpawnPoint;
    public int spawnLimit;
    private int showCount;
    public bool spawnTrollEnemy;
    private int spawnValue;

    private AudioSource audioS;
    public AudioClip press_sound;

    public bool checkGradualPanel;
    public GradualPanel gradualPanel;

    public TMP_Text cubeStationText;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        near = false;
        spawnValue = 0;

        if (cubeStationText != null)
        {
            showCount = spawnLimit;
            cubeStationText.text = Convert.ToString(showCount);
        }
    }

    private void Update()
    {
        if (checkGradualPanel)
        {
            if (!gradualPanel.panelActive)
            {
                return;
            }
        }

        if (near == true && Input.GetKeyDown(KeyCode.F))
        {
            Switch();
        }
    }

    private void Switch()
    {
        if (anim.GetBool("Switch") == false)
        {
            anim.SetBool("Switch", true);
        }
        else
        {
            anim.SetBool("Switch", false);
        }

        audioS.PlayOneShot(press_sound, audioS.volume);

        for (int i = 0; i < aims.Length; i++)
        {
            switch (aims[i])
            {
                case ("Swap"):
                    for (int l = 0; l < swapObjs.Length; l++)
                    {
                        if (swapObjs[l].activeInHierarchy)
                        {
                            swapObjs[l].SetActive(false);
                        }
                        else
                        {
                            swapObjs[l].SetActive(true);
                        }
                    }
                    break;

                case ("Spawn"):

                    if (spawnLimit != 0)
                    {
                        if (spawnValue >= spawnLimit)
                        {
                            return;
                        }
                        else
                        {
                            if (cubeStationText != null)
                            {
                                showCount--;
                                cubeStationText.text = Convert.ToString(showCount);
                            }

                            if (spawnTrollEnemy)
                            {
                                spawnTrollEnemy = false;
                                GameObject spawnedTrollEnemy = Instantiate(trollEnemy, new Vector2(objSpawnPoint.position.x, objSpawnPoint.position.y), trollEnemy.transform.rotation);
                                spawnedTrollEnemy.GetComponent<SpawnEnemy>().points = trollEnemyPoints;
                                spawnedTrollEnemy.GetComponent<SpawnEnemy>().hit = trollEnemyHitSound;
                                spawnedTrollEnemy.GetComponent<AudioSource>().volume = GameObject.Find("AudioManager").GetComponent<BGmusic>().soundEffectsFloat;
                            }
                            else
                            {
                                for (int a = 0; a < spawnObj.Length; a++)
                                {
                                    GameObject spawnedObject = Instantiate(spawnObj[a], new Vector2(objSpawnPoint.position.x, objSpawnPoint.position.y), spawnObj[a].transform.rotation);

                                    if (spawnedObject.TryGetComponent(out AudioSource audio))
                                    {
                                        audio.volume = GameObject.Find("AudioManager").GetComponent<BGmusic>().soundEffectsFloat;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int a = 0; a < spawnObj.Length; a++)
                        {
                            Instantiate(spawnObj[a], objSpawnPoint.position, spawnObj[a].transform.rotation);
                        }
                    }
                    spawnValue++;
                    break;

                case ("Destroy"):

                    for (int m = 0; m < destroyObj.Length; m++)
                    {
                        Destroy(destroyObj[m]);
                    }

                    for (int m = 0; m < aims.Length; m++)
                    {
                        if (aims[i] == "Destroy")
                        {
                            aims[i] = "DontDestroy";
                        }
                    }

                    break;

                case "CutSceneActivate":

                    DEA.EdgesActivate();
                    DEA.DialogActivate();

                    for (int m = 0; m < aims.Length; m++)
                    {
                        if (aims[i] == "CutSceneActivate")
                        {
                            aims[i] = "DontSceneActivate";
                        }
                    }

                    break;

                case "ScriptEvent":

                    scriptEvent.StartEvent();

                    for (int m = 0; m < aims.Length; m++)
                    {
                        if (aims[i] == "ScriptEvent")
                        {
                            aims[i] = "DontScriptEvent";
                        }
                    }

                    break;

                case "ActivateGates":

                    for (int m = 0; m < gates.Length; m++)
                    {
                        gates[m].Unlocked();
                    }

                    for (int m = 0; m < aims.Length; m++)
                    {
                        if (aims[m] == "ActivateGates")
                        {
                            aims[m] = "DontActivateGates";
                        }
                    }

                    break;

                case "SpawnEmailBot":

                    if (spawnValue >= spawnLimit)
                    {
                        return;
                    }

                    GameObject spawnedEmailBot = Instantiate(emailBot, new Vector2(objSpawnPoint.position.x, objSpawnPoint.position.y), emailBot.transform.rotation);
                    spawnedEmailBot.GetComponent<SpawnEmailBot>().points = emailBotPoints;
                    spawnedEmailBot.GetComponent<SpawnEmailBot>().dead_Sound = emailBotDeadSound;
                    spawnedEmailBot.GetComponent<AudioSource>().volume = GameObject.Find("AudioManager").GetComponent<AudioSource>().volume;

                    spawnValue++;
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            near = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            near = false;
        }
    }

}
