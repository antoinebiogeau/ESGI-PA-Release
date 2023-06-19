using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CupPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cupNameUI;
    
    private string cupName;
    
    private List<string> cupText;

    private void Start()
    {
        cupText = new();
    }

    // Start is called before the first frame update
    public string CupName
    {
        get => cupName;
        set
        {
            cupName = value;
            cupNameUI.text = cupName;
        }
    }

    public List<string> CupText
    {
        get => cupText;
        set
        {
            cupText = value;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        Debug.Log("Updating preview : " + cupText);
    }
}
