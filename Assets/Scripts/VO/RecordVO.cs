using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using UnityEngine;
using System;

[Serializable]
public class RecordVO
{
    public string name;
    public int total_score;

    public RecordVO(string name, string msg, string score)
    {
        this.name = name;
        this.total_score = int.Parse(score);
    }
}
