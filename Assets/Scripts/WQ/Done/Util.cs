using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 做规定:
///     1.句子和句子间用[]来隔开
///     2.关卡和关卡间用{}来隔开
///     3.句子中排序: %淡入时间%淡出时间%句子文字
///     4.文本中出现回车也不显示
///     5.{[%1.6%你好啊][%1.1%我不好][%0.2%那我就放心了]}
///         也可以写成这样:
///             {
///                 [
///                     %1.6
///                     %你好啊
///                 ]
///                 [
///                     %1.1
///                     %我不好
///                 ]
///                 [
///                     %0.2
///                     %那我就放心了
///                 ]
///             }
/// </summary>
public class Util : MonoBehaviour
{
    public const char PASS_SEPARATOR_0 = '{';
    public const char PASS_SEPARATOR_1 = '}';
    public const char SENTENCE_SEPARATOR_0 = '[';
    public const char SENTENCE_SEPARATOR_1 = ']';
    public const char IN_SENTENCE_SEPARATOR = '%';
    public class PassData
    {
        public class TextData
        {
            /// <summary>
            /// 这句话持续的时间
            /// </summary>
            public float time = 0;
            public string text = "";
        }
        public List<TextData> textdatas = new List<TextData>();
    }

    [Header("文本资源")]
    public TextAsset asset;
    public static Util Instance
    {
        get
        {
            return _instance;
        }
    }
    private static Util _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    /// <summary>
    /// 规定:关卡从1开始
    /// </summary>
    /// <param name="passIndex"></param>
    /// <returns></returns>
    public PassData Get(int passIndex)
    {
        PassData ret = new PassData();
        // 先找出第几个关卡的数据
        int passBeginIndex = Search(asset.text, PASS_SEPARATOR_0, passIndex); // 这个关卡的{的序号
        int passEndinIndex = Search(asset.text, PASS_SEPARATOR_1, passIndex); // 这个关卡的}的序号
        string passText = asset.text.Substring(passBeginIndex + 1, passEndinIndex - passBeginIndex - 1); // 去除了{}的纯关卡数据
        passText = passText.Replace(System.Environment.NewLine, ""); // 把换行符给去掉.
        // 上述提取出来要的关卡的数据了
        // 下面开始解析有几个对话
        List<string> sentenceText = Spilit(passText, SENTENCE_SEPARATOR_0, SENTENCE_SEPARATOR_1);
        foreach (var item in sentenceText)
        {
            PassData.TextData temp = new PassData.TextData();
            // 到这的格式是: %时间%句子
            int one = item.IndexOf(IN_SENTENCE_SEPARATOR); // 第一个%
            int two = item.IndexOf(IN_SENTENCE_SEPARATOR, one + 1); // 第二个%
            temp.time = float.Parse(item.Substring(one + 1, two - one - 1).Trim()); // 取数据去空格,转化
            temp.text = item.Substring(two + 1);
            ret.textdatas.Add(temp);
        }
        // 取完数据了,返回数据
        return ret;
    }
    /// <summary>
    /// 查找text中的第index个key,返回序号 (index从1开始计数,代表第一个)----如果没找到,返回-1
    /// </summary>
    /// <returns></returns>
    int Search(string text, char key, int index)
    {
        int assist = 0; // 用于定位第几个
        for (int i = 0; i < index; i++)
        {
            assist = text.IndexOf(key, assist);
            if (assist == -1) return -1; // 如果没找到,返回-1
            assist++; // 如果找到了,往前走一步,继续下一次寻找
        }
        assist--;//走多了一步,走回去
        return assist; // 走出循环代表找到了
    }
    List<string> Spilit(string text, char key_0, char key_1)
    {
        List<string> ret = new List<string>();
        bool isHas = false; // 前面是否有key_0
        int index_0 = 0;
        // 逐字符分析
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == key_0)
            {
                if (!isHas)  // 如果之前找到了key_0而且没找到key_1,就不记录
                {
                    isHas = true; // 如果找到了前面的字符key_0,记录一下,并记录当前序号
                    index_0 = i;
                }
            }
            if (text[i] == key_1)
            {
                if (isHas)
                {
                    isHas = false; // 如果前面有,进来后初始化,方便下一波进入
                    ret.Add(text.Substring(index_0 + 1, i - index_0 - 1));
                }
            }
        }
        return ret;
    }
}
