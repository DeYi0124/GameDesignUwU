using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using TMPro;

public class PuzzlegameManager : MonoBehaviour
{
  [Header("Game Elements")]
  [Range(2, 6)]
  [SerializeField] private int difficulty = 4;
  [SerializeField] private Transform gameHolder;
  [SerializeField] private Transform piecePrefab;
  [SerializeField] private TextMeshProUGUI UwU;
  [SerializeField] private TextMeshProUGUI timeText;
  public GameObject exitButton;


  [SerializeField] private List<Texture2D> imageTextures;

  private List<Transform> pieces;
  private Vector2Int dimensions;
  private float width;
  private float height;

  private Transform draggingPiece = null;
  private Vector3 offset;

  private int piecesCorrect;
  private float timer = 60f;
  private bool pauseTime = false;
  void Awake(){
        InvokeRepeating("updateTime",1f,1f);
        pauseTime = false;
    }
    void updateTime(){
        if(!pauseTime){
            timer-=1f;
            timeText.text = (timer.ToString() + " sec");
        }
    }
  void Start() {
    int r = Random.Range(0, imageTextures.Count);
    difficulty = Random.Range(4, 7);
    timer *= (difficulty-1);
    timeText.text = (timer.ToString() + " sec");
    UwU.gameObject.SetActive(false);
    exitButton.SetActive(false);
    StartGame(imageTextures[r]);
  }

  public void StartGame(Texture2D jigsawTexture) {
    pieces = new List<Transform>();
    dimensions = GetDimensions(jigsawTexture, difficulty);
    CreateJigsawPieces(jigsawTexture);
    Scatter();
    UpdateBorder();
    piecesCorrect = 0;
  }
  private void Scatter() {
    float orthoHeight = Camera.main.orthographicSize;
    float screenAspect = (float)Screen.width / Screen.height;
    float orthoWidth = (screenAspect * orthoHeight);
    float pieceWidth = width * gameHolder.localScale.x;
    float pieceHeight = height * gameHolder.localScale.y;

    orthoHeight -= pieceHeight;
    orthoWidth -= pieceWidth;
    foreach (Transform piece in pieces) {
      float x = Random.Range(-orthoWidth, orthoWidth);
      float y = Random.Range(-orthoHeight, orthoHeight);
      piece.position = new Vector3(x, y, -1);
    }
  }
  private void UpdateBorder() {
    LineRenderer lineRenderer = gameHolder.GetComponent<LineRenderer>();
    float halfWidth = (width * dimensions.x) / 2f;
    float halfHeight = (height * dimensions.y) / 2f;
    float borderZ = 0f;
    lineRenderer.SetPosition(0, new Vector3(-halfWidth, halfHeight, borderZ));
    lineRenderer.SetPosition(1, new Vector3(halfWidth, halfHeight, borderZ));
    lineRenderer.SetPosition(2, new Vector3(halfWidth, -halfHeight, borderZ));
    lineRenderer.SetPosition(3, new Vector3(-halfWidth, -halfHeight, borderZ));
    lineRenderer.startWidth = 0.1f;
    lineRenderer.endWidth = 0.1f;
    lineRenderer.enabled = true;
  }

  Vector2Int GetDimensions(Texture2D jigsawTexture, int difficulty) {
    Vector2Int dimensions = Vector2Int.zero;
    if (jigsawTexture.width < jigsawTexture.height) {
      dimensions.x = difficulty;
      dimensions.y = (difficulty * jigsawTexture.height) / jigsawTexture.width;
    } else {
      dimensions.x = (difficulty * jigsawTexture.width) / jigsawTexture.height;
      dimensions.y = difficulty;
    }
    return dimensions;
  }

  void CreateJigsawPieces(Texture2D jigsawTexture) {
    height = 1f / dimensions.y;
    float aspect = (float)jigsawTexture.width / jigsawTexture.height;
    width = aspect / dimensions.x;

    for (int row = 0; row < dimensions.y; row++) {
      for (int col = 0; col < dimensions.x; col++) {
        Transform piece = Instantiate(piecePrefab, gameHolder);
        piece.localPosition = new Vector3(
          (-width * dimensions.x / 2) + (width * col) + (width / 2),
          (-height * dimensions.y / 2) + (height * row) + (height / 2),
          -1);
        piece.localScale = new Vector3(width, height, 1f);
        piece.name = $"Piece {(row * dimensions.x) + col}";
        pieces.Add(piece);
        float width1 = 1f / dimensions.x;
        float height1 = 1f / dimensions.y;
        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(width1 * col, height1 * row);
        uv[1] = new Vector2(width1 * (col + 1), height1 * row);
        uv[2] = new Vector2(width1 * col, height1 * (row + 1));
        uv[3] = new Vector2(width1 * (col + 1), height1 * (row + 1));
        Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
        mesh.uv = uv;
        piece.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", jigsawTexture);
      }
    }
  }
  void Update() {
    if(timer == 0) {
      pauseTime = true;
      Debug.Log("game lose");
      GameManager.Instance.ReasonText = "Your truck is now forever stuck in the NTUST. You are now fired.";
      SceneManager.LoadScene("DeathReport");
    }
    if (Input.GetMouseButtonDown(0)) {
      RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
      if (hit) {
        draggingPiece = hit.transform;
        offset = draggingPiece.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset += Vector3.back;
      }
    }

    if (draggingPiece && Input.GetMouseButtonUp(0)) {
      SnapAndDisableIfCorrect();
      draggingPiece.position += Vector3.forward;
      draggingPiece = null;
    }
    if (draggingPiece) {
      Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      newPosition += offset;
      draggingPiece.position = newPosition;
    }
  }

  private void SnapAndDisableIfCorrect() {
    int pieceIndex = pieces.IndexOf(draggingPiece);
    int col = pieceIndex % dimensions.x;
    int row = pieceIndex / dimensions.x;

    Vector2 targetPosition = new((-width * dimensions.x / 2) + (width * col) + (width / 2),
                                 (-height * dimensions.y / 2) + (height * row) + (height / 2));

    if (Vector2.Distance(draggingPiece.localPosition, targetPosition) < (width / 2)) {
      draggingPiece.localPosition = targetPosition;

      draggingPiece.GetComponent<BoxCollider2D>().enabled = false;

      piecesCorrect++;
      if (piecesCorrect == pieces.Count) {
        pauseTime = true;
        UwU.gameObject.SetActive(true);
        exitButton.SetActive(true);
      }
    }
  }

    public void onExit() {
        GameManager.Instance.pause = false;
        SceneManager.LoadSceneAsync("MainScene");

    }
}
