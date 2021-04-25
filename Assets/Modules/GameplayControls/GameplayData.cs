using System;

[Serializable]
public class GameplayData
{
    public long timeStamp; //NOTE: in seconds
    public int heroCount;
    public int killCount;

    public GameplayData()
    {
        timeStamp = DateTime.Now.Ticks;
    }
}