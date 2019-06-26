﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum TipType
{
    ChooseTip,//选择弹窗，包括确定和取消两个按钮
    AlertTip, //警告弹窗，包含确定按钮
    SimpleTip //简易提示，单行文字
}

public class TipManager : MonoBehaviour
{
    private static TipManager instance;
    public static TipManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = UIUtils.CreateGameObject<TipManager>(EternalGameObject.Instance.rootCanvas, typeof(TipManager).ToString());
                RectTransform rt = instance.gameObject.AddComponent<RectTransform>();
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMax = Vector2.zero;
                rt.offsetMin = Vector2.zero;
            }
            return instance;
        }
        set { instance = value; }
    }

    /// <summary>
    /// 根据tip类型打开指定tip窗口
    /// </summary>
    /// <param name="tipType">tip类型（枚举）</param>
    /// <param name="describe">文本描述</param>
    /// <param name="waitTime">窗口停留时间，结束后关闭tip窗口,SimpleTip默认该值为3</param>
    /// <param name="sureAction">点击确定执行的函数</param>
    /// <param name="cancelAction">点击取消执行的函数（可以为null）</param>
    public void OpenTip(TipType tipType, string describe, float waitTime = 0, UnityAction sureAction = null, UnityAction cancelAction = null)
    {
        Instance.transform.SetAsLastSibling();
        TipItem ti = GetTip(tipType);
        if (ti)
        {
            ti.Init(describe, waitTime, sureAction, cancelAction);
            ti.gameObject.SetActive(true);
            LoadingPnl.CloseLoading();
        }
    }

    public void CloseTipByType(TipType tipType)
    {
        Transform tf = transform.Find(tipType.ToString());
        if (tf)
            tf.GetComponent<TipItem>().Close();
    }

    public void CloseAllTip()
    {
        TipItem[] tis = transform.GetComponentsInChildren<TipItem>(true);
        for (int i = 0; i < tis.Length; i++)
            tis[i].Close();
    }

    TipItem GetTip(TipType tipType)
    {
        RectTransform tf = transform.Find(tipType.ToString()) as RectTransform;
        if (!tf)
        {
            tf = BundleManager.Instance.GetGameObject(tipType.ToString(), "Tips").transform as RectTransform;
            tf.SetParent(transform);
        }
        tf.gameObject.SetActive(false);
        return tf.GetComponent<TipItem>();
    }

    void OnDestroy()
    {
        Instance = null;
    }
}