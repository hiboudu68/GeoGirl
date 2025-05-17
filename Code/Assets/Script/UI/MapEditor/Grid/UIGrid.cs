using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UIGrid : Graphic
{
    [Header("Grid Settings")]
    public Camera targetCamera;
    public float cellSize = 1f;
    public Vector2 baseOffset = new Vector2(-0.5f, 0.5f);

    [Header("Line Appearance")]
    public Color lineColor = Color.white;
    public float lineThickness = 1f; // in UI units

    private Vector2 lastCamPos = Vector2.positiveInfinity;
    private RectTransform rectTransformCache;
    private Canvas parentCanvas;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (targetCamera == null)
            targetCamera = Camera.main;
        rectTransformCache = transform as RectTransform;
        parentCanvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        if (targetCamera == null) return;
        Vector2 camPos = targetCamera.transform.position;
        if (camPos != lastCamPos)
        {
            lastCamPos = camPos;
            SetVerticesDirty();
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (targetCamera == null || rectTransformCache == null)
            return;

        // Calculate world camera bounds
        float camHeight = targetCamera.orthographicSize * 2f;
        float camWidth = camHeight * targetCamera.aspect;
        Vector2 worldBL = (Vector2)targetCamera.transform.position + new Vector2(-camWidth / 2f, -camHeight / 2f);
        Vector2 worldTR = worldBL + new Vector2(camWidth, camHeight);

        // Snap grid extents to cell size
        float startX = Mathf.Floor((worldBL.x - baseOffset.x) / cellSize) * cellSize + baseOffset.x;
        float endX = Mathf.Ceil((worldTR.x - baseOffset.x) / cellSize) * cellSize + baseOffset.x;
        float startY = Mathf.Floor((worldBL.y - baseOffset.y) / cellSize) * cellSize + baseOffset.y;
        float endY = Mathf.Ceil((worldTR.y - baseOffset.y) / cellSize) * cellSize + baseOffset.y;

        // Draw vertical lines
        for (float x = startX; x <= endX; x += cellSize)
            AddLine(vh, new Vector2(x, startY), new Vector2(x, endY));
        // Draw horizontal lines
        for (float y = startY; y <= endY; y += cellSize)
            AddLine(vh, new Vector2(startX, y), new Vector2(endX, y));
    }

    private void AddLine(VertexHelper vh, Vector2 worldP1, Vector2 worldP2)
    {
        // Convert world to screen points
        Vector2 screenP1 = targetCamera.WorldToScreenPoint(worldP1);
        Vector2 screenP2 = targetCamera.WorldToScreenPoint(worldP2);

        // Convert screen points to local UI positions (Overlay canvas: camera = null)
        Vector2 localP1, localP2;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransformCache, screenP1, parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : targetCamera, out localP1);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransformCache, screenP2, parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : targetCamera, out localP2);

        // Build quad with thickness
        Vector2 dir = (localP2 - localP1).normalized;
        Vector2 normal = new Vector2(-dir.y, dir.x) * (lineThickness * 0.5f);

        UIVertex v1 = UIVertex.simpleVert; v1.color = lineColor; v1.position = localP1 + normal;
        UIVertex v2 = UIVertex.simpleVert; v2.color = lineColor; v2.position = localP2 + normal;
        UIVertex v3 = UIVertex.simpleVert; v3.color = lineColor; v3.position = localP2 - normal;
        UIVertex v4 = UIVertex.simpleVert; v4.color = lineColor; v4.position = localP1 - normal;

        int idx = vh.currentVertCount;
        vh.AddVert(v1); vh.AddVert(v2); vh.AddVert(v3); vh.AddVert(v4);
        vh.AddTriangle(idx, idx + 1, idx + 2);
        vh.AddTriangle(idx + 2, idx + 3, idx);
    }
}
