using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WorldGalleryManager : MonoBehaviour
{

    private int inputIndex = 0;
    private bool canChangeIndex = true;
    public int GetInputIndex() => inputIndex;
    public MainViewPortWorld mainViewPortWorld;
    private int buildInputIndex=0;
    private int stageInputIndex = 0;
    private int introIndex = 0;

    private bool LeftStickcanChange = true;

    void Update()
    {
        InputHandle();
        WorldGalleryInput();
    }

    private void InputHandle()
    {
        if(GalleryManager.Instance.CurrentState != GalleryState.WorldSelect) { return; }
        {
            int childNum = transform.childCount;
            Vector2 axisInput = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis);
            if (axisInput.y < -0.5f && canChangeIndex)
            {
                inputIndex++;
                if (inputIndex >= childNum) { inputIndex = 0; }
                WorldMainSelect(inputIndex,childNum);
                canChangeIndex = false;
            }
            else if (axisInput.y > 0.5f && canChangeIndex)
            {
                inputIndex--;
                if (inputIndex < 0) { inputIndex = childNum - 1; }
                WorldMainSelect(inputIndex, childNum);
                canChangeIndex = false;
            }
            else if (axisInput.y > -0.5f && axisInput.y < 0.5f)
            {
                canChangeIndex = true;
            }
        }
    }

    void WorldGalleryInput()
    {
        Vector2 Inout = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis);
        if (mainViewPortWorld.GetMoveState()) { return; }
        if (InputManager.GetKeyDown(BoolActions.SouthButton))
        {
            if (inputIndex == 0 && GalleryManager.Instance.CurrentState == GalleryState.WorldSelect)
            {
                GalleryManager.Instance.SetState(GalleryState.WorldIllGallery);
                mainViewPortWorld.GenerateBulidingObject();
                mainViewPortWorld.StartCoroutine(mainViewPortWorld.HandleBuildingExitSequentially());
            }
            if (inputIndex == 1 && GalleryManager.Instance.CurrentState == GalleryState.WorldSelect)
            {
                GalleryManager.Instance.SetState(GalleryState.WorldTextGallery);
                mainViewPortWorld.GenerateIntroObject();
                mainViewPortWorld.StartCoroutine(mainViewPortWorld.HandleIntroExitSequentially());
                mainViewPortWorld.LoadIntroText(0);
            }
        }

        if (InputManager.GetKeyDown(BoolActions.EastButton))
        {
            if (GalleryManager.Instance.CurrentState == GalleryState.WorldIllGallery)
            {
                introIndex = 0;
                mainViewPortWorld.StartCoroutine(mainViewPortWorld.HandleBuildingResetSequentially());
            }
            if(GalleryManager.Instance.CurrentState == GalleryState.WorldTextGallery)
            {
                introIndex = 0;
                mainViewPortWorld.StartCoroutine(mainViewPortWorld.HandleIntroResetSequentially());
            }
        }

        if (GalleryManager.Instance.CurrentState == GalleryState.WorldIllGallery)
        {
            if (Inout.x > 0.5f && LeftStickcanChange)
            {
                stageInputIndex++;
                buildInputIndex = 0;
                if (stageInputIndex > mainViewPortWorld.allWorldGalleryData.worldGalleryDatas.Count - 1) { stageInputIndex = 0; }
                mainViewPortWorld.GetViewPortWorld().StageLoad(stageInputIndex);
                mainViewPortWorld.GetViewPortWorld().MoveScrollView(-2);
                mainViewPortWorld.LoadSpriteAndText(stageInputIndex, buildInputIndex);
                LeftStickcanChange = false;
            }
            else if (Inout.x < -0.5f && LeftStickcanChange)
            {
                stageInputIndex--;
                buildInputIndex = 0;
                if (stageInputIndex < 0) { stageInputIndex = mainViewPortWorld.allWorldGalleryData.worldGalleryDatas.Count - 1; }
                mainViewPortWorld.GetViewPortWorld().StageLoad(stageInputIndex);
                mainViewPortWorld.GetViewPortWorld().MoveScrollView(-2);
                mainViewPortWorld.LoadSpriteAndText(stageInputIndex, buildInputIndex);
                LeftStickcanChange = false;
            }

            if (Inout.y > 0.5f && LeftStickcanChange)
            {
                buildInputIndex--;
                if (buildInputIndex < 0)
                {
                    buildInputIndex = 0;
                    return;
                }
                mainViewPortWorld.GetViewPortWorld().MoveScrollView(-1);
                mainViewPortWorld.LoadSpriteAndText(stageInputIndex, buildInputIndex);
                LeftStickcanChange = false;
            }
            else if (Inout.y < -0.5f && LeftStickcanChange)
            {
                buildInputIndex++;
                if(buildInputIndex> mainViewPortWorld.GetViewPortWorld().GetMaxBuild() - 1)
                {
                    buildInputIndex = mainViewPortWorld.GetViewPortWorld().GetMaxBuild() - 1;
                    return;
                }
                mainViewPortWorld.GetViewPortWorld().MoveScrollView(1);
                mainViewPortWorld.LoadSpriteAndText(stageInputIndex, buildInputIndex);
                LeftStickcanChange = false;
            }
            else if (Inout.y > -0.5f && Inout.y < 0.5f && Inout.x > -0.5f && Inout.x < 0.5f)
            {
                LeftStickcanChange = true;
            }
        }

        if (GalleryManager.Instance.CurrentState == GalleryState.WorldTextGallery)
        {
            if (Inout.x > 0.5f && LeftStickcanChange)
            {
                introIndex++;
                if (introIndex > mainViewPortWorld.worldIntroData.worldIntro.Count - 1)
                {
                    introIndex = mainViewPortWorld.worldIntroData.worldIntro.Count - 1;
                    return;
                }
                mainViewPortWorld.LoadIntroText(introIndex);
            }
            else if (Inout.x < -0.5f && LeftStickcanChange)
            {
                introIndex--;
                if (introIndex < 0)
                {
                    introIndex = 0;
                    return;
                }
                mainViewPortWorld.LoadIntroText(introIndex);
            }
            else if (Inout.x > -0.5f && Inout.x < 0.5f)
            {
                LeftStickcanChange = true;
            }
        }


    }

    private void WorldMainSelect(int index,int maxChild)
    {
        for (int i = 0; i < maxChild; i++)
        {
            if (i == index)
            {
                Image image = transform.GetChild(i).GetComponent<Image>();
                if (image != null)
                {
                    Color color = image.color;
                    color.a = 1.0f;
                    image.color = color;
                }
            }
            else
            {
                Image image = transform.GetChild(i).GetComponent<Image>();
                if (image != null)
                {
                    Color color = image.color;
                    color.a = 0.0f;
                    image.color = color;
                }
            }
        }
    }
}
