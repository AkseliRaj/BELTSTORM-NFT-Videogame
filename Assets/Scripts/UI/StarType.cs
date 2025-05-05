using UnityEngine;

[System.Serializable]
public class StarType
{
    [Tooltip("Sprite representing this star type.")]
    public Sprite sprite;

    [Tooltip("Seconds for full fade-in → fade-out cycle.")]
    public float fadeCycleTime = 2f;

    [Tooltip("Peak scale multiplier. 1 = no change.")]
    public float maxScaleMultiplier = 1.5f;

    [Tooltip("Seconds for full grow → shrink cycle.")]
    public float scaleCycleTime = 3f;

    [Tooltip("Spawn likelihood (higher = more common).")]
    public float spawnWeight = 1f;
}
