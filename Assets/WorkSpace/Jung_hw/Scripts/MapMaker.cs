using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[System.Serializable]
public class TileInfo
{
    public int tileId;
    public Vector3 tileRot;

    //프로퍼티쓰니까 Json에 데이터 안써짐
    /*public int TileId
    {
        get
        {
            return tileId;
        }
        set
        {
            tileId = value;
        }
    }


    public Vector3 TileRot
    {
        get
        {
            return tileRot;
        }
        set
        {
            tileRot = value;
        }
    }*/

    public TileInfo(int id, Vector3 rot)
    {
        tileId = id;
        tileRot = rot;
    }
}

[System.Serializable]
public class Serialization<T>
{
    public Serialization(List<T> _target) => map = _target;
    public List<T> map;
}
public class MapMaker : MonoBehaviour
{
    [SerializeField] NavMeshSurface surface;
    [SerializeField] bool isRight; //오른쪽 맵인지

    [Header("Objects")]
    [SerializeField][Tooltip("바닥 타일")] GameObject baseObj;
    [SerializeField][Tooltip("시작점 Transform")] Transform startPos;
    [SerializeField][Tooltip("타일 정보 ScriptableObject")] Placeables placeables;
    [SerializeField][Tooltip("설치할 타일들 프리팹")] List<Placeables.Placeable> tiles;
    Dictionary<string, int> tileNum;
    List<GameObject> placingTiles; //설치 미리보기용

    [Header("Materials")]
    [SerializeField][Tooltip("설치가능할 때")] Material canPlaceMat;
    [SerializeField][Tooltip("설치불가능할 때")] Material cannotPlaceMat;

    [Header("Size")]
    [SerializeField][Tooltip("가로 칸 수")] int hor;
    [SerializeField][Tooltip("세로 칸 수")] int ver;
    [SerializeField][Tooltip("간격(길이)")] float dist;

    [Header("for Test")]
    [SerializeField] float playFallTime;
    [SerializeField] float fallingTime;
    [SerializeField] int fallHeight;
    public bool placeMode = false;
    [SerializeField] bool canPlace = false;
    [SerializeField] bool randomGenerate = false;
    [SerializeField][Tooltip("현재 선택한 타일 인덱스")] int selected = 0;
    List<TileInfo> tileList;

    Ray ray;
    RaycastHit hit;
    Vector3 placeRot = new Vector3(0, 15, 0); //회전간격

