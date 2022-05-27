﻿using System;
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
        MerchantEmployee=4
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
        MerchantBranch=4,
        MerchantEmployee=5,
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
}
