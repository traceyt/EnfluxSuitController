using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
class ConnectionStateButton : MonoBehaviour
{
    [Header("Enable Interaction While: ")]

    public bool None;
    public bool Attached;
    public bool Detached;
    public bool Conected;
    public bool Disconnected;
    public bool Calibrating;
    public bool Streaming;

    void OnEnable()
    {
        GetComponent<Selectable>().interactable = None;
        EVRSuitManager suitmanager = FindObjectOfType<EVRSuitManager>();
        if (suitmanager != null)
        {
            SetState(suitmanager.operatingState);
        }

    }

    public void SetState(EVRSuitManager.ConnectionState currentState)
    {
        switch (currentState)
        {
            case EVRSuitManager.ConnectionState.NONE :
                GetComponent<Selectable>().interactable = None; break;
            case EVRSuitManager.ConnectionState.ATTACHED :
                GetComponent<Selectable>().interactable = Attached; break;
            case EVRSuitManager.ConnectionState.DETACHED :
                GetComponent<Selectable>().interactable = Detached; break;
            case EVRSuitManager.ConnectionState.CONNECTED :
                GetComponent<Selectable>().interactable = Conected; break;
            case EVRSuitManager.ConnectionState.DISCONNECTED:
                GetComponent<Selectable>().interactable = Disconnected; break;
            case EVRSuitManager.ConnectionState.CALIBRATING:
                GetComponent<Selectable>().interactable = Calibrating; break;
            case EVRSuitManager.ConnectionState.STREAMING :
                GetComponent<Selectable>().interactable = Streaming; break;
        }
    }
}