using UnityEngine;
using Game.Noticfacation;
using Game;
using System.Collections.Generic;


class Directions
{
    public static Vector2 none = new Vector2(0, 0);
    public static Vector2 up = new Vector2(0, 1);
    public static Vector2 down = new Vector2(0, -1);
    public static Vector2 left = new Vector2(-1, 0);
    public static Vector2 right = new Vector2(1, 0);

    public static Vector2[] all = { up, down, left, right };

}

public class MapGenerate : MonoNotice
{
    public static MapGenerate Instance;
    private void Awake()
    {
        Instance = this;
    }
    /**=====================================================================mazeandroom=========================================================*/
    //尝试生成房间的数量
    private int numRoomTries = 50;
    //在已经连接的房间和走廊中再次连接的机会，使得地牢不完美
    private int extraConnectorChance = 20;
    //控制生成房间的大小
    private int roomExtraSize = 20;
    //控制迷宫的曲折程度
    private int windingPercent = 10;
    private int width = 199;
    private int height = 199;

    private Transform mapParent;

    //生成的有效房间
    private List<Rect> rooms;

    //正被雕刻的区域的索引。(每个房间一个索引，每个不连通的迷宫一个索引，在连通之前)
    private int currentRegion = 0;

    private int[,] _regions;
    //private Tiles[,] map;
    private int[,] map;

    //宽 高 随机房间次数
    public void creatDungeon(int w,int h,int addroomtimes,int extraChance,int roomsize) {
        width = w;
        height = h;
        numRoomTries = addroomtimes;
        extraConnectorChance = extraChance;
        roomExtraSize = roomsize;

        rooms = new List<Rect>();
        //map = new Tiles[width, height];
        map = new int[width, height];
        _regions = new int[width, height];
        mapParent = GameObject.FindGameObjectWithTag("mapparent").transform;
        Generate();
    }

    public void Generate()
    {
        if (width % 2 == 0 || height % 2 == 0)
        {
            Debug.Log("地图长宽不能为偶数");
            return;
        }
        InitMap();
        AddRooms();
        FillMaze();
        ConnectRegions();
        InstanceMap();
    }

