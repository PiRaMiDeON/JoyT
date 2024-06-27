using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIsNear : MonoBehaviour
{
    private bool _isNear, pressed;
    public ScriptEvent scriptEvent;

    private void Update()
    {
        if(_isNear && !pressed &&Input.GetKeyDown(KeyCode.F))
        {
            ActivateScriptEvent();
            pressed = true;
        }
    }
    private void ActivateScriptEvent()
    {
        scriptEvent.StartEvent();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out CharacterController2D player))
        {
            _isNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterController2D player))
        {
            _isNear = false;
        }
    }

}
