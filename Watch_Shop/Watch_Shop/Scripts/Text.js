
$(document).ready(function () {
    _getAll();
})

function _getAll() {
    $.ajax({
        url: "/Admin_HomePage/List",
        type: "GET",
        contentType: "application/json; charset=urf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.id + '</td>';
                html += '<td>' + item.name + '</td>';
                html += '<td>' + item.age + '</td>';
                html += '<td> <a href="#" onclick = "return _getById(' + item.id + ')">Edit</a> | <a href="#" onclick = "return _delete(' + item.id + ')">Delete</a> </td>';
                html += '</tr>';
            });
            $('#list tbody').html(html);
        },
        error: function (errormsg) {
            alert(errormsg.responseText);
        }
    });

    $('#btnUpdate').css("display","none"); // hoặc có thể sử dụng $('#btnUpdate').hide();
    $('#btnAdd').show();
    return false;
}

function _getById(id) {
    $.ajax({
        url: '/Admin_HomePage/Get/' + id,
        type: 'GET',
        dataType: "json",
        contentType: "application/json; charset=urf-8",
        async: true,
        success: function (result) {
            $('#Text_ID').val(result.id);
            $('#Text_Name').val(result.name);
            $('#Text_Age').val(result.age);

            $('#modalLoginForm').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormsg) {
            alert(errormsg.responseText);
        }
    });
}
function _add() {
    var obj = {
        id: $('#Text_ID').val(),
        name: $('#Text_Name').val(),
        age: $('#Text_Age').val(),
    }
    $.ajax({
        url: '/Admin_HomePage/Create',
        data: JSON.stringify(obj),
        type: 'POST',
        dataType: "json",
        contentType: "application/json; charset=urf-8",
        success: function (result) {
            _getAll();
            $('#modalLoginForm').modal('hide');
            
        },
        error: function (errormsg) {
            alert(errormsg.responseText);
        }
    });
}

function _delete(id) {
    var cf = confirm("Bạn có muốn xóa id " + id + " này không ");
    if (cf) {
        $.ajax({
            url: '/Admin_HomePage/Delete?id=' + id,
            type: 'POST',
            dataType: "json",
            contentType: "application/json; charset=urf-8",
            async: true,
            success: function (result) {
                _getAll();
            },
            error: function (errormsg) {
                alert(errormsg.responseText);
            }
        });
    }
    
}

function _update() {
    var obj = {
        id: $('#Text_ID').val(),
        name: $('#Text_Name').val(),
        age: $('#Text_Age').val(),
    }
    $.ajax({
        url: '/Admin_HomePage/Update',
        data: JSON.stringify(obj),
        type: 'POST',
        dataType: "json",
        contentType: "application/json; charset=urf-8",
        success: function (result) {
            _getAll();
            $('#modalLoginForm').modal('hide');
            alert("Cập nhật thành công !!!")

        },
        error: function (errormsg) {
            alert(errormsg.responseText);
        }
    });
}


