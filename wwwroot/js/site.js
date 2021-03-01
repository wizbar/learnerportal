// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification

// for details on configuring this project to bundle and minify static web assets spinner spinner-track spinner-dark mr-15.
/*$(document).ajaxStart($.blockUI({ message: '<h1><img src="../Images/busy.gif"/>Just a moment...</h1>' })).ajaxStop($.unblockUI);*/
$(document).ajaxStart($.blockUI).ajaxStop($.unblockUI());

$('.select2').select2({
    placeholder: 'Select an option'
});

// Write your JavaScript code.

//This is for Add/Edit/and Details

showInPopUp = (url, title) => {

    $.ajax({

        type: "GET",

        url: url,

        success: function (res) {

            $("#form-modal .modal-body").html(res);

            $("#form-modal .modal-title").html(title)

            $("#form-modal").modal('show');

        }

    })

};



//This is for  the ig wizards

showInPopUpLg = (url, title) => {

    $.ajax({

        type: "GET",

        url: url,



        success: function (res) {

            $("#form-modal-lg .modal-body").html(res);

            $("#form-modal-lg .modal-title").html(title)

            $("#form-modal-lg").modal('show');

        }

    })

};





$(document).ready(function() {

    $('.select2').select2();

});





createPersonDataTable = (table_id, url) => {



    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        responsive: true,
        dom: 'Bfrtip',

        buttons: [

            {

                extend: 'excelHtml5',

                text: '<i class="icon-xl la la-file-excel mr-3"></i>',

                titleAttr: 'Excel'

            },

            {

                extend: 'pdfHtml5',

                text: '<i class="icon-xl la la-file-pdf"></i>',

                titleAttr: 'PDF'

            }

        ],

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side      

        "filter": true, // this is for disable filter (search box)

        "orderMulti": false, // for disable multiple column at once      

        "ajax": {

            //Service that will be called     

            "url": url,

            //This is the type of metod which will be executed

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "nationalID", "name": "nationalID", "autoWidth": true },

            {
                "render": function (data, type, full) {
                    if(full.photoPath != null){
                        return '<img src="'  + full.photoPath + full.photoName + '" style="height:100px;width:100px;" alt=""/>';
                    }
                    else {
                        return '<img src="../Images/default-avatar.png" style="height:100px;width:100px;" alt=""/>';
                    }

                }},

            { "data": "firstName", "name": "firstName", "autoWidth": true },

            { "data": "lastName", "name": "lastName", "autoWidth": true },

            { "data": "personsDob", "name": "personsDob", "autoWidth": true },

            { "data": "age", "name": "age", "autoWidth": true },

            { "data": "email", "name": "email", "autoWidth": true },

            { "data": "phoneNumber", "name": "phoneNumber", "autoWidth": true },

            { "data": "provinceName", "name": "provinceName", "autoWidth": true },

            { "data": "countryName", "name": "countryName", "autoWidth": true },

            {

                "render": function (data, type, full) {



                    return `<a href="/Person/Edit/` + full.nationalID + `"  class=" mr-2" ><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>| ` +

                        `<a href="/Person/Delete/` + full.nationalID + `" class=" mr-2" ><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>| ` +

                        `<a href="/Person/Details/` + full.nationalID + `" class=" mr-2" ><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>| `;

                }

            }



        ],



    });

};


