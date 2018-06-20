using System;

using Sawczyn.EFDesigner.EFModel;

#region Additional License

/**************************************************************************************
 * This file is based on code in the JaDAL project by Benjamin Schroeter 
 * under New BSD license.
 *
 * Copyright (c) <year>, <copyright holder>
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the <organization> nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * For more information please visit http://www.codeplex.com/JaDAL 
 */

#endregion

namespace Sawczyn.EFDesigner
{
   public enum Restriction
   {
      Hidden,
      ReadOnly
   }

   /// <summary>
   ///    Decorates a ModelElement with special restrictions for properties.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
   public class RestrictedPropertyAttribute : Attribute
   {
      public RestrictedPropertyAttribute(EFVersion efVersion, EFCoreVersion efCoreVersion, string propertyName, Restriction restriction)
      {
         PropertyName    = propertyName;
         Restriction     = restriction;
         EFVersion       = efVersion;
         EFCoreVersion   = efCoreVersion;
      }

      public RestrictedPropertyAttribute(EFVersion efVersion, string propertyName, Restriction restriction)
      {
         PropertyName  = propertyName;
         Restriction   = restriction;
         EFVersion     = efVersion;
         EFCoreVersion = null;
      }

      public string PropertyName { get; set; }
      public Restriction Restriction { get; set; }
      public EFVersion EFVersion { get; set; }
      public EFCoreVersion? EFCoreVersion { get; set; }
   }
}
