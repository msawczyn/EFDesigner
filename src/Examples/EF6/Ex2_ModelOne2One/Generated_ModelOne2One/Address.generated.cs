//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
//
//     Produced by Entity Framework Visual Editor v3.0.7.2
//     Source:                    https://github.com/msawczyn/EFDesigner
//     Visual Studio Marketplace: https://marketplace.visualstudio.com/items?itemName=michaelsawczyn.EFDesigner
//     Documentation:             https://msawczyn.github.io/EFDesigner/
//     License (MIT):             https://github.com/msawczyn/EFDesigner/blob/master/LICENSE
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ex2_ModelOne2One
{
   public partial class Address
   {
      partial void Init();

      /// <summary>
      /// Default constructor
      /// </summary>
      public Address()
      {
         Init();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="person"></param>
      public Address(global::Ex2_ModelOne2One.Person person)
      {
         if (person == null) throw new ArgumentNullException(nameof(person));
         this.Person = person;
         person.Address = this;

         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="person"></param>
      public static Address Create(global::Ex2_ModelOne2One.Person person)
      {
         return new Address(person);
      }

      /*************************************************************************
       * Properties
       *************************************************************************/

      /// <summary>
      /// Identity, Indexed, Required
      /// </summary>
      [Key]
      [Required]
      public long AddressId { get; set; }

      public string Unit { get; set; }

      public string Number { get; set; }

      public string StreetLine1 { get; set; }

      public string StreetType { get; set; }

      public string StreetLine2 { get; set; }

      public string City { get; set; }

      public string PostalCode { get; set; }

      public string State { get; set; }

      public string Country { get; set; }

      public double? Latitude { get; set; }

      public double? Longitude { get; set; }

      /*************************************************************************
       * Navigation properties
       *************************************************************************/

      /// <summary>
      /// Required
      /// </summary>
      public virtual global::Ex2_ModelOne2One.Person Person { get; set; }

   }
}

