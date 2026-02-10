using UnityEngine;

[ExecuteAlways]
public class HorizontalLayout3D : MonoBehaviour
{
    public enum Alignment
    {
        Left,
        Center,
        Right
    }

    [Header("Layout Settings")]
    public float spacing = 0.5f;
    public Alignment alignment = Alignment.Center;

    [Tooltip("If true, uses each renderer's bounds width")]
    public bool useRendererBounds = true;

    [Tooltip("Fallback width when no Renderer is found")]
    public float defaultChildWidth = 1f;

    private void Update()
    {
        ApplyLayout();
    }

    void ApplyLayout()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        // Calculate total width
        float totalWidth = 0f;
        int activeCount = 0;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!child.gameObject.activeSelf) continue;

            totalWidth += GetChildWidth(child);
            activeCount++;
        }

        if (activeCount > 1)
            totalWidth += spacing * (activeCount - 1);

        // Determine start X
        float startX = 0f;

        switch (alignment)
        {
            case Alignment.Left:
                startX = 0f;
                break;
            case Alignment.Center:
                startX = -totalWidth * 0.5f;
                break;
            case Alignment.Right:
                startX = -totalWidth;
                break;
        }

        float currentX = startX;

        // Position children
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!child.gameObject.activeSelf) continue;

            float width = GetChildWidth(child);

            Vector3 localPos = child.localPosition;
            localPos.x = currentX + width * 0.5f;
            child.localPosition = localPos;

            currentX += width + spacing;
        }
    }

    float GetChildWidth(Transform child)
    {
        if (useRendererBounds)
        {
            Renderer r = child.GetComponentInChildren<Renderer>();
            if (r != null)
                return r.bounds.size.x;
        }

        return defaultChildWidth;
    }
}
