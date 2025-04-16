using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] bool placeMode = false;
    [SerializeField] bool canPlace = false;
    [SerializeField][Tooltip("현재 선택한 타일 인덱스")] int selected = 0;
    TileInfo[] tileList;

    Ray ray;
    RaycastHit hit;
    Vector3 placeRot = new Vector3(0, 15, 0); //회전간격
    // Start is called before the first frame update
    void Start()
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //설치모드 진입
        {
            placeMode = !placeMode;
            placingTiles[selected]?.SetActive(placeMode);
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
                    if (placingTiles[selected].TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
                    {
                        mesh.material = canPlaceMat;
                    }
                }
                else
                {
                    canPlace = false;
                    if (placingTiles[selected].TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
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
                            Debug.Log(num + " " + tileList[num]);
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
        Debug.Log("MakeMap");
        for (int i = 0; i < tileList.Length; i++)
        {
            if (tileList[i].TileId != 0)
            {
                Instantiate(tiles[tileList[i].TileId - 1], startPos.transform.GetChild(i).GetChild(0)).transform.rotation = Quaternion.Euler(tileList[i].TileRot);
            }
        }
    }
}