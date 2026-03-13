using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance { get; private set; }

    [Serializable]
    public struct ColorEntry
    {
        public ColorType colorType;
        public Color color;
    }

    public List<ColorEntry> colorEntries = new List<ColorEntry>();

    private Dictionary<ColorType, Color> colorMap = new Dictionary<ColorType, Color>();

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        foreach (var entry in colorEntries)
            colorMap[entry.colorType] = entry.color;
    }

    public static Color Get(ColorType type)
    {
        if (Instance != null && Instance.colorMap.TryGetValue(type, out Color c)) return c;
        return Color.white;
    }
}