﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductList.Domain.Entities
{
    [Table("tblProductImages")]
    public class ProductImage
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Priority { get; set; }
        [ForeignKey("Product")]
        public int ProductId{get;set;}
        public virtual Product Product { get; set; }
       

    }
}
