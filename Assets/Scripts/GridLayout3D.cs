using UnityEngine;

[ExecuteAlways] // Работает и в редакторе, и в игре
public class GridLayout3D: MonoBehaviour
{
    public enum LayoutType
    {
        Horizontal,
        Vertical,
        Grid
    }

    [Header("Layout Settings")]
    public LayoutType layoutType = LayoutType.Horizontal;
    public float spacingX = 1f;
    public float spacingY = 1f;
    public float spacingZ = 1f;

    [Header("Grid Settings")]
    public int gridColumns = 5; // Для Grid режима

    void Update()
    {
        ArrangeObjects();
    }

    void ArrangeObjects()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            switch (layoutType)
            {
                case LayoutType.Horizontal:
                    child.localPosition = new Vector3(i * spacingX, 0, 0);
                    break;

                case LayoutType.Vertical:
                    child.localPosition = new Vector3(0, i * spacingY, 0);
                    break;

                case LayoutType.Grid:
                    int row = i / gridColumns;
                    int col = i % gridColumns;
                    child.localPosition = new Vector3(col * spacingX, 0, row * spacingZ);
                    break;
            }
        }
    }
}
