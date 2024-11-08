using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum GalleryState
{
    GalleryMain,
    CharacterSelect,
    CharacterGallery,
    WorldSelect,
    WorldIllGallery,
    WorldTextGallery,
    SoundSelect,
    SoundGallery,
}

public class GalleryManager : MonoBehaviour
{
    private static GalleryManager _instance;
    public static GalleryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GalleryManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("GalleryStateManager");
                    _instance = obj.AddComponent<GalleryManager>();
                }
            }
            return _instance;
        }
    }

    public GalleryState CurrentState { get; private set; }

    public GameObject largeCategory;
    public GameObject charCategory;
    public GameObject worldCategory;
    public GameObject soundCategory;


    //入力処理用(GalleryMain)
    private bool canChangeIndex = true;
    private int inputIndex = -1;
    private List<Transform> childTransforms = new List<Transform>();
    private Vector3 selecterSize = new Vector3(1.1f, 1.1f, 1.1f);
    private Vector3 defaultSize = new Vector3(1f, 1f, 1f);

    //fade
    public float fadeDuration = 1f;
    private bool isFade=false;
    private int nextState = -1;
    private static float Duration = 1.0f;

    //バター移動用変数
    private static Vector3 targetPositionCount = new Vector3(-108,70,0);
    private List<Vector3> currentPosition = new List<Vector3>();
    private Vector3[] worldCurrentPosition=new Vector3[2];
    private Vector3[] targetWorldPosition=new Vector3[2];
    private Vector3 currentPositionCount;


    private void Start()
    {
        childTransforms.Clear();
        currentPosition.Clear();
        if (largeCategory != null)
        {
            for (int i = 0; i < largeCategory.transform.childCount; i++)
            {
                Transform child = largeCategory.transform.GetChild(i);
                childTransforms.Add(child);
                currentPosition.Add(child.localPosition);
            }
        }

        if (worldCategory != null)
        {
            for(int i = 0; i < worldCategory.transform.childCount; i++)
            {
                worldCurrentPosition[i] = worldCategory.transform.GetChild(i).localPosition;
                targetWorldPosition[i] = new Vector3(221, worldCurrentPosition[i].y, worldCurrentPosition[i].z);
            }
        }
        SetState(GalleryState.GalleryMain);
    }

    public void SetState(GalleryState newState)
    {
        CurrentState = newState;
        UpdateGalleryUI();
    }

    private void UpdateGalleryUI()
    {
        switch (CurrentState)
        {
            case GalleryState.GalleryMain:
                charCategory.SetActive(false);
                worldCategory.SetActive(false);
                soundCategory.SetActive(false);
                break;
            case GalleryState.CharacterSelect:
                charCategory.SetActive(true);
                worldCategory.SetActive(false);
                soundCategory.SetActive(false);
                break;
            case GalleryState.CharacterGallery:
                break;
            case GalleryState.WorldSelect:
                charCategory.SetActive(false);
                worldCategory.SetActive(true);
                soundCategory.SetActive(false);
                break;
            case GalleryState.WorldIllGallery:
                break;
            case GalleryState.WorldTextGallery:
                break;
            case GalleryState.SoundSelect:
                charCategory.SetActive(false);
                worldCategory.SetActive(false);
                soundCategory.SetActive(true);
                break;
            case GalleryState.SoundGallery:
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case GalleryState.GalleryMain:
                GalleryMainUpdate();
                HandleGalleryMainInput();
                break;
            case GalleryState.CharacterSelect:
                HandleGalleryMainInput();
                HandleCharSelectInput();
                break;
            case GalleryState.CharacterGallery:
                HandleCharGalleryInput();
                break;
            case GalleryState.WorldSelect:
                HandleGalleryMainInput();
                HandleWorldSelectInput();
                break;
            case GalleryState.SoundSelect:
                HandleGalleryMainInput();
                HandleSoundSelectInput();
                break;
            default:
                break;
        }
    }













    /*--------------------------------------------------------------------------------------------------------------
     * -------------------------------------------------------------------------------------------------------------
     * -------------------------------------------アップデート関連関数----------------------------------------------
     * -------------------------------------------------------------------------------------------------------------
     * -------------------------------------------------------------------------------------------------------------
     * -------------------------------------------------------------------------------------------------------------*/
    
    private void HandleGalleryMainInput()
    {
        if (isFade) { return; }
        Vector2 axisInput = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis);
        if (axisInput.y < -0.5f && canChangeIndex)
        {
            inputIndex++;
            if (inputIndex >= childTransforms.Count) { inputIndex = 0; }
            GalleryMainSelect();
            canChangeIndex = false;
        }
        else if (axisInput.y > 0.5f && canChangeIndex)
        {
            inputIndex--;
            if (inputIndex < 0) { inputIndex = childTransforms.Count-1; }
            GalleryMainSelect();
            canChangeIndex = false;
        }
        else if (axisInput.y > -0.5f && axisInput.y < 0.5f)
        {
            canChangeIndex = true;
        }

        if (InputManager.GetKeyDown(BoolActions.SouthButton) && inputIndex >= 0)
        {
            if(CurrentState == GalleryState.GalleryMain)
            {
                nextState = inputIndex;
                TransFadeOut(inputIndex);
            }
        }

        if (InputManager.GetKeyDown(BoolActions.EastButton))
        {
            charCategory.SetActive(false);
            nextState = -1;
            switch (CurrentState)
            {
                case GalleryState.CharacterSelect:
                    inputIndex = 0;
                    break;
                case GalleryState.WorldSelect:
                    inputIndex = 1;
                    break;
                case GalleryState.SoundSelect:
                    inputIndex = 2;
                    break;
                default:
                    break;
            }
            if (CurrentState == GalleryState.WorldSelect)
            {
                WorldFadeOut(0.5f);
            }
            SetCategoryChild(largeCategory);
            TransFadeIn(inputIndex);
        }

        if (!isFade) 
        {
            switch (nextState)
            {
                case -1:
                    SetState(GalleryState.GalleryMain);
                    break;
                case 0:
                    SetState(GalleryState.CharacterSelect);
                    childTransforms.Clear();
                    currentPosition.Clear();
                    break;
                case 1:
                    SetCategoryChild(worldCategory);
                    WorldFadeIn(0.5f);
                    SetState(GalleryState.WorldSelect);
                    break;
                case 2:
                    SetState(GalleryState.SoundSelect);
                    SetCategoryChild(soundCategory);
                    break;
                default:
                    break;
            }
        }

    }

    private void HandleCharSelectInput()
    {
    }

    private void HandleCharGalleryInput()
    {

    }


    private void GalleryMainUpdate()
    {
        BackToMainTitle();
    }

    private void HandleWorldSelectInput()
    {
    }

    private void HandleSoundSelectInput()
    {
    }

    private void GalleryMainSelect()
    {

        for (int i = 0; i < childTransforms.Count; i++)
        {
            childTransforms[i].localScale = defaultSize;
        }

        if (inputIndex >= 0 && inputIndex < childTransforms.Count)
        {
            childTransforms[inputIndex].localScale = selecterSize;
        }
    }

    private void BackToMainTitle()
    {
        if (InputManager.GetKeyDown(BoolActions.EastButton)&&!isFade)
        {
            Managers.instance.ChangeState(GAME_STATE.TITLE);
            Managers.instance.ChangeScene(GAME_STATE.TITLE);
            isFade = true;
        }
    }









    /*--------------------------------------------------------------------------------------------------------------
     * -------------------------------------------------------------------------------------------------------------
     * -------------------------------------------Fade、バター操作関連関数----------------------------------------------
     * -------------------------------------------------------------------------------------------------------------
     * -------------------------------------------------------------------------------------------------------------
     * -------------------------------------------------------------------------------------------------------------*/

    private void ButtonFadeIn(Transform gameobject)
    {
        Image imageObject = gameobject.GetComponent<Image>();
        if (imageObject != null)
        {
            StartCoroutine(FadeIn(imageObject, Duration));
        }
    }

    private void ButtonFadeOut(Transform gameobject)
    {
        Image imageObject = gameobject.GetComponent<Image>();
        if (imageObject != null)
        {
            StartCoroutine(FadeOut(imageObject,Duration));
        }
    }

    private void ButtonMove(Transform targetobject,Vector3 targetposition,float _duration)
    {
        StartCoroutine(MoveObject(targetobject, targetposition, _duration));
    }

    private IEnumerator FadeIn(Image image, float duration)
    {
        float elapsedTime = 0f;
        isFade = true;
        Color color = image.color;

        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(0, 1, elapsedTime / duration);
            image.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 1;
        image.color = color;
        isFade = false;
    }

    private IEnumerator FadeOut(Image image, float duration)
    {
        float elapsedTime = 0f;
        isFade = true;
        Color color = image.color;

        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(1, 0, elapsedTime / duration);
            image.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 0;
        image.color = color;
        isFade = false;
    }

    private IEnumerator MoveObject(Transform targetObject, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = targetObject.localPosition;

        while (elapsedTime < duration)
        {
            targetObject.localPosition = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetObject.localPosition = targetPosition;
    }

    private void TransFadeIn(int index) 
    {

         for (int i = 0; i < childTransforms.Count; i++)
         {
             if (i == index)
             {
                 currentPosition[i] = currentPositionCount;
                 ButtonMove(childTransforms[i], currentPosition[i],Duration);
                 continue;
             }
             ButtonFadeIn(childTransforms[i]);
         }
        
    }

    private void TransFadeOut(int index)
    {
        for (int i = 0; i < childTransforms.Count; i++)
        {
            if (i == index)
            {
                currentPositionCount = childTransforms[i].localPosition;
                ButtonMove(childTransforms[i], targetPositionCount,Duration);
                continue;
            }
            ButtonFadeOut(childTransforms[i]);
        }
    }

    private void WorldFadeIn(float _duration)
    {
        if (CurrentState == GalleryState.GalleryMain)
        {
            for (int i = 0; i < childTransforms.Count; i++)
            {
                currentPosition[i] = currentPositionCount;
                ButtonMove(childTransforms[i], targetWorldPosition[i], Duration);
                ButtonFadeIn(childTransforms[i]);
            }
        }
    }

    private void WorldFadeOut( float _duration)
    {
        if (CurrentState == GalleryState.WorldSelect)
        {
            for (int i = 0; i < childTransforms.Count; i++)
            {
                ButtonMove(childTransforms[i], worldCurrentPosition[i], Duration);
                ButtonFadeOut(childTransforms[i]);
            }
        }
    }

    public bool GetFadeState()
    {
        return isFade;
    }

    private void SetCategoryChild(GameObject _CategoryObject)
    {
        childTransforms.Clear();
        currentPosition.Clear();
        if (_CategoryObject != null)
        {
            for (int i = 0; i < _CategoryObject.transform.childCount; i++)
            {
                Transform child = _CategoryObject.transform.GetChild(i);
                childTransforms.Add(child);
                currentPosition.Add(child.localPosition);
            }
        }
    }


    /*--------------------------------------------------------------------------------------------------------------
    * -------------------------------------------------------------------------------------------------------------
    * -------------------------------------------変数取得関連関数----------------------------------------------
    * -------------------------------------------------------------------------------------------------------------
    * -------------------------------------------------------------------------------------------------------------
    * -------------------------------------------------------------------------------------------------------------*/

    public int GetInputIndex()
    {
        return inputIndex;
    }
}
