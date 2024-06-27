using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DataStreamPoint : MonoBehaviour
{
    public GameObject[] nextPoint;
    public GameObject[] dataStream;
    public ParticleSystem[] partSysNGO;
    public AudioSource[] gurgleSounds;
    public GameObject dataStreamCollider;
    public float upStreamTime, nextPointActivatingTime;
    public int Ydistance, divisionValue, remainder;
    public float waitUpTime, gradualStopTime;
    [HideInInspector] public Vector2 moveDistance;
    private bool playerContacts;
    public bool firstPoint;
    public DialogEdgesActivator DEA;
    private void Start()
    {
        moveDistance = new Vector2(0, transform.position.y + Ydistance);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out CharacterController2D player) && !playerContacts)
        {
            playerContacts = true;
            if(DEA != null)
            {
                StartCutScene();
            }
            StartCoroutine(MoveDataStream(moveDistance));
        }
    }

    public IEnumerator MoveDataStream(Vector2 distance)
    {
        if(Ydistance < 0)
        {
            if (Ydistance < -5)
            {
                if (Ydistance < -40)
                {
                    remainder = Ydistance % 10;
                    divisionValue = -1 * (Ydistance / 10);

                    for (int i = 0; i < divisionValue + 1; i++)
                    {
                        for (int m = 0; m < dataStream.Length; m++)
                        {
                            if (i >= divisionValue)
                            {
                                dataStream[m].transform.DOMove(new Vector2(0, remainder), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                            }
                            else
                            {
                                dataStream[m].transform.DOMove(new Vector2(0, -10), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                            }

                            yield return new WaitForSeconds(gradualStopTime);

                            if (m == 0)
                            {
                                if (i >= divisionValue)
                                {
                                    dataStreamCollider.transform.DOMove(new Vector2(0, remainder), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                                }
                                else
                                {
                                    dataStreamCollider.transform.DOMove(new Vector2(0, -10), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                                }
                            }
                        }

                        yield return new WaitForSeconds(waitUpTime);
                    }

                }
            
                else
                {
                    remainder = Ydistance % 5;
                    divisionValue = -1 * (Ydistance / 5);

                    if(firstPoint)
                    {
                        for (int i = 0; i < gurgleSounds.Length; i++)
                        {
                            gurgleSounds[i].Play();
                        }
                        StartCoroutine(activateParticleSystems());
                    }

                    for (int i = 0; i < divisionValue + 1; i++)
                    {
                        for (int m = 0; m < dataStream.Length; m++)
                        {
                            if (i >= divisionValue)
                            {
                                dataStream[m].transform.DOMove(new Vector2(0, remainder), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                            }
                            else
                            {
                                dataStream[m].transform.DOMove(new Vector2(0, -5), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                            }

                            yield return new WaitForSeconds(gradualStopTime);

                            if (m == 0)
                            {
                                if (i >= divisionValue)
                                {
                                    dataStreamCollider.transform.DOMove(new Vector2(0, remainder), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                                }
                                else
                                {
                                    dataStreamCollider.transform.DOMove(new Vector2(0, -5), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                                }
                            }
                        }

                        yield return new WaitForSeconds(waitUpTime);
                    }
                }
            }
            else
            {
                for (int i = 0; i < dataStream.Length; i++)
                {
                    dataStream[i].transform.DOMove(distance, upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);

                    yield return new WaitForSeconds(gradualStopTime);

                    if (i == 0)
                    {
                        dataStreamCollider.transform.DOMove(distance, upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                    }
                }
            }

            StartCoroutine(ActivateNextPoint());
            yield break;
        }
        
        if (Ydistance > 5)
        {
            if(Ydistance > 40)
            {
                remainder = Ydistance % 10;
                divisionValue = Ydistance / 10;

                for (int i = 0; i < divisionValue + 1; i++)
                {
                    for (int m = 0; m < dataStream.Length; m++)
                    {
                        if (i >= divisionValue)
                        {
                            dataStream[m].transform.DOMove(new Vector2(0, remainder), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                        }
                        else
                        {
                            dataStream[m].transform.DOMove(new Vector2(0, 10), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                        }

                        yield return new WaitForSeconds(gradualStopTime);

                        if (m >= dataStream.Length - 1)
                        {
                            if (i >= divisionValue)
                            {
                                dataStreamCollider.transform.DOMove(new Vector2(0, remainder), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                            }
                            else
                            {
                                dataStreamCollider.transform.DOMove(new Vector2(0, 10), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                            }
                        }
                    }

                    yield return new WaitForSeconds(waitUpTime);
                }
            }
            else
            {
                remainder = Ydistance % 5;
                divisionValue = Ydistance / 5;

                if(firstPoint)
                {
                    for (int i = 0; i < gurgleSounds.Length; i++)
                    {
                        gurgleSounds[i].Play();
                    }
                    StartCoroutine(activateParticleSystems());
                }

                for (int i = 0; i < divisionValue + 1; i++)
                {
                    for (int m = 0; m < dataStream.Length; m++)
                    {
                        if (i >= divisionValue)
                        {
                            dataStream[m].transform.DOMove(new Vector2(0, remainder), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                        }
                        else
                        {
                            dataStream[m].transform.DOMove(new Vector2(0, 5), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                        }               

                        yield return new WaitForSeconds(gradualStopTime);

                        if(m >= dataStream.Length - 1)
                        {
                            if(i >= divisionValue)
                            {      
                                dataStreamCollider.transform.DOMove(new Vector2(0, remainder), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                            }
                            else
                            {  
                                dataStreamCollider.transform.DOMove(new Vector2(0, 5), upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                            }
                        }               
                    }

                    yield return new WaitForSeconds(waitUpTime);
                }
            }
        }
        else
        {
            for (int i = 0; i < dataStream.Length; i++)
            {
                dataStream[i].transform.DOMove(distance, upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                
                yield return new WaitForSeconds(gradualStopTime);

                if (i >= dataStream.Length - 1)
                {               
                    dataStreamCollider.transform.DOMove(distance, upStreamTime).SetEase(Ease.OutCubic).SetRelative(true);
                }
            }
        }

        StartCoroutine(ActivateNextPoint());
    }

    private IEnumerator ActivateNextPoint()
    {
        for (int i = 0; i < nextPoint.Length; i++)
        {
            if (!nextPoint[i].activeInHierarchy)
            {
                nextPoint[i].SetActive(true);
            }
        }

        yield return new WaitForSeconds(nextPointActivatingTime);
    }

    private IEnumerator activateParticleSystems()
    {
        for (int i = 0; i < partSysNGO.Length; i++)
        {
            partSysNGO[i].Play();
            yield return new WaitForSeconds(gradualStopTime);
        }
    }

    private void StartCutScene()
    {
        DEA.DialogActivate();
        DEA.EdgesActivate();
    }
}
