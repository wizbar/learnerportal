#pragma checksum "/home/endorsed/code/learner_portal/Views/Ofo/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7777f5b58b79be2f0ca792c038d88dfb7eeb6e14"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Ofo_Index), @"mvc.1.0.view", @"/Views/Ofo/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7777f5b58b79be2f0ca792c038d88dfb7eeb6e14", @"/Views/Ofo/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"72b0b0e672e02d858b8773e4f02fa1206d4c35e8", @"/Views/_ViewImports.cshtml")]
    public class Views_Ofo_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<div class=""card card-custom gutter-b"">
    <div class=""card-header"">
        <h4 class=""mt-8"">Organising Framework For Occupations</h4>
    </div>
    <div class=""card-body"">
        <table id=""allofo"" class=""table "">
            <thead>
            <tr>
                <div class=""form-actions no-color col-md-3 float-left"">
                    <button class=""btn btn-primary btn-sm mb-2 btn-rounded""");
            BeginWriteAttribute("onclick", " onclick=\"", 414, "\"", 508, 5);
            WriteAttributeValue("", 424, "showInPopUp(\'", 424, 13, true);
#nullable restore
#line 11 "/home/endorsed/code/learner_portal/Views/Ofo/Index.cshtml"
WriteAttributeValue("", 437, Url.Action("Create", "Ofo", null, Context.Request.Scheme), 437, 58, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 495, "\',", 495, 2, true);
            WriteAttributeValue(" ", 497, "\'Add", 498, 5, true);
            WriteAttributeValue(" ", 502, "Ofo\')", 503, 6, true);
            EndWriteAttribute();
            WriteLiteral(@" id=""btnCreate""><i class=""fa fa-plus""></i>Create Ofo</button>
                </div>
            </tr>
            <tr>
                <th>ID</th>
                <th>OFO Code</th>
                <th>OFO Title</th>
                <th>OFO Unit Title</th>
                <th>Financial Year</th>
                <th>Action</th>
            </tr>
            </thead>

        </table>

    </div>
</div>

<script>

    $(document).ready(function () {
        $(""#divProcessing"").hide();
 
        createOfoDataTable('allofo', '/Ofo/GetAllOfo');

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
