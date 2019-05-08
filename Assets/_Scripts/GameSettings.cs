using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Game", order = 1)]
public class GameSettings : ScriptableObject
{
    public float playTime;
    
    // C какой частотой генерятся шарики?
    public float ballCreatingFrequency = 0.1f;
    // минимальная граница рандома скейла шарика
    public float ballScaleMin = 1;
    // максимальная граница рандома скейла шарика
    public float ballScaleMax = 5;
    
    // на минимальной границе рандома скейла шарик будет иметь такую скорость
    public float speedOnMinScale = 5;
    public float speedOnMaxScale = 1;
    
    // минимальный шарик дает это значение очков. Чем ближе шарик к максимальному скейлу тем меньше очков он дает
    public int scoreForMinBall = 10;
    public int scoreForMaxBall = 1;
    
    // Скорость будет линейно возрастать к концу игры, можно в будущем заменить на animation curve
    public float nearTheEndSpeedMultiplier = 2;
}