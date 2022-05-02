using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebRequestTest : MonoBehaviour
{

    public void OnClickBtnDownloadTest()
    {
        WebManager.Instance.WebRequestGet(@"https://forest-cdn.flerogamessvc.com/beta/datapatch/DataPatch.zip", (request) => {

        });
    }

}
