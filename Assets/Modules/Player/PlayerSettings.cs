using System;
using UnityEngine;

[Serializable]
public class PlayerSettings
{
    public Transform playerStartPoint;
    public AvatarPlaceHolder avatarPlaceHolderPrefab;
    public PlayerAvatarUI playerAvatarUI;
    public DirectionIndicatorProcessor indicatorProcessor;
}