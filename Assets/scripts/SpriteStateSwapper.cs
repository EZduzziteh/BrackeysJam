using UnityEngine;

public class SpriteStateSwapper : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private int _spriteIndex;
    public int spriteIndex
    {
        get { return _spriteIndex; }
        set 
        { 
            if (value > sprites.Length - 1)
                _spriteIndex = 0;
            else
                _spriteIndex = value;
            GetComponent<SpriteRenderer>().sprite = sprites[_spriteIndex];
        }
    }
    private void OnValidate()
    {
        if(sprites.Length>0)
            spriteIndex = _spriteIndex;
    }
}
