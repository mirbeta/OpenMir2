namespace SystemModule.SubSystem
{
    public interface INoticeSystem
    {
        void GetNoticeMsg(string sStr, IList<string> LoadList);

        void LoadingNotice();
    }
}