using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageLayoutCreator
{
    private const float HorizontalSpacing = 600f;
    private const float VerticalSpacing = 200f;
    private readonly List<StageNode> roots;
    private Dictionary<int, List<StageNode>> levels;
    
    private readonly RectTransform nodesContainer;
    private readonly RectTransform linesContainer;
    
    private readonly UIStageNode uiStageNode;
    private readonly GameObject linePrefab;
    
    private readonly GameObject depthSlot;
    private readonly GameObject content;

    public StageLayoutCreator(List<StageNode> roots, RectTransform nodesContainer, RectTransform linesContainer, UIStageNode uiStageNode, GameObject linePrefab, GameObject depthSlot, GameObject content)
    {
        this.roots = roots;
        this.nodesContainer = nodesContainer;
        this.uiStageNode = uiStageNode;
        this.linePrefab = linePrefab;
        this.linesContainer = linesContainer;
        this.depthSlot = depthSlot;
        this.content = content;
    }
    
    public void CreateLayout()
    {
        AssignPositions();
        CreateUI();
    }

    private void AssignPositions()
    {
        var queue = new Queue<StageNode>();
        
        levels = new Dictionary<int, List<StageNode>>();
        var nodeLevels = new Dictionary<StageNode, int>();

        foreach (var root in roots)
        {
            queue.Enqueue(root);
            nodeLevels[root] = 0;
        }

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var currentLevel = nodeLevels[current];
            if (!levels.ContainsKey(currentLevel))
                levels[currentLevel] = new List<StageNode>();
            levels[currentLevel].Add(current);

            foreach (var child in current.Children)
            {
                if (nodeLevels.ContainsKey(child)) continue;
                
                nodeLevels[child] = currentLevel + 1;
                queue.Enqueue(child);
            }
        }

        foreach (var (levelIndex, nodesAtLevel) in levels)
        {
            var yOffset = (nodesAtLevel.Count - 1) * VerticalSpacing / 2;

            for (var i = 0; i < nodesAtLevel.Count; i++)
            {
                StageNode node = nodesAtLevel[i];
                node.Position = new Vector2(levelIndex * HorizontalSpacing, yOffset - i * VerticalSpacing);
                node.Depth = levelIndex;
            }
        }
    }

    private void CreateUI()
    {
        var depth = new GameObject[levels.Count];
        foreach (var node in roots)
            CreateNodeUI(node, null, depth);
    }

    private void CreateNodeUI(StageNode node, StageNode parent, GameObject[] depth)
    {
        if (depth[node.Depth] == null)
        {
            depth[node.Depth] = Object.Instantiate(depthSlot, content.transform, false);
            var rect = (RectTransform)(depth[node.Depth].transform);
            rect.sizeDelta = new Vector2(HorizontalSpacing, Screen.height);

            if (node.Depth == 0)
            {
                linesContainer.SetParent(rect);
                linesContainer.anchoredPosition = Vector2.zero;
            }
        }
        
        UIStageNode nodeUI = Object.Instantiate(uiStageNode, depth[node.Depth].transform, true);
        nodeUI.transform.localPosition = new Vector3(0f, node.Position.y);
        nodeUI.Init(node.StageSO);
        if (parent != null)
            CreateLine(parent.Position, node.Position);
        
        foreach (var child in node.Children)
            CreateNodeUI(child, node, depth);
    }

    private void CreateLine(Vector2 startPos, Vector2 endPos)
    {
        var line = Object.Instantiate(linePrefab, linesContainer, false);
        RectTransform lineRect = line.GetComponent<RectTransform>();

        Vector2 direction = endPos - startPos;
        var distance = direction.magnitude;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        lineRect.anchoredPosition = (startPos + endPos) / 2;

        lineRect.sizeDelta = new Vector2(distance, 15f);

        lineRect.rotation = Quaternion.Euler(0, 0, angle);
        
        
    }
}
