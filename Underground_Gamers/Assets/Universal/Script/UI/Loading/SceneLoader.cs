using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingUI; // 로딩 UI(프로그레스 바 등)를 담고 있는 게임 오브젝트
    //public Slider progressBar; // 프로그레스 바
    public Image progressBar1;
    public Image progressBar2;
    public string sceneToLoad; // 로드할 씬의 이름
    private AsyncOperation asyncLoad; // 비동기 씬 로드를 위한 AsyncOperation 변수
    private float minLoadingTime = 3.0f; // 최소로 로딩창을 유지할 시간

    private float loadingTimer = 0.0f; // 로딩 화면을 보여준 시간을 측정하는 타이머
    private bool isSceneLoad = false;

    void Start()
    {
    }

    public void SceneLoad(string sceneName)
    {
        // 로딩 UI를 비활성화
        loadingUI.SetActive(false);

        // 비동기적으로 씬 로드 시작
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // 씬 자동으로 활성화되지 않도록 설정
        isSceneLoad = true;
    }

    void Update()
    {
        if(isSceneLoad)
        {
            // 로딩 UI를 활성화하여 로딩 화면 표시
            loadingUI.SetActive(true);

            float progress = Mathf.Clamp01(loadingTimer / minLoadingTime);

            progressBar1.fillAmount = progress;
            progressBar2.fillAmount = progress;

            loadingTimer += Time.unscaledDeltaTime;


            if ((loadingTimer >= minLoadingTime && asyncLoad.progress >= 0.9f) || asyncLoad.allowSceneActivation)
            {
                asyncLoad.allowSceneActivation = true; // 씬 활성화 허용
            }
        }
    }
}
