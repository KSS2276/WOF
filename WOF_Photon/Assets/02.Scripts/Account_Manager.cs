using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Globalization;


namespace Com.WOF.Sungsoo
{

    public class Account_Manager : MonoBehaviour
    {

        public GameObject o_LoginTool;
        public GameObject o_GameMode;

        public GameObject u_AlarmBar;
        public InputField u_InputNick;
        public GameObject u_Nick;

        private string serverURL = "http://13.209.6.8:3000/";


        struct PlayerInfo
        {
            public string s_Nickname;
            public string s_SerialNumber;
        };

        PlayerInfo user = new PlayerInfo();

        void Start()
        {
            DontDestroyOnLoad(gameObject);

        }

        public void LogIn()
        {
            StartCoroutine(LoginCor());
        }

        IEnumerator LoginCor()
        {
            Regex nickNameRule = new Regex(@"^[a-zA-Z]+[0-9]{4,14}$"); //영어, 숫자 조합 확인.
            Regex nickNameRule2 = new Regex(@"^[가-힣]{2,7}$"); // 한글 확인.

            // 위에 조건 하나라도 만족 시 성공.

            Match nickNameChecking = nickNameRule.Match(u_InputNick.text);
            Match nickNameChecking2 = nickNameRule2.Match(u_InputNick.text);

            Debug.Log(nickNameChecking.Success);
            Debug.Log(nickNameChecking2.Success);

            if (u_InputNick.text == "" || (!nickNameChecking.Success && !nickNameChecking2.Success))
            {
                StartCoroutine(Send_Alarm("닉네임을 확인해주세요."));
            }

            else
            {
                WWWForm form = new WWWForm();
                form.AddField("username", u_InputNick.text);
                form.AddField("code", DateTime.Now.ToString("yyyyMMddHHmmss"));

                WWW www = new WWW(serverURL, form);
                yield return www;
                string wwwReuslt = www.text;
                Debug.Log(wwwReuslt);

                if (wwwReuslt != "Unauthorized")
                    user = JsonUtility.FromJson<PlayerInfo>(wwwReuslt);

                Debug.Log(user);

                u_Nick.GetComponent<Text>().text = user.s_Nickname;
                u_Nick.SetActive(true);
                StartCoroutine(Send_Alarm("닉네임이 생성되었습니다."));
                o_LoginTool.SetActive(false);
                o_GameMode.SetActive(true);

            }
        }

        IEnumerator Send_Alarm(string message)
        {
            u_AlarmBar.transform.GetChild(0).GetComponent<Text>().text = message;
            u_AlarmBar.GetComponent<Animator>().SetBool("drop", true);
            yield return null;
            u_AlarmBar.GetComponent<Animator>().SetBool("drop", false);
        }

    }
}