/// <reference path="Web.Panel.js" />

Web.Panel.City = {
    list: function () {
        Web.Panel.Loader.show();
        $.ajax({
            type: 'GET',
            url: "/City/List",
            data: null,
            cache: false,
            success: function (result) {
                $("#list-city").html(result.html);
                Web.Panel.Result.show(result);
                Web.Panel.Loader.hide();
            },
            error: function (xhr, status, error) {
                alert("Bir problem oluştu!" + error);
                Web.Panel.Loader.hide();
            }
        });
    },
    new: function () {
        Web.Panel.Loader.show();
        $.ajax({
            type: 'GET',
            url: "/City/Create",
            data: null,
            cache: false,
            success: function (result) {
                Web.Panel.Modal.Item.show(Web.Panel.ModalItemTypeEnum.CREATE, result.html, Web.Panel.City.save);
                Web.Panel.Result.show(result);
                Web.Panel.Loader.hide();
            },
            error: function (xhr, status, error) {
                alert("Bir problem oluştu!");
                Web.Panel.Loader.hide();
            }
        });
    },
    edit: function (id) {
        Web.Panel.Loader.show();
        $.ajax({
            type: 'GET',
            url: "/City/Edit/" + id,
            data: null,
            cache: false,
            success: function (result) {
                if (result.isSuccess) {
                    Web.Panel.Modal.Item.show(Web.Panel.ModalItemTypeEnum.EDIT, result.html, Web.Panel.City.save);
                }
                Web.Panel.Result.show(result);
                Web.Panel.Loader.hide();
            },
            error: function (xhr, status, error) {
                alert("Bir problem oluştu!");
                Web.Panel.Loader.hide();
            }
        });
    },
    save: function () {
        var form = $("#form-city");
        if (form.length) {
            Web.Panel.Loader.show();
            $.ajax({
                url: "/City/Save",
                type: 'POST',
                dataType: "json",
                data: form.serialize(),
                cache: false,
                success: function (result) {
                    Web.Panel.Modal.Item.show(Web.Panel.ModalItemTypeEnum.INFORMATION, result.html, Web.Panel.Modal.Item.hide);
                    Web.Panel.Result.show(result);
                    Web.Panel.City.list();
                    Web.Panel.Loader.hide();
                },
                error: function (xhr, status, error) {
                    alert("Bir problem oluştu!");
                    Web.Panel.Loader.hide();
                }
            });
        }
    },
    confirmDelete: function (id, message) {
        Web.Panel.Modal.Item.show(Web.Panel.ModalItemTypeEnum.DELETE, message, Web.Panel.City.delete, id);
    },
    delete: function (id) {
        Web.Panel.Loader.show();
        $.ajax({
            url: "/City/Delete",
            type: 'POST',
            data: { id: id },
            cache: false,
            success: function (result) {
                if (result.isSuccess) {
                    window.location.reload();
                }
                Web.Panel.Result.show(result);
                Web.Panel.Modal.Item.hide();
            },
            error: function (xhr, status, error) {
                alert("Bir problem oluştu!");
            }
        });
        Web.Panel.Loader.hide();
    }
};