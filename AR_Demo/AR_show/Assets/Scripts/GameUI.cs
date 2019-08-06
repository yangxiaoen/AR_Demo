using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    //开始界面
    public GameObject StartPanel;
    //选择场景界面
    public GameObject ChoosePanel;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    public void StartGame()
    {
        //开始界面隐藏
        StartPanel.SetActive(false);
        //选择场景界面显示
        ChoosePanel.SetActive(true);
    }
    public void chooseOnClick(int a)
    {
        //利用参数进行页面跳转
        SceneManager.LoadScene(a);
    }
    public void quick()
    {
        Application.Quit();
    }
}
