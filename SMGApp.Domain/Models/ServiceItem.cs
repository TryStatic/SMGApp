﻿using System;
using System.Globalization;

namespace SMGApp.Domain.Models
{
    public class ServiceItem : DomainObject
    {
        public Customer Customer { get; set; }
        public string DeviceDescription { get; set; }
        public string DamageDescription { get; set; }
        public string DevicePassword { get; set; }
        public string SimPassword { get; set; }
        public string DeviceAccountUsername { get; set; }
        public string DeviceAccountPassword { get; set; }
        public bool ChargerIncluded { get; set; }
        public bool BagIncluded { get; set; }
        public bool CaseIncluded { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; }
        public ServiceState State { get; set; }
        public string Notes { get; set; }
        public double Price { get; set; }


        public string GetCost => Price.ToString("C", CultureInfo.CurrentCulture);
        public string CustomerDetails => Customer != null ? $"{Customer.LastName} {Customer.FirstName} (ID: {Customer.ID})" : "";
    }

    public enum ServiceState
    {
        Arrived,
        Fixed,
        Delivered,
        Issue
    }
}