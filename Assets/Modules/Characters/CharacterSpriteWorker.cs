using UnityEngine;

public class CharacterSpriteWorker
{
    private float _prevX;
    private Transform _transform;

    public CharacterSpriteWorker(Transform transform)
    {
        _transform = transform;
    }
    public void HandleSprite(float x)
    {
        if (x == 0)
            return;
        
        var localScale = _transform.localScale;

        if (Mathf.Approximately(x, _prevX))
            return;

        _prevX = x;
        _transform.localScale = new Vector2(x * 2.5f, localScale.y);
    }
}