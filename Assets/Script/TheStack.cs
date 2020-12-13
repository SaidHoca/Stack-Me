using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour
{
    public static TheStack instance;

    public Color32[] gameColors = new Color32[4];
    public Color32[] gameColors2 = new Color32[4];
    public Color32[] gameColors3 = new Color32[4];
    public Color32[] gameColors4 = new Color32[4];
    public Material stackMat;
    private Color32[] oldColors;

    private const float BOUNDS_SIZE = 5F;
    private const float STACK_MOVING_SPEED = 3F; // bu aşağı doğru hareket.
    private const float ERROR_MARGIN = 0.15F;
    private const float STACK_BOUNDS_GAIN = 0.25F;
    private const int COMBO_START_GAIN = 3;

    private GameObject[] theStack;

    private Vector2 stackBounds = new Vector2(BOUNDS_SIZE, BOUNDS_SIZE);

    private int scoreCount;
    private int stackIndex = 0;
    private int combo = 0;
    private int colorCount = 11;
    private int taleCount;
    private int chooseColorSet;
    private int setReverse;

    private float tileTransition = 0.0f;
    private float tileSpeed = 1.5f; // bu sağa sola hareket hızı olacak..
    private float secondaryPosition;

    private bool isMovingOnX = true;

    private bool gameOver = true;

    private Vector3 desiredPosition;
    private Vector3 lastTilePosition;

    private Transform[] startTransforms;

	private void Awake()
	{
        if (instance == null) instance = this;
	}


	private void Start()
    {
        taleCount = transform.childCount;
        theStack = new GameObject[taleCount];
        startTransforms = new Transform[taleCount];
        //setReverse = taleCount - 1;
        chooseColorSet = Random.Range(0, 4);
		for (int i = 0; i < taleCount; i++)
		{
           
            theStack[i] = transform.GetChild(i).gameObject;

			ColorMesh(theStack[i].GetComponent<MeshFilter>().mesh);
			//setReverse--;
		}	
    }

    private void Update()
    {
		if (!gameOver)
		{
            if (Input.GetMouseButtonDown(0))
            {
                if (PlaceTile())
                {
                    SpawnTile();
                    scoreCount++;
                    UIControl.instance.setScore(scoreCount);
                }
                else
                {
                    EndGame();
                }
            }
        }
		
        MoveTile(); // Salınım hareketleri...

        // Aşağı yönlü hareket...
        transform.position = Vector3.Lerp(transform.position, desiredPosition, STACK_MOVING_SPEED * Time.deltaTime);      
    }

	private void CreateRubble(Vector3 pos , Vector3 scale)
	{
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.AddComponent<Rigidbody>();
        go.transform.tag = "rubble";
        go.GetComponent<Rigidbody>().AddForce(Vector3.down * 100);
        go.GetComponent<MeshRenderer>().material = stackMat;
        go.GetComponent<MeshFilter>().mesh.colors32 = oldColors;
    }

    private void MoveTile()
	{
        if (gameOver)
            return;

        tileTransition += Time.deltaTime * tileSpeed;
		if (isMovingOnX)
            theStack[stackIndex].transform.localPosition =
                new Vector3(Mathf.Cos(tileTransition) * BOUNDS_SIZE * 1.5f, scoreCount, secondaryPosition);
                
        else
            theStack[stackIndex].transform.localPosition =
                new Vector3(secondaryPosition, scoreCount, Mathf.Cos(tileTransition) * BOUNDS_SIZE * 1.5f);

    }

    private void SpawnTile()
	{
        tileTransition = 0;
        lastTilePosition = theStack[stackIndex].transform.localPosition;
        stackIndex--;
        if (stackIndex < 0) stackIndex = transform.childCount - 1;
        desiredPosition = (Vector3.down) * scoreCount;
        //GameObject go = theStack[stackIndex];    // fazladan malzeme eklersem diye yaptığım birşeyler ama emin değilim.
        //Instantiate(go, gameObject.transform);
        theStack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0);
        theStack[stackIndex].transform.localScale = new Vector3(stackBounds.x,1,stackBounds.y);
        ColorMesh(theStack[stackIndex].GetComponent<MeshFilter>().mesh);
        
    }

    private void ColorMesh(Mesh mesh)
	{
        Vector3[] vertices = mesh.vertices;
        Color32[] colors = new Color32[vertices.Length];

        colorCount++;

        float f =Mathf.Abs( Mathf.Sin(colorCount*0.05f));


        if(chooseColorSet == 0)
		{
            Debug.Log("1");
            for (int i = 0; i < vertices.Length; i++)
            {
                colors[i] = Lerp4(gameColors[0], gameColors[1], gameColors[2], gameColors[3], f);
            }
        }else if(chooseColorSet == 1)
		{
            Debug.Log("2");
            for (int i = 0; i < vertices.Length; i++)
            {
                colors[i] = Lerp4(gameColors2[0], gameColors2[1], gameColors2[2], gameColors2[3], f);
            }
		}
		else if(chooseColorSet == 2)
		{
            Debug.Log("3");
            for (int i = 0; i < vertices.Length; i++)
            {
                colors[i] = Lerp4(gameColors3[0], gameColors3[1], gameColors3[2], gameColors3[3], f);
            }
        }
        else if (chooseColorSet == 3)
        {
            Debug.Log("4");
            for (int i = 0; i < vertices.Length; i++)
            {
                colors[i] = Lerp4(gameColors4[0], gameColors4[1], gameColors4[2], gameColors4[3], f);
            }
        }


        mesh.colors32 = colors;
        oldColors = colors;
       
	}

    private bool PlaceTile()
	{
        Transform t = theStack[stackIndex].transform;

		if (isMovingOnX)
		{
            float deltaX = lastTilePosition.x -  t.position.x;
            
            if(Mathf.Abs(deltaX) > ERROR_MARGIN)
			{
                // CUT THE TILE
                combo = 0;
                stackBounds.x -= Mathf.Abs(deltaX);
                if (stackBounds.x <= 0) return false;
                float middle = lastTilePosition.x + t.localPosition.x / 2;
                t.localScale = new Vector3(stackBounds.x,1,stackBounds.y);
                CreateRubble(
                     new Vector3( (t.position.x > 0) 
                     ? t.position.x +(t.localScale.x/2)
                     : t.position.x - (t.localScale.x / 2)
                     , t.position.y
                     , t.position.z)
                     , new Vector3(Mathf.Abs(deltaX),1,t.localScale.z)
                    );
                t.localPosition = new Vector3(middle - (lastTilePosition.x / 2), scoreCount, lastTilePosition.z);
            }
			else
			{
            
                SoundManager.instance.ComboSounds(combo);
                if(PlayerPrefs.GetInt("vibration") == 1) Handheld.Vibrate();
                if (combo > COMBO_START_GAIN)
                {
                    stackBounds.x += STACK_BOUNDS_GAIN;
                    if (stackBounds.x > BOUNDS_SIZE) stackBounds.x = BOUNDS_SIZE;
                    float middle = lastTilePosition.x + t.localPosition.x / 2;
                    t.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);                 
                    t.localPosition = new Vector3(middle - (lastTilePosition.x / 2), scoreCount, lastTilePosition.z);
                }
                combo++;
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, lastTilePosition.z);
            }
		}
		else
		{
            float deltaZ = lastTilePosition.z - t.position.z;
            if (Mathf.Abs(deltaZ) > ERROR_MARGIN)
            {
                // CUT THE TILE
                combo = 0;
                stackBounds.y -= Mathf.Abs(deltaZ);
                if (stackBounds.y <= 0) return false;
                float middle = lastTilePosition.z + t.localPosition.z / 2;
                t.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);
                CreateRubble(
                     new Vector3(t.position.x
                     , t.position.y
                     , (t.position.z > 0)
                     ? t.position.z + (t.localScale.z / 2)
                     : t.position.z - (t.localScale.z / 2))
                     , new Vector3(t.localScale.x, 1,  Mathf.Abs(deltaZ))
                    );
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - (lastTilePosition.z/2));
            }
            else
            {
                SoundManager.instance.ComboSounds(combo);
                if (PlayerPrefs.GetInt("vibration") == 1) Handheld.Vibrate();
                if (combo > COMBO_START_GAIN)
                {
                    stackBounds.y += STACK_BOUNDS_GAIN;
                    if (stackBounds.y > BOUNDS_SIZE) stackBounds.y = BOUNDS_SIZE;
                    float middle = lastTilePosition.z + t.localPosition.z / 2;
                    t.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);
                    t.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - (lastTilePosition.z / 2));
                }
                combo++;
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, lastTilePosition.z);
            }
        }

        secondaryPosition = (isMovingOnX) 
            ? t.localPosition.x
            : t.localPosition.z;

        isMovingOnX = !isMovingOnX;        
        
        return true; // doğru yerleştiyse bu dönsün.. 
	}

    private void EndGame()
	{
		if (!gameOver)
		{
            Debug.Log("End Game");
            gameOver = true;
            theStack[stackIndex].AddComponent<Rigidbody>();
            UIControl.instance.OpenMenu();
        }
      
	}

    private Color32 Lerp4(Color32 a, Color32 b, Color32 c, Color32 d, float t)
	{
        if (t < 0.33f)
            return Color.Lerp(a, b, t / 0.33f);
        else if (t < 0.66f)
            return Color.Lerp(b, c, (t - 0.33f) / 0.33f);
        else
            return Color.Lerp(c, d, (t - 0.66f) / 0.66f);

	}

    public void SetGameOver()
	{
        gameOver = false;
	}

    public void StartGame()
	{
        // burada tüm stackler eski konumuna getirilecek..
        // bg değiştirilecek..
        // stackların renk paleti değiştirilecek...
        if(theStack[stackIndex].GetComponent<Rigidbody>())Destroy(theStack[stackIndex].GetComponent<Rigidbody>());
        colorCount = 0;
        stackIndex = 0;
        scoreCount = 0;
        combo = 0;
        secondaryPosition = 0;
        DestroyRubbles("rubble");
        desiredPosition = Vector3.zero;
        lastTilePosition = Vector3.zero;
        stackBounds = new Vector2(BOUNDS_SIZE, BOUNDS_SIZE);
        isMovingOnX = true;
        taleCount = transform.childCount;
        setReverse = taleCount - 1;
        gameOver = false;
        chooseColorSet = Random.Range(0, 4);
        for (int i = 0; i < taleCount; i++)
        {

            theStack[i].transform.localPosition = new Vector3(0, -i, 0);
            theStack[i].transform.localRotation = new Quaternion(0,0,0,0);
            theStack[i].transform.localScale = new Vector3(BOUNDS_SIZE,1,BOUNDS_SIZE);
            ColorMesh(theStack[setReverse].GetComponent<MeshFilter>().mesh);
            setReverse--;
        }
        GradientBg.instance.Start();   
    }

    private void DestroyRubbles(string tagName)
    {
        GameObject[] prefabs = GameObject.FindGameObjectsWithTag(tagName);
        foreach (GameObject prefab in prefabs)
        {
            Destroy(prefab);
        }
    }



}

// SORUNUN ÇÖZÜMÜYLE İLGİLİ.. TÜM NESNELERİ YOKEDİP YENİLERİNİ OLUŞTURABİLİRİZ Mİ? HAYIR TABİKİ DEMİ
// artan malzemelerin yokedilmesi meselesi var


// tam yerine koyma efekti düzenlenecek
// müzikler eklenecek...
// transform t yi global yapıp place tile düzenlenecek.. 
// aşağı doğru daha fazla küp spawn edecez gerekirse 70 - 80 birim... 
// post processing bakalım biraz.. ışık v.s.
// privacy policy ile ilgilen..
// altta banner reklam olacak sürekli




