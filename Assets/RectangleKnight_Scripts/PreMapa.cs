using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapConstruct
{
    private struct LimitantesDeMapa
    {
        public int xMin;
        public int xMax;
        public int yMin;
        public int yMax;
    }

    public Dictionary<MyVector2Int, MyColor> GetMapDates { get; set; } = new Dictionary<MyVector2Int, MyColor>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    public void AtualizarMapa()
    {
        TilemapCollider2D[] mapCols = GameObject.FindObjectsOfType<TilemapCollider2D>();

        for (int i = 0; i < mapCols.Length; i++)
        {
            if(!mapCols[i].isTrigger)
                ConstruaMapa(mapCols[i].GetComponent<Tilemap>());
        }
    }

    private Color CorAlvoMap(int row, int col, LimitantesDeMapa L)
    {
        MyVector2Int V = new MyVector2Int(col + L.xMin, row + L.yMin);

        if (GetMapDates.ContainsKey(V))
        {
            return GetMapDates[V].cor;
        }

        return Color.white;
    }

    private LimitantesDeMapa CalculaLimitanteMapBase()
    {
        LimitantesDeMapa L = new LimitantesDeMapa() { xMin = 0, xMax = 0, yMin = 0, yMax = 0 };
        Dictionary<MyVector2Int, MyColor>.KeyCollection.Enumerator z = GetMapDates.Keys.GetEnumerator();
        z.MoveNext();
        MyVector2Int V = z.Current;
        L = new LimitantesDeMapa() { xMin = V.x, xMax = V.x, yMin = V.y, yMax = V.y };
        return L;
    }

    private LimitantesDeMapa CalculaLimitanteMap()
    {
        LimitantesDeMapa L = CalculaLimitanteMapBase();

        Dictionary<MyVector2Int, MyColor>.KeyCollection keys = GetMapDates.Keys;

        foreach (MyVector2Int V in keys)
        {
            L = VerificaMudancasDeLimitantes(V, L);
        }

        return L;

    }

    private void ConstruaMapa(Tilemap tilemap)
    {
        BoundsInt area = tilemap.cellBounds;

        for (int col = area.yMax; col >= area.yMin; col--)
        {
            for (int row = area.xMin; row <= area.xMax; row++)
            {
                
                Vector3 V = tilemap.CellToWorld(new Vector3Int(row, col, 0));

                if (tilemap.GetTile(new Vector3Int(row, col, 0)) != null)
                {
                    GetMapDates[new MyVector2Int((int)V.x, (int)V.y)] = new MyColor(Color.black);
                }
            }
        }

        /*
        LimitantesDeMapa L = CalculaLimitanteMap();

        tex = new Texture2D(L.xMax-L.xMin, L.yMax - L.yMin);
        for (int row=0;row<L.yMax-L.yMin;row++)
            for(int col=0;col<L.xMax-L.xMin;col++)
                tex.SetPixel(col, row, CorAlvoMap(row,col,L));

        tex.Apply();

        img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), tex.texelSize);
        */
    }

    public Texture2D TexturaDeMapaAtual()
    {
        LimitantesDeMapa L = CalculaLimitanteMap();

        Texture2D tex = new Texture2D(L.xMax - L.xMin, L.yMax - L.yMin);
        for (int row = 0; row < L.yMax - L.yMin; row++)
            for (int col = 0; col < L.xMax - L.xMin; col++)
                tex.SetPixel(col, row, CorAlvoMap(row, col, L));

        tex.Apply();

        return tex; 
    }

    LimitantesDeMapa VerificaMudancasDeLimitantes(MyVector2Int V, LimitantesDeMapa L)
    {
        if (L.xMin > V.x)
            L.xMin = V.x;

        if (L.yMin > V.y)
            L.yMin = V.y;

        if (L.xMax < V.x)
            L.xMax = V.x;

        if (L.yMax < V.y)
            L.yMax = V.y;



        return L;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    private void Ant(Tilemap tilemap)
    {
        //Grid T = FindObjectOfType<Grid>();

        //Debug.Log(T.transform.GetChild(0).GetComponent<Tilemap>().name);


        //Tilemap tilemap = T;T.transform.GetChild(0).GetComponent<Tilemap>();


        //tex = new Texture2D(rowNumbers,colNumbers);

        //Debug.Log("Cell size: "+tilemap.cellSize);

        
        for (int col = area.yMax; col >= area.yMin; col--)
        {
            for (int row = area.xMin; row <= area.xMax; row++)
            {
                int x = row - area.xMin;
                int y = col - area.yMin;
                Vector3 V = tilemap.CellToWorld(new Vector3Int(row, col, 0));

                
                if (tilemap.GetTile(new Vector3Int(row, col, 0)) != null)
                {
                    //Debug.Log((int)V.x+" : "+(int)V.y);
                    //Debug.Log(tilemap.CellToWorld(new Vector3Int(row, col, 0)) + " : " + tilemap.GetTile(new Vector3Int(row, col, 0)));
                    // tex.SetPixel( x, y, Color.black);

                    mapDates[new Vector2Int((int)V.x, (int)V.y)] = Color.black;
                    /*
                    if (V.x >= 0 && V.y >= 0)
                        q_PosPos[new Vector2Int((int)V.x, (int)V.y)] = Color.black;
                    else if(V.x>=0 &&V.y<0)
                        q_PosNeg[new Vector2Int((int)V.x, (int)(V.y))] = Color.black;
                    else if (V.x < 0 && V.y >= 0)
                        q_NegPos[new Vector2Int((int)(V.x), (int)(V.y))] = Color.black;
                    else if (V.x < 0 && V.y < 0)
                        q_NegNeg[new Vector2Int((int)(V.x), (int)(V.y))] = Color.black;
                        
                }*/

    /*
    else
    {
        tex.SetPixel( row - area.xMin, col - area.yMin, Color.white);
    }
}
}

    //tex.Apply();

    //img.sprite = Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),tex.texelSize);

    LimitantesDeMapa L = CalculaLimitanteMap();

        
        tex = new Texture2D(L.xMax-L.xMin, L.yMax - L.yMin);
        for (int row=0;row<L.yMax-L.yMin;row++)
            for(int col=0;col<L.xMax-L.xMin;col++)
                tex.SetPixel(col, row, CorAlvoMap(row,col,L));

        tex.Apply();

        img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), tex.texelSize);
    }*/



    /*
    Color CorAlvo(int row,int col,LimitantesDeMapa L)
    {
        Vector2Int V = new Vector2Int(col + L.xMin, row + L.yMin);

        if (V.x < 0)
        {
            if (V.y < 0)
                if (q_NegNeg.ContainsKey(V))
                {
                    return q_NegNeg[V];
                }

            if (V.y >= 0)
                if (q_NegPos.ContainsKey(V))
                    return q_NegPos[V];
        }
        else if(V.x>=0)
        {
            if (V.y < 0)
                if (q_PosNeg.ContainsKey(V))
                {
                    Debug.Log("aqui: "+V);
                    return q_PosNeg[V];
                }
                else
                {
                    Debug.Log("não teve: "+V);
                }

            if (V.y >= 0)
                if (q_PosPos.ContainsKey(V))
                    return q_PosPos[V];
        }

        return Color.white;
    }

    LimitantesDeMapa ColocarUmLimitanteBase()
    {
        LimitantesDeMapa L = new LimitantesDeMapa() { xMin = 0, xMax = 0, yMin = 0, yMax = 0 };
        Dictionary<Vector2Int, Color>.KeyCollection keys = q_PosPos.Keys;
        Vector2Int V;
        Dictionary<Vector2Int, Color>.KeyCollection.Enumerator z;

        Debug.Log("PosPos: "+keys.Count);
        if (keys.Count > 0)
        {
            z = keys.GetEnumerator();
            z.MoveNext();
            V = z.Current;
            L = new LimitantesDeMapa() { xMin = V.x, xMax = V.x, yMin = V.y, yMax = V.y };
        }
        else
        {
            keys = q_PosNeg.Keys;

            Debug.Log("PosNeg: " + keys.Count);
            if (keys.Count > 0)
            {
                z = keys.GetEnumerator();
                z.MoveNext();
                V = z.Current;
                L = new LimitantesDeMapa() { xMin = V.x, xMax = V.x, yMin = V.y, yMax = V.y };
                Debug.Log(L.xMin + " : " + L.xMax + " : " + L.yMin + " : " + L.yMax);
            }
            else
            {
                keys = q_NegPos.Keys;

                Debug.Log("NegPos: " + keys.Count);
                if (keys.Count > 0)
                {
                    z = keys.GetEnumerator();
                    z.MoveNext();
                    V = z.Current;
                    L = new LimitantesDeMapa() { xMin = V.x, xMax = V.x, yMin = V.y, yMax = V.y };
                }
                else
                {
                    keys = q_NegNeg.Keys;

                    Debug.Log("NegNeg: " + keys.Count);
                    if (keys.Count > 0)
                    {
                        z = keys.GetEnumerator();
                        z.MoveNext();
                        V = z.Current;
                        L = new LimitantesDeMapa() { xMin = V.x, xMax = V.x, yMin = V.y, yMax = V.y };
                    }
                }
            }
        }

        return L;
    }

    LimitantesDeMapa CalculaLimitante()
    {
        LimitantesDeMapa L = ColocarUmLimitanteBase();
        Dictionary<Vector2Int, Color>.KeyCollection keys = q_PosPos.Keys;
        
        foreach (Vector2Int V in keys)
        {
            L = VerificaMudancasDeLimitantes(V, L);
        }

        keys = q_NegPos.Keys;
        foreach (Vector2Int V in keys)
        {
            L = VerificaMudancasDeLimitantes(new Vector2Int(-V.x,V.y), L);
        }



        keys = q_NegNeg.Keys;
        foreach (Vector2Int V in keys)
        {
            L = VerificaMudancasDeLimitantes(new Vector2Int(-V.x, -V.y), L);
        }



        keys = q_PosNeg.Keys;
        foreach (Vector2Int V in keys)
        {
            L = VerificaMudancasDeLimitantes(new Vector2Int(V.x, -V.y), L);
        }

        Debug.Log(L.xMin + " : " + L.xMax + " : " + L.yMin + " : " + L.yMax);

        return L;
    }*/


}
