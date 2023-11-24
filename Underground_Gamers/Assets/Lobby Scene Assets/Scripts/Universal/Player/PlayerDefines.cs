using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerClass
{
    Attacker,
    Defender,
    Supporter,
    Sniper
}
public class Player
{
    public int code = -1;
    public int grade = -1;
    public int gearCode = -1;
    public float ID = -1f;
}

public struct PlayerInfo
{
    public int code;
    public string name;
    public string comment;    //선수 설명
    public int hp;
    public int atk;
    public float atkRate;
    public int capacity;
    public float range;
    public float reloadingSpeed;

    public float moveSpeed;
    public float sight;
    public float reactionSpeed;
    public float criticalChance;
    public float Accuracy;         //명중률
    public float avoidRate;        //회피율
    public float collectingRate;   //집탄율
    public float detectionRange;   //감지범위
}
