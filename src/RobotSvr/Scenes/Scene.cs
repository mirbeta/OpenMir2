namespace RobotSvr;

public class Scene
{
    public SceneType Scenetype;

    public Scene(SceneType scenetype)
    {
        scenetype = scenetype;
    }
    public virtual void Initialize()
    {
    }

    public virtual void Finalize()
    {
    }

    public virtual void OpenScene()
    {
    }

    public virtual void CloseScene()
    {
    }

    public virtual void OpeningScene()
    {
    }

    public virtual void KeyPress(ref char key)
    {
    }

    public virtual void KeyDown(ref short key, Keys shift)
    {
    }

    public virtual void MouseMove(Keys shift, int x, int y)
    {
    }

    public virtual void MouseDown(MouseButtons button, Keys shift, int x, int y)
    {

    }

    public virtual void PlayScene()
    {

    }
}