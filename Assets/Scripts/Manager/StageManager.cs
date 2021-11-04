using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour //현제 스테이지의 정보를 가지고있다.
{
    public int nowStage;

    public void OnClearStage()
    {
        StageVO vo = new StageVO(nowStage + 1);
        string json = JsonUtility.ToJson(vo);
        NetworkManager.instance.SendPostRequest("insertstage", json, result =>
        {
            //ResponseVO 형태로 result를 파싱해서
            // 그 결과가 true라면 얼럿을 클릭시 회원가입창도 같이 닫히고
            // false라면 얼럿 클릭시 얼럿만 닫히게
             
            ResponseVO vo = JsonUtility.FromJson<ResponseVO>(result);
            if (vo.result)
            {
                print(vo.payload);
            }
            else
            {
                print("실패");
            }
        });
    }
}