createCompaniesDataTable = (table_id, url) => {


    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        responsive : true,
        dom: 'Bfrtip',

        buttons: [

            {

                extend: 'excelHtml5',

                text: '<i class="icon-xl la la-file-excel mr-3"></i>',

                titleAttr: 'Excel'

            },

            {

                extend: 'pdfHtml5',

                text: '<i class="icon-xl la la-file-pdf"></i>',

                titleAttr: 'PDF'

            }

        ],

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side      

        "filter": true, // this is for disable filter (search box)

        "orderMulti": false, // for disable multiple column at once      

        "ajax": {

            //Service that will be called     

            "url": url,

            //This is the type of metod which will be executed

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "companyId", "name": "companyId", "autoWidth": true },
            
            {
                "render": function (data, type, full) {

                    if(full.photoPath != null){
                        return '<img src="'  + full.photoPath + full.photoName + '" style="height:100px;width:100px;" alt=""/>';
                    }
                    else {
                        return '<img src="../assets/media/misc/bg-3.jpg" style="height:100px;width:100px;" alt=""/>';
                    }



                }},

    { "data": "companyName", "name": "companyName", "autoWidth": true },

            { "data": "contactEmail", "name": "contactEmail", "autoWidth": true },

            { "data": "contactMobile", "name": "contactMobile", "autoWidth": true },

            { "data": "contactTelephone", "name": "contactTelephone", "autoWidth": true },

            { "data": "streetName", "name": "streetName", "autoWidth": true },

            { "data": "suburbName", "name": "suburbName", "autoWidth": true },

            { "data": "cityName", "name": "cityName", "autoWidth": true },

            { "data": "provinceName", "name": "provinceName", "autoWidth": true },

            { "data": "countryName", "name": "countryName", "autoWidth": true },

            { "data": "addressType", "name": "addressType", "autoWidth": true },

            {

                "render": function (data, type, full) {



                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUpLg('/Companies/Edit/` + full.companyId + `','Edit Company')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUpLg('/Companies/Delete/` + full.companyId + `','Delete Company')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUpLg('/Companies/Details/` + full.companyId + `','Company Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;

                }

            }



        ],



    });



};



createJobDataTable = (table_id, url) => {
    
    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side       

        "filter": true, // this is for disable filter (search box)     

        "orderMulti": false, // for disable multiple column at once     

        "ajax": {

            //Service that will be called

            "url": url,

            //This is the type of metod which will be executed 

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "id", "name": "id", "autoWidth": true },
            
            { "data": "jobCode", "name": "jobCode", "autoWidth": true },

            { "data": "jobTitle", "name": "jobTitle", "autoWidth": true },

            { "data": "jobDesc", "name": "jobDesc", "autoWidth": true },

            { "data": "listedDate", "name": "listedDate", "autoWidth": true },

            { "data": "expiryDate", "name": "expiryDate", "autoWidth": true },

            { "data": "sectorDesc", "name": "sectorDesc", "autoWidth": true },

            { "data": "jobTypeDesc", "name": "jobTypeDesc", "autoWidth": true },

            { "data": "ofoTitle", "name": "ofoTitle", "autoWidth": true },

            { "data": "provinceName", "name": "provinceName", "autoWidth": true },

            {

                "render": function (data, type, full) {

                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUpLg('/Job/Edit/` + full.id + `','Edit Job')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +

                        `<a href="javascript:"  class=" mr-2 " onClick="showInPopUpLg('/Job/Delete/` + full.id + `','Delete Job')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +

                        `<a href="javascript:"  class=" mr-2 " onClick="showInPopUpLg('/Job/Details/` + full.id + `','Job Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;

                    /*`<a href="/Job/Details/` + full.jobCode + `" class=" mr-2" ><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>| ` ;*/

                    /*`<a href="/JobApplication/Apply/"   class="btn btn-outline-success  btn-rounded btn-sm"><i class="fab fa-sistrix"></i> Apply</a>`;*/

                }

            }



        ],



    });

};



createJobSectorDataTable = (table_id, url) => {



    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side       

        "filter": true, // this is for disable filter (search box)     

        "orderMulti": false, // for disable multiple column at once     

        "ajax": {

            //Service that will be called     

            "url": url,

            //This is the type of metod which will be executed 

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "jobSectorId", "name": "jobSectorId", "autoWidth": true },

            { "data": "jobSectorCode", "name": "jobSectorCode", "autoWidth": true },

            { "data": "jobSectorDesc", "name": "jobSectorDesc", "autoWidth": true },

            {

                "render": function (data, type, full) {



                    return `<a href="/JobSector/Edit/` + full.jobSectorId + `"  class=" mr-2" ><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>| ` +

                        `<a href="/JobSector/Delete/` + full.jobSectorId + `" class=" mr-2" ><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>| ` +

                        `<a href="/JobSector/Details/` + full.jobSectorId + `" class=" mr-2" ><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>| ` ;

                }

            }



        ],



    });



};





