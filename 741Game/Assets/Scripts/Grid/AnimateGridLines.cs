using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateGridLines : MonoBehaviour
{
    GridManager gridManager;
    List<LineRenderer> horizontalLines = new List<LineRenderer>();
    List<LineRenderer> verticalLines = new List<LineRenderer>();
    GameObject lineStorage;

    [SerializeField]
    float _animationLength;
    [SerializeField]
    float _lineWidth;
    [SerializeField]
    Material _lineMaterial;
    [SerializeField]
    Color _color;
    [SerializeField]
    float _lineAccuracy;


    void Start()
    {
        gridManager = GetComponent<GridManager>();
        lineStorage = new GameObject();
        lineStorage.name = "LineStorage";
        lineStorage.transform.parent = transform;
    }

    public void CreateLineRenderers()
    {
        GameObject[][] grid = gridManager.GetGrid();
        for (int i = 0; i < grid.Length; i++)
        {
            GameObject lineObj = new GameObject();
            lineObj.name = "HorizontalLineObject" + i;
            lineObj.transform.parent = lineStorage.transform;
            LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
            lineRenderer.startWidth = _lineWidth;
            lineRenderer.material = _lineMaterial;
            lineRenderer.startColor = _color;
            lineRenderer.endColor = _color;
            lineRenderer.positionCount = (int)_lineAccuracy;
            horizontalLines.Add(lineRenderer);
        }
        for (int j = 0; j < grid.Length; j++)
        {
            GameObject lineObj = new GameObject();
            lineObj.name = "VerticalLineObject" + j;
            lineObj.transform.parent = lineStorage.transform;
            LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
            lineRenderer.startWidth = _lineWidth;
            lineRenderer.material = _lineMaterial;
            lineRenderer.startColor = _color;
            lineRenderer.endColor = _color;
            lineRenderer.positionCount = (int)_lineAccuracy;
            verticalLines.Add(lineRenderer);
        }
        lineStorage.SetActive(false);
    }

    IEnumerator AnimateLine(LineRenderer l)
    {
        int totalPoints = l.positionCount;
        Vector3[] originalPositions = new Vector3[totalPoints];

        // Store the original positions
        for (int i = 0; i < totalPoints; i++)
        {
            originalPositions[i] = l.GetPosition(i);
        }

        // Start by showing only the first point
        l.positionCount = 1;
        l.SetPosition(0, originalPositions[0]);

        for (int i = 1; i < totalPoints; i++)
        {
            // Incrementally increase the number of visible points and set their positions
            l.positionCount = i + 1;
            l.SetPosition(i, originalPositions[i]);

            // Wait a short duration before showing the next point
            yield return null;
        }
    }

    public void AdjustLinePoints()
    {
        Terrain _terrain = gridManager.GetTerrain();
        _terrain.
        GameObject[][] grid = gridManager.GetGrid();
        for (int i = 0; i < grid.Length; i++)
        {
            
            LineRenderer l = horizontalLines[i];
            GameObject startTile = gridManager.GetTile(i, 0);
            GameObject endTile = gridManager.GetTile(i, grid[i].Length - 1);
            float tileScale = startTile.transform.localScale.x;
            float startY = _terrain.SampleHeight(startTile.transform.position);
            float endY = _terrain.SampleHeight(endTile.transform.position);
            float startX = startTile.transform.position.x + (startTile.GetComponent<BoxCollider>().bounds.extents.x );
            float endX = endTile.transform.position.x + (endTile.GetComponent<BoxCollider>().bounds.extents.x );
            Vector3 endPos = new Vector3(startX, startY, startTile.transform.position.z);
            Vector3 startPos = new Vector3(endX, endY, endTile.transform.position.z);
            l.SetPosition(0, startPos);
            float startZ = startTile.transform.position.z;
            float endZ = endTile.transform.position.z;
            for (int p = 1; p< _lineAccuracy-1; p++)
            {
                float nextX = startX + (startX - endX) * (p / _lineAccuracy);
                float zDiff = startZ - endZ;
                float multiplier = p / _lineAccuracy;
                float add = zDiff * multiplier;
                float nextZ = endZ + add;
                float nextY = _terrain.SampleHeight(new Vector3(nextX, 0, nextZ))+ 0.2f;
                Vector3 pos = new Vector3(nextX, nextY, nextZ);
                l.SetPosition(p, pos);
            }
            l.SetPosition(l.positionCount-1, endPos);
        }
        for (int j = 0; j < grid.Length; j++)
        {
            LineRenderer lineRenderer = verticalLines[j];
            GameObject startTile = gridManager.GetTile(0, j);
            GameObject endTile = gridManager.GetTile(grid[0].Length - 1, j);
            float tileScale = startTile.transform.localScale.z;
            float startY = _terrain.SampleHeight(startTile.transform.position);
            float endY = _terrain.SampleHeight(endTile.transform.position);
            float startZ = startTile.transform.position.z + (startTile.GetComponent<BoxCollider>().bounds.extents.z );
            float endZ = endTile.transform.position.z + (endTile.GetComponent<BoxCollider>().bounds.extents.z );
            Vector3 endPos = new Vector3(startTile.transform.position.x, startY, startZ);
            Vector3 startPos = new Vector3(endTile.transform.position.x, endY, endZ);
            lineRenderer.SetPosition(0, startPos);
            float startX = startTile.transform.position.x;
            float endX = endTile.transform.position.x;
            for (int p = 1; p < _lineAccuracy-1; p++)
            {
                float nextX = endX + (startX - endX) * (p / _lineAccuracy);
                float nextZ = startZ + (startZ - endZ) * (p / _lineAccuracy);
                float nextY = _terrain.SampleHeight(new Vector3(nextX, 0, nextZ)) + 0.2f;
                Vector3 pos = new Vector3(nextX, nextY, nextZ);
                lineRenderer.SetPosition(p, pos);
            }
            lineRenderer.SetPosition(lineRenderer.positionCount-1, endPos);
        }
    }

    public void AdjustLineColours(Color color)
    {
        foreach (LineRenderer l in horizontalLines)
        {
            l.startColor = color;
            l.endColor = color;
        }
        foreach (LineRenderer l in verticalLines)
        {
            l.startColor = color;
            l.endColor = color;
        }
    }


    public void EnableLines()
    {
        lineStorage.SetActive(true);
        AdjustLinePoints();
        foreach (LineRenderer l in horizontalLines)
        {
            StartCoroutine(AnimateLine(l));
        }
        foreach (LineRenderer l in verticalLines)
        {
            StartCoroutine(AnimateLine(l));
        }
    }

    public void DisableLines()
    {
        lineStorage.SetActive(false);
    }

    public Color GetColor()
    {
        return _color;
    }
}
