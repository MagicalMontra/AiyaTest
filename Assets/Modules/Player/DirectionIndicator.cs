using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _activeObject;
    [SerializeField] private GameObject _deactiveObject;
    [SerializeField] private Vector2 _dir;
    public void SetActive(Vector2 dir)
    {
        _activeObject.SetActive(_dir == dir);
        _deactiveObject.SetActive(_dir != dir);
    }
}