createJobTypeDataTable = (table_id, url) => {



    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side       

        "filter": true, // this is for disable filter (search box)     

        "orderMulti": false, // for disable multiple column at once     

        "ajax": {

            //Service that will be called     

            "url": url,

            //This is the type of metod which will be executed 

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "jobTypeId", "name": "jobTypeId", "autoWidth": true },

            { "data": "jobTypeCode", "name": "jobTypeCode", "autoWidth": true },

            { "data": "jobTypeDesc", "name": "jobTypeDesc", "autoWidth": true },

            {

                "render": function (data, type, full) {



                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/JobType/Edit/` + full.jobTypeId + `','Edit Job Type')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/JobType/Delete/` + full.jobTypeId + `','Delete Job Type')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/JobType/Details/` + full.jobTypeId + `','Job Type Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;

                }

            }



        ],



    });

};





createInstitutionTypeDataTable = (table_id, url) => {



    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side       

        "filter": true, // this is for disable filter (search box)     

        "orderMulti": false, // for disable multiple column at once     

        "ajax": {

            //Service that will be called     

            "url": url,

            //This is the type of metod which will be executed 

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "institutionTypeId", "name": "institutionTypeId", "autoWidth": true },

            { "data": "institutionTypeCode", "name": "institutionTypeCode", "autoWidth": true },

            { "data": "institutionTypeDesc", "name": "institutionTypeDesc", "autoWidth": true },

            {

                "render": function (data, type, full) {



                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/InstitutionType/Edit/` + full.institutionTypeId + `','Edit Institution Type')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/InstitutionType/Delete/` + full.institutionTypeId + `','Delete Institution Type')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/InstitutionType/Details/` + full.institutionTypeId + `','Institution Type Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;

                }

            }



        ],



    });

};





createInstitutionDataTable = (table_id, url) => {



    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side       

        "filter": true, // this is for disable filter (search box)      

        "orderMulti": false, // for disable multiple column at once     

        "ajax": {

            //Service that will be called     

            "url": url,

            //This is the type of metod which will be executed 

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "id", "name": "id", "autoWidth": true },
            { "data": "institutionCode", "name": "institutionCode", "autoWidth": true },

            { "data": "institutionName", "name": "institutionName", "autoWidth": true },

            { "data": "institutionTypeDesc", "name": "institutionTypeDesc", "autoWidth": true },

            {

                "render": function (data, type, full) {
                    
                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Institution/Edit/` + full.id + `','Edit Institution')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Institution/Delete/` + full.id + `','Delete Institution')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Institution/Details/` + full.id + `','Institution Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;

                }

            }
            
        ],
        
    });

}; 


createSchoolDataTable = (table_id, url) => {
    

    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side       

        "filter": true, // this is for disable filter (search box)     

        "orderMulti": false, // for disable multiple column at once     

        "ajax": {

            //Service that will be called     

            "url": url,

            //This is the type of metod which will be executed  

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "id", "name": "id", "autoWidth": true },
            { "data": "schoolCode", "name": "schoolCode", "autoWidth": true },

            { "data": "emisNo", "name": "emisNo", "autoWidth": true },

            { "data": "schoolName", "name": "schoolName", "autoWidth": true },



            {

                "render": function (data, type, full) {



                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/School/Edit/` + full.id + `','Edit School')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/School/Delete/` + full.id + `','Delete School')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/School/Details/` + full.id + `','School Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;

                }

            }



        ],



    });

};





createSchoolGradeDataTable = (table_id, url) => {



    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({
        "responsive": true,
        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side       

        "filter": true, // this is for disable filter (search box)     

        "orderMulti": false, // for disable multiple column at once     

        "ajax": {

            //Service that will be called     

            "url": url,

            //This is the type of metod which will be executed 

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "schoolGradeId", "name": "schoolGradeId", "autoWidth": true },

            { "data": "schoolGradeCode", "name": "schoolGradeCode", "autoWidth": true },

            { "data": "schoolGradeName", "name": "schoolGradeName", "autoWidth": true },

            {

                "render": function (data, type, full) {



                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/SchoolGrade/Edit/` + full.schoolGradeId + `','Edit School Grade')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/SchoolGrade/Delete/` + full.schoolGradeId + `','Delete School Grade')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/SchoolGrade/Details/` + full.schoolGradeId + `','School Grade Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;

                }

            }



        ],



    });

};





