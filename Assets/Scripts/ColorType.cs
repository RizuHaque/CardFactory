public enum ColorType { Red, Blue }

public static class ColorTypeExtensions
{
    public static UnityEngine.Color ToColor(this ColorType type) => type switch
    {
        ColorType.Red  => UnityEngine.Color.red,
        ColorType.Blue => UnityEngine.Color.blue,
        _              => UnityEngine.Color.white
    };
}