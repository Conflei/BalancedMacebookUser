using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    public Spawner[] spawners;

    public Image frontPost;
    public Image timerImage;
    public Text timerLabel;

    public Text username;
    public Text followerLabel;

    public float timePerPost = 5f;
    public int imagesPerLevel = 3;
    public Sprite[] level0;
    public Sprite[] level1;
    public Sprite[] level2;

    public Sprite[] gamePosts;


    int[] reactionsArray = new int[] { 0, 0, 0, 0 };

    private bool waitingForReaction;
    private int followerCount = 0;

    public GameObject[] reactButtons;

    public GameObject endGamePanel;
    public CanvasGroup endGameCG;
    public Text endGameLabel;

    // Start is called before the first frame update
    void Start()
    {
        username.text = "@"+PlayerPrefs.GetString("username");
        StartCoroutine(GameWorker());
    }

    public IEnumerator GameWorker()
    {
        gamePosts = new Sprite[imagesPerLevel * 3];
        int postRandomIndex = 0;

        bool[] taken = new bool[level0.Length];
        int randIndex = Random.Range(0, level0.Length);

        for(int i = 0; i< imagesPerLevel; i++)
        {
            while (taken[randIndex] == true)
            {
                randIndex = Random.Range(0, level0.Length);
            }
            taken[randIndex] = true;
            gamePosts[postRandomIndex] = level0[randIndex];
            postRandomIndex++;
        }
        yield return null;

        taken = new bool[level1.Length];
        randIndex = Random.Range(0, level1.Length);

        for (int i = 0; i < imagesPerLevel; i++)
        {
            while (taken[randIndex] == true)
            {
                randIndex = Random.Range(0, level1.Length);
            }
            taken[randIndex] = true;
            gamePosts[postRandomIndex] = level1[randIndex];
            postRandomIndex++;
        }

        yield return null;

        taken = new bool[level2.Length];
        randIndex = Random.Range(0, level2.Length);

        for (int i = 0; i < imagesPerLevel; i++)
        {
            while (taken[randIndex] == true)
            {
                randIndex = Random.Range(0, level2.Length);
            }
            taken[randIndex] = true;
            gamePosts[postRandomIndex] = level2[randIndex];
            postRandomIndex++;
        }
        yield return null;

        Debug.Log("Start Posting");
        for(int i = 0; i< gamePosts.Length; i++)
        {
            yield return StartCoroutine(ShowPost(i));
            Debug.Log("Finished post " + i);
        }
        Debug.Log("Finished Posting");
        //while (waitingForReaction) { yield return null; }

        var cg = frontPost.gameObject.GetComponent<CanvasGroup>();
        while (cg.alpha > 0f)
        {
            cg.alpha -= Time.deltaTime * 2f;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        EndGame();

        yield return null;
    }

    public IEnumerator ShowPost(int index)
    {
        var cg = frontPost.gameObject.GetComponent<CanvasGroup>();
        while (cg.alpha > 0f)
        {
            cg.alpha -= Time.deltaTime*2f;
            yield return null;
        }

        timerImage.fillAmount = 0f;
        frontPost.sprite = gamePosts[index];
        float ownTimer = 0f;
        waitingForReaction = true;

        while (cg.alpha < 1f)
        {
            cg.alpha += Time.deltaTime*2f;
            yield return null;
        }

        while (ownTimer < timePerPost && waitingForReaction)
        {
            timerImage.fillAmount = ownTimer / timePerPost;
            ownTimer += Time.deltaTime;
            timerLabel.text = (10 - (int)ownTimer) +"";
            yield return null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnRection(int type)
    {
        if (!waitingForReaction) return;
        StartCoroutine(buttonAnimation(reactButtons[type]));
        reactionsArray[type] += 1;
        followerCount += 20;
        followerLabel.text = followerCount+"";
        StartCoroutine(buttonAnimation(followerLabel.gameObject));
        spawners[type].Spawn(type, followerCount);
        waitingForReaction = false;
    }

    public IEnumerator buttonAnimation(GameObject button)
    {
        iTween.ScaleTo(button, iTween.Hash("scale", Vector3.one * 1.4f, "time", 0.25f, "easeType", iTween.EaseType.easeOutExpo));
        yield return new WaitForSeconds(0.25f);
        iTween.ScaleTo(button, iTween.Hash("scale", Vector3.one, "time", 0.25f, "easeType", iTween.EaseType.easeInExpo));
    }

    public void NewPost()
    {
        StartCoroutine(NewPostWorker());
    }

    public IEnumerator NewPostWorker()
    { 
        
        yield return null;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void RemoveFollowers()
    {
        followerCount -= 20;
        followerLabel.text = followerCount + "";
        StartCoroutine(buttonAnimation(followerLabel.gameObject));
    }

    public void EndGame(bool platformDown = false)
    {
        StartCoroutine(EndGameWorker(platformDown));
    }

    public IEnumerator EndGameWorker(bool platformDown)
    {
        Debug.Log("End Game Worker");
        while (endGameCG.alpha < 1f)
        {
            endGameCG.alpha += Time.deltaTime * 2f;
            yield return null;
        }
        endGameCG.interactable = true;
        endGameCG.blocksRaycasts = true;

        if (platformDown)
        {
            endGameLabel.text = "The platform went down, you lost all your followers and it's fine they don't mean anything after all but you repaired yourself and your social network habits.";
        }
        else
        {
            endGameLabel.text = "You got "+followerCount+" followers congratulations! \n\nUse them wisely through your life. If you feel that something went wrong, play it again and react they way you want and not for the followers anymore";
        }

        iTween.ScaleTo(endGamePanel, iTween.Hash("scale", Vector3.one, "time", 0.75f, "easeType", iTween.EaseType.easeOutExpo));
        yield return null;
    }
}
