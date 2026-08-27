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
            _spriteIndex = Mathf.Clamp(value, 0, sprites.Length-1); 
            GetComponent<SpriteRenderer>().sprite = sprites[_spriteIndex];
        }
    }
    private void OnValidate()
    {
        if(sprites.Length>0)
            spriteIndex = _spriteIndex;
    }
}
