using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using GeoAPI.Geometries;

namespace GarbageCollector.Domain
{
    public class GarbageAppUser
    {
        public Guid Id { get; set; }

        public string Login { get; set; }
        public ICollection<TrashCan> TrashCans { get; set; }

        public Location CurrentLocation { get; set; }

        public async Task UpdateTrashCansByCurrentLocationAsync()
        {
            var leftCategories = new HashSet<WasteCategory>();
            var leftNearestTakePoints =
                (await WasteTakePoint.GetNearestTakePoinsAsync(CurrentLocation).ConfigureAwait(false))
                .ToHashSet();
            var acceptableTakePoints = new HashSet<WasteTakePoint>();
            var continueLoop = true;
            while (leftCategories.Any() && leftNearestTakePoints.Any() && continueLoop)
            {
                continueLoop = false;
                //todo: можно сортировать leftNearest по количеству категорий и не пересекать те точки, у которых категорий меньше чем у best
                var currentStepBestTakePointAcc = leftNearestTakePoints.Aggregate(
                    (BestTakePoint: (WasteTakePoint) null, BestIntersectCount: 0),
                    (bestPointAcc, currentPoint) =>
                    {
                        var currentPointIntersectCount = leftCategories
                            .Join(currentPoint.AcceptingCategories,
                                cat => cat, cat => cat, (cat1, cat2) => 1)
                            .Count();
                        if (currentPointIntersectCount > 0 &&
                            currentPointIntersectCount > bestPointAcc.BestIntersectCount)
                        {
                            bestPointAcc.BestIntersectCount = currentPointIntersectCount;
                            bestPointAcc.BestTakePoint = currentPoint;
                        }

                        return bestPointAcc;
                    });
                var currentStepBestTakePoint = currentStepBestTakePointAcc.BestTakePoint;
                if (currentStepBestTakePoint != null)
                {
                    foreach (var takePointCategory in currentStepBestTakePoint.AcceptingCategories)
                    {
                        leftCategories.Remove(takePointCategory);
                    }

                    acceptableTakePoints.Add(currentStepBestTakePoint);
                    leftNearestTakePoints.Remove(currentStepBestTakePoint);
                    continueLoop = true;
                }
            }

            TrashCans = acceptableTakePoints.Select(tp => new TrashCan()
            {
                WasteCategories = tp.AcceptingCategories,
                WasteTakePoint = tp
            }).ToHashSet();
        }
    }

    public class Location
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public IPoint Coordinates { get; set; }
    }

    public class TrashCan
    {
        public Guid Id { get; set; }
        public WasteTakePoint WasteTakePoint { get; set; }
        public ICollection<WasteCategory> WasteCategories { get; set; }
    }

    public class WasteTakePoint
    {
        public Guid Id { get; set; }

        public Location Location { get; set; }
        public ICollection<WasteCategory> AcceptingCategories { get; set; }

        public static async Task<WasteTakePoint[]> GetNearestTakePoinsAsync(Location location)
        {
            return Array.Empty<WasteTakePoint>();
        }
    }

    public class WasteCategory
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public Material Material { get; set; }
    }

    public enum Material
    {
        None = 0,
        Plastic = 1
    }
}