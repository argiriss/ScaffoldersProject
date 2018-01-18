﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemID { get; set; }
        public Products Product { get; set; }
        public int Quantity { get; set; }

        public CartItem(Products p, int q)
        {
            Product = p;
            Quantity = q;
        }

        public CartItem()
        {

        }
    }
}
