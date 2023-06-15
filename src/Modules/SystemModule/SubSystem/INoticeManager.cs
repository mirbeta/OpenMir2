namespace M2Server.Notices
{
    public interface INoticeSystem
    {
        void GetNoticeMsg(string sStr, IList<string> LoadList);

        void LoadingNotice();
    }
}