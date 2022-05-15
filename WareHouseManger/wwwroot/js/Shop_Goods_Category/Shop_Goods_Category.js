var nameController = "Shop_Goods_Category";
window.onload = function () {
    var TableCategoriElememt = $('#TableCategoriElememt')[0]
    var firstTrInTbody = TableCategoriElememt.children[0]
    var firstTdInTr = firstTrInTbody.children[0].children[0].value
    selectRow(firstTdInTr)
};
var selectRow = function (val) {
    $.ajax({
        type: "GET",
        url: "/" + nameController + '/GetSubCateByCateParentId',
        data: {
            parentId: val,
        },
        success: function (result) {
            $('#btnCreateNewSubCate').attr('href', "/" + nameController + '/CreateSubCate?parentId=' + val)
            var nameParentCateElement = $('#CategoryName').empty();
            var checkStatus = result.succes, checkData = result.data.length;
            console.log(result.data.length);
            if (checkStatus == true && checkData > 0) {
                var TableSubCategoriElememt = $('#TableSubCategori').empty();
                var htmlBlock = "";
                $.each(result.data, function (index, item) {
                    var stringInside = "<tr><td>" + item.name + "</td>";
                    stringInside += "<td>" + item.descrip + "</td>";
                    stringInside += "<td>" + item.subname + "</td>";
                    stringInside += "<td class='text-nowrap text-center'>";
                    stringInside += "<a class='btn btn-sm btn-warning' asp-action='EditSubCate' asp-route-id='" + item.id + "'> <i class='fa fa-edit'> Chỉnh sửa</i></a>";
                    //stringInside += "<a class='btn btn-sm btn-info' asp-action='Details' asp-route-id='" + item.id + "'><i class='fa fa-file'> Chi tiết</i></a>";
                    stringInside += "<a class='btn btn-sm btn-danger' asp-action='DeleteSubCate' asp-route-id='" + item.id + "'><i class='fa fa-trash-o'> Xóa</i></a>";
                    stringInside += "</td></tr >";
                    htmlBlock += stringInside;
                    console.log(item);
                });
                TableSubCategoriElememt.append(htmlBlock);
            }
            if (result.cateParentName != "") {
                nameParentCateElement.append(result.cateParentName)
            }
            else {
                nameParentCateElement.append("Undefined")
                TableSubCategoriElememt.append("<tr><td></td><td></td><td></td><td></td></tr >");
            }
        },

    });
}

var objectId;
var objectName;
var deleteConfirm = function (val) {
    $.ajax({
        type: "GET",
        url: "/" + nameController + '/Detail',
        data: {
            id: val,
        },
        success: function (result) {
            var info = result.data;
            objectId = info.id;
            objectName = info.name;
            $('#deletedValueName').text(objectName);
            $('#delete-conformation').modal('show');
        }
    });
};

var deleteData = function () {
    $.ajax({
        type: "POST",
        url: "/" + nameController + '/DeleteConfirmed',
        data: {
            id: objectId,
        },
        success: function (result) {
            $("#delete-conformation").modal('hide');
            alert("Xóa thành công: " + objectName);
            location.reload();
        },
        error: function () {
            $("#delete-conformation").modal('hide');
        }
    });
}