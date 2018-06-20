using System;

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
   /// <summary>
   ///    Restrict the merging of two elements.
   /// </summary>
   /// <remarks>To use with the domain classes</remarks>
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
   public class RestrictElementMergeAttribute : Attribute
   {
      /// <summary>
      ///    Initializes a new instance of the <see cref="RestrictElementMergeAttribute" /> class.
      /// </summary>
      /// <param name="restrictionMode">The restriction mode.</param>
      /// <param name="compartment">The element name.</param>
      /// <param name="isRestricted">if set to <c>true</c> addition of the given element is not allowd in this restriction mode.</param>
      public RestrictElementMergeAttribute(int restrictionMode, string compartment, bool isRestricted)
      {
         Element         = compartment;
         IsRestricted    = isRestricted;
         RestrictionMode = restrictionMode;
      }

      /// <summary>
      ///    Gets or sets the restriction mode this setting is used for.
      /// </summary>
      /// <value>The restriction mode this setting is used for.</value>
      public int RestrictionMode { get; }

      /// <summary>
      ///    Gets or sets the element.
      /// </summary>
      /// <value>The element.</value>
      public string Element { get; }

      /// <summary>
      ///    Gets or sets a value indicating whether the addition of this element should be restricted.
      /// </summary>
      /// <value>
      ///    <c>true</c> if the addition of this element items is restricted; otherwise, <c>false</c>.
      /// </value>
      public bool IsRestricted { get; }
   }
}