createLearnersDataTable = (table_id, url) => {



    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({
        responsive : true,
        dom: 'Bfrtip',

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side       

        "filter": true, // this is for disable filter (search box)     

        "orderMulti": false, // for disable multiple column at once     

        "ajax": {

            //Service that will be called      

            "url": url,

            //This is the type of metod which will be executed 

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "learnerId", "name": "learnerId", "autoWidth": true },
            {
                "render": function (data, type, full) {
                   if(full.photoPath != null){ 
                    return '<img src="'  + full.photoPath + full.photoName + '" style="height:100px;width:100px;" alt=""/>';
                   }
                   else {
                       return '<img src="../Images/default-avatar.png" style="height:100px;width:100px;" alt=""/>';
                   }

                }},

            { "data": "firstName", "name": "firstName", "autoWidth": true },

            { "data": "lastName", "name": "lastName", "autoWidth": true },

            { "data": "email", "name": "email", "autoWidth": true },

            { "data": "phoneNumber", "name": "phoneNumber", "autoWidth": true },

            { "data": "personsDob", "name": "personsDob", "autoWidth": true },

            { "data": "age", "name": "age", "autoWidth": true },

            { "data": "genderName", "name": "genderName", "autoWidth": true },

            { "data": "schoolName", "name": "schoolName", "autoWidth": true },

            { "data": "schoolGradeName", "name": "schoolGradeName", "autoWidth": true },

            { "data": "yearSchoolCompleted", "name": "yearSchoolCompleted", "autoWidth": true },
           /* { "data": "appliedYn", "name": "appliedYn", "autoWidth": true },*/

            {
                "render": function (data, type, full) {

                    return '<span class="label label-lg font-weight-boldest label-rounded label-outline-'+ ((full.appliedYn === 'true')? 'success label-inline"><i class="fas fa-check mr-2" style="color:#00b300 "></i>' : 'danger label-inline"><i class="fas fa-times mr-2" style="color:#a71d2a "></i>')  +  full.appliedYn +'</span>';

                }},
            {
                "render": function (data, type, full) {

                    return '<span class="label label-lg font-weight-boldest label-rounded label-outline-'+ ((full.recruitedYn === 'true')? 'success label-inline"><i class="fas fa-check mr-2" style="color:#00b300 "></i>' : 'danger label-inline"><i class="fas fa-times mr-2" style="color:#a71d2a"></i>') +  full.recruitedYn +'</span>';

                }},

            {
 
                "render": function (data, type, full) {
                    if(full.appliedYn === 'false' && full.recruitedYn === 'false')
                      return  `<a href="javascript:" class="btn btn-sm btn-light-danger font-weight-bold ml-3" onClick="showInPopUpLg('/Learners/Details/` + full.learnerId + `','Learner Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" ></i>View</a>`;
                   else if(full.appliedYn === 'true' && full.recruitedYn === 'false')
                      return  `<a href="javascript:" class="btn btn-sm btn-light-warning font-weight-bold ml-3" onClick="showInPopUpLg('/JobApplications/Recruited/` + full.learnerId + `','Learner Requitment')" id="btnView"><i class="fa fa-eye" aria-hidden="true" ></i>Recruit</a>`;
                    else 
                        return  `<a href="javascript:" class="btn btn-sm btn-light-success font-weight-bold ml-3" onClick="showInPopUpLg('/Learners/Details/` + full.learnerId + `','View Employed Learner')" id="btnView"><i class="fa fa-eye" aria-hidden="true" ></i>View</a>`;

                }

            }



        ],



    });

};





createUsersDataTable = (table_id, url) => {



    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side       

        "filter": true, // this is for disable filter (search box)     

        "orderMulti": false, // for disable multiple column at once     

        "ajax": {

            //Service that will be called

            "url": url,

            //This is the type of metod which will be executed

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "userId", "name": "userId", "autoWidth": true },

            { "data": "username", "name": "username", "autoWidth": true },

            { "data": "email", "name": "email", "autoWidth": true },

            { "data": "role", "name": "role", "autoWidth": true },

            { "data": "activeYn", "name": "activeYn", "autoWidth": true },

            {

                "render": function (data, type, full) {



                    return `<a href="/Users/Edit/` + full.userId + `"  class=" mr-2" ><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>| ` +

                        `<a href="/Users/Delete/` + full.userId + `" class=" mr-2" ><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>| ` +

                        `<a href="/Users/Details/` + full.userId + `" class=" mr-2" ><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>| ` ;

                }

            }



        ],



    });

};





