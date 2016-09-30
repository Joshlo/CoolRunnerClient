﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolRunnerClient.Models
{
    public class CreateShipmentModel
    {
        public string ReceiverName { get; set; }
        public string ReceiverAttention { get; set; }
        public string ReceiverStreet1 { get; set; }
        public string ReceiverStreet2 { get; set; }
        public string ReceiverZipcode { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverCountry { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverEmail { get; set; }
        public bool ReceiverNotify { get; set; }
        public string ReceiverNotifySms { get; set; }
        public string ReceiverNotifyEmail { get; set; }
        public string SenderName { get; set; }
        public string SenderAttention { get; set; }
        public string SenderStreet1 { get; set; }
        public string SenderStreet2 { get; set; }
        public string SenderZipcode { get; set; }
        public string SenderCity { get; set; }
        public string SenderCountry { get; set; }
        public string SenderPhone { get; set; }
        public string SenderEmail { get; set; }
        public bool Droppoint { get; set; }
        public string DroppointId { get; set; }
        public string DroppointName { get; set; }
        public string DroppointStreet1 { get; set; }
        public string DroppointZipcode { get; set; }
        public string DroppointCity { get; set; }
        public string DroppointCountry { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string Carrier { get; set; }
        public string CarrierProduct { get; set; }
        public string CarrierService { get; set; }
        public bool Insurance { get; set; }
        public int InsuranceValue { get; set; }
        public string InsuranceCurrency { get; set; }
        public int CustomsValue { get; set; }
        public string CustomsCurrency { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public LabelFormat LabelFormat { get; set; } = LabelFormat.A4;
    }
}
