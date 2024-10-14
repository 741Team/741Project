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
            GameObject startTile = gridManager.GetTile(i, 0);
            GameObject endTile = gridManager.GetTile(i, grid[i].Length - 1);
            float tileScale = startTile.transform.localScale.x;
            float startX = startTile.GetComponent<BoxCollider>().bounds.center.x + (startTile.GetComponent <BoxCollider>().bounds.extents.x * tileScale);
            float endX = endTile.GetComponent<BoxCollider>().bounds.center.x + (endTile.GetComponent<BoxCollider>().bounds.extents.x * tileScale);
            Vector3 endPos = new Vector3(startX, startTile.transform.position.y,startTile.transform.position.z);
            Vector3 startPos = new Vector3(endX, endTile.transform.position.y, endTile.transform.position.z);
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
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
            GameObject startTile = gridManager.GetTile(0, j);
            GameObject endTile = gridManager.GetTile(grid[0].Length - 1, j);
            float tileScale = startTile.transform.localScale.z;
            float startZ = startTile.GetComponent<BoxCollider>().bounds.center.z + (startTile.GetComponent<BoxCollider>().bounds.extents.z * tileScale);
            float endZ = endTile.GetComponent<BoxCollider>().bounds.center.z + (endTile.GetComponent<BoxCollider>().bounds.extents.z * tileScale);
            Vector3 endPos = new Vector3(startTile.transform.position.x, startTile.transform.position.y, startZ);
            Vector3 startPos = new Vector3(endTile.transform.position.x, endTile.transform.position.y, endZ);
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
            verticalLines.Add(lineRenderer);
        }
        lineStorage.SetActive(false);
    }

    IEnumerator AnimateLine(LineRenderer l)
    {
        float startTime = Time.time;
        Vector3 startPos = l.GetPosition(0);
        Vector3 endPos = l.GetPosition(1);

        Vector3 pos = startPos;
        while (pos != endPos)
        {
            float t = (Time.time - startTime)/ _animationLength;
            pos = Vector3.Lerp(startPos, endPos, t);
            l.SetPosition(1, pos);
            yield return null;
        }

    }

    public void EnableLines()
    {
        lineStorage.SetActive(true);
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
