using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPanelCntrl_BossRoom : MonoBehaviour
{
    public DialogEdgesActivator[] cutScenes;
    public ScriptEvent scriptEvent;
    private int cutSceneIndex = 0;
    public void StartCutScene()
    {
        if(cutSceneIndex >= 2)
        {
            scriptEvent.StartEvent();
            return;
        }

        cutScenes[cutSceneIndex].DialogActivate();
        cutScenes[cutSceneIndex].EdgesActivate();

        cutSceneIndex++;

    }
}
