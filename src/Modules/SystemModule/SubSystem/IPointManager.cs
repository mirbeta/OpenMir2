using OpenMir2.Enums;
using SystemModule.Maps;

namespace SystemModule.SubSystem
{
    public interface IPointManager
    {
        FindPathType PathType { get; set; }

        bool GetPoint(ref short nX, ref short nY);

        bool GetPoint1(ref short nX, ref short nY);

        void Initialize(IEnvirnoment Envir);
    }
}