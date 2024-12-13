using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharGalleryController : MonoBehaviour
{
    public MainViewPort mainViewPort;
    public Transform contentParent;
    private CharacterData characterData;

    private bool LeftStickcanChange = true;
    private bool EnterDown = false;

    private int inputIndex=1;

    private int spriteIndex = 0;


    void Start()
    {
        
    }

    void Update()
    {
        CharGalleryInput();
        //Debug.Log("EastKeyInput" + EastDown);
        //Debug.Log("GalleryState" + GalleryManager.Instance.CurrentState);
    }

    void CharGalleryInput()
    {
        if (GalleryManager.Instance.CurrentState != GalleryState.CharacterGallery) { return; }
        Vector2 Inout = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis);

        if (Inout.x > 0.5f && LeftStickcanChange&&!mainViewPort.GetPageMove())
        {
            inputIndex++;
            if (inputIndex > mainViewPort.GetMaxPage())
            {
                inputIndex = 1;
            }
            mainViewPort.SwitchPage(inputIndex, characterData);
            LeftStickcanChange = false;
        }
        else if (Inout.x < -0.5f && LeftStickcanChange && !mainViewPort.GetPageMove())
        {
            inputIndex--;
            if (inputIndex <= 0)
            {
                inputIndex = mainViewPort.GetMaxPage();
            }
            mainViewPort.SwitchPage(inputIndex, characterData);
            LeftStickcanChange = false;
        }

        if (Inout.y > 0.5f && LeftStickcanChange)
        {
            spriteIndex--;                                                                          
            if (spriteIndex < 0)
            {
                spriteIndex = characterData.animatorControllers.Count-1;
            }
            mainViewPort.UpdateCharAnime(spriteIndex,characterData);
            LeftStickcanChange = false;
        }
        else if (Inout.y < -0.5f && LeftStickcanChange)
        {
            spriteIndex++;
            if (spriteIndex > characterData.animatorControllers.Count-1)
            {
                spriteIndex = 0;
            }
            mainViewPort.UpdateCharAnime(spriteIndex, characterData);
            LeftStickcanChange = false;
        }
        else if (Inout.y > -0.5f && Inout.y < 0.5f&& Inout.x > -0.5f && Inout.x < 0.5f                                                                                                                                                                      )
        {
            LeftStickcanChange = true;
        }

        if (InputManager.GetKeyUp(BoolActions.SouthButton))
        {
            EnterDown = false;
        }


        if (InputManager.GetKeyDown(BoolActions.EastButton) && !mainViewPort.GetPageMove())
        {
            mainViewPort.PageReturn();
            inputIndex = 1;
            GalleryManager.Instance.SetState(GalleryState.CharacterSelect);
        }
    }

    public void SetCharData(CharacterData data)
    {
        if (data != null)
        {
            characterData = data;
        }
    }
    public void ReSetSpriteState()
    {
        spriteIndex = 0;
    }
}
