using System;
using ApplicationCore.Domain;
using ApplicationCore.Domain.Trilateration.Spheres;
using ApplicationCore.Shared;
using Libs.Beacons;
using Shiny;

namespace UseCase.Trilateration.Managed
{
    public class SphereDto : NotifyPropertyChanged
    {
        public SphereDto(Sphere sphere) => Sphere = sphere;
        public Sphere Sphere { get; }
        
        
        public string StrId => $"{Sphere.BeaconId.Uuid}\nMajor= {Sphere.BeaconId.Major}  Minor= {Sphere.BeaconId.Minor}";  
        
        
        private Proximity _prox;
        public Proximity Proximity
        {
            get => _prox;
            internal set => Set(ref _prox, value);
        }
        
        private DateTimeOffset _lastSeen;
        public DateTimeOffset LastSeen
        {
            get => _lastSeen;
            internal set => Set(ref _lastSeen, value);
        }
        
        private string _analitic;
        public string Analitic
        {
            get => _analitic;
            internal set => Set(ref _analitic, value);
        }
        
        private Point _center;
        public Point Center
        {
            get => _center;
            internal set => Set(ref _center, value);
        }
        
        private double _radius;
        public double Radius
        {
            get => _radius;
            internal set => Set(ref _radius, value);
        }
        
        
        // private Xamarin.Forms.Color _colorItem;
        // public Color ColorItem
        // {
        //     get => _colorItem;
        //     internal set => Set(ref _colorItem, Value);
        // }
        
        //public bool CenterIsNotEmpty => !Center.Equals(Point.EmptyPoint);

        public bool CenterIsNotEmpty => false;
    }
}