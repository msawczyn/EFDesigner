# Entity Framework Visual Designer Template API

Content/Welcome.aml


This document describes the subset of the API you can use to modify the T4 templates for code generation.


While the T4 templates supplied generate only C# code, the API help here shows both C# and VB.NET for those who might, for some particular reason, need the VB.NET syntax. And, really, since it's generated via Sandcastle, it was only an extra checkbox click to include that.



## Getting Started

If you somehow got here by direct link, and need details on how to use the designer, please head over to the root of this documentation site to get more information.
&nbsp;<ul><li>
_ContentLayout.content_ - Use the content layout file to manage the conceptual content in the project and define its layout in the table of contents.</li><li>
The _.\media_ folder - Place images in this folder that you will reference from conceptual content using `medialLink` or `mediaLinkInline` elements. If you will not have any images in the file, you may remove this folder.</li><li>
The _.\icons_ folder - This contains a default logo for the help file. You may replace it or remove it and the folder if not wanted. If removed or if you change the file name, update the **Transform Args** project properties page by removing or changing the filename in the `logoFile` transform argument. Note that unlike images referenced from conceptual topics, the logo file should have its **BuildAction** property set to `Content`.</li><li>
The _.\Content_ folder - Use this to store your conceptual topics. You may name the files and organize them however you like. One suggestion is to lay the files out on disk as you have them in the content layout file as shown in this project but the choice is yours. Files can be added via the Solution Explorer or from within the content layout file editor. Files must appear in the content layout file in order to be compiled into the help file.</li></ul>&nbsp;
See the **Conceptual Content** topics in the Sandcastle Help File Builder's help file for more information. See the **Sandcastle MAML Guide** for details on Microsoft Assistance Markup Language (MAML) which is used to create these topics.


