using ICKT;
using ICKT.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGameOver : UIBase
{
    public void OnRetryButtonClicked()
    {
        GameInstance.StartLevel();
        FMODUnity.RuntimeManager.PlayOneShot(Const.FMOD_UI_CLICK_EVENT);
    }

    public void OnMainMenuButtonClicked()
    {
        GameInstance.GoToInitialScene();
        FMODUnity.RuntimeManager.PlayOneShot(Const.FMOD_UI_CLICK_EVENT);
    }
}
    