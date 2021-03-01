#pragma checksum "/home/endorsed/code/learner_portal/Views/Cities/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "45379a4ef6bc37b7fb81a6323b4875665b83ea7c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Cities_Index), @"mvc.1.0.view", @"/Views/Cities/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"45379a4ef6bc37b7fb81a6323b4875665b83ea7c", @"/Views/Cities/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"72b0b0e672e02d858b8773e4f02fa1206d4c35e8", @"/Views/_ViewImports.cshtml")]
    public class Views_Cities_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<div class=""card card-custom gutter-b"">
    <div class=""card-header"">
        <h4 class=""mt-8"">Cities</h4>
    </div>
    <div class=""card-body"">
        <table id=""allProvinces"" class=""table "">
            <thead>
            <tr>
                
                <div class=""form-actions no-color col-md-3 float-left"">
                    <button class=""btn btn-primary btn-sm mb-3 btn-rounded""");
            BeginWriteAttribute("onclick", " onclick=\"", 408, "\"", 506, 5);
            WriteAttributeValue("", 418, "showInPopUp(\'", 418, 13, true);
#nullable restore
#line 12 "/home/endorsed/code/learner_portal/Views/Cities/Index.cshtml"
WriteAttributeValue("", 431, Url.Action("Create", "Cities", null, Context.Request.Scheme), 431, 61, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 492, "\',", 492, 2, true);
            WriteAttributeValue(" ", 494, "\'Add", 495, 5, true);
            WriteAttributeValue(" ", 499, "City\')", 500, 7, true);
            EndWriteAttribute();
            WriteLiteral(@" id=""btnCreate""><i class=""fa fa-plus""></i>Add</button>
                </div>
            </tr>
            <tr>
                <th>ID</th>
                <th>City Code</th>
                <th>City Name</th>
                <th>Province</th>
                <th>Action</th>
            </tr>
            </thead>

        </table>
    </div> 
</div>

<script>

    $(document).ready(function () {
        $(""#divProcessing"").hide();

        createCitiesDataTable('allProvinces', '/Cities/GetAllCities');

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