    // Start is called before the first frame update
    void Awake()
    {
        tiles = new(placeables.placeableList);
        tileNum = new();
        for (int i = 0; i < tiles.Count; i++)
        {
            tileNum.Add(tiles[i].placeable.name, i);

        }

        placingTiles = new List<GameObject>();
        tileList = new List<TileInfo>(hor * ver);
        for (int i = 0; i < tileList.Capacity; i++)
        {
            tileList.Add(new TileInfo(0, Vector3.zero));
        }

        Debug.Log("ca " + tileList.Capacity);
        foreach (var tile in tiles) //미리보기용 타일 프리팹들 생성해놓기
        {
            GameObject tempTile = (tile.placeable == null ? null : Instantiate(tile.placeable));
            if (tempTile != null)
            {
                tempTile.SetActive(false);
            }
            placingTiles.Add(tempTile);
        }
        MakeBase();
        if (randomGenerate)
        {
            RandomTile();
            DisplayMap();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //설치모드 진입
        {
            placeMode = !placeMode;
            placingTiles[selected]?.SetActive(placeMode);
        }
        if (Input.GetKeyDown(KeyCode.R)) //모든 타일 삭제
        {
            RandomGenerator();
        }

        if (Input.GetKeyDown(KeyCode.D)) //모든 타일 삭제
        {
            DeleteTiles();
        }

        if (Input.GetKeyDown(KeyCode.M)) //저장된 모든 타일 생성
        {
            MakeMap();
        }
        if (placeMode)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) //타일 인덱스 이전
            {
                placingTiles[selected]?.SetActive(false);
                selected -= (selected > 0 ? 1 : 0);
                placingTiles[selected]?.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)) //타일 인덱스 다음
            {
                placingTiles[selected]?.SetActive(false);
                selected += (selected < placingTiles.Count - 1 ? 1 : 0);
                placingTiles[selected]?.SetActive(true);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0) //휠업 (반시계)
            {
                placingTiles[selected].transform.rotation = Quaternion.Euler(placingTiles[selected].transform.eulerAngles - placeRot);
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0) //휠다운 (시계)
            {
                placingTiles[selected].transform.rotation = Quaternion.Euler(placingTiles[selected].transform.eulerAngles + placeRot);
            }


            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("FLOOR")) && hit.transform.CompareTag("FLOOR")) //바닥타일 체크
            {
                placingTiles[selected].SetActive(true);
                placingTiles[selected].transform.position = hit.transform.GetChild(0).position; //설치위치로 미리보기 오브젝트 이동
                if (hit.transform.GetChild(0).childCount == 0) //이미 설치된 타일이 없으면?
                {
                    canPlace = true;
                    if (placingTiles[selected].transform.GetChild(0).TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
                    {
                        mesh.material = canPlaceMat;
                    }
                }
                else
                {
                    canPlace = false;
                    if (placingTiles[selected].transform.GetChild(0).TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
                    {
                        mesh.material = cannotPlaceMat;
                    }
                }
            }
            else
            {
                canPlace = false;
                placingTiles[selected].SetActive(false);
            }

            if (Input.GetMouseButtonDown(0)) //설치시도
            {
                if (canPlace)
                {
                    if (hit.transform.gameObject != null)
                    {
                        if (tiles[selected].stock > 0)
                        {
                            //구조체 리스트 내용 바꾸려면 새로 선언해야됨;;
                            tiles[selected] = new Placeables.Placeable(tiles[selected].placeable, tiles[selected].stock - 1);

                            GameObject placed = Instantiate(tiles[selected].placeable, hit.transform.GetChild(0));
                            placed.name = tiles[selected].placeable.name;
                            placed.transform.rotation = placingTiles[selected].transform.rotation;
                            Debug.Log(hit.transform.name);
                            if (int.TryParse(hit.transform.name, out int num))
                            {
                                tileList[num] = new TileInfo(selected + 1, placed.transform.eulerAngles);
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("몬한다");
                }
            }

            if (Input.GetMouseButtonDown(1)) //삭제 시도
            {
                if (!canPlace && hit.collider != null)
                {
                    if (hit.transform.gameObject != null && hit.transform.GetChild(0).childCount > 0)
                    {
                        if (int.TryParse(hit.transform.name, out int num))
                        {
                            Debug.Log(num + " " + tileList[num]);
                            tileList[num] = new TileInfo(0, Vector3.zero);
                        }
                        string tileName = hit.transform.GetChild(0).GetChild(0).name;
                        Debug.Log("TN " + tileName);
                        Destroy(hit.transform.GetChild(0).GetChild(0).gameObject);
                        tiles[tileNum[tileName]] =
                            new Placeables.Placeable(tiles[tileNum[tileName]].placeable, tiles[tileNum[tileName]].stock + 1);
                    }
                }
            }
        }
    }

    public void MakeBase() //바닥타일 생성
    {
        for (int i = 0; i < tileList.Capacity; i++)
        {
            Vector3 pos = new Vector3(i % hor * dist, 0, i / hor * dist);
            GameObject baseTile = Instantiate(baseObj, startPos.position + pos, Quaternion.identity);
            baseTile.name = i.ToString();
            baseTile.transform.parent = startPos;
        }
        //마주보게 만들기
        if (isRight)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        surface.BuildNavMesh();
    }

    public void DeleteTiles() //설치된 타일 제거
    {
        for (int i = 0; i < startPos.childCount; i++)
        {
            if (startPos.GetChild(i).GetChild(0).childCount > 0)
            {
                Destroy(startPos.GetChild(i).GetChild(0).GetChild(0).gameObject);
            }
        }
    }

    public void MakeMap() //저장된 타일 불러오기
    {
        DeleteTiles();
        Debug.Log("MakeMap");
        for (int i = 0; i < tileList.Capacity; i++)
        {
            if (tileList[i].tileId != 0)
            {
                GameObject placed = Instantiate(tiles[tileList[i].tileId - 1].placeable, startPos.transform.GetChild(i).GetChild(0));
                placed.transform.rotation = Quaternion.Euler(tileList[i].tileRot);
                placed.name = tiles[tileList[i].tileId - 1].placeable.name;

                Vector3 height = new Vector3(0, fallHeight, 0);
                if (startPos.GetChild(i).GetChild(0).childCount > 0)
                {
                    startPos.GetChild(i).GetChild(0).GetChild(0).transform.localPosition += height;
                }
            }
        }
        StartCoroutine(PlayTileFall());
    }

    public void DisplayMap()
    {
        DeleteTiles();
        Debug.Log("MakeMap");
        for (int i = 0; i < tileList.Capacity; i++)
        {
            if (tileList[i].tileId != 0)
            {
                GameObject placed = Instantiate(tiles[tileList[i].tileId - 1].placeable, startPos.transform.GetChild(i).GetChild(0));
                placed.transform.rotation = Quaternion.Euler(tileList[i].tileRot);
                placed.name = tiles[selected].placeable.name;

                tiles[tileList[i].tileId - 1] = new Placeables.Placeable(tiles[tileList[i].tileId - 1].placeable, tiles[tileList[i].tileId - 1].stock - 1);
            }
        }
    }

    IEnumerator PlayTileFall()
    {
        WaitForSeconds wait = new WaitForSeconds(playFallTime / tileList.Capacity);
        for (int i = 0; i < tileList.Capacity; i++)
        {
            if (startPos.GetChild(i).GetChild(0).childCount > 0)
            {
                StartCoroutine(FallTile(startPos.GetChild(i).GetChild(0).GetChild(0).gameObject));
            }
            yield return wait;
        }
    }

    IEnumerator FallTile(GameObject fallingObj)
    {
        float tick = Time.deltaTime;
        float moveAmount = fallingObj.transform.localPosition.y / (fallingTime / tick);
        Vector3 moveVec = new Vector3(0, moveAmount, 0);
        WaitForSeconds wait = new WaitForSeconds(tick);

        while (fallingObj.transform.localPosition.y > 0)
        {
            fallingObj.transform.localPosition -= moveVec;
            yield return wait;
        }
        fallingObj.transform.localPosition = Vector3.zero;
    }

    public void RandomTile()
    {
        for (int i = 0; i < tileList.Capacity; i++)
        {
            Debug.Log("aa " + i);
            Debug.Log("bb " + tileList[i].tileId + " " + tileList[i].tileRot);
            Vector3 rot = new Vector3(0, Random.Range(0f, 360f), 0);
            int tileNum = Random.Range(0, tiles.Count + 1);
            if (tileNum == 0)
            {
                tileList[i] = new TileInfo(tileNum, Vector3.zero);
            }
            else
            {
                tileList[i] = new TileInfo(tileNum, tiles[tileNum - 1].placeable.transform.eulerAngles + rot);
            }
        }
    }

    public void RandomGenerator()
    {
        DeleteTiles();
        RandomTile();
        MakeMap();
        surface.BuildNavMesh();
    }

    public string GetTileList()
    {
        string json = JsonUtility.ToJson(new Serialization<TileInfo>(tileList), true);
        Debug.Log("js " + json);
        return json;
    }

    public void SetTileList(string toMap)
    {
        tileList = JsonUtility.FromJson<Serialization<TileInfo>>(toMap).map;
    }
}