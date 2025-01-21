using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LobbySpace
{
    CommandRoom,
    TrainingPlatform
}

public class LobbyManager : Singleton<LobbyManager>
{
    [SerializeField] private GameObject[] lobbySpaces;
    
    [SerializeField] LobbySpace currentSpace = LobbySpace.CommandRoom;
    private Dictionary<LobbySpace, GameObject> spaceMap;

    protected override void Awake()
    {
        base.Awake();
        spaceMap = new Dictionary<LobbySpace, GameObject>();
        for (int i = 0; i < lobbySpaces.Length; i++)
            spaceMap.Add((LobbySpace)i, lobbySpaces[i]);
    }

    public void MoveSpace(LobbySpace newSpace)
    {
        spaceMap[currentSpace].SetActive(false);
        
        currentSpace = newSpace;
        
        spaceMap[currentSpace].SetActive(true);
    }
}
