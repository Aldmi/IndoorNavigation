﻿using System;
using System.Collections.Generic;
using ApplicationCore.Shared.Models;
using CSharpFunctionalExtensions;

namespace ApplicationCore.Domain.RssiFingerprinting.Model
{
    /// <summary>
    /// Конечный отпечаток, снятый по разным сторонам света (разныее координаты компаса)
    /// Привязанный к координатам помещения 
    /// </summary>
    public class TotalFingerprint
    {
        public TotalFingerprint(
            Guid id,
            Point roomCoordinate,
            Dictionary<CompassCoordinates, CompassFingerprint> mask)
        {
            Id = id;
            RoomCoordinate = roomCoordinate;
            Mask = mask;
        }

        public Guid Id { get; }
        
        
        /// <summary>
        /// Координата отпечатка в помещении.
        /// </summary>
        public Point RoomCoordinate { get; }

        /// <summary>
        /// Отпечатки по сторонам света
        /// </summary>
        public Dictionary<CompassCoordinates, CompassFingerprint> Mask { get; }


        
        public Result<SimilarCompassFingerprint> CalcSimilarCompassFingerprint(CompassFingerprint cf)=>
            GetCompassFingerprint(cf).Bind(totalCf => totalCf.CreateSimilar(cf));
        


        /// <summary>
        /// Вернуть значение из словаря по ключу (стороне света)
        /// </summary>
        private Result<CompassFingerprint> GetCompassFingerprint(CompassFingerprint cf)
        {
            return Mask.TryGetValue(cf.CompassCoordinate, out var resCf) ?
                resCf :
                Result.Failure<CompassFingerprint>($"CompassFingerprint не найденн в словаре Mask по ключу {cf}");
        }
    }
}