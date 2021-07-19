namespace ApplicationCore.App.Infrastructure
{
    public class CoreDelegateServices
    {
        public CoreDelegateServices(SampleSqliteConnection conn,
                                    AppNotifications notifications)
        {
            Connection = conn;
            Notifications = notifications;
        }


        public SampleSqliteConnection Connection { get; }
        public AppNotifications Notifications { get; }
    }
}
