using ICKT;
using ICKT.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenu : UIBase
{
    public void OnResumeButtonClicked()
    {
        GameInstance.ResumeGame();
        Close();
        FMODUnity.RuntimeManager.PlayOneShot(Const.FMOD_UI_CLICK_EVENT);
    }

    public void OnExitButtonClicked()
    {
        GameInstance.GoToInitialScene();
        FMODUnity.RuntimeManager.PlayOneShot(Const.FMOD_UI_CLICK_EVENT);
    }
}
