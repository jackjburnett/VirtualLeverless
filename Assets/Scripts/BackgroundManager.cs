using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public Image background;
    private Color _backgroundColour;
    private Sprite _backgroundImage;

    private Sprite _defaultBackground;

    private void Awake()
    {
        _defaultBackground = Resources.Load<Sprite>("background");
    }

    public void SetBackgroundColour(Color colour)
    {
        _backgroundColour = colour;
        background.color = _backgroundColour;
    }

    public Color GetBackgroundColour()
    {
        return _backgroundColour;
    }

    public void SetBackgroundFromTexture(Texture2D texture)
    {
        if (texture == null) return;
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        SetBackgroundImage(sprite);
    }

    public void SetBackgroundImage(Sprite image)
    {
        SetBackgroundColour(Color.white);
        _backgroundImage = image;
        background.sprite = _backgroundImage;
    }

    public void ClearBackgroundImage()
    {
        SetBackgroundImage(_defaultBackground);
    }
}