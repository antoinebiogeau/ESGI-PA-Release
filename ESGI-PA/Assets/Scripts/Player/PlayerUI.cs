using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerUI : MonoBehaviour
{
    private Player _playerInfo;

    public Player PlayerInfo
    {
        get => _playerInfo;
        set => _playerInfo = value;
    }
    
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI rankText;

    //private PlayerState info;
    private bool _loaded = false;

    void Start()
    {
        /*StartCoroutine(GetPlayerInfo());
        turnText = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        rankText = transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();*/
    }
    
    void Update()
    {
        updateTurns();
    }

    private void updateTurns()
    {
        if (!_playerInfo) return;
        Debug.Log("Update UI");
        turnText.text = $"Turn : {_playerInfo.TurnCount}/3";
        rankText.text = $"Rank : 0/8";
        /*if (!_loaded) return;
        info = loop.PlayerInfo[player];
        turnText.text = "Turn : " + info.turnCount + "/2";
        rankText.text = "Rank : " + (loop.PlayersRank.IndexOf(player) + 1);*/
    }

    private IEnumerator GetPlayerInfo()
    {
        /*yield return new WaitForSeconds(1);
        info = loop.PlayerInfo[player];
        _loaded = true;*/
        yield break;
    }
}
