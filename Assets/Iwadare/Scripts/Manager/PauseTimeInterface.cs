/*
メモ
ポーズ方法:
 OnEnableとOnDisableに
{
    TimeScaleManager.ChangeTimeScaleAction += TimeScaleChange;
    TimeScaleManager.StartPauseAction += StartPause;
    TimeScaleManager.EndPauseAction += EndPause;
} OnEnableは+= OnDisableは-=
を書く。
*/

public interface PauseTimeInterface
{
    public abstract void TimeScaleChange(float timeScale);
    public abstract void StartPause();
    public abstract void EndPause();
}