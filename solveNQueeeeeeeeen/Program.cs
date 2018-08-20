using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace solveNQueeeeeeeeen
{
    /// <summary>
    /// nQueenの解の一つをランダムに表示する
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // 連続処理用ループ
            while (true)
            {
                // 終了判定用bool変数
                bool isEnd = false;

                // 入力判定用bool変数
                bool isCorrectNumber = false;
                
                // 解を求める盤面のサイズ
                int boardSize = 0;

                //　正しい入力が得られるまで入力待ちを繰り返す
                while (!isCorrectNumber)
                {
                    Console.Write("生成したい盤面の幅を入力してください。終了: E -> ");
                    string line = Console.ReadLine();

                    // 終了判定
                    if (line == "E")
                    {
                        isEnd = true;
                        break;
                    }

                    isCorrectNumber = int.TryParse(line, out boardSize);

                    // 入力が整数に変換可能か判定
                    if (!isCorrectNumber)
                    {
                        Console.WriteLine("整数を入力してください。");
                        continue;
                    }

                    // 解が存在する盤面サイズか判定
                    if (boardSize < 4)
                    {
                        Console.WriteLine("整数は4以上で入力してください。");
                        isCorrectNumber = false;
                    }
                }

                // 終了判定
                if (isEnd)
                {
                    Console.WriteLine("終了します。");
                    break;
                }

                int[,] board = new int[boardSize, boardSize];

                // 処理用stackList
                List<int[,]> stack = new List<int[,]>();

                stack.Add(board);

                // 解決判定用bool変数
                bool isResolved = false;

                // 処理順をランダムにするための配列を生成
                int[] processSequence = generateSequence(boardSize);

                // 解が出るまで繰り返す
                while (!isResolved)
                {
                    isResolved = addQueen(boardSize, stack, processSequence);
                }

                // 解答盤面を表示
                showBoard(stack[stack.Count - 1]);
            }

            Console.ReadLine();
        }

        /// <summary>
        /// 盤面にQueenを配置する
        /// </summary>
        /// <param name="stack">処理待ち盤面のList</param>
        /// <param name="processSequence">処理順の配列</param>
        /// <returns>Queenを全て配置し終えた:true</returns>
        private static bool addQueen(int boardSize, List<int[,]> stack, int[] processSequence)
        {
            // 配置済みQueenの数
            int queenCount = 0;

            int index = stack.Count - 1;

            // Queenの数を数える
            for (int i = 0; i < boardSize; i++)
            {
                bool isQueenSet = false;

                for (int j = 0; j < boardSize; j++)
                {
                    if (stack[index][processSequence[i], processSequence[j]] == 1)
                    {
                        queenCount++;
                        isQueenSet = true;
                        break;
                    }
                }

                if (!isQueenSet)
                {
                    break;
                }
            }

            // 盤面のprocessSequence[queenCount]行目の全ての配置可能マス数分の配置後盤面を生成してstackに追加する
            for (int i = 0; i < boardSize; i++)
            {
                // Queenが配置できるか判定
                if (stack[index][processSequence[queenCount], processSequence[i]] == 0)
                {
                    // Queenを配置した新しい盤面を生成
                    int[,] newBoard = setQueen(stack[index], processSequence[queenCount], processSequence[i]);

                    stack.Add(newBoard);

                    // 配置したのが最後のQueenか判定
                    if (queenCount == boardSize - 1)
                    {
                        return true;
                    }
                }
            }

            stack.RemoveAt(index);

            return false;
        }

        /// <summary>
        /// 盤面の指定した座標にQueenを配置した新しい盤面を生成する
        /// </summary>
        /// <param name="board">元の盤面</param>
        /// <param name="rowIndex">配置する行のindex</param>
        /// <param name="columnIndex">配置する列のindex</param>
        /// <returns></returns>
        private static int[,] setQueen(int[,] board ,int rowIndex, int columnIndex)
        {
            // 新しい盤面を生成
            int[,] newBoard = copyIntArray(board);

            // 指定座標に1:Queenを配置
            newBoard[rowIndex, columnIndex] = 1;

            // Queen配置箇所から下方向に、-1:配置不可箇所を追加していく
            for (int i = rowIndex + 1; i < board.GetLength(0); i++)
            {
                // 下方向に配置可能箇所があるか判定
                if (newBoard[i, columnIndex] == 0)
                {
                    newBoard[i, columnIndex] = -1;
                }

                // 盤面の右下方向が存在するか判定
                if (columnIndex + (i - rowIndex) < newBoard.GetLength(1))
                {
                    // 配置可能箇所か判定
                    if (newBoard[i, columnIndex + (i - rowIndex)] == 0)
                    {
                        newBoard[i, columnIndex + (i - rowIndex)] = -1;
                    }
                }

                // 盤面の左下方向が存在するか判定
                if (0 <= columnIndex - (i - rowIndex))
                {
                    // 配置可能箇所か判定
                    if (newBoard[i, columnIndex - (i - rowIndex)] == 0)
                    {
                        newBoard[i, columnIndex - (i - rowIndex)] = -1;
                    }
                }
            }

            // Queen配置箇所から上方向に、-1:配置不可箇所を追加していく
            for (int i = rowIndex - 1; 0 <= i; i--)
            {
                // 上方向に配置可能箇所があるか判定
                if (newBoard[i, columnIndex] == 0)
                {
                    newBoard[i, columnIndex] = -1;
                }

                // 盤面の右上方向が存在するか判定
                if (columnIndex + (rowIndex - i) < newBoard.GetLength(1))
                {
                    // 配置可能箇所か判定
                    if (newBoard[i, columnIndex + (rowIndex - i)] == 0)
                    {
                        newBoard[i, columnIndex + (rowIndex - i)] = -1;
                    }
                }

                // 盤面の左上方向が存在するか判定
                if (0 <= columnIndex - (rowIndex - i))
                {
                    // 配置可能箇所か判定
                    if (newBoard[i, columnIndex - (rowIndex - i)] == 0)
                    {
                        newBoard[i, columnIndex - (rowIndex - i)] = -1;
                    }
                }
            }

            // Queen配置箇所から右方向に、-1:配置不可箇所を追加していく
            for (int i = columnIndex + 1; i < newBoard.GetLength(1); i++)
            {
                // 配置可能箇所か判定
                if (newBoard[rowIndex, i] == 0)
                {
                    newBoard[rowIndex, i] = -1;
                }
            }

            // Queen配置箇所から左方向に、-1:配置不可箇所を追加していく
            for (int i = columnIndex - 1; 0 <= i; i--)
            {
                // 配置可能箇所か判定
                if (newBoard[rowIndex, i] == 0)
                {
                    newBoard[rowIndex, i] = -1;
                }
            }

            return newBoard;
        }

        /// <summary>
        /// int型二次元配列をdeepCopyする
        /// </summary>
        /// <param name="intArray">元の二次元配列</param>
        /// <returns>コピーした二次元配列</returns>
        private static int[,] copyIntArray(int[,] intArray)
        {
            int[,] newIntArray = new int[intArray.GetLength(0), intArray.GetLength(1)];

            for (int i = 0; i < intArray.GetLength(0); i++)
            {
                for (int j = 0; j < intArray.GetLength(1); j++)
                {
                    newIntArray[i, j] = intArray[i, j];
                }
            }

            return newIntArray;
        }

        /// <summary>
        /// 盤面を表示する
        /// </summary>
        /// <param name="board">表示する盤面</param>
        private static void showBoard(int[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                StringBuilder sb = new StringBuilder();

                for (int j = 0; j < board.GetLength(1); j++)
                {
                    // 1:Queenの場合"Ｑ"を文字列に追加する
                    if (board[i, j] == 1)
                    {
                        sb.Append("Ｑ");
                    }
                    else
                    {
                        sb.Append("□");
                    }
                }

                Console.WriteLine(sb.ToString());
            }

            Console.WriteLine();
        }

        /// <summary>
        /// 指定した長さの配列を生成し連番をランダムな順序で格納する
        /// </summary>
        /// <param name="length">生成する配列の長さ</param>
        /// <returns>生成した配列</returns>
        private static int[] generateSequence(int length)
        {
            int[] processSequence = new int[length];

            Random rand = new Random();

            // 0からlength - 1までの数を持つListの作成
            List<int> numbersList = Enumerable.Range(0, length).ToList();

            // numbersListからランダムに一つ数を取得し生成した配列に代入する
            for (int i = 0; i < length; i++)
            {
                // 取得する数のindexを乱数で決定する
                int index = rand.Next(numbersList.Count);
                processSequence[i] = numbersList[index];

                // 取得した数をnumbersListから削除する
                numbersList.RemoveAt(index);
            }

            return processSequence;
        }
    }
}