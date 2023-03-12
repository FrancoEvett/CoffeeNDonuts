using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// most player controllable featurtes are modifed from here, enables for things like level selection, difficulty selection, volume controlls and help menus
/// </summary>




public class MenuController : MonoBehaviour
{
    [Header("Script Refs")]
    [SerializeField] private CupManager cupManager;
    [SerializeField] private BallBehavior ballBehavior;
    [SerializeField] private SoundController soundController;

    //intergface references
    [Header("Interface Refs")]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private bool mainMenu;  //is the menu curently in the title scree

    [Header("Lives")]
    [SerializeField] private Image[] crosses;   //the cross images to toggle on and off for lives


    private IronSourceAdds ironSourceAdds;
    private bool gameRunning;   //is teh game ucrrently running? AKA gameplay happenign not menu stuff, when play button clicked set this true, when player dies set false
    private int lives = 3;


    public void Start()
    {        

        //gte brelevant component references
        ironSourceAdds = GetComponent<IronSourceAdds>();

        //load a basic banner, that will update every 25 seconds
        ironSourceAdds.LoadBannerAd();    
        
        if (mainMenu)
        {
            //if in the main menu, then load high score
            scoreText.text = Score.GetHighScore().ToString();
        }
        else
        {
            //make sure game is unpaused
            UnPause();

            //make sure death screen is hidden
            LoadDeathScreen(false);

            //refresh life counter
            UpdateLives();
        }

        
    }
    public void Update()
    {
        if (!mainMenu)
        {
            //update the score element of the game
            scoreText.text = "x" + Score.GetScore().ToString();
        }      
    }



    public void LoadDeathScreen(bool state)
    {
        Score.SaveScore();
        deathScreen.SetActive(state);
        if (state)
        {
            Pause();
        }
        else
        {
            UnPause();
            //this here will be when death is reversed, so give one life back
            LifeBack();
        }
    }




    #region life monetoring

    public void UpdateLives()
    {
        //updater the life counter
        for(int i = 0; i < 3; i++)
        {
            if (lives > i) { crosses[i].enabled = false; } else { crosses[i].enabled = true; }
        }
    }
    public void LifeBack()
    {
        //gaina  life back
        lives += 1;
        if (lives > 3)
        {
            lives = 3;
        }
        UpdateLives();
    }
    public void MugSmash()
    {
        //this mug did not have a donut in it
        lives -= 1;
        if (lives <= 0)
        {
            lives = 0;
            LoadDeathScreen(true);
        }
        UpdateLives();
    }


    #endregion














    #region menu buttons

    //menu button interactions

    public void Menu()
    {
        Score.SaveScore();
        Score.Reset();

        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        //player chose to restart
        Score.SaveScore();
        Score.Reset();

        SceneManager.LoadScene(1);
    }
    public void OnApplicationQuit()
    {
        Score.SaveScore();
    }
    public void PlayGame()
    {
        Score.Reset();
        SceneManager.LoadScene(1);
    }
    public void MoreDonuts()
    {
        //if the player wnat to keep playing they can watch a rewarded ad to get extra lives/chances to play

        InvokeRepeating("CheckReward", 0, 0.5f);
        ironSourceAdds.LoadRewardedVideo();
        ironSourceAdds.ShowRewardedVideo();        
    }
    private void CheckReward()
    {
        //check to see if reward has been processed, thuis is run every half second
        if (ironSourceAdds.rewardGained)
        {
            ironSourceAdds.rewardGained = false;
            LoadDeathScreen(false);
            CancelInvoke("CheckReward");//stop check for reward complete

            //cause donut to respawn
            ballBehavior.NewLife();
        }
    }


    #endregion



    public void Pause()
    {
        ballBehavior.gameObject.SetActive(false);
        cupManager.Pause();
    }
    public void UnPause()
    {
        ballBehavior.gameObject.SetActive(true);
        cupManager.UpPause();
    }

}