createRolesDataTable = (table_id, url) => {



    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side       

        "filter": true, // this is for disable filter (search box)     

        "orderMulti": false, // for disable multiple column at once     

        "ajax": {

            //Service that will be called

            "url": url,

            //This is the type of metod which will be executed

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "roleId", "name": "roleId", "autoWidth": true },

            { "data": "roleName", "name": "roleName", "autoWidth": true },

            { "data": "roleDesc", "name": "roleDesc", "autoWidth": true },

            { "data": "activeYn", "name": "activeYn", "autoWidth": true },

            {

                "render": function (data, type, full) {



                    return `<a href="/Roles/Edit/` + full.roleId + `"  class=" mr-2" ><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>| ` +

                        `<a href="/Roles/Delete/` + full.roleId + `" class=" mr-2" ><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>| ` +

                        `<a href="/Roles/Details/` + full.roleId + `" class=" mr-2" ><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>| ` ;

                }

            }



        ],



    });

};





createOfoDataTable = (table_id, url) => {



    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side       

        "filter": true, // this is for disable filter (search box)     

        "orderMulti": false, // for disable multiple column at once     

        "ajax": {

            //Service that will be called 

            "url": url,

            //This is the type of metod which will be executed

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "id", "name": "id", "autoWidth": true },
            { "data": "ofoCode", "name": "ofoCode", "autoWidth": true },

            { "data": "ofoTitle", "name": "ofoTitle", "autoWidth": true },

            { "data": "ofoUnitName", "name": "ofoUnitName", "autoWidth": true },

            { "data": "financialyearName", "name": "financialyearName", "autoWidth": true },

            {

                "render": function (data, type, full) {



                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Ofo/Edit/` + full.id + `','Edit Ofo')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Ofo/Delete/` + full.id + `','Delete Ofo')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Ofo/Details/` + full.id + `','Ofo Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;

                }

            }



        ],



    });

};





createFinancialyearDataTable = (table_id, url) => {



    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side       

        "filter": true, // this is for disable filter (search box)     

        "orderMulti": false, // for disable multiple column at once     

        "ajax": {

            //Service that will be called

            "url": url,

            //This is the type of metod which will be executed

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "financialyearId", "name": "financialyearId", "autoWidth": true },

            { "data": "startDate", "name": "startDate", "autoWidth": true },

            { "data": "endDate", "name": "endDate", "autoWidth": true },

            { "data": "activeForWsp", "name": "activeForWsp", "autoWidth": true },

            { "data": "financialyearDesc", "name": "financialyearDesc", "autoWidth": true },

            { "data": "lockedForWspSubmission", "name": "lockedForWspSubmission", "autoWidth": true },

            { "data": "activeYn", "name": "activeYn", "autoWidth": true },

            {

                "render": function (data, type, full) {



                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Financialyear/Edit/` + full.financialyearId + `','Edit Financial year')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Financialyear/Delete/` + full.financialyearId + `','Delete Financial year')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Financialyear/Details/` + full.financialyearId + `','Financial year Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;

                }

            }



        ],



    });

};





createSectorDataTable = (table_id, url) => {



    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side       

        "filter": true, // this is for disable filter (search box)     

        "orderMulti": false, // for disable multiple column at once     

        "ajax": {

            //Service that will be called

            "url": url,

            //This is the type of metod which will be executed

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "sectorId", "name": "sectorId", "autoWidth": true },

            { "data": "sectorDesc", "name": "sectorDesc", "autoWidth": true },

            { "data": "activeYn", "name": "activeYn", "autoWidth": true },

            {

                "render": function (data, type, full) {



                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Sector/Edit/` + full.sectorId + `','Edit Sector')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Sector/Delete/` + full.sectorId + `','Delete Sector')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Sector/Details/` + full.sectorId + `','Sector Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;

                }

            }



        ],



    });

};





createOfoUnitDataTable = (table_id, url) => {



    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side       

        "filter": true, // this is for disable filter (search box)     

        "orderMulti": false, // for disable multiple column at once     

        "ajax": {

            //Service that will be called

            "url": url,

            //This is the type of metod which will be executed

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "id", "name": "id", "autoWidth": true },
            { "data": "ofoUnitCode", "name": "ofoUnitCode", "autoWidth": true },
            { "data": "ofoUnitTitle", "name": "ofoUnitTitle", "autoWidth": true },
            { "data": "ofoMinorTitle", "name": "ofoMinorTitle", "autoWidth": true },
            { "data": "financialYearDesc", "name": "financialYearDesc", "autoWidth": true },

            {

                "render": function (data, type, full) {



                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/OfoUnit/Edit/` + full.id + `','Edit Ofo Unit')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/OfoUnit/Delete/` + full.id + `','Delete Ofo Unit')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/OfoUnit/Details/` + full.id + `','Ofo Unit Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;

                }

            }



        ],



    });

};



