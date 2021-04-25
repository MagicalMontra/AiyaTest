public class EncounterSignal
{
    public string encountedId => _encountedId;
    private string _encountedId;

    public EncounterSignal(string encountedId)
    {
        _encountedId = encountedId;
    }
}