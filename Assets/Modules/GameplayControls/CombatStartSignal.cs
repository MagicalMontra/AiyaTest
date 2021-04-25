public class CombatStartSignal
{
    public Avatar data => _data;
    private Avatar _data;

    public CombatStartSignal(Avatar data)
    {
        _data = data;
    }
}