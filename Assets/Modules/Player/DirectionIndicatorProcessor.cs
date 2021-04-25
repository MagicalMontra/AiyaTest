using UnityEngine;

public class DirectionIndicatorProcessor : MonoBehaviour
{
    [SerializeField] private DirectionIndicator[] _directionIndicators;
    public void ProcessMovementIndicator(Vector2 dir)
    {
        for (int i = 0; i < _directionIndicators.Length; i++)
            _directionIndicators[i].SetActive(dir);
    }
}