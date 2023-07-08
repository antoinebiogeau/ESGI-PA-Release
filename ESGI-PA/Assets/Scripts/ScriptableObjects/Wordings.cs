using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Language wordings", menuName = "Language/Language data")]
public class Wordings : ScriptableObject
{
    [SerializeField] private List<string> wordings = new();

}
