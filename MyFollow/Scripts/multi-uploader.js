/// <reference path="jquery-1.8.2.js" />
/// <reference path="_references.js" />
$(document).ready(function () {
    $(document).on("click", "#fileUpload", beginUpload);
});

function beginUpload(evt) {
    //$scope.newProduct = {};
   
    var files = $("#selectFile").get(0).files;
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            var data = new FormData();
            var Input = "";
            if (files.length < 6)
                {
            for (i = 0; i < files.length; i++) {
                data.append("file" + i, files[i]);
                var img = '../Images/' + files[i].name;
                $('<img />').attr('src', img.toString()).width('113px').height('113px').attr('class', "img-thumbnail").attr('id', i.toString()).appendTo($('#show_images'))
                Input += files[i].name + ",";
            }
            $('#txtimg').val(Input.toString());
            $('#txtimg').focus();
            $.ajax({
                type: "POST",
                url: "/api/upload",
                contentType: false,
                processData: false,
                data: data,
                success: function (results) {

                    
                    //var imgpath = "";
                    
                    //$.each(results, function (index, value) {
                    //    imgpath = " VALUE: " + index + " VALUE: " + value + "\n";
                        
                    //    for (var i = 1; i < value.length; i++) {
                    //       // if (value[i] != "")
                    //        {
                    //            var aa = 'Images/' + value[i];
                    //        $('<img />').attr('src',  ""+ aa+"").width('113px').height('113px').appendTo($('#show_images'));
                    //        }
                    //    }
                        
                    //});
                    //alert(imgpath);
                }
            });
        } else {
            alert("Not More then 5 Images");
            }
        }
    }
}