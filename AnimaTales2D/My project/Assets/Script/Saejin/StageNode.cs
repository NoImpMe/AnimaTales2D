using System.Collections.Generic;
using UnityEngine;

public class StageNode : MonoBehaviour
{
    public bool isSelected = false;
    public bool isLast = false;
    public bool isBoss = false;
    public List<StageNode> prevNodes = new List<StageNode>();
    public List<StageNode> nextNodes = new List<StageNode>();
    public List<LineGenerate> lines = new List<LineGenerate>();
    public int idx;
    public int prevNodeCount = 0;
    public int type = 0; // ∏  ≈∏¿‘¿∫ 0 ~ 6 √— 7∞≥ ∑£¥˝«œ∞‘ πË¡§
    public string stat;
}
