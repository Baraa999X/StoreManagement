using System.ComponentModel.DataAnnotations;
using StoreManagement.Domain.Enums;
namespace StoreManagement.Application.DTOs
{
    public class UpdateOrderStatusDto
    {
        [EnumDataType(typeof(EnumsOrderStatus))]
        public EnumsOrderStatus Status { get; set; }
    }
}
