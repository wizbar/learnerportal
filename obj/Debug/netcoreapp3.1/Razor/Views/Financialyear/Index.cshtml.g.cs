#pragma checksum "/home/endorsed/code/learner_portal/Views/Financialyear/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ed5502c883f923938967c8a62880b2cbadff6aa0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Financialyear_Index), @"mvc.1.0.view", @"/Views/Financialyear/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ed5502c883f923938967c8a62880b2cbadff6aa0", @"/Views/Financialyear/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"72b0b0e672e02d858b8773e4f02fa1206d4c35e8", @"/Views/_ViewImports.cshtml")]
    public class Views_Financialyear_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "/home/endorsed/code/learner_portal/Views/Financialyear/Index.cshtml"
  
    Layout = "_Layout";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"card card-custom gutter-b\">\r\n    <div class=\"card-header\">\r\n        <h4 class=\"mt-8\">Financial years</h4>\r\n    </div>\r\n    <div class=\"card-body\">\r\n        <table id=\"allfinancialyear\" class=\"table \">\r\n            <thead> \r\n");
            WriteLiteral(@"            <tr>
                <th>ID</th>
                <th>Start Date</th>
                <th>End Date</th>
                <th>Active For WSP</th>
                <th>Description</th>
                <th>Locked For WSP Submission</th>
                <th>Active(Y/N)</th>
                <th>Action</th>
            </tr>
            </thead>

        </table>
    </div>
</div>

<script>

    $(document).ready(function () {
        $(""#divProcessing"").hide();

        createFinancialyearDataTable('allfinancialyear', '/Financialyear/GetAllFinancialyear');

        // Hide the ""busy"" Gif at load:


    });


</script>    ");
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