createOfoMinorDataTable = (table_id, url) => {



    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        "order": [[2, "asc"]],

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side       

        "filter": true, // this is for disable filter (search box)     

        "orderMulti": false, // for disable multiple column at once     

        "ajax": {

            //Service that will be called

            "url": url,

            //This is the type of metod which will be executed

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "id", "name": "id", "autoWidth": true },
            
            { "data": "ofoMinorCode", "name": "ofoMinorCode", "autoWidth": true },

            { "data": "ofoMinorTitle", "name": "ofoMinorTitle", "autoWidth": true },

            { "data": "financialYearDesc", "name": "financialYearDesc", "autoWidth": true },

            {

                "render": function (data, type, full) {



                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/OfoMinor/Edit/` + full.id + `','Edit Ofo Minor')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/OfoMinor/Delete/` + full.id + `','Delete Ofo Minor')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/OfoMinor/Details/` + full.id + `','Ofo Minor Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;

                }

            }



        ],



    });

};





createJobApplicationsDataTable = (table_id, url) => {



    // The table which will be configured to use Datatables

    $("#" + table_id).DataTable({

        dom: 'Bfrtip',
        responsive: true,

        buttons: [

            {

                extend: 'excelHtml5',

                text: '<i class="icon-xl la la-file-excel mr-3"></i>',

                titleAttr: 'Excel'

            },

            {

                extend: 'pdfHtml5',

                text: '<i class="icon-xl la la-file-pdf"></i>',

                titleAttr: 'PDF'

            }

        ],

        "order": [[2, "asc"]],
          

        "processing": true, // for show progress bar      

        "serverSide": false, // for process server side      

        "filter": true, // this is for disable filter (search box)

        "orderMulti": false, // for disable multiple column at once      

        "ajax": {

            //Service that will be called     

            "url": url,

            //This is the type of metod which will be executed

            "type": "post",

            "datatype": "json"

        },

        "columnDefs": [{

            "targets": [0],

            "visible": false,

            "searchable": true

        }],

        //List number of Columns which will be displayed 

        "columns": [

            { "data": "nationalID", "name": "nationalID", "autoWidth": true },

            { "data": "firstName", "name": "firstName", "autoWidth": true },

            { "data": "lastName", "name": "lastName", "autoWidth": true },

            { "data": "email", "name": "email", "autoWidth": true },

            { "data": "phoneNumber", "name": "phoneNumber", "autoWidth": true },

            { "data": "age", "name": "age", "autoWidth": true },

            { "data": "genderName", "name": "genderName", "autoWidth": true },

            { "data": "equityName", "name": "equityName", "autoWidth": true },

            { "data": "disabilityStatus", "name": "disabilityStatus", "autoWidth": true },

            { "data": "appllications[i].jobTitle", "name": "appllications[i].jobTitle", "autoWidth": true },

            { "data": "appllications[i].sectorDesc", "name": "appllications[i].sectorDesc", "autoWidth": true },

            { "data": "appllications[i].dateApplied", "name": "dateApplied[i].sectorDesc", "autoWidth": true },
/*
            { "data": "provinceName", "name": "provinceName", "autoWidth": true },

            { "data": "cityName", "name": "cityName", "autoWidth": true },*/

            { "data": "appllications[i].jobApplicationStatus", "name": "appllications[i].jobApplicationStatus", "autoWidth": true },

            {

                "render": function (data, type, full) {



                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUpLg('/JobApplications/Edit/` + full.nationalID + `','Edit Job Application')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUpLg('/JobApplications/Delete/` + full.nationalID + `','Delete Job Application')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +

                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUpLg('/JobApplications/Details/` + full.nationalID + `','Job Application Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;

                }

            }



        ]



    });

};


createAddressTypesDataTable = (table_id, url) => {

    // The table which will be configured to use Datatables
    $("#" + table_id).DataTable({
        "order": [[2, "asc"]],
        "processing": true, // for show progress bar      
        "serverSide": false, // for process server side       
        "filter": true, // this is for disable filter (search box)     
        "orderMulti": false, // for disable multiple column at once     
        "ajax": {
            //Service that will be called
            "url": url,
            //This is the type of metod which will be executed
            "type": "post",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": true
        }],
        //List number of Columns which will be displayed
        "columns": [
            { "data": "id", "name": "id", "autoWidth": true },
            { "data": "addressTypeCode", "name": "addressTypeCode", "autoWidth": true },
            { "data": "addressTypeName", "name": "addressTypeName", "autoWidth": true },
            {
                "render": function (data, type, full) {

                    return `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/AddressTypes/Edit/` + full.id + `','Edit Address Type')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>` +
                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/AddressTypes/Delete/` + full.id + `','Delete Address Type')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>` +
                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/AddressTypes/Details/` + full.id + `','Address Type Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;
                }
            }

        ],

    });
};

