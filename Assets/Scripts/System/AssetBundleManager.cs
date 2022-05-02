using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VersionData
{
    public string buildType;
    public int assetPosition;
    public string dataVersion;
    public string clientVersion;
    public string assetVersion;
    public string revision;

    public List<AssetBundleSaveItem> assets = new List<AssetBundleSaveItem>();
    public bool Contain(string assetName)
    {
        return assets.Exists(item => item.fileName == assetName);
    }

    public string GetSqliteName()
    {
        return string.Format("{0}.db", dataVersion);
    }
}

public class AssetBundleManager : MonoBehaviour
{
#if UNITY_IOS
    string platform = "ios/";
#else
    string platform = "android/";
#endif
    public static string STR_VERSION_FILE_NAME = "version.json";

    public static string URL_CDN => "http://172.20.102.71:8080";
    private Dictionary<string, AssetBundle> m_dicAssetBundles = new Dictionary<string, AssetBundle>();      // 로드한 에셋번들을 저장해 놓은 컨테이너
    private List<AssetBundleSaveItem> m_lstFirstLoadFiles = new List<AssetBundleSaveItem>();                // 최초로 다운로드 받는 번들 리스트 (스트리밍애셋)
    public string m_strLocalVersionFilePath = string.Empty;
    public VersionData m_cLocalVersionData = null;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetLocalVersionData());
    }
    private IEnumerator GetLocalVersionData()
    {
        while (!Caching.ready)
        {
            yield return null;
        }

        string strStreamingPath = Application.streamingAssetsPath;
        if (strStreamingPath.Contains("://") == false)
        {
            strStreamingPath = "file://" + strStreamingPath;
        }

        m_strLocalVersionFilePath = string.Format("{0}/AssetBundles/{1}", strStreamingPath, STR_VERSION_FILE_NAME);

        UnityWebRequest cReq = UnityWebRequest.Get(m_strLocalVersionFilePath);
        cReq.SendWebRequest();
        while (!cReq.isDone)
        {
            if (cReq.isHttpError || cReq.isNetworkError)
            {
                //OnGetLocalVerData(false);
                Debug.LogError(cReq.error + " " + m_strLocalVersionFilePath);
                GetServerVersionData();
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }

        m_cLocalVersionData = JsonUtility.FromJson<VersionData>(cReq.downloadHandler.text);
        GetServerVersionData();
    }

    private void GetServerVersionData()
    {
        //var verurl = "http://172.20.102.71:8080/client/patch/android/version.json";
        //StartCoroutine(VersionManager.Inst.GetCdnVersionData(verurl));
    }

    //public IEnumerator GetCdnVersionData(string verFileUrl)
    //{
    //    UnityWebRequest cReq = UnityWebRequest.Get(verFileUrl);
    //    cReq.SendWebRequest();
    //    while (!cReq.isDone)
    //    {
    //        if (cReq.isHttpError || cReq.isNetworkError)
    //        {
    //            Debug.LogError(cReq.error);
    //            OnGetCdnVerData(false);
    //            yield break;
    //        }

    //        yield return new WaitForSeconds(0.5f);
    //    }

    //    m_cCdnVersionData = JsonUtility.FromJson<VersionData>(cReq.downloadHandler.text);

    //    OnGetCdnVerData(true);
    //}

    void OnGetLocalVer(bool success)
    {
        //var verurl = URL_CDN + "/client/patch/" + platform + "version.json";
        ////WebAPI.AddLog("[GetCdnVersionData]\n" + verurl);
        //StartCoroutine(GetCdnVersionData(verurl));
    }
    //public IEnumerator GetCdnVersionData(string verFileUrl)
    //{
    //    yield return null;
    
    //}
    
    public void CalcPatchFileList(string strCdnUrl, string strLocalUrl, VersionData cLocalVersionData, VersionData cCdnVersionData)
    {
    
    }
    
    public IEnumerator PathDownload(System.Action callback)
    {



        string strStreamingPath = Application.streamingAssetsPath;
        if (strStreamingPath.Contains("://") == false)
        {
            strStreamingPath = "file://" + strStreamingPath;
        }
#if UNITY_IOS
        string ospath = "ios/";
#else
        string ospath = "android/";
#endif
        string remotePath = URL_CDN + "/client/patch/" + ospath;

        yield return null;
    }

}
