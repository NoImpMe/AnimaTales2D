using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> field;

    public void EnterNewField()
    {
        for (int i = 0; i < field.Count; i++)
        {
            if (field[i].gameObject.activeSelf)
            {
                var checkField = field[i].gameObject.GetComponent<IsVisitedField>();
                if (checkField.isVisited || !checkField.isSelected)
                {
                    foreach (Transform tile in field[i].transform)
                    {
                        SetTileVisualState(tile, colliderEnabled: false, alpha: 0.37f);
                    }
                }
            }
        }
    }
    public void ShowNextField()
    {
        for (int i = 0; i < field.Count; i++)
        {
            if (field[i].gameObject.activeSelf)
            {
                var checkField = field[i].gameObject.GetComponent<IsVisitedField>();
                if (checkField.isVisited)
                {
                    foreach(GameObject tile in checkField.nearFields)
                    {
                        tile.SetActive(true);
                    }
                    foreach(GameObject chkfield in checkField.nearFields)
                    {
                        foreach (Transform tile in chkfield.transform)
                        {
                            SetTileVisualState(tile, colliderEnabled: true, alpha: 1f);
                        }
                    }

                }
            }
        }
    }
    public void ShowLastField()
    {
        field[5].gameObject.SetActive(true);
    }

    private void SetTileVisualState(Transform tile, bool colliderEnabled, float alpha)
    {
        var collider = tile.gameObject.GetComponent<TilemapCollider2D>();
        if (collider != null)
        {
            collider.enabled = colliderEnabled;
        }

        var tilemap = tile.gameObject.GetComponent<Tilemap>();
        var color = tilemap.color;
        color.a = alpha;
        tilemap.color = color;
    }
}
