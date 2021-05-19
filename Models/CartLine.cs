﻿// ========================================================
// CartLine.cs, 210331
// Author: Russell Fisher
// Recast CartLine type for general purpose
// ========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection; // reqd for GetService method
using SHTWIMS02.Models;
using SHTWIMS02.Areas.Pull.Models;

namespace SHTWIMS02.Models
{

    public partial class CartLine // --------------------------------------------------------
    {
        //private readonly ICatalogItemRepository ciRepository; // add Description term back to incoming PullItem
        private static IServiceProvider sp;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartLineId { get; set; }
        [BindNever]
        [ForeignKey("PullHdrId")]
        public int PullHdrId { get; set; }       
        public string ItemId { get; set; }
        public string Description { get; set; }
        public string UoM { get; set; }        
        [Required]
        [Range(minimum: 1, maximum: 5000, ErrorMessage = "Please enter a quantity of one or more")]
        public int Qty { get; set; }
        [Required(ErrorMessage = "Please enter a valid date mm/dd/yy")]
        public DateTime DateNeeded { get; set; }
        public string Comment { get; set; }

        //private string itemId;
        //private string description;
        //private DateTime dateNeeded = DateTime.Today;


        //public virtual PullHdr PullHdr { get; set; }

        public CartLine() // default constructor --------------------------------------------------
        {          
            // empty at present

        } // eo default constructor ---------------------------------------------------------------


        public CartLine(CatalogItem ci) // alt 1 constructor---------------------------------------
        {
            // upgrade a CatalogItem to a CartLine object
            this.ItemId = ci.ItemId;
            this.Description = ci.Description;
            this.UoM = ci.UoM;
            this.Qty = 1;
            this.DateNeeded = DateTime.Today;

        }// eo constructor from CatalogItem--------------------------------------------------------

        public CartLine(PullItem pi)  // alt 2 constructor-----------------------------------------
        {
            
            var ciRepository = sp.GetService<ICatalogItemRepository>();            
            
            // ** convert PullItem to Cartline object
            this.ItemId = pi.ItemId;
            this.Description = ciRepository.CatalogItems.FirstOrDefault(ci => ci.ItemId == pi.ItemId).Description;
            this.UoM = pi.UoM;
            this.Qty = pi.QtyRequested;
            this.DateNeeded = pi.DateNeeded;
            this.Comment = pi.Comment;

        } // eo constructor from PullItem ---------------------------------------------------------

    } //eo CartLine class -------------------------------------------------------------------------

} // eo namespace