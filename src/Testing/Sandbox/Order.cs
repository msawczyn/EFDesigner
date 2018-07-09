namespace TuyaTech.v2.Data.Models.Entities.Order
{
    using System;
    using System.Collections.Generic;
    using Base;
    using Identity;
    using Data.Common;

    public class Order : DataEntity, IDeletable
    {
        /// <summary>
        /// Shipper user account id
        /// </summary>
        public Guid UserAccountId { get; set; }

        public UserAccount UserAccount { get; set; }
        public string Code { get; set; }
        public long StatusId { get; set; }
        public OrderStatus Status { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsCreatedOnMobile { get; set; }
        public long AcceptanceSequenceNumber { get; set; }
        public Guid? CurrentVersionId { get; set; }
        public OrderVersion CurrentVersion { get; set; }
        public List<OrderVersion> OrderVersions { get; set; } = new List<OrderVersion>();
        public List<OrderAssignment> Assigments { get; set; } = new List<OrderAssignment>();
    }
}