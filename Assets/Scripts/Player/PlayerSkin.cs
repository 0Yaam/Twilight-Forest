using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    private const string PLAYER_SKIN_PREF_KEY = "PlayerSkinIndex";

    private static readonly string[] SkinNames =
    {
        "Default",
        "Blue",
        "Red",
        "Purple"
    };

    private static readonly Color[] SkinColors =
    {
        Color.white,
        new Color(0.45f, 0.75f, 1f, 1f),
        new Color(1f, 0.45f, 0.45f, 1f),
        new Color(0.75f, 0.45f, 1f, 1f)
    };

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplySavedSkin();
    }

    private void OnEnable()
    {
        ApplySavedSkin();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void EnsurePlayerSkin()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player == null) { return; }
        if (player.GetComponent<PlayerSkin>() != null) { return; }

        player.gameObject.AddComponent<PlayerSkin>();
    }

    public static void SelectNextSkin()
    {
        SaveSkinIndex(GetSkinIndex() + 1);
    }

    public static void SelectPreviousSkin()
    {
        SaveSkinIndex(GetSkinIndex() - 1);
    }

    public static string GetSelectedSkinName()
    {
        return SkinNames[GetSkinIndex()];
    }

    public void ApplySavedSkin()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer == null) { return; }

        spriteRenderer.color = SkinColors[GetSkinIndex()];
    }

    private static int GetSkinIndex()
    {
        int savedIndex = PlayerPrefs.GetInt(PLAYER_SKIN_PREF_KEY, 0);
        return Mathf.Clamp(savedIndex, 0, SkinNames.Length - 1);
    }

    private static void SaveSkinIndex(int skinIndex)
    {
        int wrappedIndex = skinIndex;

        if (wrappedIndex < 0)
        {
            wrappedIndex = SkinNames.Length - 1;
        }
        else if (wrappedIndex >= SkinNames.Length)
        {
            wrappedIndex = 0;
        }

        PlayerPrefs.SetInt(PLAYER_SKIN_PREF_KEY, wrappedIndex);
        PlayerPrefs.Save();
    }
}
