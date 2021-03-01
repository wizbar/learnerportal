#pragma checksum "/home/endorsed/code/learner_portal/Views/Provinces/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5512ae6bf9d8fc278ea4b2c9f186e0ba56b17ea7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Provinces_Index), @"mvc.1.0.view", @"/Views/Provinces/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5512ae6bf9d8fc278ea4b2c9f186e0ba56b17ea7", @"/Views/Provinces/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"72b0b0e672e02d858b8773e4f02fa1206d4c35e8", @"/Views/_ViewImports.cshtml")]
    public class Views_Provinces_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<div class=""card card-custom gutter-b"">
    <div class=""card-header"">
        <h4 class=""mt-8"">Provinces</h4>
    </div>
    <div class=""card-body"">
        <table id=""allProvinces"" class=""table "">
            <thead>
            <tr>
                <div class=""form-actions no-color col-md-3 float-left"">
                    <button class=""btn btn-primary btn-sm mb-2 btn-rounded""");
            BeginWriteAttribute("onclick", " onclick=\"", 393, "\"", 498, 5);
            WriteAttributeValue("", 403, "showInPopUp(\'", 403, 13, true);
#nullable restore
#line 11 "/home/endorsed/code/learner_portal/Views/Provinces/Index.cshtml"
WriteAttributeValue("", 416, Url.Action("Create", "Provinces", null, Context.Request.Scheme), 416, 64, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 480, "\',", 480, 2, true);
            WriteAttributeValue(" ", 482, "\'Add", 483, 5, true);
            WriteAttributeValue(" ", 487, "Province\')", 488, 11, true);
            EndWriteAttribute();
            WriteLiteral(@" id=""btnCreate""><i class=""fa fa-plus""></i>Add</button>
                </div>
            </tr>
            <tr>
                <th>ID</th>
                <th>province Code</th>
                <th>province Name</th>
                <th>Country Name</th>
                <th>Action</th>
            </tr>
            </thead>

        </table>
    </div>
</div>

<script>

    $(document).ready(function () {
        $(""#divProcessing"").hide();

        createProvincesDataTable('allProvinces', '/Provinces/GetAllProvinces');

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
