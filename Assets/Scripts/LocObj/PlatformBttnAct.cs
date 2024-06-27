using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBttnAct : MonoBehaviour
{
    public string[] aims;
    private Animator anim;

    public GameObject[] swapObjs;
    public DialogEdgesActivator DEA;
    public Exit exit;
    public GameObject[] activateObj;
    public GameObject[] disactivateObj;
    public GameObject[] destroyObj;
    public ScriptEvent scriptEvent;

    public int spawnLimit;
    private int spawnValue = 0;
    public GameObject[] spawnObj;
    public Transform objSpawnPoint;

    public List<Transform> emailBotPoints;
    public GameObject emailBot;
    public AudioClip emailBotDeadSound;

    public GatesCntrl[] gates;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Pressed()
    {
        anim.SetTrigger("Press");

        for (int i = 0; i < aims.Length; i++)
        {
            switch(aims[i])
            
            {

            case ("Exit"):
                exit.active = true;
                exit.anim.SetBool("OpenOn", true);
                break;

            case ("Destroy"):
                    for (int a = 0; a < destroyObj.Length; a++)
                    {
                        Destroy(destroyObj[a]);
                    }
                
                break;

            case ("CutSceneActivate"):
                    if (DEA == null)
                    {
                        return;
                    }
                DEA.DialogActivate();
                DEA.EdgesActivate();
                break;

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

            case ("Activate"):
                    for (int m = 0; m < activateObj.Length; m++)
                    {
                        if(activateObj[m].activeInHierarchy == true)
                        {
                            return;
                        }
                        else
                        {
                            activateObj[m].SetActive(true);
                        }
                    }
                break;

            case ("Disactivate"):
                    for (int m = 0; m < disactivateObj.Length; m++)
                    {
                        if (disactivateObj[m].activeInHierarchy == false)
                        {
                            return;
                        }
                        else
                        {
                            disactivateObj[m].SetActive(false);
                        }
                    }
                    break;

                case ("ScriptEvent"):

                    scriptEvent.StartEvent();

                    for (int m = 0; m < aims.Length; m++)
                    {
                        if (aims[i] == "ScriptEvent")
                        {
                            aims[i] = "DontScriptEvent";
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
                            for (int a = 0; a < spawnObj.Length; a++)
                            {
                                GameObject spawnedObject = Instantiate(spawnObj[a], new Vector2(objSpawnPoint.position.x, objSpawnPoint.position.y), spawnObj[a].transform.rotation);
                                if(spawnedObject.TryGetComponent(out AudioSource audio))
                                {
                                    audio.volume = GameObject.Find("AudioManager").GetComponent<BGmusic>().soundEffectsFloat;
                                }
                            }                           
                        }
                    }
                    else
                    {
                        for (int a = 0; a < spawnObj.Length; a++)
                        {
                            Instantiate(spawnObj[a], objSpawnPoint);
                        }
                    }
                    spawnValue++;
                    break;

                case "SpawnEmailBot":

                    if (spawnValue >= spawnLimit)
                    {
                        return;
                    }

                    GameObject spawnedEmailBot = Instantiate(emailBot, new Vector2(objSpawnPoint.position.x, objSpawnPoint.position.y), emailBot.transform.rotation);
                    spawnedEmailBot.GetComponent<SpawnEmailBot>().points = emailBotPoints;
                    spawnedEmailBot.GetComponent<SpawnEmailBot>().dead_Sound = emailBotDeadSound;
                    spawnedEmailBot.GetComponent<AudioSource>().volume = GameObject.Find("AudioManager").GetComponent<BGmusic>().soundEffectsFloat;

                    spawnValue++;
                    break;

                case "ActivateGates":

                    for (int n = 0; n < aims.Length; n++)
                    {
                        if (aims[n] == "ActivateGates")
                        {
                            aims[n] = "DontActivateGates";
                        }
                    }
                    for (int m = 0; m < gates.Length; m++)
                    {
                        gates[m].Unlocked();
                    }

                    break;
            }
        }
       
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Cube")
        {
            Pressed();
        }
    }

}
