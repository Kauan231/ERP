﻿using System.ComponentModel.DataAnnotations;

namespace ERP.Data.Dtos
{
    public class CreateInventoryDto
    {
        public string Name { get; set; }
        public string businessId { get; set; }
    }
}
