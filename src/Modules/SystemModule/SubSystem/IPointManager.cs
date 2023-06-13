using SystemModule.Enums;

namespace SystemModule
{
    public interface IPointManager
    {
        FindPathType PathType { get; set; }

        bool GetPoint(ref short nX, ref short nY);

        bool GetPoint1(ref short nX, ref short nY);

        void Initialize(IEnvirnoment Envir);
    }
}