createCitiesDataTable = (table_id, url) => {

    // The table which will be configured to use Datatables
    $("#" + table_id).DataTable({
        "order": [[2, "asc"]],
        "processing": true, // for show progress bar      
        "serverSide": false, // for process server side       
        "filter": true, // this is for disable filter (search box)     
        "orderMulti": false, // for disable multiple column at once     
        "ajax": {
            //Service that will be called
            "url": url,
            //This is the type of metod which will be executed
            "type": "post",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": true
        }],
        //List number of Columns which will be displayed
        "columns": [
            { "data": "id", "name": "id", "autoWidth": true },
            { "data": "cityCode", "name": "cityCode", "autoWidth": true },
            { "data": "cityName", "name": "cityName", "autoWidth": true },
            { "data": "provinceName", "name": "provinceName", "autoWidth": true },
            {
                "render": function (data, type, full) {

                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Cities/Edit/` + full.id + `','Edit City')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +
                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Cities/Delete/` + full.id + `','Delete City')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +
                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Cities/Details/` + full.id + `','City Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;
                }
            }

        ],

    });
};

createCountriesDataTable = (table_id, url) => {

    // The table which will be configured to use Datatables
    $("#" + table_id).DataTable({
        "order": [[2, "asc"]],
        "processing": true, // for show progress bar      
        "serverSide": false, // for process server side       
        "filter": true, // this is for disable filter (search box)     
        "orderMulti": false, // for disable multiple column at once     
        "ajax": {
            //Service that will be called
            "url": url,
            //This is the type of metod which will be executed
            "type": "post",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": true
        }],
        //List number of Columns which will be displayed
        "columns": [
            { "data": "id", "name": "id", "autoWidth": true },
            { "data": "countryName", "name": "countryName", "autoWidth": true },
            { "data": "countryCode", "name": "countryCode", "autoWidth": true },
            {
                "render": function (data, type, full) {

                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Countries/Edit/` + full.id + `','Edit Country')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +
                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Countries/Delete/` + full.id + `','Delete Country')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +
                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Countries/Details/` + full.id + `','Country Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;
                }
            }

        ],

    });
};

createProvincesDataTable = (table_id, url) => {

    // The table which will be configured to use Datatables
    $("#" + table_id).DataTable({
        "order": [[2, "asc"]],
        "processing": true, // for show progress bar      
        "serverSide": false, // for process server side       
        "filter": true, // this is for disable filter (search box)     
        "orderMulti": false, // for disable multiple column at once     
        "ajax": {
            //Service that will be called
            "url": url,
            //This is the type of metod which will be executed
            "type": "post",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": true
        }],
        //List number of Columns which will be displayed
        "columns": [
            { "data": "id", "name": "id", "autoWidth": true },
            { "data": "provinceCode", "name": "provinceCode", "autoWidth": true },
            { "data": "provinceName", "name": "provinceName", "autoWidth": true },
            { "data": "countryName", "name": "countryName", "autoWidth": true },
            {
                "render": function (data, type, full) {

                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Provinces/Edit/` + full.id + `','Edit province')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +
                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Provinces/Delete/` + full.id + `','Delete province')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +
                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Provinces/Details/` + full.id + `','province Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;
                }
            }

        ],

    });
};


