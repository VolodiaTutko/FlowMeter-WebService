// <copyright file="Service.cs" company="FlowMeter">
// Copyright (c) FlowMeter. All rights reserved.
// </copyright>

namespace Application.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Reflection;

    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceId { get; set; }

        public int? HouseId { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове для заповнення.")]
        [EnumDataType(typeof(ServiceType))]

        required public string TypeOfAccount { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове для заповнення.")]
        [Range(0, double.MaxValue, ErrorMessage = "Будь ласка, введіть додатне число.")]
        public int? Price { get; set; }

        [ForeignKey(nameof(HouseId))]
        public House House { get; set; }
    }

    public enum ServiceType
    {
        [Display(Name = "Холодна вода")]
        ColdWater,
        [Display(Name = "Гаряча вода")]
        HotWater,
        [Display(Name = "Газ")]
        Gas,
        [Display(Name = "Світло")]
        Electricity,
    }

    public static class ServiceTypeColumn
    {
        public static string GetDisplayName(ServiceType type)
        {
            MemberInfo memberInfo = typeof(ServiceType).GetMember(type.ToString())[0];
            DisplayAttribute displayAttribute = (DisplayAttribute)memberInfo.GetCustomAttribute(typeof(DisplayAttribute));
            return displayAttribute.Name;
        }
    }
}