    private void ConnectRegions()
    {
        //找到区域所有可连接的空间墙wall
        Dictionary<Vector2, List<int>> connectorRegions = new Dictionary<Vector2, List<int>>();
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < height - 1; j++)
            {
                //不是墙的跳过
                if (map[i, j] != 0)
                    continue;
                List<int> regions = new List<int>();
                foreach (Vector2 dir in Directions.all)
                {
                    int region = _regions[i + (int)dir.x, j + (int)dir.y];
                    //如果周围不是墙（墙的索引为regions的初始值为0）
                    //去重
                    if (region != 0 && !regions.Contains(region))
                        regions.Add(region);
                }
                //如果这个墙没有连接一个以上的区域，那就不是一个连接点
                if (regions.Count < 2)
                    continue;
                connectorRegions[new Vector2(i, j)] = regions;
                //标志连接点
                //SetConnectCube(i,j);
            }
        }
        //所有连接点
        List<Vector2> connectors = new List<Vector2>(connectorRegions.Keys);
        //跟踪哪些区域已合并。将区域索引映射为它已合并的区域索引。
        List<int> merged = new List<int>();
        List<int> openRegions = new List<int>();
        for (int i = 0; i <= currentRegion; i++)
        {
            merged.Add(i);
            openRegions.Add(i);
        }
        //使区域连接最终只剩下一个
        while (openRegions.Count > 1)
        {
            //随机选择一个连接点
            Vector2 connector = connectors[Random.Range(0, connectors.Count - 1)];
            //连接
            AddJunction(connector);
            //合并连接区域我们将选择第一个区域（任意）和
            //将所有其他区域映射到其索引。
            //connectorRegions[connector]
            List<int> regions = connectorRegions[connector];
            for (int i = 0; i < regions.Count; i++)
            {
                regions[i] = merged[regions[i]];
            }
            int dest = regions[0];
            regions.RemoveAt(0);
            List<int> sources = regions;
            //合并所有受影响的区域
            for (int i = 0; i < currentRegion; i++)
            {
                if (sources.Contains(merged[i]))
                {
                    merged[i] = dest;
                }
            }
            //移除已经连接的区域
            foreach (int s in sources)
            {
                openRegions.RemoveAll(value => (value == s));
            }
            connectors.RemoveAll(index => IsRemove(merged, connectorRegions, connector, index));
        }
    }

    private bool IsRemove(List<int> merged, Dictionary<Vector2, List<int>> ConnectRegions, Vector2 connector, Vector2 pos)
    {
        //不让连接器相连（包括斜向相连）
        if ((connector - pos).SqrMagnitude() < 2)
        {
            return true;
        }
        List<int> temp = ConnectRegions[pos];
        for (int i = 0; i < temp.Count; i++)
        {
            temp[i] = merged[temp[i]];
        }
        HashSet<int> set = new HashSet<int>(temp);
        //判断连接点是否和两个区域相邻，不然移除
        if (set.Count > 1)
        {
            return false;
        }
        //增加连接，使得地图连接不是单连通的
        if (Random.Range(0, extraConnectorChance) == 0) AddJunction(pos);
        return true;
    }

    private void AddJunction(Vector2 pos)
    {
        map[(int)pos.x, (int)pos.y] = 1;
    }

    private void FillMaze()
    {
        //0处为墙
        for (int x = 1; x < width; x += 2)
        {
            for (int y = 1; y < height; y += 2)
            {
                Vector2 pos = new Vector2(x, y);
                //if (map [pos] == Tiles.Wall) {
                if (map[x, y] == 0)
                {
                    GrowMaze(pos);
                }
            }
        }
    }

    private void GrowMaze(Vector2 start)
    {
        List<Vector2> cells = new List<Vector2>();
        Vector2 lastDir = Directions.none;
        StartRegion();
        //cells添加之前需要变成Floor
        Carve(start);
        cells.Add(start);
        while (cells != null && cells.Count != 0)
        {
            Vector2 cell = cells[cells.Count - 1];
            //可以扩展的方向的集合
            List<Vector2> unmadeCells = new List<Vector2>();
            //加入能扩展迷宫的方向
            foreach (Vector2 dir in Directions.all)
            {
                if (CanCarve(cell, dir))
                {
                    unmadeCells.Add(dir);
                }
            }
            if (unmadeCells != null && unmadeCells.Count != 0)
            {
                Vector2 dir;
                //得到扩展方向 windingPercent用来控制是否为原方向
                if (unmadeCells.Contains(lastDir) && Random.Range(0, 100) > windingPercent)
                {
                    dir = lastDir;
                }
                else
                {
                    dir = unmadeCells[Random.Range(0, unmadeCells.Count - 1)];
                }

                Carve(cell + dir);
                Carve(cell + dir * 2);
                //添加第二个单元
                cells.Add(cell + dir * 2);
                lastDir = dir;
            }
            else
            {
                //没有相邻可以雕刻的单元，就删除
                cells.Remove(cells[cells.Count - 1]);
                //置空路径
                lastDir = Directions.none;
            }

        }
    }

    private bool CanCarve(Vector2 pos, Vector2 dir)
    {
        Vector2 temp = pos + 3 * dir;
        int x = (int)temp.x, y = (int)temp.y;
        //判断是否超过边界
        if (x < 0 || x > width || y < 0 || y > height)
        {
            return false;
        }
        //需要判断方向第二个单元的原因是cells中需要添加下一个cell
        //所以下一个cell要变为Floor,然后需要判断是否第二个单元是否为墙
        //如果不为墙，则第一个cell被变为Floor为，和第二个单元就连通了，不可行
        //判断第二个单元主要用来判断不能＆其他房间或走廊（regions）连通
        temp = pos + 2 * dir;
        x = (int)temp.x;
        y = (int)temp.y;
        //是墙则能雕刻迷宫
        return map[x, y] == 0;
    }

    private void AddRooms()
    {
        for (int i = 0; i < numRoomTries; i++)
        {
            //确保房间长宽为奇数
            int size = Random.Range(1, 3 + roomExtraSize) * 2 + 1;
            int rectangularity = Random.Range(0, 1 + size / 2) * 2;
            int w = size, h = size;
            if (0 == Random.Range(0, 1))
            {
                w += rectangularity;
            }
            else
            {
                h += rectangularity;
            }
            int x = Random.Range(0, (width - w) / 2) * 2 + 1;
            int y = Random.Range(0, (height - h) / 2) * 2 + 1;
            Rect room = new Rect(x, y, w, h);
            //判断房间是否和已存在的重叠
            bool overlaps = false;
            foreach (Rect r in rooms)
            {
                if (room.Overlaps(r))
                {
                    overlaps = true;
                    break;
                }
            }
            //如果重叠，抛弃该房间
            if (overlaps)
                continue;
            //如果不重叠，把房间放入rooms中
            rooms.Add(room);
            //设置新房间索引
            StartRegion();

            for (int j = x; j < x + w; j++)
            {
                for (int k = y; k < y + h; k++)
                {
                    Carve(new Vector2(j, k));
                }
            }
        }
    }
    private void StartRegion()
    {
        currentRegion++;
    }
    private void Carve(Vector2 pos)
    {
        int x = (int)pos.x, y = (int)pos.y;
        map[x, y] = 1;
        _regions[x, y] = currentRegion;
    }



    private void InitMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = 0;
            }
        }
    }


    private void InstanceMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (map[i, j] == 1)
                {
                    /**GameObject go = Instantiate(floor, new Vector3(i, 1, j), Quaternion.identity) as GameObject;
                    go.transform.SetParent(mapParent);
                    //设置层级
                    go.layer = LayerMask.NameToLayer("floor");*/
                }
                else if (map[i, j] == 0)
                {
                    GameObject go = Instantiate(MapManager.Instance.getRandomWall(), new Vector3(i, 1, j), Quaternion.identity) as GameObject;
                    go.transform.SetParent(mapParent);
                    //go.layer = LayerMask.NameToLayer("wall");
                }
            }
        }
        for (int i = 0; i < rooms.Count; i++)
        {
            this.creatRoom(rooms[i]);
        }
        

        Notice.GAME_PLAYER_ATIVE.broadcast();
    }

    private void creatRoom(Rect rect) {
        MonsterManager.Instance.creatEnemy(rect);
        //GameObject go = Instantiate(MapManager.Instance.getRandomFlower(), new Vector3(rect.x +rect.width/2, 1.1f, rect.y+rect.height/2), Quaternion.identity) as GameObject;
        //GameObject enemy = Instantiate(MonsterManager.Instance.getWarriorMonster(), new Vector3(rect.x + rect.width / 2, 1.0f, rect.y + rect.height / 2), Quaternion.identity) as GameObject; 
        //enemy.transform.position = new Vector3(rect.x + rect.width / 2, 1.1f, rect.y + rect.height / 2);
    }









    /**private void RemoveDeadEnds()
{
    bool done = false;
    while (!done)
    {
        done = true;
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < height - 1; j++)
            {
                if (map[i, j] == 0)
                    continue;
                int exists = 0;
                foreach (Vector2 dir in Directions.all)
                {
                    if (map[i + (int)dir.x, j + (int)dir.y] != 0)
                    {
                        exists++;
                    }
                }
                //如果exists==1则是三面环墙
                if (exists != 1)
                {
                    continue;
                }
                done = false;
                _regions[i, j] = 0;//变成墙
                map[i, j] = 0;
            }
        }
    }
}**/
    /*===============================================maze========================================*/
    public int[,] maplist;
    public int[,] disposeList;
    public List<List<int>> weekList;
    private List<int> curtoarr;

    public void creatBaseMap(int w, int h)
    {
        maplist = new int[200, 200];
        disposeList = new int[200, 200];
        weekList = new List<List<int>>();

        for (int i = 0; i < 200; i++)
        {

            for (int j = 0; j < 200; j++)
            {
                if (i % 2 == 1 && j % 2 == 1)
                {
                    maplist[i, j] = 1;
                }
                else
                {
                    maplist[i, j] = 0;
                }
            }
        }

        this.randomDirection(1, 1);


        //this.gettargetpoint();
    }
    private void randomDirection(int x, int y)
    {
        List<List<int>> maybeList = this.maybeGoList(x, y);
        if (maybeList.Count > 0)
        {
            int random = UnityEngine.Random.Range(0, maybeList.Count);
            List<int> toarr = maybeList[random];
            int tox = (toarr[0] - x) / 2 + x;
            int toy = (toarr[1] - y) / 2 + y;

            maplist[tox, toy] = 1;

            disposeList[toarr[0], toarr[1]] = 1;
            this.weekList.Add(toarr);
            //Debug.Log("走过"+ this.weekList.Count);

            mazeindex++;
            if (mazeindex < 100)
                this.randomDirection(toarr[0], toarr[1]);
            else
            {
                curtoarr = toarr;
                Invoke("detonateBoom", .2f);
            }
            return;
        }
        else
        {
            //console.log("一条路走到头 ，查看之前的岔道"+ this.weekList.length);
            for (var i = this.weekList.Count - 1; i >= 0; i--)
            {
                if (this.maybeGoList(this.weekList[i][0], this.weekList[i][1]).Count > 0)
                {
                    this.randomDirection(this.weekList[i][0], this.weekList[i][1]);
                    return;
                }
            }
        }

        Notice.MAZE_CREAT_COMPLETE.broadcast(maplist);
        Notice.GAME_PLAYER_ATIVE.broadcast();
        Debug.Log("==============================================================完成");
    }
    public void detonateBoom()
    {
        mazeindex = 0;
        this.randomDirection(curtoarr[0], curtoarr[1]);
    }
    private int mazeindex;
    private List<List<int>> maybeGoList(int x, int y)
    {
        List<List<int>> maybeList = new List<List<int>>();
        if (x - 2 > 0)
        {
            if (disposeList[x - 2, y] == 0)
            {
                List<int> arr = new List<int>();
                arr.Add(x - 2);
                arr.Add(y);
                maybeList.Add(arr);
            }

        }
        if (x + 2 < 200)
        {
            //Debug.Log(disposeList[x + 2, y]);
            if (disposeList[x + 2, y] == 0)
            {
                List<int> arr = new List<int>();
                arr.Add(x + 2);
                arr.Add(y);
                maybeList.Add(arr);
            }
        }
        if (y - 2 > 0)
        {
            if (disposeList[x, y - 2] == 0)
            {
                List<int> arr = new List<int>();
                arr.Add(x);
                arr.Add(y - 2);
                maybeList.Add(arr);
            }
        }
        if (y + 2 < 200)
        {
            if (disposeList[x, y + 2] == 0)
            {
                List<int> arr = new List<int>();
                arr.Add(x);
                arr.Add(y + 2);
                maybeList.Add(arr);
            }
        }
        ///Debug.Log(maybeList.Count);
        return maybeList;
    }

}