using System;
using Shiny;

namespace Libs.Beacons.Managed.Flows.TrilaterationFlow
{
    public class ManagedSphere : NotifyPropertyChanged
    {
        public ManagedSphere(Sphere sphere) => Sphere = sphere;
        public Sphere Sphere { get; }
        
        
        public string StrId => $"{Sphere.Beacon.Uuid}\nMajor= {Sphere.Beacon.Major}  Minor= {Sphere.Beacon.Minor}";  
        
        
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
        
        private int _rssi;
        public int Rssi
        {
            get => _rssi;
            internal set => Set(ref _rssi, value);
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
        //     internal set => Set(ref _colorItem, value);
        // }
        
        //public bool CenterIsNotEmpty => !Center.Equals(Point.EmptyPoint);

        public bool CenterIsNotEmpty => false;
    }
}