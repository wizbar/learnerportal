#pragma checksum "/home/endorsed/code/learner_portal/Views/School/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3c14cf201c136a763c20c29a6ad6b5a15351045d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_School_Index), @"mvc.1.0.view", @"/Views/School/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3c14cf201c136a763c20c29a6ad6b5a15351045d", @"/Views/School/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"72b0b0e672e02d858b8773e4f02fa1206d4c35e8", @"/Views/_ViewImports.cshtml")]
    public class Views_School_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<div class=""card card-custom gutter-b"">
    <div class=""card-header"">
        <h4 class=""mt-8"">Schools</h4>
    </div>
    <div class=""card-body"">
        <table id=""allSchool"" class=""table "">
            <thead>
            <tr>
                <div class=""form-actions no-color col-md-3 float-left"">
                    <button class=""btn btn-primary btn-sm mb-2 btn-rounded""");
            BeginWriteAttribute("onclick", " onclick=\"", 388, "\"", 488, 5);
            WriteAttributeValue("", 398, "showInPopUp(\'", 398, 13, true);
#nullable restore
#line 11 "/home/endorsed/code/learner_portal/Views/School/Index.cshtml"
WriteAttributeValue("", 411, Url.Action("Create", "School", null, Context.Request.Scheme), 411, 61, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 472, "\',", 472, 2, true);
            WriteAttributeValue(" ", 474, "\'Add", 475, 5, true);
            WriteAttributeValue(" ", 479, "School\')", 480, 9, true);
            EndWriteAttribute();
            WriteLiteral(@" id=""btnCreate""><i class=""fa fa-plus""></i>Add</button>
                </div>
            </tr>
            <tr>
                <th>Id</th>
                <th>School Code</th>
                <th>EMIS No</th>
                <th>School Name</th>
                <th>Action</th>
            </tr>
            </thead>

        </table>

    </div>
</div>

<script>

    $(document).ready(function () {
        $(""#divProcessing"").hide();

        createSchoolDataTable('allSchool', '/School/GetAllSchool'); 

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
