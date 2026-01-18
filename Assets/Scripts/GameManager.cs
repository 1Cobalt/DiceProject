using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private GameObject[,] tiles = new GameObject[7, 7];
    private int[,] tileValues = new int[7, 7];

    private int points1 = 0;
    private int points2 = 0;

    [SerializeField] private GameObject buttons;

    public GameObject column1;
    public GameObject column2;
    public GameObject column3;

    public TMPro.TextMeshProUGUI points1Shower;
    public TMPro.TextMeshProUGUI points2Shower;



    private int whoseTurn = 1;
    private bool isPlayingAgainstAI = false;

    private bool isCameraMove = false;
    private bool isButtonsMove = false;
    private bool isColorChanging = false;
    private string pressedButton = "";

    void Start()
    {
        //Time.deltaTime = 1.0f;

        isPlayingAgainstAI = SaveScript.isPlayingAgainstAI;
        Debug.Log("Is Play Against Ai == " + isPlayingAgainstAI);

        isCameraMove = false;
        isButtonsMove = false;
        isColorChanging = false;
        whoseTurn = 1;

        points1Shower.color = Color.yellow;
        RefreshButtons();
        points1Shower.text = "Player1: " + '0';
        points2Shower.text = "Player2: " + '0';

        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                string tileName = $"Tile_{x}_{y}";
                tileValues[x, y] = 0;
                GameObject tile = GameObject.Find(tileName);
                if (tile != null)
                {
                    tiles[x, y] = tile;


                }
                else
                {
                    Debug.LogWarning($"Не найден тайл {tileName}");
                }
            }
        }



    }

    public int GetTileValue(int x, int y)
    {
        return tileValues[x, y];
    }

    public void SetTileValue(int x, int y, int value)
    {
        if (x >= 0 && x <= 6 && y >= 0 && y <= 6)
        {
            tileValues[x, y] = value;
        }

        tiles[x, y].gameObject.transform.Find("tile").transform.Find("text").gameObject.GetComponent<TextMesh>().text = " " + value;




        if (whoseTurn == 1)
        {
            tiles[x, y].gameObject.transform.Find("tile").GetComponent<Renderer>().material.color = Color.blue;

        }
        else
        {
            tiles[x, y].gameObject.transform.Find("tile").GetComponent<Renderer>().material.color = Color.red;

        }



        tiles[x, y].gameObject.transform.Find("tile").transform.localPosition = new Vector3(0.0f, 300.0f, 0.0f);

        StartCoroutine(tiles[x, y].gameObject.GetComponent<TileBehaviour>().PlaySettingAnimation());
        AnalyseCheck(Check(x, y));



        //if (AnalyseCheck(Check(x, y)) == 0)
        //{
        ChangeTurn();
        //}

 

            if (!isPlayingAgainstAI)
            {
                StartCoroutine(ButtonsOffAnimation());
                StartCoroutine(ButtonsOnAnimation());
            }
            else if (whoseTurn == 1)
            {
                StartCoroutine(AiTurnAnimations());
            }
        


    }

    IEnumerator ChangeScore()
    {
        yield return new WaitForSeconds(1.0f);
        points1Shower.text = "Player1: " + points1;
        points2Shower.text = "Player2: " + points2;
    }

    IEnumerator AiTurnAnimations()
    {
        StartCoroutine(ButtonsOnAnimation());
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(ButtonsOffAnimation());
    }

    IEnumerator ButtonsOnAnimation()
    {
        NumberButton.isReloading = true;
        //pressedButton = "";
        yield return new WaitForSeconds(0.5f);

        RefreshButtons();
        buttons.GetComponent<Animation>().Play("ButtonsOnScreen");
        while (isColorChanging)
            yield return null;


    }
    IEnumerator ButtonsOffAnimation()
    {

        yield return new WaitForSeconds(1.0f);
        while (isColorChanging || isCameraMove || isButtonsMove)
            yield return null;
        yield return new WaitForSeconds(1.0f);
        while (isColorChanging || isCameraMove || isButtonsMove)
            yield return null;

        buttons.GetComponent<Animation>().Play("ButtonsOffScreen");
        yield return new WaitForSeconds(0.5f);
        NumberButton.isReloading = false;
    }


    private int CheckTriple(int a, int b, int c)
    {
        int sum = a + b + c;

        if (sum == 3 && !(a == 1 && b == 1 && c == 1)) {
            return 0;
        }

        if (a == 0 || b == 0 || c == 0)
            return 0;



        return sum;
    }
    private int CheckDouble(int a, int b)
    {
        int sum = a + b;

        if (a == 0 || b == 0)
            return 0;

        return sum;
    }

    public (int row, int col, int chosenNumber) GetBestMove(int[,] tileNumb, int[] availableNumbers)
    {

        int[,] board = new int[7, 7];
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                board[i, j] = tileNumb[i, j];
            }
        }


        List<(int row, int col, int number, int score)> possibleMoves = new List<(int, int, int, int)>();

        for (int r = 0; r < 7; r++)
        {
            for (int c = 0; c < 7; c++)
            {
                if (board[r, c] == 0) // клетка свободна
                {
                    foreach (int number in availableNumbers)
                    {
                        // Ставим временно число
                        board[r, c] = number;

                        // считаем очки (твой метод Check)
                        int score = Check(r, c);

                        // сохраняем вариант
                        possibleMoves.Add((r, c, number, score));

                        // откатываем обратно
                        board[r, c] = 0;
                    }
                }
            }
        }

        if (possibleMoves.Count == 0)
            return (-1, -1, -1); // нет ходов вообще (поле забито)

        // ищем варианты с максимальными очками
        int maxScore = possibleMoves.Max(m => m.score);

        if (maxScore > 0)
        {
            // если есть хоть какая-то комбинация — выбираем лучший ход
            var bestMoves = possibleMoves.Where(m => m.score == maxScore).ToList();
            var chosen = bestMoves[UnityEngine.Random.Range(0, bestMoves.Count)];
            return (chosen.row, chosen.col, chosen.number);
        }
        else
        {
            // если комбинаций нет — ставим случайно
            var randomMove = possibleMoves[UnityEngine.Random.Range(0, possibleMoves.Count)];
            return (randomMove.row, randomMove.col, randomMove.number);
        }
    }

    public int Check(int row, int col)
    {
        List<MatchResult> matches = new List<MatchResult>();

        // Локальная функция для добавления совпадений
        void AddMatch(int check, params (int r, int c)[] coords)
        {
            if (check == 3 || check == 7 || check == 11)
            {
                MatchResult result = new MatchResult(check);
                result.cells.AddRange(coords);
                matches.Add(result);
            }
        }

        // --- Горизонталь ---
        for (int start = -2; start <= 0; start++)
        {
            int c1 = col + start;
            if (c1 >= 0 && c1 + 2 < 7)
            {
                AddMatch(CheckTriple(tileValues[row, c1], tileValues[row, c1 + 1], tileValues[row, c1 + 2]),
                         (row, c1), (row, c1 + 1), (row, c1 + 2));
            }
        }

        for (int start = -1; start <= 0; start++)
        {
            int c1 = col + start;
            if (c1 >= 0 && c1 + 1 < 7)
            {
                AddMatch(CheckDouble(tileValues[row, c1], tileValues[row, c1 + 1]),
                         (row, c1), (row, c1 + 1));
            }
        }


        // --- Вертикаль ---
        for (int start = -2; start <= 0; start++)
        {
            int r1 = row + start;
            if (r1 >= 0 && r1 + 2 < 7)
            {
                AddMatch(CheckTriple(tileValues[r1, col], tileValues[r1 + 1, col], tileValues[r1 + 2, col]),
                         (r1, col), (r1 + 1, col), (r1 + 2, col));
            }
        }

        for (int start = -1; start <= 0; start++)
        {
            int r1 = row + start;
            if (r1 >= 0 && r1 + 1 < 7)
            {

                AddMatch(CheckDouble(tileValues[r1, col], tileValues[r1 + 1, col]),
                         (r1, col), (r1 + 1, col));
            }
        }


        // --- Диагонали ↘ и ↙ ---
        for (int dr = -2; dr <= 0; dr++)
        {
            for (int dc = -2; dc <= 0; dc++)
            {
                int r1 = row + dr;
                int c1 = col + dc;

                if (r1 >= 0 && r1 + 2 < 7 && c1 >= 0 && c1 + 2 < 7)
                    AddMatch(CheckTriple(tileValues[r1, c1], tileValues[r1 + 1, c1 + 1], tileValues[r1 + 2, c1 + 2]),
                             (r1, c1), (r1 + 1, c1 + 1), (r1 + 2, c1 + 2));

                if (r1 >= 0 && r1 + 2 < 7 && c1 - 2 >= 0)
                    AddMatch(CheckTriple(tileValues[r1, c1], tileValues[r1 + 1, c1 - 1], tileValues[r1 + 2, c1 - 2]),
                             (r1, c1), (r1 + 1, c1 - 1), (r1 + 2, c1 - 2));
            }
        }

        int[][] lShapes = new int[][]
{
    // варианты, где (row,col) в корне
    new int[]{0,0, 1,0, 1,1},
    new int[]{0,0, 0,1, 1,1},
    new int[]{0,0, 1,0, 1,-1},
    new int[]{0,0, 0,-1, 1,-1},
    new int[]{0,0, -1,0, -1,1},
    new int[]{0,0, 0,1, -1,0},
    new int[]{0,0, -1,0, -1,-1},
    new int[]{0,0, 0,-1, -1,0},

    // варианты, где (row,col) на "лапке" угла
    new int[]{0,0, -1,0, 0,1},  // клетка снизу от угла
    new int[]{0,0, -1,0, 0,-1}, // клетка снизу другой стороны
    new int[]{0,0, 1,0, 0,1},   // клетка сверху от угла
    new int[]{0,0, 1,0, 0,-1},  // клетка сверху другой стороны
    new int[]{0,0, 0,1, 1,0},   // клетка слева от угла
    new int[]{0,0, 0,-1, 1,0},  // клетка справа от угла
    new int[]{0,0, 0,1, -1,0},  // клетка слева снизу
    new int[]{0,0, 0,-1, -1,0}, // клетка справа снизу
};

        foreach (var shape in lShapes)
        {
            int r0 = row + shape[0], c0 = col + shape[1];
            int r1 = row + shape[2], c1 = col + shape[3];
            int r2 = row + shape[4], c2 = col + shape[5];

            if (r0 >= 0 && r0 < 7 && c0 >= 0 && c0 < 7 &&
                r1 >= 0 && r1 < 7 && c1 >= 0 && c1 < 7 &&
                r2 >= 0 && r2 < 7 && c2 >= 0 && c2 < 7)
            {
                AddMatch(CheckTriple(tileValues[r0, c0], tileValues[r1, c1], tileValues[r2, c2]),
                         (r0, c0), (r1, c1), (r2, c2));
            }
        }

        // --- Выбираем наибольший результат ---
        if (matches.Count > 0)
        {
            MatchResult best = matches
                .OrderByDescending(m => m.score)        // сначала очки
                .ThenByDescending(m => m.cells.Count)   // потом количество клеток
                .First();

            // Помечаем клетки ×100 и запускаем анимацию
            foreach (var (r, c) in best.cells)
            {
                tileValues[r, c] *= 100;
                StartCoroutine(DelayColorChange(tiles[r, c], whoseTurn));
            }

            return best.score;
        }

        return 0;
    }

    IEnumerator CameraMove(int whichAnimation)
    {
  

        while (isColorChanging || isCameraMove)
            yield return null;

        isCameraMove = true;

        yield return new WaitForSeconds(1.0f);

        this.GetComponent<Animation>().Play(whichAnimation == 1 ? "CameraOn" : "CameraOff");
        yield return new WaitForSeconds(this.GetComponent<Animation>()[whichAnimation == 1 ? "CameraOn" : "CameraOff"].length);
        if (whichAnimation == 1)
        {
            points1Shower.color = Color.white;
            points2Shower.color = Color.yellow;
        }
        else
        {
            points1Shower.color = Color.yellow;
            points2Shower.color = Color.white;
        }

        isCameraMove = false;
    }

    IEnumerator DelayColorChange(GameObject tile, int whoseTurn)
    {
        yield return new WaitForSeconds(0.5f);
        while (isButtonsMove || isCameraMove || isColorChanging)
            yield return null;

        

        tile.GetComponent<TileBehaviour>().ChangeColor(
            whoseTurn == 1 ? new Color(0.0f, 1.0f, 0.0f) : new Color(1.0f, 1.0f, 0.0f)
        );

        

        isColorChanging = true;
        while (TileBehaviour.isAnyAnimationPlaying)
            yield return null;

     

        isColorChanging = false;


    }

    public bool IsBoardFull(int[,] board)
    {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (board[r, c] == 0) // есть пустая клетка
                    return false;
            }
        }

        return true; // все клетки заполнены
    }

    private void AiTurn()
    {
        if (isPlayingAgainstAI && whoseTurn == 2)
        {
            int[] availableNumbers = new int[]
            {
                    Random.Range(1, 7),
                    Random.Range(1, 7),
                    Random.Range(1, 7)
            };
            (int row, int col, int number) = GetBestMove(tileValues, availableNumbers);
            SetTileValue(row, col, number);

        }
    }

    public void ChangeTurn()
    {

        if (IsBoardFull(tileValues)) { 
            GameObject.Find("MenuManager").GetComponent<MainMenu>().FinishGame(points1, points2); 
            return; 
        }

        Debug.Log(" " + whoseTurn);


        if (whoseTurn == 1)
        {
            whoseTurn = 2;
            StartCoroutine(CameraMove(1));
        

            if(isPlayingAgainstAI)
            AiTurn();
        }
        else if (whoseTurn == 2) 
        {
            whoseTurn = 1;
            //Camera.main.GetComponent<Animation>().Play("CameraOn");
            StartCoroutine(CameraMove(2));
        }



    }



    public void AddScore(int scoreToAdd)
    {
        if (whoseTurn == 1)
            points1 += scoreToAdd;
        else
            points2 += scoreToAdd;

        points1Shower.text = "Player1: " + points1;
        points2Shower.text = "Player2: " + points2;
    }

    public int AnalyseCheck(int checkResult)
    {
        int scoreToAdd = 0;

        if (checkResult == 3)
        {
            scoreToAdd = 11;
        }
        else if (checkResult == 7)
        {
            scoreToAdd = 3;
        }
        else if (checkResult == 11)
        {
            scoreToAdd = 7;
        }
        else if (checkResult == 0)
        {
            scoreToAdd = 0;
        }



        if (whoseTurn == 1)
            points1 += scoreToAdd;
        else
            points2 += scoreToAdd;

        StartCoroutine(ChangeScore());

        Debug.Log(points1 + " : " + points2);
        return scoreToAdd;
     }

    public void RefreshButtons()
    {
        for (int i = 0; i < column1.transform.childCount; i++)
        {
            column1.transform.GetChild(i).gameObject.SetActive(false);
            column2.transform.GetChild(i).gameObject.SetActive(false);
            column3.transform.GetChild(i).gameObject.SetActive(false);
        }

       

        int random = Random.Range(0, 6);
        column1.transform.GetChild(random).gameObject.SetActive(true);
        random = Random.Range(0, 6);
        column2.transform.GetChild(random).gameObject.SetActive(true);
        random = Random.Range(0, 6);
        column3.transform.GetChild(random).gameObject.SetActive(true);

    }


    public string GetPressedButton()
    {
        return pressedButton;
    }

    public void SetPressedButton(string newPressedButton)
    {
        pressedButton = newPressedButton;
    }

    public int GetWhoseTurn()
    {
        return whoseTurn;
    }


    class MatchResult
    {
        public int score;
        public List<(int r, int c)> cells;

        public MatchResult(int score)
        {
            this.score = score;
            cells = new List<(int r, int c)>();
        }
    }
}
