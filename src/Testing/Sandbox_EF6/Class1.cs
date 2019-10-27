using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TrainingManagerSchema.Reverse
{
   public partial class EntityMinimal
   {
      [Key]
      [Required]
      public int Id { get; private set; }
      public string Test { get; set; }
   }
}`
