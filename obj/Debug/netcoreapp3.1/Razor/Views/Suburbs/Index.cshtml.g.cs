#pragma checksum "/home/endorsed/code/learner_portal/Views/Suburbs/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8aa96e4db4f9cecebbd3a508aaed3b9bfc81b27d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Suburbs_Index), @"mvc.1.0.view", @"/Views/Suburbs/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "/home/endorsed/code/learner_portal/Views/_ViewImports.cshtml"
using learner_portal;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/home/endorsed/code/learner_portal/Views/_ViewImports.cshtml"
using learner_portal.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8aa96e4db4f9cecebbd3a508aaed3b9bfc81b27d", @"/Views/Suburbs/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"72b0b0e672e02d858b8773e4f02fa1206d4c35e8", @"/Views/_ViewImports.cshtml")]
    public class Views_Suburbs_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "/home/endorsed/code/learner_portal/Views/Suburbs/Index.cshtml"
  
    Layout = "_Layout";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<div class=""card card-custom gutter-b"">
    <div class=""card-header"">
        <h4 class=""mt-8"">Suburbs</h4>
    </div>
    <div class=""card-body"">
        <table id=""allSuburbs"" class=""table "">
            <thead>
            <tr>
                <div class=""form-actions no-color col-md-3 float-left"">
                    <button class=""btn btn-primary btn-sm mb-2 btn-rounded""");
            BeginWriteAttribute("onclick", " onclick=\"", 421, "\"", 522, 5);
            WriteAttributeValue("", 431, "showInPopUp(\'", 431, 13, true);
#nullable restore
#line 14 "/home/endorsed/code/learner_portal/Views/Suburbs/Index.cshtml"
WriteAttributeValue("", 444, Url.Action("Create", "Suburbs", null, Context.Request.Scheme), 444, 62, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 506, "\',", 506, 2, true);
            WriteAttributeValue(" ", 508, "\'Add", 509, 5, true);
            WriteAttributeValue(" ", 513, "Suburb\')", 514, 9, true);
            EndWriteAttribute();
            WriteLiteral(@" id=""btnCreate""><i class=""fa fa-plus""></i>Create Suburb</button>
                </div>
            </tr>
            <tr>
                <th>Id</th>
                <th>Suburb Code</th>
                <th>Suburb Name</th>
                <th>City Name</th>
                <th>Action</th>
            </tr>
            </thead>

        </table>
    </div>
</div>

<script>

    $(document).ready(function () {
        $(""#divProcessing"").hide();

        createSuburbsDataTable('allSuburbs', '/Suburbs/GetAllSuburbs');

        // Hide the ""busy"" Gif at load:


    });


</script>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
