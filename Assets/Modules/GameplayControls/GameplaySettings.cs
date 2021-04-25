using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameplaySettings
{
    public Transform gameBoardArea;
    public GameObject selectionPanel;
    public CharacterData defaultPlayer;
    public List<CharacterData> heroPool = new List<CharacterData>();
    public GameOverPanel gameOverPanel;
}