using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shiny;
using Shiny.Notifications;
using Shiny.Stores;

namespace ApplicationCore.App
{
    public class NotificationRegistration
    {
        public NotificationRegistration(string description, Type type, bool hasEntryExit, bool isInitStart)
        {
            Description = description;
            Type = type;
            HasEntryExit = hasEntryExit;
            IsInitStart = isInitStart;
        }


        public string Description { get; }
        public Type Type { get; }
        public bool HasEntryExit { get; }
        
        public bool IsInitStart { get; }
        public string Name => Type.Name;
    }


    public class AppNotifications
    {
        private readonly IKeyValueStore _settings;
        private readonly INotificationManager _notifications;
        private readonly Dictionary<Type, NotificationRegistration> _registrations;


        public AppNotifications(IKeyValueStoreFactory storeFactory, INotificationManager notifications)
        {
            _settings = storeFactory.GetStore("settings");
            _notifications = notifications;
            _registrations = new Dictionary<Type, NotificationRegistration>();
        }


        public NotificationRegistration[] GetRegistrations()
            => _registrations.Values.OrderBy(x => x.Description).ToArray();


        public void Set(Type type, bool entry, bool enabled)
            => _settings.Set(ToKey(type, entry), enabled);


        public void Register(Type type, bool hasEntryExit, string description)
        {
            if (_registrations.ContainsKey(type))
                return;

            _registrations.Add(type, new NotificationRegistration(description, type, hasEntryExit, false));
        }
        
        
        public void RegisterInit(Type type, bool hasEntryExit, string description)
        {
            if (_registrations.ContainsKey(type))
                return;

            _registrations.Add(type, new NotificationRegistration(description, type, hasEntryExit, true));
        }


        public bool IsEnabled(Type type, bool entry) => _settings.Get<bool>(ToKey(type, entry));


        public async Task Send(Type type, bool entry, string title, string message)
        {
            if (IsEnabled(type, entry))
                await _notifications.Send(title, message);
        }


        private static string ToKey(Type type, bool entry)
        {
            var e = entry ? "Entry" : "Exit";
            var key = $"{type.FullName}.{e}";
            return key;
        }
    }
}
