using System.Collections.Generic;
using CRClient.Enums;

namespace CRClient.Responses
{
    public class TrackingResponse
    {
        public Carrier Carrier { get; internal set; }
        public long PackageNumber { get; internal set; }
        public TrackingStatus TrackingStatus { get; internal set; }
        public List<TrackingHistory> TrackingHistory { get; internal set; }

        public static TrackingResponse Map(dynamic o)
        {
            var history = new List<TrackingHistory>();

            foreach (var item in o.tracking.history)
            {
                history.Add(new TrackingHistory { Message = item.message, Time = item.time });
            }

            return new TrackingResponse
            {
                Carrier = o.carrier,
                PackageNumber = o.package_number,
                TrackingStatus = new TrackingStatus
                {
                    Message = o.tracking.status.body,
                    Time = o.tracking.status.time
                },
                TrackingHistory = history
            };
        }
    }

    public class TrackingHistory
    {
        public string Message { get; internal set; }
        public string Time { get; internal set; }
    }

    public class TrackingStatus
    {
        public string Message { get; internal set; }
        public string Time { get; internal set; }
    }
}