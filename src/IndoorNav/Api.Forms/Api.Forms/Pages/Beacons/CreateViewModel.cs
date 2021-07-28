using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Api.Forms.Infrastructure;
using Libs.Beacons;
using Libs.Beacons.Models;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny;

namespace Api.Forms.Pages.Beacons
{
    public class CreateViewModel : ViewModel
    {
        //const string EstimoteUuid = "e2c56db5-dffb-48d2-b060-d0f5a71096e0"; //Белый квадрат
        //const string EstimoteUuid = "f7826da6-4fa2-4e98-8024-bc5b71e0893e"; //Зеленая большая метка
        //const string EstimoteUuid = "f7826da6-4fa2-4e98-5555-bc5b71e0893e"; //НЕ ВЕРНЫЙ
        const string EstimoteUuid = "f7826da6-4fa2-4e98-8024-bc5b71e0893e";


        public CreateViewModel(INavigationService navigator,
                               IDialogs dialogs,
                               IBeaconRangingManager rangingManager)
        
        {
            EstimoteDefaults = ReactiveCommand.Create(() =>
            {
                Identifier = "You Company Name";
                Uuid = EstimoteUuid;
            });

            this.WhenAnyValue(x => x.Major)
                .Select(x => !x.IsEmpty() && UInt16.TryParse(x, out _))
                .ToPropertyEx(this, x => x.IsMajorSet);

            
            StartRanging = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    var result = await rangingManager.RequestAccess();
                    if (result != AccessState.Available)
                    {
                        await dialogs.AlertAccess(result);
                    }
                    else
                    {
                        var region = GetBeaconRegion();
                        await navigator.GoBack(false, (nameof(BeaconRegion), region));
                    }
                },
                this.WhenAny(
                    x => x.Identifier,
                    x => x.Uuid,
                    x => x.Major,
                    x => x.Minor,
                    (idValue, uuidValue, majorValue, minorValue) => IsValid()
                )
            );
        }

        
        public bool IsMajorSet { [ObservableAsProperty] get; }
        public ICommand StartRanging { get; }
        public ICommand EstimoteDefaults { get; }
        [Reactive] public string Identifier { get; set; }
        [Reactive] public string Uuid { get; set; }
        [Reactive] public string Major { get; set; }
        [Reactive] public string Minor { get; set; }
        [Reactive] public bool NotifyOnEntry { get; set; } = true;
        [Reactive] public bool NotifyOnExit { get; set; } = true;
        public bool IsMonitoringSupported { get; }


       
        /// <summary>
        /// Установить переданный регион.
        /// </summary>
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var region = parameters.GetValue<BeaconRegion>(nameof(BeaconRegion));
            if (region != null)
            {
                SetBeaconRegion(region);
            }
        }
        
        /// <summary>
        /// Вернуть через параметры сохраненный регион
        /// </summary>
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            var region = GetBeaconRegion();
            parameters.Add(nameof(BeaconRegion), region);
        }


        BeaconRegion GetBeaconRegion() => new BeaconRegion(
            Identifier,
            Guid.Parse(Uuid),
            GetNumberAddress(Major),
            GetNumberAddress(Minor)
        )
        {
            NotifyOnEntry = NotifyOnEntry,
            NotifyOnExit = NotifyOnExit
        };


        void SetBeaconRegion(BeaconRegion region)
        {
            Identifier = region.Identifier;
            Uuid = region.Uuid.ToString("D");
            Major = region.Major.ToString();
            Minor = region.Minor.ToString();
        }


        bool IsValid()
        {
            if (Identifier.IsEmpty())
                return false;

            if (!Uuid.IsEmpty() && !Guid.TryParse(Uuid, out _))
                return false;

            if (!ValidateNumberAddress(Major))
                return false;

            if (!ValidateNumberAddress(Minor))
                return false;

            return true;
        }


        static bool ValidateNumberAddress(string value)
        {
            if (value.IsEmpty())
                return true;

            return UInt16.TryParse(value, out _);
        }


        ushort? GetNumberAddress(string value)
        {
            if (value.IsEmpty())
                return null;

            return UInt16.Parse(value);
        }
    }
}