createSuburbsDataTable = (table_id, url) => {

    // The table which will be configured to use Datatables
    $("#" + table_id).DataTable({
        "order": [[2, "asc"]],
        "processing": true, // for show progress bar      
        "serverSide": false, // for process server side       
        "filter": true, // this is for disable filter (search box)     
        "orderMulti": false, // for disable multiple column at once     
        "ajax": {
            //Service that will be called
            "url": url,
            //This is the type of metod which will be executed
            "type": "post",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": true
        }],
        //List number of Columns which will be displayed
        "columns": [
            { "data": "id", "name": "id", "autoWidth": true },
            { "data": "suburbCode", "name": "suburbCode", "autoWidth": true },
            { "data": "suburbName", "name": "suburbName", "autoWidth": true },
            { "data": "cityName", "name": "cityName", "autoWidth": true },
            {
                "render": function (data, type, full) {

                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Suburbs/Edit/` + full.id + `','Edit Suburb')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +
                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Suburbs/Delete/` + full.id + `','Delete Suburb')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +
                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Suburbs/Details/` + full.id + `','Suburb Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;
                }
            }

        ],

    });
};


createDocumentTypesDataTable = (table_id, url) => {

    // The table which will be configured to use Datatables
    $("#" + table_id).DataTable({
        "order": [[2, "asc"]],
        "processing": true, // for show progress bar      
        "serverSide": false, // for process server side       
        "filter": true, // this is for disable filter (search box)     
        "orderMulti": false, // for disable multiple column at once     
        "ajax": {
            //Service that will be called
            "url": url,
            //This is the type of metod which will be executed
            "type": "post",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": true
        }],
        //List number of Columns which will be displayed
        "columns": [
            { "data": "id", "name": "id", "autoWidth": true },
            { "data": "typeName", "name": "typeName", "autoWidth": true },
            { "data": "description", "name": "description", "autoWidth": true },
            { "data": "roleName", "name": "roleName", "autoWidth": true },
            {
                "render": function (data, type, full) {

                    return '<input type="checkbox" class="checkbox checkbox-circle" '+ ((full.activeYn === 'true')? 'checked="checked"' : '')  + ' disabled name="select" id="ActiveYn"  value="true" />';


                }},
            
            
            {
                "render": function (data, type, full) {

                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/DocumentTypes/Edit/` + full.id + `','Edit Document Type')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +
                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/DocumentTypes/Delete/` + full.id + `','Delete Document Type')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +
                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/DocumentTypes/Details/` + full.id + `','Document Type Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;
                }
            }

        ],

    });
};


createDocumentsDataTable = (table_id, url) => {

    // The table which will be configured to use Datatables
    $("#" + table_id).DataTable({
        "order": [[2, "asc"]],
        "processing": true, // for show progress bar      
        "serverSide": false, // for process server side       
        "filter": true, // this is for disable filter (search box)     
        "orderMulti": false, // for disable multiple column at once     
        "ajax": {
            //Service that will be called
            "url": url,
            //This is the type of metod which will be executed
            "type": "post",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": true
        }],
        //List number of Columns which will be displayed
        "columns": [
            { "data": "id", "name": "id", "autoWidth": true },
            { "data": "documentTypeName", "name": "documentTypeName", "autoWidth": true },
            { "data": "comment", "name": "comment", "autoWidth": true },
            { "data": "learnerId", "name": "learnerId", "autoWidth": true },
            { "data": "companyName", "name": "companyName", "autoWidth": true },
            { "data": "verified", "name": "verified", "autoWidth": true },
            { "data": "verificationDate", "name": "verificationDate", "autoWidth": true },
            { "data": "verifiedBy", "name": "verifiedBy", "autoWidth": true },
            { "data": "jobApplicationId", "name": "jobApplicationId", "autoWidth": true },
            {
                "render": function (data, type, full) {

                    return  `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Documents/Edit/` + full.id + `','Edit Document')" id="btnView"><i class="fa fa-edit" aria-hidden="true" style="color:#727cf5"></i></a>`  +
                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Documents/Delete/` + full.id + `','Delete Document')" id="btnView"><i class="fa fa-trash" aria-hidden="true" style="color:#fa5c7c"></i></a>`  +
                        `<a href="javascript:" class=" mr-2 " onClick="showInPopUp('/Documents/Details/` + full.id + `','Document Details')" id="btnView"><i class="fa fa-eye" aria-hidden="true" style="color:#0acf97"></i></a>`;
                }
            }

        ],

    });
};