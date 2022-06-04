using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.Enum
{
    public enum Roles
    {
        administrator = 1,
        marchent = 2,
        driver = 3,
        MerchantEmployee = 4
    }
    public enum SocialStatus
    {
        single = 1,
        engaged = 2,
        married = 3,
        divorced = 4,
    }
    public enum WorkTime
    {
        FullTime = 1,
        partTime = 2,
    }
    public enum Days
    {
        Saturday = 1,
        Sunday = 2,
        Monday = 3,
        Tuesday = 4,
        Wednesday = 5,
        Thursday = 6,
        Friday = 7
    }
    public enum Module
    {
        administrator = 1,
        marchent = 2,
        driver = 3,
        MerchantBranch = 4,
        MerchantEmployee = 5,
    }
    public enum DeliveryStatus
    {
        Location = 1,
        Distance = 2,
        Offer = 3,
    }

    public enum DriverCaseStatus
    {
        available = 1,
        hold = 2,
        unavailable = 3,
        block = 4
    }
    public enum MediaType
    {
        Image = 1,
        Video = 2,
        Audio = 3,
        File = 4,

    }
    public enum OrderStatus
    {
        PendingOrder = 1,
        AssignToDriver = 2,
        DriverRejected = 3,
        DriverAccepted = 4,
        DriverArrived= 5,
        OrderPicked = 6,
        OrderArrived = 7,
        PreOrder=8,
    }
    public enum DriverOrderEnum
    {
        PenddingOrder = 1,
        AcceptedOrder = 2,
        RejectedOrder = 3,
    }
}
