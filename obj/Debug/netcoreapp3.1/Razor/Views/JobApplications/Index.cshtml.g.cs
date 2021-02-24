#pragma checksum "/home/endorsed/code/learner_portal/Views/JobApplications/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b1746e4cef895a176e39288023ff8dbc5a072e6f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_JobApplications_Index), @"mvc.1.0.view", @"/Views/JobApplications/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b1746e4cef895a176e39288023ff8dbc5a072e6f", @"/Views/JobApplications/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"72b0b0e672e02d858b8773e4f02fa1206d4c35e8", @"/Views/_ViewImports.cshtml")]
    public class Views_JobApplications_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "/home/endorsed/code/learner_portal/Views/JobApplications/Index.cshtml"
  
    Layout = "_Layout_Main";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<div class=""card card-custom gutter-b"">
    <div class=""card-header"">
        <h4 class=""mt-8""> Job Applications</h4>
    </div>
    <div class=""card-body"">
        <table id=""alljobApplications"" class=""table dt-responsive nowrap w-100"">
            <thead>
            <tr>
                <div class=""form-actions no-color col-md-3 float-left"">
                    <button class=""btn btn-primary btn-sm mb-2 btn-rounded""");
            BeginWriteAttribute("onclick", " onclick=\"", 470, "\"", 588, 6);
            WriteAttributeValue("", 480, "showInPopUp(\'", 480, 13, true);
#nullable restore
#line 14 "/home/endorsed/code/learner_portal/Views/JobApplications/Index.cshtml"
WriteAttributeValue("", 493, Url.Action("Create", "JobApplications", null, Context.Request.Scheme), 493, 70, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 563, "\',", 563, 2, true);
            WriteAttributeValue(" ", 565, "\'Add", 566, 5, true);
            WriteAttributeValue(" ", 570, "Job", 571, 4, true);
            WriteAttributeValue(" ", 574, "Application\')", 575, 14, true);
            EndWriteAttribute();
            WriteLiteral(@" id=""btnCreate""><i class=""fa fa-plus""></i>Create Job Application</button>
                </div>
            </tr>
            <tr>
                <th>National ID</th>
                <th>First Name</th>
                <th>Last Name</th>
                <th>Email</th>
                <th>Phone Number</th>
                <th>Age</th>
                <th>Gender</th>
                <th>Equity</th>
                <th>Disability Status</th>
                <th>Job Title</th>
                <th>Sector</th>
                <th>Date Applied</th>
");
            WriteLiteral(@"                <th>Application Status</th>
                <th>Action</th>
            </tr>
            </thead>

        </table>
    </div>
</div>

<script>

    $(document).ready(function () {
        $(""#divProcessing"").hide();

        createJobApplicationsDataTable('alljobApplications', '/JobApplications/GetAllJobApplications'); 

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
