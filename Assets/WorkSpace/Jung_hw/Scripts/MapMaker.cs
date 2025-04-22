using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public struct TileInfo
{
    public int TileId
    {
        get; set;
    }

    public Vector3 TileRot
    {
        get; set;
    }

    public TileInfo(int id, Vector3 rot)
    {
        TileId = id;
        TileRot = rot;
    }
}

public class MapMaker : MonoBehaviour
{
    [SerializeField] NavMeshSurface surface;
    [SerializeField] bool isRight; //오른쪽 맵인지

    [Header("Objects")]
    [SerializeField][Tooltip("바닥 타일")] GameObject baseObj;
    [SerializeField][Tooltip("시작점 Transform")] Transform startPos;
    [SerializeField][Tooltip("설치할 타일들 프리팹")] List<GameObject> tiles;
    [SerializeField][Tooltip("설치 미리보기용")] List<GameObject> placingTiles;

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
    TileInfo[] tileList;

    Ray ray;
    RaycastHit hit;
    Vector3 placeRot = new Vector3(0, 15, 0); //회전간격

    // Start is called before the first frame update
    void Awake()
    {
        placingTiles = new List<GameObject>();
        tileList = new TileInfo[hor * ver];
        foreach (var tile in tiles) //미리보기용 타일 프리팹들 생성해놓기
        {
            GameObject tempTile = (tile == null ? null : Instantiate(tile));
            if (tempTile != null)
            {
                tempTile.SetActive(false);
            }
            placingTiles.Add(tempTile);
        }
        MakeBase();
        if(randomGenerate)
        {
            RandomTile();
        }
        DisplayMap();
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
                        GameObject placed = Instantiate(tiles[selected], hit.transform.GetChild(0));
                        placed.transform.rotation = placingTiles[selected].transform.rotation;
                        Debug.Log(hit.transform.name);
                        if (int.TryParse(hit.transform.name, out int num))
                        {
                            tileList[num] = new TileInfo(selected + 1, placed.transform.eulerAngles);
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
                if (!canPlace)
                {
                    if (hit.transform.gameObject != null && hit.transform.GetChild(0).childCount > 0)
                    {
                        if (int.TryParse(hit.transform.name, out int num))
                        {
                            Debug.Log(num + " " + tileList[num]);
                            tileList[num] = new TileInfo(0, Vector3.zero);
                        }
                        Destroy(hit.transform.GetChild(0).GetChild(0).gameObject);
                    }
                }
            }
        }
    }

    public void MakeBase() //바닥타일 생성
    {
        for (int i = 0; i < tileList.Length; i++)
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
        for (int i = 0; i < tileList.Length; i++)
        {
            if (tileList[i].TileId != 0)
            {
                Instantiate(tiles[tileList[i].TileId - 1], startPos.transform.GetChild(i).GetChild(0)).transform.rotation = Quaternion.Euler(tileList[i].TileRot);
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
        for (int i = 0; i < tileList.Length; i++)
        {
            if (tileList[i].TileId != 0)
            {
                Instantiate(tiles[tileList[i].TileId - 1], startPos.transform.GetChild(i).GetChild(0)).transform.rotation = Quaternion.Euler(tileList[i].TileRot);
            }
        }
    }

    IEnumerator PlayTileFall()
    {
        WaitForSeconds wait = new WaitForSeconds(playFallTime / tileList.Length);
        for (int i = 0; i < tileList.Length; i++)
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
        for (int i = 0; i < tileList.Length; i++)
        {
            Vector3 rot = new Vector3(0, Random.Range(0f, 360f), 0);
            int tileNum = Random.Range(0, tiles.Count + 1);
            if (tileNum == 0)
            {
                tileList[i] = new TileInfo(tileNum, Vector3.zero);
            }
            else
            {
                tileList[i] = new TileInfo(tileNum, tiles[tileNum - 1].transform.eulerAngles + rot);
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
}