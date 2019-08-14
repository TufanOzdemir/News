/// <reference path="Web.Panel.js" />

Web.Panel.ResultEnum = {
    INFORMATION: 1,
    SUCCESS: 2,
    WARNING: 3,
    ERROR: 4
};

Web.Panel.Result = {
    show: function (result) {
        var message = result.message;
        var resultType = result.type;
        var isSuccess = result.isSuccess;
        toastr.options = {
            "closeButton": true
        };
        if (resultType === Web.Panel.ResultEnum.INFORMATION) {
            toastr["info"](message,"Bilgi!");
        }
        if (resultType === Web.Panel.ResultEnum.SUCCESS) {
            toastr["success"](message, "Başarılı!");
        }
        if (resultType === Web.Panel.ResultEnum.WARNING) {
            toastr["warning"](message, "Uyarı!");
        }
        if (resultType === Web.Panel.ResultEnum.ERROR) {
            toastr["error"](message, "Hata!");
        }
    }
};