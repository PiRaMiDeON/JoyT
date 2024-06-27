using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPanelController : MonoBehaviour
{
    public CubeStation[] cubeStations;
    public AttackPanel[] attackPanels;

    private int cubeStationIndex = 0;
    private int requiredValue;
    [HideInInspector] public int currentValue;

    private void Start()
    {
        requiredValue = attackPanels.Length;
    }

    private void Update()
    {
        if(currentValue >= requiredValue)
        {
            currentValue = 0;
            StartCoroutine(ActivateAttack());
        }
    }

    private IEnumerator ActivateAttack()
    {
        if (cubeStationIndex >= cubeStations.Length)
        {
            cubeStationIndex = 0;
        }

        if (!cubeStations[cubeStationIndex].notSpawn)
        {
            cubeStations[cubeStationIndex].notSpawn = true;
        }

        yield return new WaitForSeconds(3f);

        cubeStations[cubeStationIndex].Spawn();
        cubeStationIndex++;

        for (int i = 0; i < attackPanels.Length; i++)
        {
            attackPanels[i].ResetAttackPanel();
        }
    }

}
