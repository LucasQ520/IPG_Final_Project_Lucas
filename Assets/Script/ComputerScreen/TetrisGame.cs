using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TetrisGame : MonoBehaviour
{
    public RectTransform gridParent;
    public GameObject blockPrefab;
    public TextMeshProUGUI scoreText;

    public int width = 10;
    public int height = 20;
    public float fallInterval = 0.8f;

    Transform[,] grid;
    Vector2Int[] currentShape;
    Vector2Int currentPos;
    List<Image> currentBlocks = new List<Image>();
    float timer;
    int score;

    Color currentColor;

    Color[] shapeColors =
{
    Color.cyan,
    new Color(0.6f, 0.3f, 0.9f),
    Color.yellow,
    Color.red,
    Color.green,
    new Color(1f, 0.5f, 0.1f),
    Color.blue
};

    void OnEnable()
    {
        grid = new Transform[width, height];
        ClearGrid();
        SpawnPiece();
        score = 0;
        UpdateScore();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) TryMove(Vector2Int.left);
        if (Input.GetKeyDown(KeyCode.RightArrow)) TryMove(Vector2Int.right);
        if (Input.GetKeyDown(KeyCode.UpArrow)) Rotate();
        if (Input.GetKey(KeyCode.DownArrow)) timer += Time.deltaTime * 5f;

        timer += Time.deltaTime;
        if (timer >= fallInterval)
        {
            timer = 0f;
            if (!TryMove(Vector2Int.down))
            {
                LockPiece();
                ClearLines();
                SpawnPiece();
            }
        }
    }

    void SpawnPiece()
    {
        int index = Random.Range(0, TetrisData.Shapes.Length);
        currentShape = TetrisData.Shapes[index];
        currentColor = shapeColors[index];

        currentPos = new Vector2Int(width / 2, height - 2);
        currentBlocks.Clear();

        foreach (var cell in currentShape)
        {
            GameObject b = Instantiate(blockPrefab, gridParent);
            Image img = b.GetComponent<Image>();
            img.color = currentColor;
            currentBlocks.Add(img);
        }

        UpdatePieceVisual();
    }

    void UpdatePieceVisual()
    {
        for (int i = 0; i < currentShape.Length; i++)
        {
            Vector2Int pos = currentPos + currentShape[i];
            currentBlocks[i].rectTransform.anchoredPosition =
                new Vector2(pos.x * 32, pos.y * 32);
        }
    }

    bool TryMove(Vector2Int dir)
    {
        Vector2Int newPos = currentPos + dir;
        if (!IsValid(newPos, currentShape)) return false;
        currentPos = newPos;
        UpdatePieceVisual();
        return true;
    }

    void Rotate()
    {
        Vector2Int[] rotated = new Vector2Int[currentShape.Length];
        for (int i = 0; i < currentShape.Length; i++)
            rotated[i] = new Vector2Int(-currentShape[i].y, currentShape[i].x);

        if (!IsValid(currentPos, rotated)) return;
        currentShape = rotated;
        UpdatePieceVisual();
    }

    bool IsValid(Vector2Int pos, Vector2Int[] shape)
    {
        foreach (var cell in shape)
        {
            Vector2Int p = pos + cell;
            if (p.x < 0 || p.x >= width || p.y < 0) return false;
            if (p.y < height && grid[p.x, p.y] != null) return false;
        }
        return true;
    }

    void LockPiece()
    {
        for (int i = 0; i < currentShape.Length; i++)
        {
            Vector2Int p = currentPos + currentShape[i];
            Image block = currentBlocks[i];
            block.rectTransform.anchoredPosition =
                new Vector2(p.x * 32, p.y * 32);
            grid[p.x, p.y] = block.transform;
        }
    }

    void ClearLines()
    {
        for (int y = 0; y < height; y++)
        {
            bool full = true;
            for (int x = 0; x < width; x++)
                if (grid[x, y] == null) full = false;

            if (!full) continue;

            for (int x = 0; x < width; x++)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }

            for (int yy = y + 1; yy < height; yy++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (grid[x, yy] != null)
                    {
                        grid[x, yy].position += Vector3.down * 32f;
                        grid[x, yy - 1] = grid[x, yy];
                        grid[x, yy] = null;
                    }
                }
            }

            score += 100;
            UpdateScore();
            y--;
        }
    }

    void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void ClearGrid()
    {
        foreach (Transform t in gridParent)
            Destroy(t.gameObject);
    }

    public void CloseGame()
    {
        gameObject.SetActive(false);
    }
}
