//using UnityEngine;

//public class PuzzleTilesInitializer : MonoBehaviour
//{
//    public PuzzleTilesController controller;
//    public int width = 3;
//    public int height = 3;

//    void Start()
//    {
//        controller.grid = new RotatingTile[width, height];

//        int i = 0;
//        foreach (Transform t in transform)
//        {
//            RotatingTile tile = t.GetComponent<RotatingTile>();

//            int x = i % width;
//            int y = i / width;

//            controller.grid[x, y] = tile;

//            i++;
//        }
//    }
//}
